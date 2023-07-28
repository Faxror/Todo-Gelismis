using Business.Model;
using DataAcces;
using Entity;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

while (true)
{

    Console.WriteLine("Hoşgeldiniz, lütfen giriş yapınız.");
    Console.WriteLine("Giriş Yap (5)");
    Console.WriteLine("Kayıt Ol (6)");

    ConsoleKeyInfo keyInfo = Console.ReadKey();
    Users user = AccountService(keyInfo);
    bool basariliGiris = GirisYap(user.Username, user.Password);

    if (!basariliGiris)
    {
        
       
    }
    else
    {
        ListMainMenu(user.Username);
    }


}
static async Task AddToTodo(string kullaniciAdi)
{
    Console.Write("Başlık Giriniz: ");
    string bk = Console.ReadLine();

    Console.Write("İçerik Giriniz: ");
    string ik = Console.ReadLine();

    Console.Write("Günlük(1)/Haftalık(2)/Aylık(3) Seçiniz: ");
    string input = Console.ReadLine();

    Console.Write("Yapılıyor/Az Kaldı/Bitti Seçiniz: ");
    string inputtss = Console.ReadLine();

    if (int.TryParse(input, out int selectedCategoryId))
    {
        Category selectedCategory = (Category)selectedCategoryId;
        string categoryName = selectedCategory.ToString();

        using (var dbContext = new DBContext())
        {
            // Check if the person (kk) exists in the "users" database
            var exiduser = dbContext.Users.FirstOrDefault(u => u.Username == kullaniciAdi);

            if (exiduser == null)
            {
                Console.WriteLine("Kişi bulunamadı. Kayıt yapılmadı.");
            }
            else
            {
                Categorys existingCategory = dbContext.Categorys.FirstOrDefault(c => c.Id == selectedCategoryId);

                if (existingCategory == null)
                {
                    Console.WriteLine("Geçersiz kategori seçimi!");
                }
                else
                {
                    Test newTodo = new Test
                    {
                        Title = bk,
                        Contents = ik,
                        AssignedPerson = kullaniciAdi,
                        Montly = categoryName,
                        RowStatus = true,
                        Status = inputtss

                    };

                    dbContext.Tessts.Add(newTodo);
                    dbContext.SaveChangesAsync();

                    Console.WriteLine("Başarılı şekilde kaydedildi.");

                    Console.WriteLine("Devam etmek için bir tuşa basın.");
                    Console.ReadKey();
                    ListMainMenu(kullaniciAdi);
                }
            }
        }
    }
    else
    {
        Console.WriteLine("Geçersiz kategori seçimi! Sadece 1, 2 veya 3 giriniz.");
        Console.WriteLine("Devam etmek için bir tuşa basın.");
        Console.ReadKey();
        ListMainMenu(kullaniciAdi);
    }
}
static async Task DeleteToTodo(string kullaniciAdi)
{
    Console.Write("Lütfen Silinicek Olan Başlığın Adını Giriniz: ");
    string titleInput = Console.ReadLine();

    using (var dbContext = new DBContext())
    {
   
        Test todoToDelete = dbContext.Tessts.FirstOrDefault(t => t.Title == titleInput);

        if (todoToDelete != null)
        {
            
            todoToDelete.RowStatus = false;

            
            dbContext.SaveChanges();

            Console.WriteLine("Başarılı şekilde değiştirildi.");
            Console.Write("Devam etmek için bir tuşa basın.");
            Console.ReadKey();
            ListMainMenu(kullaniciAdi);
        }
        else
        {
            Console.WriteLine("Belirtilen başlıkla eşleşen bir todo bulunamadı.");
            Console.Write("Devam etmek için bir tuşa basın.");
            Console.ReadKey();
            ListMainMenu(kullaniciAdi);
        }
    }
}
static async Task EditToTodo(string kullaniciAdi)
{
    using (var dbContext = new DBContext())
    {
        Console.Write("Düzenlenecek Olan Başlığın Adını Giriniz: ");
        string inputTitle = Console.ReadLine();

        Test todoToEdit = dbContext.Tessts.FirstOrDefault(t => t.Title == inputTitle);

        if (todoToEdit != null)
        {
            Console.Write("Düzenlenecek Olan Başlığın Adını Giriniz: ");
            string bk = Console.ReadLine();
            todoToEdit.Title = bk;

            Console.Write("Düzenlenecek Olan İçeriğin Adını Giriniz: ");
            todoToEdit.Contents = Console.ReadLine();

            Console.Write("Düzenlenecek Olan Günlük/Haftalık/Aylık Seçiniz: ");
            string inputStatus = Console.ReadLine();
            todoToEdit.Montly = inputStatus;

            Console.Write("Düzenlenecek Olan Yapılıyor/Az kaldı /Bitti Seçiniz: ");
            string Status = Console.ReadLine();
            todoToEdit.Status = Status;

            todoToEdit.modifieddate = DateTime.Now;

            // Check if the person (assignedPerson) exists in the "users" database
            var exiduser = dbContext.Users.FirstOrDefault(u => u.Username == kullaniciAdi);
            todoToEdit.AssignedPerson = kullaniciAdi;

            if (exiduser == null)
            {
                Console.WriteLine("Kişi bulunamadı. Değişiklikler kaydedilmedi.");
            }
            else
            {
                dbContext.SaveChangesAsync();
                Console.WriteLine("Başarılı şekilde düzenlendi.");
                Console.WriteLine("Devam etmek için bir tuşa basın.");
                Console.ReadKey();
                ListMainMenu(kullaniciAdi);
            }
        }
        else
        {
            Console.WriteLine("Belirtilen başlığa sahip todo bulunamadı.");
            Console.WriteLine("Devam etmek için bir tuşa basın.");
            Console.ReadKey();
            ListMainMenu(kullaniciAdi);
        }
    }

}
static async Task ListToTodo(string kullaniciAdi)
{
    using (DBContext dbContext = new DBContext())
    {
        var user = dbContext.Users.FirstOrDefault(z => z.Username == kullaniciAdi);

        if (user == null)
        {
            Console.WriteLine("Kullanıcı bulunamadı!");
            return;
        }

        List<response> tumListe = dbContext.Tessts
            .Where(t => t.RowStatus && t.AssignedPerson == user.Username)
            .Select(c => new response
            {
                Title = c.Title,
                Contents = c.Contents,
                Status = c.Status
            }).ToList();

        if (tumListe.Count == 0)
        {
            Console.WriteLine("Henüz veri yok");
        }
        else
        {
            int index = 1;

            // Durumlara göre gruplama yapılıyor
            var gruplanmisListe = tumListe.GroupBy(item => item.Status);

            foreach (var grup in gruplanmisListe)
            {
                switch (grup.Key)
                {
                    case "Yapılıyor":
                        Console.WriteLine("TODO YAPILIYOR Line");
                        break;
                    case "Az Kaldı":
                        Console.WriteLine("TODO AZ KALDI Line");
                        break;
                    case "Bitti":
                        Console.WriteLine("TODO BİTTİ Line");
                        break;
                }

                foreach (var item in grup)
                {
                    // Aynı başlık altında tüm öğeleri gösteriyoruz
                    Console.WriteLine($"{index}-) Başlık: {item.Title}");
                    Console.WriteLine($"   İçerik: {item.Contents}");
                    Console.WriteLine($"   Durum: {item.Status}");
                    index++;
                }
            }
        }

        Console.WriteLine("Devam etmek için bir tuşa basın.");
        Console.ReadKey();
        ListMainMenu(kullaniciAdi);
    }




}
static async Task ListMainMenu(string kullaniciAdi)
{
    Console.Clear();

    Console.WriteLine("Hoşgeldiniz, lütfen yapmak istediğiniz işlemi seçiniz.");
    Console.WriteLine("Todo Ekle (1)");
    Console.WriteLine("Todo Sil (2)");
    Console.WriteLine("Todo Düzenle (3)");
    Console.WriteLine("Todo Listele (4)");
    Console.WriteLine("Admin İsmi Düzenleme (7)");
    Console.WriteLine("Admin Hesap Silme (8)");


    ConsoleKeyInfo keyInfo = Console.ReadKey();
    ProcessUserChoice(keyInfo, kullaniciAdi);

}
static Users Register()
{
    Console.Write("Register Name: ");
    string kullaniciAdi = Console.ReadLine();
    Console.Write("Register Password: ");
    string sifre = Console.ReadLine();


    string sifrehash = ComputeSHA256Hash(sifre);

    using (var dbContext = new DBContext())
    {
        Users newTodo = new Users
        {
            Username = kullaniciAdi,
            Password = sifrehash
        };

        dbContext.Users.Add(newTodo);
        dbContext.SaveChanges();

        Console.WriteLine("Register is confired!");
        return newTodo;
    }
}
static Users Login()
{
    Console.Write("Login Name: ");
    string kullaniciAdi = Console.ReadLine();
    Console.Write("Login Password: ");
    string sifre = Console.ReadLine();

    return new Users { Username = kullaniciAdi, Password = sifre };
}
static string ComputeSHA256Hash(string input)
{
    using (SHA256 sha256 = SHA256.Create())
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = sha256.ComputeHash(inputBytes);

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            builder.Append(hashBytes[i].ToString("x2"));
        }
        return builder.ToString();
    }
}
static async Task ChangeToAdminUsernameAndPassword(string kullaniciAdi)
{


    using (var dbContext = new DBContext())
    {

        var userToEdit = dbContext.Users.FirstOrDefault(u => u.Username == kullaniciAdi);

        if (userToEdit != null)
        {
            Console.Write("Yeni Admin İsmini Giriniz: ");
            string newUsername = Console.ReadLine();
            userToEdit.Username = newUsername;

            Console.Write("Yeni Admin Şifrenizi Giriniz: ");
            string newPassword = Console.ReadLine();
            userToEdit.Password = newPassword;

            dbContext.SaveChangesAsync();
            Console.Write("Başarılı Şekilde Değiştirildi. ");
            Console.WriteLine("Devam etmek için bir tuşa basın.");
            Console.ReadKey();
            ListMainMenu(kullaniciAdi);

        }
    }

}
static async Task DeleteToAdminAccount(string kullaniciAdi)
{
    Console.Write("Lütfen Silinicek Olan Hesabın Adını Giriniz: ");
    string bk = Console.ReadLine();



    using (var dbContext = new DBContext())
    {
        Users AdminToDelete = dbContext.Users.FirstOrDefault(t => t.Username == bk);

        if (AdminToDelete != null)
        {
            dbContext.Users.Remove(AdminToDelete);
            dbContext.SaveChangesAsync();

            Console.Write("Başarılı şekilde Silindi.");
            Console.WriteLine("Devam etmek için bir tuşa basın.");
            Console.ReadKey();
            ListMainMenu(kullaniciAdi);
        }
        else
        {
            Console.Write("Elimiz de yok, elimiz de yok, eee elinize vereyim o zaman? - Burak Oyunda.");
        }
    }
}
static bool GirisYap(string kullaniciAdi, string sifre)
{

    using (var dbContext = new DBContext())
    {

        string sifrehash = ComputeSHA256Hash(sifre);

        bool kullaniciVarMi = dbContext.Users.Any(u => u.Username == kullaniciAdi && u.Password == sifrehash);

        if (kullaniciVarMi)
        {
            Console.WriteLine("Başarılı şekilde giriş yapıldı!");
            return true;
        }
        else
        {
            Console.WriteLine("Kullanıcı adı veya şifre hatalı!");
            return false;
            Console.Clear();
        }
    }
}
static void ProcessUserChoice(ConsoleKeyInfo keyInfo, string kullaniciAdi)
{
    switch (keyInfo.Key)
    {
        case ConsoleKey.D1:
            Console.Clear();
            AddToTodo(kullaniciAdi);
            break;
        case ConsoleKey.D2:
            Console.Clear();
            DeleteToTodo(kullaniciAdi);
            break;
        case ConsoleKey.D3:
            Console.Clear();
            EditToTodo(kullaniciAdi);
            break;
        case ConsoleKey.D4:
            Console.Clear();
            ListToTodo(kullaniciAdi);
            break;
        case ConsoleKey.D7:
            Console.Clear();
            ChangeToAdminUsernameAndPassword(kullaniciAdi);
            break;
        case ConsoleKey.D8:
            Console.Clear();
            DeleteToAdminAccount(kullaniciAdi);
            break;
        default:
            Console.WriteLine("Geçersiz seçim!");
            break;
    }
}
Users AccountService(ConsoleKeyInfo keyInfo)
{
    switch (keyInfo.Key)
    {
        case ConsoleKey.D5:
            Console.Clear();
            return Login();
        case ConsoleKey.D6:
            Console.Clear();
            return Register();
        default:
            return null;
    }
}
