# EmailConfirmationWithJWT-Identity


**EmailConfirmationWithJWT-Identity**, **Onion Architecture** kullanılarak tasarlanmış, **JWT (JSON Web Tokens)** ile ASP.NET Core Identity'yi entegre eden bir e-posta doğrulama projesidir. Bu proje, modern yazılım geliştirme prensiplerine uygun olarak katmanlı bir yapı sunar ve güvenli bir e-posta doğrulama mekanizması sağlar.

## 🚀 Özellikler

- **JWT ile E-Posta Doğrulama:** Kullanıcı kayıtlarının ardından e-posta adresinin doğrulanması için JWT tabanlı token oluşturma ve doğrulama.
- **Onion Architecture:** Temiz ve sürdürülebilir bir kod tabanı için katmanlı mimari (Core, Application, Infrastructure, Presentation).
- **ASP.NET Core Identity:** Kullanıcı yönetimi ve kimlik doğrulama için güçlü bir altyapı.
- **Güvenli Token İşlemleri:** JWT ile güvenli token üretimi ve yönetimi.
- **Kolay Genişletilebilirlik:** Yeni işlevselliklerin kolayca eklenmesini sağlayan esnek yapı.

---

## 🛠️ Proje Yapısı

Proje, **Onion Architecture**'ın temel prensiplerine göre aşağıdaki katmanlara ayrılmıştır:

1. **Core (Domain):**  
   - İş kuralları ve domain modelleri burada yer alır.
   - İş mantığına özel servisler ve arayüzler tanımlanır.

2. **Application:**  
   - Use Case'ler (iş akışları) ve uygulama mantığı burada işlenir.
   - DTO'lar ve servisler bu katmanda bulunur.

3. **Infrastructure:**  
   - Veri erişimi, e-posta servisi ve Identity gibi harici sistemlere bağlanan kodlar burada bulunur.
   - EF Core ile veri tabanı işlemleri yönetilir.

4. **Presentation:**  
   - Kullanıcıya sunulan API'ler ve uygulama arayüzleri bu katmanda yer alır.
   - Swagger desteği ile API dokümantasyonu.

---

## ⚙️ Kurulum

### 1. Depoyu Klonlayın
```bash
git clone https://github.com/username/EmailConfirmationWithJWT-Identity.git
cd EmailConfirmationWithJWT-Identity
