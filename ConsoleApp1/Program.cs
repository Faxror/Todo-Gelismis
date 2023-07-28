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

            Console.Write("Başarılı şekilde değiştirildi.");
            Console.WriteLine("Devam etmek için bir tuşa basın.");
            Console.ReadKey();
            ListMainMenu(kullaniciAdi);
        }
        else
        {
            Console.Write("Belirtilen başlıkla eşleşen bir todo bulunamadı.");
            Console.WriteLine("Devam etmek için bir tuşa basın.");
            Console.ReadKey();
            ListMainMenu(kullaniciAdi);
        }
    }
}
static async Task EditToTodo(string kullaniciAdi)
{
    Console.Write("Düzenlenecek Olan Başlığın İdsini Giriniz: ");
    string inputId = Console.ReadLine();

    if (int.TryParse(inputId, out int kimlik))
    {
        using (var dbContext = new DBContext())
        {
            Test todoToEdit = dbContext.Tessts.FirstOrDefault(t => t.Id == kimlik);

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
                Console.WriteLine("Belirtilen kimlik numarasına sahip todo bulunamadı.");
                Console.WriteLine("Devam etmek için bir tuşa basın.");
                Console.ReadKey();
                ListMainMenu(kullaniciAdi);

            }
        }
    }
    else
    {
        Console.WriteLine("Geçersiz kimlik numarası girişi! Lütfen bir tam sayı giriniz.");
        Console.WriteLine("Devam etmek için bir tuşa basın.");
        Console.ReadKey();
        ListMainMenu(kullaniciAdi);

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

        // Create separate lists for each status
        List<response> yapiliyorList = dbContext.Tessts
            .Where(t => t.RowStatus && t.AssignedPerson == user.Username && t.Status == "Yapılıyor")
            .Select(c => new response
            {
                Title = c.Title,
                Contents = c.Contents,
                Status = c.Status
            }).ToList();

        List<response> azKaldiList = dbContext.Tessts
            .Where(t => t.RowStatus && t.AssignedPerson == user.Username && t.Status == "Az Kaldı")
            .Select(c => new response
            {
                Title = c.Title,
                Contents = c.Contents,
                Status = c.Status
            }).ToList();

        List<response> bittiList = dbContext.Tessts
            .Where(t => t.RowStatus && t.AssignedPerson == user.Username && t.Status == "Bitti")
            .Select(c => new response
            {
                Title = c.Title,
                Contents = c.Contents,
                Status = c.Status
            }).ToList();

        if (yapiliyorList.Count == 0 && azKaldiList.Count == 0 && bittiList.Count == 0)
        {
            Console.WriteLine("Henüz veri yok");
        }
        else
        {
            int index = 1;

            // Print items with "Yapılıyor" status
            foreach (var item in yapiliyorList)
            {
                Console.WriteLine($"TODO YAPILIYOR Line");
                Console.WriteLine($"{index}-) Başlık: {item.Title}");
                Console.WriteLine($"   İçerik: {item.Contents}");
                Console.WriteLine($"   Durum: {item.Status}");
                index++;
            }

            // Print items with "Az Kaldı" status
            foreach (var item in azKaldiList)
            {
                Console.WriteLine($"TODO AZ KALDI Line");
                Console.WriteLine($"{index}-) Başlık: {item.Title}");
                Console.WriteLine($"   İçerik: {item.Contents}");
                Console.WriteLine($"   Durum: {item.Status}");
                index++;
            }

            // Print items with "Bitti" status
            foreach (var item in bittiList)
            {
                Console.WriteLine($"TODO BİTTİ Line");
                Console.WriteLine($"{index}-) Başlık: {item.Title}");
                Console.WriteLine($"   İçerik: {item.Contents}");
                Console.WriteLine($"   Durum: {item.Status}");
                index++;
            }
        }

        Console.WriteLine("Devam etmek için bir tuşa basın.");
        Console.ReadKey();
        ListMainMenu(kullaniciAdi);
    }


}

static async Task ListToTodoMain(string kullaniciAdi)
{
    Console.Write("Başlık Giriniz: ");
    string bk = Console.ReadLine();

    Console.Write("İçerik Giriniz: ");
    string ik = Console.ReadLine();

    Console.Write("Günlük(1)/Haftalık(2)/Aylık(3) Seçiniz: ");
    string input = Console.ReadLine();

    Console.Write("Yapılıyor(3)/Az Kaldı(4)/Bitti(5) Seçiniz: ");
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
static async Task ListMainMenu(string kullaniciAdi)
{
    Console.Clear();

    Console.WriteLine("Hoşgeldiniz, lütfen yapmak istediğiniz işlemi seçiniz.");
    Console.WriteLine("Liste Ekle (1)");
    Console.WriteLine("Liste Sil (2)");
    Console.WriteLine("Liste Düzenle (3)");
    Console.WriteLine("Listeyi Listeleme (4)");
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
