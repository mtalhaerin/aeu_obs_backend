Harika bir fikir. Proje dizinine ekleyebileceğin, hem kurulumu hem de versiyonlu (v1, v2, v3...) ilerlemeyi anlatan net bir `README.md` dosyasını aşağıda hazırladım.

Bu içeriği kopyalayıp projenin ana dizininde (solution dosyasının olduğu yer) **`DATABASE_README.md`** adıyla kaydedebilirsin.

````markdown
# 🗄️ Veritabanı ve Migrasyon Yönetimi (EF Core & MySQL)

Bu proje, veritabanı işlemleri için **Entity Framework Core** ve **MySQL** kullanmaktadır. Migrasyonlar `DataAccess` katmanında tutulur, ancak komutlar ana dizinden `WebAPI` projesi referans alınarak çalıştırılır.

## 🛠️ 1. Ön Hazırlık (Kurulum)

Projeyi ilk kez indirdiyseniz veya "command not found" hatası alıyorsanız, önce EF Core araçlarını geri yükleyin:

```bash
dotnet tool restore
````

> **Not:** Eğer araç yüklü değilse şu komutla projeye özel kurabilirsiniz:
> `dotnet tool install dotnet-ef`

-----

## 🚀 2. Yeni Migrasyon Oluşturma (Versiyonlu)

Bizim projemizde migrasyonlar versiyon klasörleri altında (v1, v2, v3...) tutulmaktadır. Her yeni veritabanı değişikliğinde `v` sayısını manuel olarak bir artırarak şu komutu kullanın.

**Komut Şablonu:**

```bash
dotnet ef migrations add <MigrasyonAdi> --project DataAccess --startup-project WebAPI --output-dir Migrations/<VersiyonKlasoru>
```

### Örnek Senaryolar:

**v1 (İlk Kurulum - Zaten yapıldıysa geçin):**

```bash
dotnet ef migrations add InitialCreate --project DataAccess --startup-project WebAPI --output-dir Migrations/v1
```

**v2 (Örneğin: Tabloya yeni kolon eklendi):**
Burada çıktı klasörünü `Migrations/v2` olarak değiştiriyoruz:

```bash
dotnet ef migrations add KullaniciTablosuGuncelleme --project DataAccess --startup-project WebAPI --output-dir Migrations/v2
```

**v3 (Örneğin: Yeni tablo eklendi):**

```bash
dotnet ef migrations add YeniBolumTablosu --project DataAccess --startup-project WebAPI --output-dir Migrations/v3
```

-----

## 💾 3. Migrasyonu Veritabanına Yansıtma (Update)

Oluşturulan migrasyon dosyalarını (C\# kodlarını) MySQL veritabanına uygulayıp tabloları oluşturmak/güncellemek için:

```bash
dotnet ef database update --project DataAccess --startup-project WebAPI
```

*Bu komut, henüz uygulanmamış tüm bekleyen migrasyonları (v1, v2, v3...) sırasıyla çalıştırır.*

-----

## ↩️ 4. Hatalı Migrasyonu Silme (Geri Alma)

Eğer `add` komutuyla bir migrasyon oluşturdunuz **ancak henüz `database update` yapmadınız** ve bir hata fark ettiyseniz, son oluşturulan migrasyonu silmek için:

```bash
dotnet ef migrations remove --project DataAccess --startup-project WebAPI
```

> **Dikkat:** Eğer `database update` yaptıysanız, önce veritabanını bir önceki versiyona döndürmeniz, sonra silmeniz gerekir.

-----

## ❓ Sıkça Sorulan Sorular

**Soru:** `dotnet ef` komutu çalışmıyor.
**Cevap:** Terminali kapatıp açın. Eğer hala çalışmıyorsa `dotnet tool restore` komutunu çalıştırın.

**Soru:** `Connection refused` hatası alıyorum.
**Cevap:** MySQL servisinin çalıştığından ve `appsettings.json` içerisindeki Connection String'in doğru olduğundan emin olun.

```

### Nasıl Kullanacaksın?

1.  Bu içeriği bir dosyaya kaydet.
2.  Bundan sonra her veritabanı değişikliği yaptığında (yeni tablo, yeni sütun vb.), **Bölüm 2**'deki komutu kullan.
3.  Tek yapman gereken `--output-dir Migrations/v2`, bir sonrakinde `v3`, `v4` diye elinle değiştirmek. EF Core bunu otomatik yapmaz, bu dosya sana bunu hatırlatmış olacak.
```