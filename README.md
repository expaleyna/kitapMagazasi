"# Kitap Mağazası - Online Kitap Satış Sistemi

Bu proje, ASP.NET Core kullanılarak geliştirilmiş bir online kitap satış sistemidir. Proje iki ana bileşenden oluşmaktadır:

## Proje Yapısı

- **kitapMagazaApi**: Web API (Backend)
- **kitapMagazaMvc**: MVC Web Uygulaması (Frontend)
- **kitapsql.sql**: Veritabanı şeması

## Özellikler

- Kitap arama ve listeleme
- Kategori bazlı filtreleme
- Kullanıcı kayıt ve giriş sistemi
- Sepet yönetimi
- Sipariş takibi
- Admin paneli
- Favori kitaplar

## Kurulum

### 1. Veritabanı Kurulumu

Öncelikle `kitapsql.sql` dosyasını SQL Server'da çalıştırarak veritabanını oluşturun.

### 2. API Çalıştırma

```bash
cd kitapMagazaApi
dotnet run --urls="https://localhost:7000;http://localhost:7001"
```

API Swagger dokumentasyonu: https://localhost:7000/swagger

### 3. MVC Uygulamasını Çalıştırma

Yeni bir terminal açarak:

```bash
cd kitapMagazaMvc
dotnet run --urls="https://localhost:5001;http://localhost:5000"
```

MVC Uygulaması: https://localhost:5001

## Teknolojiler

- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server
- Bootstrap
- HTML/CSS/JavaScript

## Kullanım

1. Önce API'yi başlatın
2. Ardından MVC uygulamasını çalıştırın
3. Tarayıcıda https://localhost:5001 adresine gidin
