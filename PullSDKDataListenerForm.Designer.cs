using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PullSDKDataListener.UI
{
    partial class PullSDKDataListenerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PullSDKDataListenerForm));
            this.grpNewUser = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.txtGroup = new System.Windows.Forms.TextBox();
            this.lblGroup = new System.Windows.Forms.Label();
            this.txtPin = new System.Windows.Forms.TextBox();
            this.txtCardNo = new System.Windows.Forms.TextBox();
            this.lblPin = new System.Windows.Forms.Label();
            this.lblCardNo = new System.Windows.Forms.Label();
            this.grpUsers = new System.Windows.Forms.GroupBox();
            this.btnConnectForced = new System.Windows.Forms.Button();
            this.btnFetchOfflineLogs = new System.Windows.Forms.Button();
            this.btnDeleteSelectedUser = new System.Windows.Forms.Button();
            this.btnRefreshUsers = new System.Windows.Forms.Button();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.grpRtLog = new System.Windows.Forms.GroupBox();
            this.lstRtLog = new System.Windows.Forms.ListBox();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeaderSubtitle = new System.Windows.Forms.Label();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.lblLastAccess = new System.Windows.Forms.Label();
            this.kuculmeIslevi = new System.Windows.Forms.NotifyIcon(this.components);
            this.grpNewUser.SuspendLayout();
            this.grpUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.grpRtLog.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpNewUser
            // 
            this.grpNewUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpNewUser.BackColor = System.Drawing.Color.White;
            this.grpNewUser.Controls.Add(this.txtPassword);
            this.grpNewUser.Controls.Add(this.label1);
            this.grpNewUser.Controls.Add(this.btnAddUser);
            this.grpNewUser.Controls.Add(this.dtEnd);
            this.grpNewUser.Controls.Add(this.dtStart);
            this.grpNewUser.Controls.Add(this.lblEnd);
            this.grpNewUser.Controls.Add(this.lblStart);
            this.grpNewUser.Controls.Add(this.txtGroup);
            this.grpNewUser.Controls.Add(this.lblGroup);
            this.grpNewUser.Controls.Add(this.txtPin);
            this.grpNewUser.Controls.Add(this.txtCardNo);
            this.grpNewUser.Controls.Add(this.lblPin);
            this.grpNewUser.Controls.Add(this.lblCardNo);
            this.grpNewUser.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpNewUser.Location = new System.Drawing.Point(12, 54);
            this.grpNewUser.Name = "grpNewUser";
            this.grpNewUser.Size = new System.Drawing.Size(722, 172);
            this.grpNewUser.TabIndex = 1;
            this.grpNewUser.TabStop = false;
            this.grpNewUser.Text = "Kullanıcı Ekle / Güncelle";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(375, 58);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(150, 27);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(315, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Şifre:";
            // 
            // btnAddUser
            // 
            this.btnAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnAddUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddUser.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddUser.ForeColor = System.Drawing.Color.White;
            this.btnAddUser.Location = new System.Drawing.Point(564, 132);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(140, 32);
            this.btnAddUser.TabIndex = 6;
            this.btnAddUser.Text = "Kaydet";
            this.btnAddUser.UseVisualStyleBackColor = false;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // dtEnd
            // 
            this.dtEnd.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtEnd.Location = new System.Drawing.Point(118, 119);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Size = new System.Drawing.Size(220, 27);
            this.dtEnd.TabIndex = 5;
            // 
            // dtStart
            // 
            this.dtStart.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStart.Location = new System.Drawing.Point(118, 90);
            this.dtStart.Name = "dtStart";
            this.dtStart.Size = new System.Drawing.Size(220, 27);
            this.dtStart.TabIndex = 4;
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Location = new System.Drawing.Point(18, 122);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(66, 20);
            this.lblEnd.TabIndex = 10;
            this.lblEnd.Text = "Bitiş(TS):";
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(18, 93);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(101, 20);
            this.lblStart.TabIndex = 8;
            this.lblStart.Text = "Başlangıç(TS):";
            // 
            // txtGroup
            // 
            this.txtGroup.Location = new System.Drawing.Point(118, 58);
            this.txtGroup.Name = "txtGroup";
            this.txtGroup.Size = new System.Drawing.Size(80, 27);
            this.txtGroup.TabIndex = 2;
            // 
            // lblGroup
            // 
            this.lblGroup.AutoSize = true;
            this.lblGroup.Location = new System.Drawing.Point(18, 61);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.Size = new System.Drawing.Size(44, 20);
            this.lblGroup.TabIndex = 4;
            this.lblGroup.Text = "Grup:";
            // 
            // txtPin
            // 
            this.txtPin.Location = new System.Drawing.Point(375, 27);
            this.txtPin.Name = "txtPin";
            this.txtPin.Size = new System.Drawing.Size(150, 27);
            this.txtPin.TabIndex = 1;
            // 
            // txtCardNo
            // 
            this.txtCardNo.Location = new System.Drawing.Point(118, 27);
            this.txtCardNo.Name = "txtCardNo";
            this.txtCardNo.Size = new System.Drawing.Size(180, 27);
            this.txtCardNo.TabIndex = 0;
            // 
            // lblPin
            // 
            this.lblPin.AutoSize = true;
            this.lblPin.Location = new System.Drawing.Point(315, 30);
            this.lblPin.Name = "lblPin";
            this.lblPin.Size = new System.Drawing.Size(63, 20);
            this.lblPin.TabIndex = 2;
            this.lblPin.Text = "Sicil No:";
            // 
            // lblCardNo
            // 
            this.lblCardNo.AutoSize = true;
            this.lblCardNo.Location = new System.Drawing.Point(18, 30);
            this.lblCardNo.Name = "lblCardNo";
            this.lblCardNo.Size = new System.Drawing.Size(63, 20);
            this.lblCardNo.TabIndex = 0;
            this.lblCardNo.Text = "Kart No:";
            // 
            // grpUsers
            // 
            this.grpUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpUsers.BackColor = System.Drawing.Color.White;
            this.grpUsers.Controls.Add(this.btnConnectForced);
            this.grpUsers.Controls.Add(this.btnFetchOfflineLogs);
            this.grpUsers.Controls.Add(this.btnDeleteSelectedUser);
            this.grpUsers.Controls.Add(this.btnRefreshUsers);
            this.grpUsers.Controls.Add(this.dgvUsers);
            this.grpUsers.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpUsers.Location = new System.Drawing.Point(12, 232);
            this.grpUsers.Name = "grpUsers";
            this.grpUsers.Size = new System.Drawing.Size(722, 268);
            this.grpUsers.TabIndex = 2;
            this.grpUsers.TabStop = false;
            this.grpUsers.Text = "Cihazdaki Kullanıcılar";
            // 
            // btnConnectForced
            // 
            this.btnConnectForced.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnConnectForced.BackColor = System.Drawing.Color.Green;
            this.btnConnectForced.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnectForced.ForeColor = System.Drawing.Color.White;
            this.btnConnectForced.Location = new System.Drawing.Point(552, 228);
            this.btnConnectForced.Name = "btnConnectForced";
            this.btnConnectForced.Size = new System.Drawing.Size(150, 30);
            this.btnConnectForced.TabIndex = 4;
            this.btnConnectForced.Text = "Cihazı Zorla Bağla";
            this.btnConnectForced.UseVisualStyleBackColor = false;
            this.btnConnectForced.Click += new System.EventHandler(this.btnConnectForced_Click);
            // 
            // btnFetchOfflineLogs
            // 
            this.btnFetchOfflineLogs.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnFetchOfflineLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnFetchOfflineLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFetchOfflineLogs.ForeColor = System.Drawing.Color.White;
            this.btnFetchOfflineLogs.Location = new System.Drawing.Point(199, 228);
            this.btnFetchOfflineLogs.Name = "btnFetchOfflineLogs";
            this.btnFetchOfflineLogs.Size = new System.Drawing.Size(150, 30);
            this.btnFetchOfflineLogs.TabIndex = 2;
            this.btnFetchOfflineLogs.Text = "Geçmiş Logları Çek";
            this.btnFetchOfflineLogs.UseVisualStyleBackColor = false;
            this.btnFetchOfflineLogs.Click += new System.EventHandler(this.btnFetchOfflineLogs_Click);
            // 
            // btnDeleteSelectedUser
            // 
            this.btnDeleteSelectedUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteSelectedUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(76)))), ((int)(((byte)(61)))));
            this.btnDeleteSelectedUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteSelectedUser.ForeColor = System.Drawing.Color.White;
            this.btnDeleteSelectedUser.Location = new System.Drawing.Point(375, 228);
            this.btnDeleteSelectedUser.Name = "btnDeleteSelectedUser";
            this.btnDeleteSelectedUser.Size = new System.Drawing.Size(150, 30);
            this.btnDeleteSelectedUser.TabIndex = 3;
            this.btnDeleteSelectedUser.Text = "Seçili Kullanıcıyı Sil";
            this.btnDeleteSelectedUser.UseVisualStyleBackColor = false;
            this.btnDeleteSelectedUser.Click += new System.EventHandler(this.btnDeleteSelectedUser_Click);
            // 
            // btnRefreshUsers
            // 
            this.btnRefreshUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefreshUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnRefreshUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshUsers.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRefreshUsers.Location = new System.Drawing.Point(18, 228);
            this.btnRefreshUsers.Name = "btnRefreshUsers";
            this.btnRefreshUsers.Size = new System.Drawing.Size(150, 30);
            this.btnRefreshUsers.TabIndex = 1;
            this.btnRefreshUsers.Text = "Kullanıcıları Yükle";
            this.btnRefreshUsers.UseVisualStyleBackColor = false;
            this.btnRefreshUsers.Click += new System.EventHandler(this.btnRefreshUsers_Click);
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUsers.BackgroundColor = System.Drawing.Color.White;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(18, 22);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.RowHeadersWidth = 51;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(686, 198);
            this.dgvUsers.TabIndex = 0;
            // 
            // grpRtLog
            // 
            this.grpRtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpRtLog.BackColor = System.Drawing.Color.White;
            this.grpRtLog.Controls.Add(this.lstRtLog);
            this.grpRtLog.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpRtLog.Location = new System.Drawing.Point(740, 54);
            this.grpRtLog.Name = "grpRtLog";
            this.grpRtLog.Size = new System.Drawing.Size(618, 449);
            this.grpRtLog.TabIndex = 3;
            this.grpRtLog.TabStop = false;
            this.grpRtLog.Text = "Gerçek Zamanlı Log";
            // 
            // lstRtLog
            // 
            this.lstRtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstRtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.lstRtLog.FormattingEnabled = true;
            this.lstRtLog.HorizontalScrollbar = true;
            this.lstRtLog.ItemHeight = 18;
            this.lstRtLog.Location = new System.Drawing.Point(3, 23);
            this.lstRtLog.Name = "lstRtLog";
            this.lstRtLog.Size = new System.Drawing.Size(612, 423);
            this.lstRtLog.TabIndex = 0;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(45)))), ((int)(((byte)(60)))));
            this.pnlHeader.Controls.Add(this.lblHeaderSubtitle);
            this.pnlHeader.Controls.Add(this.lblHeaderTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1370, 42);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblHeaderSubtitle
            // 
            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(1007, 15);
            this.lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            this.lblHeaderSubtitle.Size = new System.Drawing.Size(364, 20);
            this.lblHeaderSubtitle.TabIndex = 1;
            this.lblHeaderSubtitle.Text = "Gerçek zamanlı geçişler ve geçmiş log yönetim paneli";
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.White;
            this.lblHeaderTitle.Location = new System.Drawing.Point(12, 11);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(329, 30);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "ZKTeco Pull SDK – Data Listener";
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(240)))));
            this.pnlFooter.Controls.Add(this.lblLastAccess);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 506);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1370, 36);
            this.pnlFooter.TabIndex = 4;
            // 
            // lblLastAccess
            // 
            this.lblLastAccess.AutoSize = true;
            this.lblLastAccess.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLastAccess.ForeColor = System.Drawing.Color.DimGray;
            this.lblLastAccess.Location = new System.Drawing.Point(12, 6);
            this.lblLastAccess.Name = "lblLastAccess";
            this.lblLastAccess.Size = new System.Drawing.Size(98, 20);
            this.lblLastAccess.TabIndex = 0;
            this.lblLastAccess.Text = "Bilgilendirme";
            // 
            // kuculmeIslevi
            // 
            this.kuculmeIslevi.Icon = ((System.Drawing.Icon)(resources.GetObject("kuculmeIslevi.Icon")));
            this.kuculmeIslevi.Text = "ZKTeco Data Listener(Pull SDK)";
            this.kuculmeIslevi.Visible = true;
            this.kuculmeIslevi.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.kuculmeIslevi_MouseDoubleClick);
            // 
            // PullSDKDataListenerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(1370, 542);
            this.Controls.Add(this.grpRtLog);
            this.Controls.Add(this.grpUsers);
            this.Controls.Add(this.grpNewUser);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1388, 589);
            this.Name = "PullSDKDataListenerForm";
            this.Text = "ZKTeco Data Listener(Pull SDK)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PullSDKDataListenerForm_FormClosing);
            this.Resize += new System.EventHandler(this.PullSDKDataListenerForm_Resize);
            this.grpNewUser.ResumeLayout(false);
            this.grpNewUser.PerformLayout();
            this.grpUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.grpRtLog.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.GroupBox grpNewUser;
        private System.Windows.Forms.TextBox txtCardNo;
        private System.Windows.Forms.TextBox txtPin;
        private System.Windows.Forms.TextBox txtGroup;
        private System.Windows.Forms.DateTimePicker dtStart;
        private System.Windows.Forms.DateTimePicker dtEnd;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.GroupBox grpUsers;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Button btnRefreshUsers;
        private System.Windows.Forms.Button btnDeleteSelectedUser;
        private System.Windows.Forms.GroupBox grpRtLog;
        private System.Windows.Forms.ListBox lstRtLog;
        private System.Windows.Forms.Label lblCardNo;
        private System.Windows.Forms.Label lblGroup;
        private System.Windows.Forms.Label lblPin;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFetchOfflineLogs;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderSubtitle;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblLastAccess;
        private Button btnConnectForced;
        private NotifyIcon kuculmeIslevi;
    }
}