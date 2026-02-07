using PullSDKDataListener.DAL.Repositories;
using PullSDKDataListener.Entities.Models;
using PullSDKDataListener.UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PullSDKDataListener.UI
{
    public partial class PullSDKDataListenerForm : Form
    {
        private IntPtr _handle = IntPtr.Zero;
        private string _lastPinFromLog = "";
        private string _lastCardFromLog = "";
        private RTAccessEvent _lastEvent;
        private readonly Timer _rtTimer = new Timer();
        private const string ConnStr = "protocol=TCP,ipaddress=192.168.0.99,port=4370,timeout=2000,passwd=";
        private readonly string SqlConnStr = ConfigurationManager.ConnectionStrings["CeyPASSPullSDKConnection"].ConnectionString;
        private const int CurrentFirmaId = 101;
        private readonly AccessEventRepository _accessRepo;
        private int _dbInsertedCount = 0;
        private int _dbPersonNotFoundCount = 0;
        private int _dbErrorCount = 0;
        private int _dbDuplicateCount = 0;
        private string _lastDbErrorMessage = "";
        private DateTime _lastReconnectAttempt = DateTime.MinValue;
        private readonly TimeSpan _reconnectInterval = TimeSpan.FromSeconds(10);
        private int _reconnectTryCount = 0;

        public PullSDKDataListenerForm()
        {
            InitializeComponent();
            _accessRepo = new AccessEventRepository(SqlConnStr, CurrentFirmaId, 13);
            _rtTimer.Interval = 10000;
            _rtTimer.Tick += RtTimer_Tick;

            dtStart.Value = DateTime.Today.AddMonths(-1);
            dtEnd.Value = DateTime.Today;
            txtGroup.Text = "0";

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                ConnectDevice();
                _rtTimer.Start();
            }

            kuculmeIslevi.MouseDoubleClick += kuculmeIslevi_MouseDoubleClick;
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem cikisItem = new ToolStripMenuItem("Çıkış");
            cikisItem.Click += (s, ev) => Application.Exit();
            menu.Items.Add(cikisItem);
            kuculmeIslevi.ContextMenuStrip = menu;
        }

        private bool ConnectDevice(bool isReconnect = false)
        {
            if (_handle != IntPtr.Zero)
            {
                PullSdkNative.Disconnect(_handle);
                _handle = IntPtr.Zero;
            }

            _handle = PullSdkNative.Connect(ConnStr);

            if (_handle == IntPtr.Zero)
            {
                int err = PullSdkNative.PullLastError();

                if (isReconnect)
                    lblLastAccess.Text = $"Yeniden bağlanılamadı (Deneme #{_reconnectTryCount}). err={err}";
                else
                    lblLastAccess.Text = $"Cihaza bağlanılamadı. err={err}";

                return false;
            }

            _lastReconnectAttempt = DateTime.MinValue;
            _reconnectTryCount = 0;

            if (isReconnect)
                lblLastAccess.Text = "Cihaza yeniden bağlanıldı.";
            else
                lblLastAccess.Text = "Cihaza bağlantı başarılı.";
            return true;
        }

        private void PullSDKDataListenerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _rtTimer.Stop();

            if (_handle != IntPtr.Zero)
            {
                PullSdkNative.Disconnect(_handle);
                _handle = IntPtr.Zero;
            }
        }

        private void RtTimer_Tick(object sender, EventArgs e)
        {
            if (_handle == IntPtr.Zero)
            {
                var now = DateTime.Now;

                if (now - _lastReconnectAttempt >= _reconnectInterval)
                {
                    _lastReconnectAttempt = now;
                    _reconnectTryCount++;
                    ConnectDevice(isReconnect: true);
                }
                return;
            }

            const int bufferSize = 256;
            byte[] buffer = new byte[bufferSize];

            int ret = PullSdkNative.GetRTLog(_handle, ref buffer[0], bufferSize);

            if (ret == 0)
                return;

            if (ret < 0)
            {
                if (ret == -10053 || ret == -10054)
                {
                    PullSdkNative.Disconnect(_handle);
                    _handle = IntPtr.Zero;
                    _lastReconnectAttempt = DateTime.Now - _reconnectInterval;
                    lblLastAccess.Text = "Bağlantı koptu, yeniden bağlanma deneniyor...";
                }
                return;
            }

            string raw = Encoding.Default.GetString(buffer).TrimEnd('\0', '\r', '\n');
            if (string.IsNullOrWhiteSpace(raw))
                return;

            var lines = raw.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var ev = TryCaptureFromRtLine(line);
                if (ev == null)
                    continue;

                var saveResult = _accessRepo.SaveAccessEvent(ev);

                switch (saveResult.ResultType)
                {
                    case AccessSaveResultType.Inserted:
                        _dbInsertedCount++;
                        break;

                    case AccessSaveResultType.PersonNotFound:
                        _dbPersonNotFoundCount++;
                        lblLastAccess.Text =
                            $"Kişi bulunamadı (#{_dbPersonNotFoundCount}) Kart={ev.CardNo}, PIN={ev.Pin}";
                        continue;

                    case AccessSaveResultType.Duplicate:
                        _dbDuplicateCount++;
                        continue;

                    case AccessSaveResultType.Error:
                        _dbErrorCount++;
                        _lastDbErrorMessage = saveResult.ErrorMessage;
                        lblLastAccess.Text = $"DB hata (#{_dbErrorCount}): {saveResult.ErrorMessage}";
                        continue;
                }

                string display =$"{ev.Time:yyyy-MM-dd HH:mm:ss} | Kart={ev.CardNo} | Sicil No={ev.Pin} | Mod={ev.Mode}";
                lstRtLog.Items.Insert(0, display);
                lblLastAccess.Text = $"Kaydedildi: {display}";
            }
        }

        private RTAccessEvent TryCaptureFromRtLine(string line)
        {
            // Beklenen format: TarihSaat,Pin,CardNo,Verified,DoorId,EventType,InOut
            var parts = line.Split(',');
            if (parts.Length < 4)
                return null;

            string timeStr = parts[0].Trim();
            string pin = parts[1].Trim();
            string cardNo = parts[2].Trim();
            string verified = parts[3].Trim();

            if (verified != "1")
                return null;

            DateTime dt;
            if (!DateTime.TryParse(timeStr, out dt))
                dt = DateTime.Now;

            bool hasCard = !string.IsNullOrEmpty(cardNo) && cardNo != "0";
            bool hasPin = !string.IsNullOrEmpty(pin) && pin != "0";

            string mode;
            if (hasCard && hasPin)
                mode = "Kart+Şifre";
            else if (hasCard)
                mode = "Kart";
            else if (hasPin)
                mode = "Şifre";
            else
                mode = "Bilinmiyor";

            var ev = new RTAccessEvent
            {
                Time = dt,
                CardNo = hasCard ? cardNo : "",
                Pin = hasPin ? pin : "",
                Mode = mode
            };

            if (hasCard) _lastCardFromLog = cardNo;
            if (hasPin) _lastPinFromLog = pin;
            _lastEvent = ev;

            return ev;
        }

        private int SafeSetDeviceData(string table, string data, string options = "")
        {
            if (_handle == IntPtr.Zero)
            {
                MessageBox.Show("Önce cihaza bağlanın!");
                return -1;
            }

            _rtTimer.Stop();

            int ret = PullSdkNative.SetDeviceData(_handle, table, data, options);
            int err = PullSdkNative.PullLastError();

            if (ret == -10053 || ret == -10054)
            {
                PullSdkNative.Disconnect(_handle);
                _handle = IntPtr.Zero;

                if (ConnectDevice())
                {
                    ret = PullSdkNative.SetDeviceData(_handle, table, data, options);
                    err = PullSdkNative.PullLastError();
                }
            }

            MessageBox.Show($"SetDeviceData({table}) -> ret={ret}, err={err}", "Debug");
            _rtTimer.Start();
            return ret;
        }

        private int SafeDeleteDeviceData(string table, string where)
        {
            if (string.IsNullOrWhiteSpace(where) || !where.Contains("="))
            {
                MessageBox.Show($"Güvenlik için iptal edildi.\nDeleteDeviceData({table}) : '{where}' ifadesi geçersiz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            if (_handle == IntPtr.Zero)
            {
                MessageBox.Show("Önce cihaza bağlanın!");
                return -1;
            }

            _rtTimer.Stop();

            int ret = PullSdkNative.DeleteDeviceData(_handle, table, where, "");
            int err = PullSdkNative.PullLastError();

            if (ret == -10053 || ret == -10054)
            {
                PullSdkNative.Disconnect(_handle);
                _handle = IntPtr.Zero;

                if (ConnectDevice())
                {
                    ret = PullSdkNative.DeleteDeviceData(_handle, table, where, "");
                    err = PullSdkNative.PullLastError();
                }
            }

            MessageBox.Show($"DeleteDeviceData({table}) -> ret={ret}, err={err}", "Debug");
            _rtTimer.Start();
            return ret;
        }

        private bool SafeGetDeviceData(string table, string fields, string filter, string options, out string raw)
        {
            raw = string.Empty;

            if (_handle == IntPtr.Zero)
            {
                MessageBox.Show("Önce cihaza bağlanın!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            _rtTimer.Stop();

            const int bufferSize = 64 * 1024;
            byte[] buffer = new byte[bufferSize];

            int ret = PullSdkNative.GetDeviceData(_handle, ref buffer[0], bufferSize,
                                                  table, fields, filter, options);
            int err = PullSdkNative.PullLastError();

            // ---- BAĞLANTI KOPMASI DURUMU ----
            if (ret < 0 && (ret == -2 || ret == -10053 || ret == -10054))
            {
                // Mevcut bağlantıyı kapat
                PullSdkNative.Disconnect(_handle);
                _handle = IntPtr.Zero;

                lblLastAccess.Text =
                    $"GetDeviceData sırasında bağlantı koptu (ret={ret}, err={err}), yeniden bağlanma denenecek.";

                // Tek seferlik reconnect + yeniden deneme
                if (ConnectDevice(isReconnect: true))
                {
                    ret = PullSdkNative.GetDeviceData(_handle, ref buffer[0], bufferSize,
                                                      table, fields, filter, options);
                    err = PullSdkNative.PullLastError();

                    if (ret < 0)
                    {
                        MessageBox.Show($"GetDeviceData({table}) -> ret={ret}, err={err}", "Hata",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _rtTimer.Start();
                        return false;
                    }
                }
                else
                {
                    // Bağlanılamadı, hata kutusu yerine label’dan bilgi verelim
                    _rtTimer.Start();
                    return false;
                }
            }
            else if (ret < 0)
            {
                // Diğer hatalar: normal MessageBox
                MessageBox.Show($"GetDeviceData({table}) -> ret={ret}, err={err}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _rtTimer.Start();
                return false;
            }

            // Buraya geldiysek ret >= 0
            raw = Encoding.Default.GetString(buffer).TrimEnd('\0');
            _rtTimer.Start();

            return ret > 0 && !string.IsNullOrWhiteSpace(raw);
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (_handle == IntPtr.Zero)
            {
                MessageBox.Show("Önce cihaza bağlanın!");
                return;
            }

            string cardNo = txtCardNo.Text.Trim();
            string pin = txtPin.Text.Trim();
            string group = string.IsNullOrWhiteSpace(txtGroup.Text) ? "0" : txtGroup.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(pin) && !string.IsNullOrEmpty(_lastPinFromLog))
            {
                pin = _lastPinFromLog;
                txtPin.Text = pin;
            }

            if (string.IsNullOrEmpty(cardNo) && !string.IsNullOrEmpty(_lastCardFromLog))
            {
                cardNo = _lastCardFromLog;
                txtCardNo.Text = cardNo;
            }

            if (string.IsNullOrEmpty(cardNo) || string.IsNullOrEmpty(pin))
            {
                MessageBox.Show("Kart No ve PIN boş olamaz. Kart okutmadıysanız bir kez okutun.");
                return;
            }

            // ======================================================
            // USER TABLOSUNA KAYIT  (field=value + TAB formatı)
            // CardNo,Pin,Password,Group,StartTime,EndTime,SuperAuthorize
            // ======================================================

            // Password boş bırakılırsa sorun değil; cihaz yine kartla girişe izin verir
            string userDataPairs =
                $"CardNo={cardNo}\t" +
                $"Pin={pin}\t" +
                $"Password={password}\t" +
                $"Group={group}\t" +
                $"StartTime=0\t" +
                $"EndTime=0\t" +
                $"SuperAuthorize=0";

            int retUser = SafeSetDeviceData("user", userDataPairs);
            if (retUser < 0)
            {
                MessageBox.Show("Kullanıcı kaydedilemedi (user).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ======================================================
            // USERAUTHORIZE TABLOSUNA KAPI YETKİSİ
            // Pin,AuthorizeTimezoneId,AuthorizeDoorId
            // ======================================================
            string authDataPairs =
                $"Pin={pin}\t" +
                $"AuthorizeTimezoneId=1\t" +
                $"AuthorizeDoorId=15";

            int retAuth = SafeSetDeviceData("userauthorize", authDataPairs);
            if (retAuth < 0)
            {
                MessageBox.Show("Kullanıcı kaydedildi fakat kapı yetkisi verilemedi (userauthorize).", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Kullanıcı eklendi, şifre (Password) yazıldı ve kapı yetkisi verildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            LoadDeviceUsers();
            txtCardNo.Clear();
            txtPin.Clear();
            txtGroup.Clear();
            txtPassword.Clear();
        }

        private void btnRefreshUsers_Click(object sender, EventArgs e)
        {
            LoadDeviceUsers();
        }

        private void LoadDeviceUsers()
        {
            string raw;
            if (!SafeGetDeviceData("user", "*", "", "", out raw))
                return;

            var lines = raw
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            if (lines.Count <= 1)
            {
                MessageBox.Show("Cihazda hiç kullanıcı yok.", "Bilgi");
                dgvUsers.DataSource = null;
                return;
            }

            string header = lines[0];
            string[] cols = header.Split(',');

            int idxCardNo = Array.IndexOf(cols, "CardNo");
            int idxPin = Array.IndexOf(cols, "Pin");
            int idxGroup = Array.IndexOf(cols, "Group");
            int idxStart = Array.IndexOf(cols, "StartTime");
            int idxEnd = Array.IndexOf(cols, "EndTime");

            var dt = new DataTable();
            dt.Columns.Add("CardNo");
            dt.Columns.Add("Password");
            dt.Columns.Add("PIN");
            dt.Columns.Add("Group");
            dt.Columns.Add("Start");
            dt.Columns.Add("End");

            for (int i = 1; i < lines.Count; i++)
            {
                string[] parts = lines[i].Split(',');
                if (parts.Length != cols.Length) continue;

                string cardNo = idxCardNo >= 0 ? parts[idxCardNo] : "";
                string pin = idxPin >= 0 ? parts[idxPin] : "";
                string grp = idxGroup >= 0 ? parts[idxGroup] : "0";
                string st = idxStart >= 0 ? parts[idxStart] : "0";
                string et = idxEnd >= 0 ? parts[idxEnd] : "0";

                int idxPwd = Array.IndexOf(cols, "Password");
                string pwd = idxPwd >= 0 ? parts[idxPwd] : "";


                if (string.IsNullOrWhiteSpace(cardNo) && string.IsNullOrWhiteSpace(pin))
                    continue;

                dt.Rows.Add(cardNo, pwd, pin, grp, st, et);
            }

            dgvUsers.AutoGenerateColumns = true;
            dgvUsers.Columns.Clear();
            dgvUsers.DataSource = dt;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvUsers.Columns["CardNo"] != null)
                dgvUsers.Columns["CardNo"].HeaderText = "Kart No";

            if (dgvUsers.Columns["Password"] != null)
                dgvUsers.Columns["Password"].HeaderText = "Şifre";

            if (dgvUsers.Columns["PIN"] != null)
                dgvUsers.Columns["PIN"].HeaderText = "Sicil No";

            if (dgvUsers.Columns["Group"] != null)
                dgvUsers.Columns["Group"].HeaderText = "Grup";

            if (dgvUsers.Columns["Start"] != null)
                dgvUsers.Columns["Start"].HeaderText = "Başlangıç";

            if (dgvUsers.Columns["End"] != null)
                dgvUsers.Columns["End"].HeaderText = "Bitiş";
        }

        private void btnDeleteSelectedUser_Click(object sender, EventArgs e)
        {
            if (_handle == IntPtr.Zero)
            {
                MessageBox.Show("Önce cihaza bağlanın!");
                return;
            }

            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Lütfen silmek için bir satır seçin.");
                return;
            }

            string cardNo = dgvUsers.CurrentRow.Cells["CardNo"].Value?.ToString();
            string pin = dgvUsers.CurrentRow.Cells["PIN"].Value?.ToString();

            if (string.IsNullOrWhiteSpace(cardNo) || string.IsNullOrWhiteSpace(pin))
            {
                MessageBox.Show("Seçili satırdan Kart No veya PIN okunamadı.");
                return;
            }

            var confirm = MessageBox.Show($"KartNo: {cardNo}\nPIN (Sicil): {pin}\n\nBu kullanıcı ve kapı yetkisi silinsin mi?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            bool ok = true;

            int retAuth = SafeDeleteDeviceData("userauthorize", $"Pin={pin}");
            if (retAuth < 0) ok = false;

            int retUser = SafeDeleteDeviceData("user", $"CardNo={cardNo}");
            if (retUser < 0) ok = false;

            if (ok)
                MessageBox.Show("Seçili kullanıcı ve kapı yetkisi silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Silme sırasında hata oluştu, Debug mesajlarındaki ret/err değerlerine bakın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            LoadDeviceUsers();
        }

        private void SaveEventsToCsvOnDesktop(List<RTAccessEvent> eventsToExport)
        {
            try
            {
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string fileName = $"ZK_OfflineLogs(PullSDK)_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                string fullPath = Path.Combine(desktop, fileName);

                var sb = new StringBuilder();

                sb.AppendLine("TarihSaat;KartNo;PIN;Mod");

                foreach (var ev in eventsToExport)
                {
                    string line = string.Format($"{ev.Time.ToString("yyyy-MM-dd HH:mm:ss")};{ev.CardNo};{ev.Pin};{ev.Mode}");
                    sb.AppendLine(line);
                }

                File.WriteAllText(fullPath, sb.ToString(), Encoding.UTF8);

                lblLastAccess.Text = $"Offline loglar masaüstüne kaydedildi: {fileName}";
            }
            catch (Exception ex)
            {
                lblLastAccess.Text = "CSV kaydedilirken hata: " + ex.Message;
            }
        }

        private void btnFetchOfflineLogs_Click(object sender, EventArgs e)
        {
            FetchOfflineLogsForDateRange();
        }

        private void FetchOfflineLogsForDateRange()
        {
            if (_handle == IntPtr.Zero)
            {
                lblLastAccess.Text = "Offline log için önce cihaza bağlı olmalısınız.";
                return;
            }
            _dbInsertedCount = 0;
            _dbPersonNotFoundCount = 0;
            _dbErrorCount = 0;
            _dbDuplicateCount = 0;
            _lastDbErrorMessage = "";

            string raw;
            if (!SafeGetDeviceData("transaction", "*", "", "", out raw))
            {
                lblLastAccess.Text = "Transaction tablosu okunamadı.";
                return;
            }

            var lines = raw
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            if (lines.Count <= 1)
            {
                lblLastAccess.Text = "Transaction tablosunda kayıt yok.";
                return;
            }

            string header = lines[0];
            string[] cols = header.Split(',');

            int idxCardno = Array.IndexOf(cols, "Cardno");
            int idxPin = Array.IndexOf(cols, "Pin");
            int idxTimeSecond = Array.IndexOf(cols, "Time_second");

            // Verified'ı sadece debug için filtrelemeyeceğim çünkü baseData ve baseTime buna uygun değil
            int idxVerified = Array.IndexOf(cols, "Verified");

            if (idxCardno < 0 || idxPin < 0 || idxTimeSecond < 0)
            {
                lblLastAccess.Text = "Transaction kolonları beklenen formatta değil.";
                return;
            }

            DateTime start = dtStart.Value;
            DateTime end = dtEnd.Value;

            DateTime baseTime = new DateTime(2000, 1, 1, 0, 0, 0);

            var exportedEvents = new List<RTAccessEvent>();

            int totalRows = 0;
            int withIdRows = 0;
            int inRangeRows = 0;
            int dbInsertedRows = 0;

            for (int i = 1; i < lines.Count; i++)
            {
                totalRows++;

                string line = lines[i];
                string[] parts = line.Split(',');
                if (parts.Length != cols.Length)
                    continue;

                string cardNo = parts[idxCardno].Trim();
                string pin = parts[idxPin].Trim();
                string timeSec = parts[idxTimeSecond].Trim();

                if (!int.TryParse(timeSec, out int sec))
                    continue;

                DateTime dt = baseTime.AddSeconds(sec);

                if (dt < start || dt > end)
                    continue;
                inRangeRows++;

                bool hasCard = !string.IsNullOrEmpty(cardNo) && cardNo != "0";
                bool hasPin = !string.IsNullOrEmpty(pin) && pin != "0";

                if (!hasCard && !hasPin)
                    continue;
                withIdRows++;

                string mode;
                if (hasCard && hasPin)
                    mode = "Kart+Şifre";
                else if (hasCard)
                    mode = "Kart";
                else if (hasPin)
                    mode = "Şifre";
                else
                    mode = "Bilinmiyor";

                var ev = new RTAccessEvent
                {
                    Time = dt,
                    CardNo = hasCard ? cardNo : "",
                    Pin = hasPin ? pin : "",
                    Mode = mode
                };

                var saveResult = _accessRepo.SaveAccessEvent(ev);

                switch (saveResult.ResultType)
                {
                    case AccessSaveResultType.Inserted:
                        _dbInsertedCount++;
                        exportedEvents.Add(ev);
                        dbInsertedRows++;
                        break;

                    case AccessSaveResultType.PersonNotFound:
                        _dbPersonNotFoundCount++;
                        break;

                    case AccessSaveResultType.Duplicate:
                        _dbDuplicateCount++;
                        break;

                    case AccessSaveResultType.Error:
                        _dbErrorCount++;
                        _lastDbErrorMessage = saveResult.ErrorMessage;
                        break;
                }

                if (saveResult.ResultType != AccessSaveResultType.Inserted)
                    continue;

                string display =
                    $"{ev.Time:yyyy-MM-dd HH:mm:ss} | Kart={ev.CardNo} | PIN={ev.Pin} | Mod={ev.Mode}";
                lstRtLog.Items.Insert(0, display);
            }

            if (exportedEvents.Count > 0)
                SaveEventsToCsvOnDesktop(exportedEvents);

            lblLastAccess.Text =
    $"Offline log çekme tamamlandı. Toplam satır: {totalRows}, " +
    $"Kimlikli (Kart/PIN): {withIdRows}, Aralık içinde: {inRangeRows}, " +
    $"DB'ye yazılan: {_dbInsertedCount}, " +
    $"Kişi bulunamadı: {_dbPersonNotFoundCount}, " +
    $"Çift kayıt (zaten vardı): {_dbDuplicateCount}, " +
    $"DB hata: {_dbErrorCount}" +
    (string.IsNullOrEmpty(_lastDbErrorMessage) ? "" : $" (Son hata: {_lastDbErrorMessage})") +
    $". ({dtStart.Value:yyyy-MM-dd HH:mm} - {dtEnd.Value:yyyy-MM-dd HH:mm})";
        }

        private void btnConnectForced_Click(object sender, EventArgs e)
        {
            _lastReconnectAttempt = DateTime.MinValue;
            _reconnectTryCount = 0;

            bool ok = ConnectDevice(isReconnect: _handle != IntPtr.Zero);

            if (!ok)
            {
                MessageBox.Show("Cihaza bağlanılamadı, lütfen IP/kablo/güç durumunu kontrol edin.","Bağlantı Hatası",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void kuculmeIslevi_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            kuculmeIslevi.Visible = false;
        }

        private void PullSDKDataListenerForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                kuculmeIslevi.Visible = true;
                kuculmeIslevi.ShowBalloonTip(1000, "ZKTeco Data Listener(Pull SDK)", "Program arka planda çalışıyor...", ToolTipIcon.Info);
            }
        }
    }
}
