    -- Veritabanını oluştur
    USE master;
    GO

    IF EXISTS (SELECT name FROM sys.databases WHERE name = 'KitapMagazasiDB')
    BEGIN
        ALTER DATABASE KitapMagazasiDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
        DROP DATABASE KitapMagazasiDB;
    END
    GO

    CREATE DATABASE KitapMagazasiDB;
    GO

    USE KitapMagazasiDB;
    GO
    -- Categories Tablosu
    CREATE TABLE Categories (
        Id int IDENTITY(1,1) PRIMARY KEY,
        Name nvarchar(100) NOT NULL,
        Description nvarchar(500),
        CreatedDate datetime2 NOT NULL DEFAULT GETDATE()
    );
    -- Users Tablosu
    CREATE TABLE Users (
        Id int IDENTITY(1,1) PRIMARY KEY,
        FirstName nvarchar(50) NOT NULL,
        LastName nvarchar(50) NOT NULL,
        Email nvarchar(100) NOT NULL UNIQUE,
        Password nvarchar(255) NOT NULL,
        Role nvarchar(20) NOT NULL DEFAULT 'User',
        Phone nvarchar(15),
        Address nvarchar(500),
        CreatedDate datetime2 NOT NULL DEFAULT GETDATE()
    );
    -- Kitaplar Tablosu
    CREATE TABLE Kitaplar (
        Id int IDENTITY(1,1) PRIMARY KEY,
        Title nvarchar(200) NOT NULL,
        Author nvarchar(100) NOT NULL,
        Description nvarchar(1000),
        Price decimal(10,2) NOT NULL,
        ImageUrl nvarchar(500),
        Stock int NOT NULL DEFAULT 0,
        CategoryId int NOT NULL,
        IsActive bit NOT NULL DEFAULT 1,
        CreatedDate datetime2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Kitaplar_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
    );
    -- Orders Tablosu
    CREATE TABLE Orders (
        Id int IDENTITY(1,1) PRIMARY KEY,
        UserId int NOT NULL,
        OrderDate datetime2 NOT NULL DEFAULT GETDATE(),
        TotalAmount decimal(10,2) NOT NULL,
        Status nvarchar(20) NOT NULL DEFAULT 'Pending',
        ShippingAddress nvarchar(500) NOT NULL,
        CONSTRAINT FK_Orders_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
    -- OrderItems Tablosu
    CREATE TABLE OrderItems (
        Id int IDENTITY(1,1) PRIMARY KEY,
        OrderId int NOT NULL,
        KitapId int NOT NULL,
        Quantity int NOT NULL,
        UnitPrice decimal(10,2) NOT NULL,
        CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id),
        CONSTRAINT FK_OrderItems_Kitaplar FOREIGN KEY (KitapId) REFERENCES Kitaplar(Id)
    );
    -- Favorites Tablosu
    CREATE TABLE Favorites (
        Id int IDENTITY(1,1) PRIMARY KEY,
        UserId int NOT NULL,
        KitapId int NOT NULL,
        CreatedDate datetime2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Favorites_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
        CONSTRAINT FK_Favorites_Kitaplar FOREIGN KEY (KitapId) REFERENCES Kitaplar(Id),
        CONSTRAINT UK_Favorites_User_Kitap UNIQUE (UserId, KitapId)
    );
    INSERT INTO Categories (Name, Description) VALUES
    ('Roman', 'Edebiyat ve roman kitapları'),
    ('Bilim Kurgu', 'Bilim kurgu ve fantastik kitaplar'),
    ('Tarih', 'Tarih ve sosyal bilimler'),
    ('Çocuk Kitapları', 'Çocuklar için eğitici ve eğlenceli kitaplar'),
    ('Psikoloji', 'Psikoloji ve kişisel gelişim'),
    ('Felsefe', 'Felsefe ve düşünce kitapları'),
    ('Biyografi', 'Ünlü kişilerin hayat hikayeleri'),
    ('Sanat', 'Sanat ve kültür kitapları'),
    ('Teknoloji', 'Bilgisayar ve teknoloji'),
    ('Sağlık', 'Sağlık ve yaşam');
    INSERT INTO Users (FirstName, LastName, Email, Password, Role, Phone, Address) VALUES
    ('Admin', 'User', 'admin@kitapmağazası.com', 'admin123', 'Admin', '0212 123 45 67', 'İstanbul, Türkiye'),
    ('Ahmet', 'Yılmaz', 'ahmet@email.com', 'ahmet123', 'User', '0532 111 22 33', 'Ankara, Çankaya'),
    ('Ayşe', 'Demir', 'ayse@email.com', 'ayse123', 'User', '0533 444 55 66', 'İzmir, Konak'),
    ('Mehmet', 'Kaya', 'mehmet@email.com', 'mehmet123', 'User', '0534 777 88 99', 'Bursa, Nilüfer');
    INSERT INTO Kitaplar (Title, Author, Description, Price, ImageUrl, Stock, CategoryId, IsActive) VALUES
    ('Suç ve Ceza', 'Fyodor Dostoyevski', 'Rus edebiyatının başyapıtlarından biri olan bu klasik, insan psikolojisinin derinliklerini ele alır.', 45.50, '/images/kitaplar/suc-ve-ceza.jpg', 25, 1, 1),
    ('Beyaz Zambaklar Ülkesinde', 'Grigory Petrov', 'Finlandiya''nın eğitim sistemini ve toplumsal yapısını anlatan etkileyici bir eser.', 32.00, '/images/kitaplar/beyaz-zambaklar.jpg', 18, 1, 1),
    ('Kürk Mantolu Madonna', 'Sabahattin Ali', 'Türk edebiyatının en önemli aşk romanlarından biri.', 28.75, '/images/kitaplar/kurk-mantolu-madonna.jpg', 30, 1, 1),
    ('Çalıkuşu', 'Reşat Nuri Güntekin', 'Feride''nin hayat öyküsünü anlatan ölümsüz klasik.', 25.00, '/images/kitaplar/calikusu.jpg', 22, 1, 1),
    ('Dune', 'Frank Herbert', 'Bilim kurgu edebiyatının başyapıtı. Arrakis gezegeni ve baharat savaşları.', 55.00, '/images/kitaplar/dune.jpg', 15, 2, 1),
    ('1984', 'George Orwell', 'Distopik bir gelecek tasviri. Büyük Birader''in gözetlediği dünya.', 38.50, '/images/kitaplar/1984.jpg', 40, 2, 1),
    ('Yüzüklerin Efendisi', 'J.R.R. Tolkien', 'Fantastik edebiyatın klasiklerinden. Orta Dünya macerası.', 125.00, '/images/kitaplar/yuzuklerin-efendisi.jpg', 12, 2, 1),
    ('Fahrenheit 451', 'Ray Bradbury', 'Kitapların yasaklandığı bir dünyada geçen distopik roman.', 42.00, '/images/kitaplar/fahrenheit-451.jpg', 20, 2, 1),
    ('Nutuk', 'Mustafa Kemal Atatürk', 'Kurtuluş Savaşı ve Cumhuriyet''in kuruluş tarihini anlatan tarihî belge.', 35.00, '/images/kitaplar/nutuk.jpg', 35, 3, 1),
    ('Devlet-i Aliyye', 'Halil İnalcık', 'Osmanlı İmparatorluğu''nun kapsamlı tarihî analizi.', 89.50, '/images/kitaplar/devleti-aliyye.jpg', 10, 3, 1),
    ('Sapiens', 'Yuval Noah Harari', 'İnsan türünün tarihsel gelişimi ve geleceği hakkında çok satan kitap.', 58.00, '/images/kitaplar/sapiens.jpg', 28, 3, 1),
    ('Küçük Prens', 'Antoine de Saint-Exupéry', 'Çocuklar ve yetişkinler için yazılmış ölümsüz bir klasik.', 22.50, '/images/kitaplar/kucuk-prens.jpg', 45, 4, 1),
    ('Pinokyo', 'Carlo Collodi', 'Ahşap kukladan gerçek çocuğa dönüşen Pinokyo''nun macerası.', 18.00, '/images/kitaplar/pinokyo.jpg', 38, 4, 1),
    ('Alice Harikalar Diyarında', 'Lewis Carroll', 'Alice''in sihirli dünyada yaşadığı olağanüstü maceralar.', 24.00, '/images/kitaplar/alice.jpg', 32, 4, 1),
    ('İnsan Arayışı', 'Viktor Frankl', 'Logoterapinin kurucusundan anlam arayışı ve hayatın anlamı.', 42.00, '/images/kitaplar/insan-arayisi.jpg', 25, 5, 1),
    ('Düşünme Sanatı', 'Rolf Dobelli', 'Karar verme süreçleri ve düşünme hataları hakkında pratik rehber.', 36.50, '/images/kitaplar/dusunme-sanati.jpg', 22, 5, 1),
    ('Beden Dili', 'Julius Fast', 'İletişimde beden dilinin önemi ve anlamları.', 28.00, '/images/kitaplar/beden-dili.jpg', 30, 5, 1),
    ('Simyacı', 'Paulo Coelho', 'Hayallerinin peşinden giden bir çobanın hikayesi.', 32.00, '/images/kitaplar/simyaci.jpg', 40, 6, 1),
    ('Felsefeye Giriş', 'Bertrand Russell', 'Felsefenin temel konularına giriş niteliğinde kapsamlı eser.', 45.00, '/images/kitaplar/felsefeye-giris.jpg', 15, 6, 1),
    ('Steve Jobs', 'Walter Isaacson', 'Apple kurucusunun resmi biyografisi.', 65.00, '/images/kitaplar/steve-jobs.jpg', 18, 7, 1),
    ('Einstein: Hayatı ve Evreni', 'Walter Isaacson', 'Einstein''ın hayatı ve bilimsel keşifleri.', 72.50, '/images/kitaplar/einstein.jpg', 12, 7, 1),
    ('Sanatın Öyküsü', 'E.H. Gombrich', 'Sanat tarihinin kapsamlı ve anlaşılır anlatımı.', 125.00, '/images/kitaplar/sanatin-oykusu.jpg', 8, 8, 1),
    ('Clean Code', 'Robert Martin', 'Temiz kod yazma teknikleri ve best practice''ler.', 95.00, '/images/kitaplar/clean-code.jpg', 20, 9, 1),
    ('Algoritma Giriş', 'Thomas Cormen', 'Algoritma ve veri yapıları konusunda kapsamlı kaynak.', 180.00, '/images/kitaplar/algoritma-giris.jpg', 10, 9, 1),
    ('Beslenme Rehberi', 'Prof. Dr. Osman Müftüoğlu', 'Sağlıklı beslenme ve yaşam rehberi.', 38.50, '/images/kitaplar/beslenme-rehberi.jpg', 25, 10, 1);
    INSERT INTO Orders (UserId, TotalAmount, Status, ShippingAddress) VALUES
    (2, 167.50, 'Completed', 'Ankara, Çankaya, Kızılay Mahallesi, Atatürk Bulvarı No:15'),
    (3, 95.00, 'Shipped', 'İzmir, Konak, Alsancak Mahallesi, Cumhuriyet Bulvarı No:42'),
    (4, 234.75, 'Processing', 'Bursa, Nilüfer, Görükle Mahallesi, Uludağ Üniversitesi Kampüsü');
    INSERT INTO OrderItems (OrderId, KitapId, Quantity, UnitPrice) VALUES
    (1, 1, 2, 45.50), 
    (1, 3, 1, 28.75), 
    (1, 11, 2, 22.50), 
    (2, 19, 1, 95.00), 
    (3, 5, 1, 55.00),
    (3, 9, 2, 89.50), 
    (3, 15, 1, 42.00);
    INSERT INTO Favorites (UserId, KitapId) VALUES
    (2, 1), (2, 3), (2, 6), (2, 11),
    (3, 5), (3, 7), (3, 19),
    (4, 2), (4, 9), (4, 10), (4, 13);
    CREATE INDEX IX_Kitaplar_CategoryId ON Kitaplar(CategoryId);
    CREATE INDEX IX_Kitaplar_Price ON Kitaplar(Price);
    CREATE INDEX IX_Kitaplar_IsActive ON Kitaplar(IsActive);
    CREATE INDEX IX_Orders_UserId ON Orders(UserId);
    CREATE INDEX IX_Orders_OrderDate ON Orders(OrderDate);
    CREATE INDEX IX_OrderItems_OrderId ON OrderItems(OrderId);
    CREATE INDEX IX_OrderItems_KitapId ON OrderItems(KitapId);
    CREATE INDEX IX_Favorites_UserId ON Favorites(UserId);
    CREATE INDEX IX_Favorites_KitapId ON Favorites(KitapId);
    SELECT 'Categories' as TableName, COUNT(*) as RecordCount FROM Categories
    UNION ALL
    SELECT 'Users', COUNT(*) FROM Users
    UNION ALL
    SELECT 'Kitaplar', COUNT(*) FROM Kitaplar
    UNION ALL
    SELECT 'Orders', COUNT(*) FROM Orders
    UNION ALL
    SELECT 'OrderItems', COUNT(*) FROM OrderItems
    UNION ALL
    SELECT 'Favorites', COUNT(*) FROM Favorites;
    SELECT 
        c.Name as CategoryName,
        COUNT(k.Id) as BookCount,
        AVG(k.Price) as AvgPrice,
        SUM(k.Stock) as TotalStock
    FROM Categories c
    LEFT JOIN Kitaplar k ON c.Id = k.CategoryId AND k.IsActive = 1
    GROUP BY c.Id, c.Name
    ORDER BY BookCount DESC;
    SELECT 
        k.Title,
        k.Author,
        k.Price,
        SUM(oi.Quantity) as TotalSold
    FROM Kitaplar k
    INNER JOIN OrderItems oi ON k.Id = oi.KitapId
    GROUP BY k.Id, k.Title, k.Author, k.Price
    ORDER BY TotalSold DESC;

