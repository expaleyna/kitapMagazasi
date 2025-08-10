using Microsoft.EntityFrameworkCore;
using kitapMagazaApi.Data;
using kitapMagazaApi.Models;

namespace kitapMagazaApi.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(kitapMagazaDbContext context)
        {
            // Veri varsa seeding yapma
            if (await context.Categories.AnyAsync())
                return;

            // Kategoriler
            var categories = new List<Category>
            {
                new Category { Name = "Roman", Description = "Edebiyat ve roman kitapları", CreatedDate = DateTime.Now },
                new Category { Name = "Bilim Kurgu", Description = "Bilim kurgu ve fantastik kitaplar", CreatedDate = DateTime.Now },
                new Category { Name = "Tarih", Description = "Tarih ve sosyal bilimler", CreatedDate = DateTime.Now },
                new Category { Name = "Çocuk Kitapları", Description = "Çocuklar için eğitici ve eğlenceli kitaplar", CreatedDate = DateTime.Now },
                new Category { Name = "Psikoloji", Description = "Psikoloji ve kişisel gelişim", CreatedDate = DateTime.Now },
                new Category { Name = "Felsefe", Description = "Felsefe ve düşünce kitapları", CreatedDate = DateTime.Now },
                new Category { Name = "Biyografi", Description = "Ünlü kişilerin hayat hikayeleri", CreatedDate = DateTime.Now },
                new Category { Name = "Sanat", Description = "Sanat ve kültür kitapları", CreatedDate = DateTime.Now },
                new Category { Name = "Teknoloji", Description = "Bilgisayar ve teknoloji", CreatedDate = DateTime.Now },
                new Category { Name = "Sağlık", Description = "Sağlık ve yaşam", CreatedDate = DateTime.Now }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // Kullanıcılar
            var users = new List<User>
            {
                new User 
                { 
                    FirstName = "Admin", 
                    LastName = "User", 
                    Email = "admin@kitapmagaza.com", 
                    Password = "admin123", 
                    Role = "Admin", 
                    Phone = "0212 123 45 67", 
                    Address = "İstanbul, Türkiye",
                    CreatedDate = DateTime.Now
                },
                new User 
                { 
                    FirstName = "Ahmet", 
                    LastName = "Yılmaz", 
                    Email = "ahmet@email.com", 
                    Password = "ahmet123", 
                    Role = "User", 
                    Phone = "0532 111 22 33", 
                    Address = "Ankara, Çankaya",
                    CreatedDate = DateTime.Now
                },
                new User 
                { 
                    FirstName = "Ayşe", 
                    LastName = "Demir", 
                    Email = "ayse@email.com", 
                    Password = "ayse123", 
                    Role = "User", 
                    Phone = "0533 444 55 66", 
                    Address = "İzmir, Konak",
                    CreatedDate = DateTime.Now
                },
                new User 
                { 
                    FirstName = "Mehmet", 
                    LastName = "Kaya", 
                    Email = "mehmet@email.com", 
                    Password = "mehmet123", 
                    Role = "User", 
                    Phone = "0534 777 88 99", 
                    Address = "Bursa, Nilüfer",
                    CreatedDate = DateTime.Now
                }
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            // Kitaplar
            var kitaplar = new List<Kitap>
            {
                // Roman Kategorisi (Id: 1)
                new Kitap { Title = "Suç ve Ceza", Author = "Fyodor Dostoyevski", Description = "Rus edebiyatının başyapıtlarından biri olan bu klasik, insan psikolojisinin derinliklerini ele alır.", Price = 45.50m, ImageUrl = "/images/kitaplar/suc-ve-ceza.jpg", Stock = 25, CategoryId = 1, IsActive = true, CreatedDate = DateTime.Now },
                new Kitap { Title = "Beyaz Zambaklar Ülkesinde", Author = "Grigory Petrov", Description = "Finlandiya'nın eğitim sistemini ve toplumsal yapısını anlatan etkileyici bir eser.", Price = 32.00m, ImageUrl = "/images/kitaplar/beyaz-zambaklar.jpg", Stock = 18, CategoryId = 1, IsActive = true, CreatedDate = DateTime.Now },
                new Kitap { Title = "Kürk Mantolu Madonna", Author = "Sabahattin Ali", Description = "Türk edebiyatının en önemli aşk romanlarından biri.", Price = 28.75m, ImageUrl = "/images/kitaplar/kurk-mantolu-madonna.jpg", Stock = 30, CategoryId = 1, IsActive = true, CreatedDate = DateTime.Now },
                new Kitap { Title = "Çalıkuşu", Author = "Reşat Nuri Güntekin", Description = "Feride'nin hayat öyküsünü anlatan ölümsüz klasik.", Price = 25.00m, ImageUrl = "/images/kitaplar/calikusu.jpg", Stock = 22, CategoryId = 1, IsActive = true, CreatedDate = DateTime.Now },

                // Bilim Kurgu Kategorisi (Id: 2)
                new Kitap { Title = "Dune", Author = "Frank Herbert", Description = "Bilim kurgu edebiyatının başyapıtı. Arrakis gezegeni ve baharat savaşları.", Price = 55.00m, ImageUrl = "/images/kitaplar/dune.jpg", Stock = 15, CategoryId = 2, IsActive = true, CreatedDate = DateTime.Now },
                new Kitap { Title = "1984", Author = "George Orwell", Description = "Distopik bir gelecek tasviri. Büyük Birader'in gözetlediği dünya.", Price = 38.50m, ImageUrl = "/images/kitaplar/1984.jpg", Stock = 40, CategoryId = 2, IsActive = true, CreatedDate = DateTime.Now },
                new Kitap { Title = "Yüzüklerin Efendisi", Author = "J.R.R. Tolkien", Description = "Fantastik edebiyatın klasiklerinden. Orta Dünya macerası.", Price = 125.00m, ImageUrl = "/images/kitaplar/yuzuklerin-efendisi.jpg", Stock = 12, CategoryId = 2, IsActive = true, CreatedDate = DateTime.Now },
                new Kitap { Title = "Fahrenheit 451", Author = "Ray Bradbury", Description = "Kitapların yasaklandığı bir dünyada geçen distopik roman.", Price = 42.00m, ImageUrl = "/images/kitaplar/fahrenheit-451.jpg", Stock = 20, CategoryId = 2, IsActive = true, CreatedDate = DateTime.Now },

                // Tarih Kategorisi (Id: 3)
                new Kitap { Title = "Nutuk", Author = "Mustafa Kemal Atatürk", Description = "Kurtuluş Savaşı ve Cumhuriyet'in kuruluş tarihini anlatan tarihî belge.", Price = 35.00m, ImageUrl = "/images/kitaplar/nutuk.jpg", Stock = 35, CategoryId = 3, IsActive = true, CreatedDate = DateTime.Now },
                new Kitap { Title = "Devlet-i Aliyye", Author = "Halil İnalcık", Description = "Osmanlı İmparatorluğu'nun kapsamlı tarihî analizi.", Price = 89.50m, ImageUrl = "/images/kitaplar/devleti-aliyye.jpg", Stock = 10, CategoryId = 3, IsActive = true, CreatedDate = DateTime.Now },
                new Kitap { Title = "Sapiens", Author = "Yuval Noah Harari", Description = "İnsan türünün tarihsel gelişimi ve geleceği hakkında çok satan kitap.", Price = 58.00m, ImageUrl = "/images/kitaplar/sapiens.jpg", Stock = 28, CategoryId = 3, IsActive = true, CreatedDate = DateTime.Now },

                // Çocuk Kitapları Kategorisi (Id: 4)
                new Kitap { Title = "Küçük Prens", Author = "Antoine de Saint-Exupéry", Description = "Çocuklar ve yetişkinler için yazılmış ölümsüz bir klasik.", Price = 22.50m, ImageUrl = "/images/kitaplar/kucuk-prens.jpg", Stock = 45, CategoryId = 4, IsActive = true, CreatedDate = DateTime.Now },
                new Kitap { Title = "Pinokyo", Author = "Carlo Collodi", Description = "Ahşap kukladan gerçek çocuğa dönüşen Pinokyo'nun macerası.", Price = 18.00m, ImageUrl = "/images/kitaplar/pinokyo.jpg", Stock = 38, CategoryId = 4, IsActive = true, CreatedDate = DateTime.Now },
                new Kitap { Title = "Alice Harikalar Diyarında", Author = "Lewis Carroll", Description = "Alice'in sihirli dünyada yaşadığı olağanüstü maceralar.", Price = 24.00m, ImageUrl = "/images/kitaplar/alice.jpg", Stock = 32, CategoryId = 4, IsActive = true, CreatedDate = DateTime.Now },

                // Psikoloji Kategorisi (Id: 5)
                new Kitap { Title = "İnsan Arayışı", Author = "Viktor Frankl", Description = "Logoterapinin kurucusundan anlam arayışı ve hayatın anlamı.", Price = 42.00m, ImageUrl = "/images/kitaplar/insan-arayisi.jpg", Stock = 25, CategoryId = 5, IsActive = true, CreatedDate = DateTime.Now },
                new Kitap { Title = "Düşünme Sanatı", Author = "Rolf Dobelli", Description = "Karar verme süreçleri ve düşünme hataları hakkında pratik rehber.", Price = 36.50m, ImageUrl = "/images/kitaplar/dusunme-sanati.jpg", Stock = 22, CategoryId = 5, IsActive = true, CreatedDate = DateTime.Now },
                new Kitap { Title = "Beden Dili", Author = "Julius Fast", Description = "İletişimde beden dilinin önemi ve anlamları.", Price = 28.00m, ImageUrl = "/images/kitaplar/beden-dili.jpg", Stock = 30, CategoryId = 5, IsActive = true, CreatedDate = DateTime.Now },

                // Felsefe Kategorisi (Id: 6)
                new Kitap { Title = "Simyacı", Author = "Paulo Coelho", Description = "Hayallerinin peşinden giden bir çobanın hikayesi.", Price = 32.00m, ImageUrl = "/images/kitaplar/simyaci.jpg", Stock = 40, CategoryId = 6, IsActive = true, CreatedDate = DateTime.Now },
                new Kitap { Title = "Felsefeye Giriş", Author = "Bertrand Russell", Description = "Felsefenin temel konularına giriş niteliğinde kapsamlı eser.", Price = 45.00m, ImageUrl = "/images/kitaplar/felsefeye-giris.jpg", Stock = 15, CategoryId = 6, IsActive = true, CreatedDate = DateTime.Now },

                // Biyografi Kategorisi (Id: 7)
                new Kitap { Title = "Steve Jobs", Author = "Walter Isaacson", Description = "Apple kurucusunun resmi biyografisi.", Price = 65.00m, ImageUrl = "/images/kitaplar/steve-jobs.jpg", Stock = 18, CategoryId = 7, IsActive = true, CreatedDate = DateTime.Now },
                new Kitap { Title = "Einstein: Hayatı ve Evreni", Author = "Walter Isaacson", Description = "Einstein'ın hayatı ve bilimsel keşifleri.", Price = 72.50m, ImageUrl = "/images/kitaplar/einstein.jpg", Stock = 12, CategoryId = 7, IsActive = true, CreatedDate = DateTime.Now },

                // Sanat Kategorisi (Id: 8)
                new Kitap { Title = "Sanatın Öyküsü", Author = "E.H. Gombrich", Description = "Sanat tarihinin kapsamlı ve anlaşılır anlatımı.", Price = 125.00m, ImageUrl = "/images/kitaplar/sanatin-oykusu.jpg", Stock = 8, CategoryId = 8, IsActive = true, CreatedDate = DateTime.Now },

                // Teknoloji Kategorisi (Id: 9)
                new Kitap { Title = "Clean Code", Author = "Robert Martin", Description = "Temiz kod yazma teknikleri ve best practice'ler.", Price = 95.00m, ImageUrl = "/images/kitaplar/clean-code.jpg", Stock = 20, CategoryId = 9, IsActive = true, CreatedDate = DateTime.Now },
                new Kitap { Title = "Algoritma Giriş", Author = "Thomas Cormen", Description = "Algoritma ve veri yapıları konusunda kapsamlı kaynak.", Price = 180.00m, ImageUrl = "/images/kitaplar/algoritma-giris.jpg", Stock = 10, CategoryId = 9, IsActive = true, CreatedDate = DateTime.Now },

                // Sağlık Kategorisi (Id: 10)
                new Kitap { Title = "Beslenme Rehberi", Author = "Prof. Dr. Osman Müftüoğlu", Description = "Sağlıklı beslenme ve yaşam rehberi.", Price = 38.50m, ImageUrl = "/images/kitaplar/beslenme-rehberi.jpg", Stock = 25, CategoryId = 10, IsActive = true, CreatedDate = DateTime.Now }
            };

            await context.Kitaplar.AddRangeAsync(kitaplar);
            await context.SaveChangesAsync();
        }
    }
}
