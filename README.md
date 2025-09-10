# 📘 Veresiye Takip Sistemi

Bu proje, **ASP.NET Core MVC** kullanılarak geliştirilmiş bir **veresiye defteri uygulamasıdır**. Kullanıcılar borç kayıtlarını ekleyebilir, güncelleyebilir, silebilir ve kayıtlı kişilere e-posta gönderebilir. Proje, modern bir **CI/CD** süreciyle desteklenmekte olup, **GitHub Actions** kullanılarak VPS'e otomatik olarak derlenip dağıtılmaktadır.

## 🚀 Özellikler

- Kullanıcı kaydı ve giriş sistemi
- Borç ekleme, güncelleme, silme
- Kime, ne kadar borç verildiğini tarihli olarak takip etme
- Kayıtlı müşteriye mail gönderme (SMTP ile)
- Gönderilen mailleri veritabanına kaydetme ve listeleme
- **Veritabanı**: PostgreSQL
- **Arayüz**: Razor View + DataTables
- **CI/CD**: GitHub Actions ile VPS'e otomatik derleme ve dağıtım

## 📬 Mail Gönderme

Mail gönderimi için `appsettings.json` içine aşağıdaki yapı eklenmelidir:

```json
"EmailSettings": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": 587,
  "SenderName": "Adınız",
  "SenderEmail": "youremail@gmail.com",
  "Password": "uygulama_sifresi"
}
