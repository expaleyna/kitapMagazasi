# Copilot Instructions

<!-- Use this file to provide workspace-specific custom instructions to Copilot. For more details, visit https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file -->

Bu proje, ASP.NET Core 8.0 kullanarak geliştirilmiş bir online kitapçı e-ticaret uygulamasıdır.

## Proje Yapısı

- **kitapMagazaApi**: ASP.NET Core Web API projesi (Backend)
- **kitapMagazaMvc**: ASP.NET Core MVC projesi (Frontend)

## Önemli Noktalar

1. **Entity Framework Core Code-First** yaklaşımı kullanılmıştır
2. **Bootstrap 5** ile responsive tasarım
3. **Session** tabanlı sepet yönetimi
4. **HttpClient** ile API entegrasyonu
5. **CORS** yapılandırması yapılmıştır

## Geliştirme Kuralları

- API endpoint'leri RESTful standartlarda olmalı
- Model validation için DataAnnotations kullanın
- CSRF koruması için AntiForgeryToken kullanın
- Error handling ve logging implement edin
- Responsive tasarım prensiplerini takip edin

## Teknolojiler

- .NET 8.0
- Entity Framework Core
- SQL Server / LocalDB
- Bootstrap 5
- Font Awesome
- Swagger/OpenAPI
