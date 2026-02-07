# 📡 ZKTeco Data Listener Service (Pull SDK)

![.NET Framework](https://img.shields.io/badge/.NET-4.7.2-purple) ![Status](https://img.shields.io/badge/Status-Active-success)

## 🇹🇷 Türkçe (Turkish)

### ℹ️ Proje Hakkında
Bu proje, **ZKTeco**'nun numpad (tuş takımı) bulunan cihazlarından **Pull SDK** protokolü kullanarak anlık veri dinleyen (Real-Time Monitoring) bir Windows Forms uygulamasıdır.

Cihaz üzerindeki parmak izi, kart veya şifre ile yapılan giriş-çıkış hareketlerini anlık olarak yakalar, veritabanına kaydeder ve arayüzde gösterir. Ayrıca cihazdaki kullanıcıları yönetme (ekleme, silme) ve geçmiş (offline) logları çekme yeteneğine sahiptir.

### 🚀 Özellikler
- **Anlık Veri Dinleme:** Cihazdan gelen hareketleri (Log) saniyesinde yakalar.
- **Kullanıcı Yönetimi:** Cihaza yeni kullanıcı, kart ve şifre tanımlayabilir veya silebilirsiniz.
- **Offline Log Çekme:** Cihaz bağlantısı kopsa bile, bağlantı sağlandığında geçmiş kayıtları çekebilirsiniz.
- **Veritabanı Entegrasyonu:** Yakalanan veriler MSSQL veritabanına kaydedilir.
- **CSV Dışa Aktarım:** Kayıtları masaüstüne Excel/CSV formatında raporlayabilirsiniz.

### ⚠️ Önemli Güvenlik Uyarısı
> [!IMPORTANT]
> Bu proje, **test ve geliştirme** ortamları için varsayılan ayarlarla gelir.
> Prodüksiyon (Canlı) ortamına geçmeden önce aşağıdaki dosyayı mutlaka düzenleyin:

- **Dosya:** `PullSDKDataListenerForm.cs`
- **Satır:** ~23
`private const string ConnStr = "protocol=TCP,ipaddress=192.168.0.99,port=4370,timeout=2000,passwd=";`

Lütfen kaynak kod içerisindeki **IP Adresi** ve **Cihaz Şifresini (passwd)** kendi ağ güvenliğinize göre güncelleyiniz. Şifresiz cihaz kullanımı güvenlik riski oluşturabilir.

- **Dosya:** `App.config`
`connectionString="Server=127.0.0.1;Database=CeyPASS;User Id=sa;Password=YOUR_PASSWORD_HERE;"`
Veritabanı bağlantı şifresi güvenlik nedeniyle gizlenmiştir. Projeyi çalıştırmadan önce kendi **SQL Server şifrenizi** giriniz.

---

## 🇬🇧 English

### ℹ️ About the Project
This project is a Windows Forms application that listens for real-time data from **ZKTeco** devices (specifically those with numpads) using the **Pull SDK** protocol.

It captures attendance records (fingerprint, card, or password) instantly, saves them to a database, and displays them on the UI. It also provides features to manage device users (add/delete) and fetch offline logs.

### 🚀 Features
- **Real-Time Monitoring:** Captures device logs instantly as they happen.
- **User Management:** Add or delete users, cards, and passwords directly on the device.
- **Offline Log Fetch:** Retrieve past records even if the device was disconnected for a while.
- **Database Integration:** Syncs captured data to an MSSQL database.
- **CSV Export:** Export logs to desktop in Excel/CSV format.

### ⚠️ Security Notice
> [!IMPORTANT]
> This project comes with default settings for **testing and development**.
> Before deploying to production, you MUST update the following configuration:

- **File:** `PullSDKDataListenerForm.cs`
- **Line:** ~23
`private const string ConnStr = "protocol=TCP,ipaddress=192.168.0.99,port=4370,timeout=2000,passwd=";`

Please update the **IP Address** and **Device Password (passwd)** in the source code according to your network security policies. Using devices without a password may pose a security risk.

- **File:** `App.config`
`connectionString="Server=127.0.0.1;Database=CeyPASS;User Id=sa;Password=YOUR_PASSWORD_HERE;"`
Database password is hidden for security. Please enter your own **SQL Server password** before running the project.

---

### 🛠️ Kurulum / Setup

1. **Requirements:**
   - .NET Framework 4.7.2
   - MSSQL Server
   - ZKTeco Pull SDK DLLs (`plcommpro.dll`, etc.) - *Included in project / Projede mevcut*

2. **Configuration:**
   - Update `App.config` with your SQL connection string.
   - Update device IP in `PullSDKDataListenerForm.cs`.

3. **Run:**
   - Open specific port (Default: 4370) in your firewall if necessary.
   - Build and Run via Visual Studio.
