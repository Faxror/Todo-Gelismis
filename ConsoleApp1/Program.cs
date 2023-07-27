using Business.Model;
using DataAcces;
using Entity;

while (true)
{

    Console.WriteLine("Hoşgeldiniz, lütfen giriş yapınız.");
    Console.WriteLine("Giriş Yap (5)");
    Console.WriteLine("Kayıt Ol (6)");

    ConsoleKeyInfo keyInfo = Console.ReadKey();
    string userName = AccountService(keyInfo);
    ListMainMenu(userName);
    ProcessUserChoice(keyInfo, userName);

}
    
static async Task AddTodoItem(string kullaniciAdi)
{
    Console.Write("Başlık Giriniz: ");
    string bk = Console.ReadLine();

    Console.Write("İçerik Giriniz: ");
    string ik = Console.ReadLine();

    Console.Write("Büyüklük Seçiniz -> XS(1),S(2),M(3),L(4),XL(5) Giriniz: ");
    string buk = Console.ReadLine();

    Console.Write("Kişi Seçiniz: ");
    string kk = Console.ReadLine();

    Console.Write("Günlük(1)/Haftalık(2)/Aylık(3) Seçiniz: ");
    string input = Console.ReadLine();

    if (int.TryParse(input, out int selectedCategoryId))
    {
        Category selectedCategory = (Category)selectedCategoryId;
        string categoryName = selectedCategory.ToString();

        using (var dbContext = new DBContext())
        {
            // Check if the person (kk) exists in the "users" database
            var exiduser = dbContext.Users.FirstOrDefault(u => u.Username == kk);

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
                        Size = buk,
                        AssignedPerson = kk,
                        Montly = categoryName,
                        RowStatus = false
                    };

                    dbContext.Tessts.Add(newTodo);
                    dbContext.SaveChangesAsync();

                    Console.WriteLine("Başarılı şekilde kaydedildi.");
                }
            }
        }
    }
    else
    {
        Console.WriteLine("Geçersiz kategori seçimi! Sadece 1, 2 veya 3 giriniz.");
    }
}

static async Task DeleteTodoItem()
{
    Console.Write("Lütfen Silinicek Olan Başlığın İd Giriniz: ");
    string bk = Console.ReadLine();

    using (var dbContext = new DBContext())
    {
        Test todoToDelete = dbContext.Tessts.FirstOrDefault(t => t.Title == bk);

        if (todoToDelete != null)
        {
            dbContext.Tessts.Remove(todoToDelete);
            dbContext.SaveChangesAsync();

            Console.Write("Başarılı şekilde Silindi.");
        }
        else
        {
            Console.Write("Belirtilen başlıkla eşleşen bir todo bulunamadı.");
        }
    }
}

static async Task EditTodoItem(string kullaniciAdi)
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

                Console.Write("Düzenlenecek Olan Büyüklüğü Seçiniz -> XS(1),S(2),M(3),L(4),XL(5) Giriniz: ");
                string buk = Console.ReadLine();
                todoToEdit.Size = buk;

                Console.Write("Düzenlenecek Olan Kişiyi Seçiniz: ");
                string assignedPerson = Console.ReadLine();
                todoToEdit.AssignedPerson = assignedPerson;

                Console.Write("Düzenlenecek Olan Günlük/Haftalık/Aylık Seçiniz: ");
                string inputStatus = Console.ReadLine();
                todoToEdit.Montly = inputStatus;

                todoToEdit.modifieddate = DateTime.Now;

                // Check if the person (assignedPerson) exists in the "users" database
                var exiduser = dbContext.Users.FirstOrDefault(u => u.Username == assignedPerson);

                if (exiduser == null)
                {
                    Console.WriteLine("Kişi bulunamadı. Değişiklikler kaydedilmedi.");
                }
                else
                {
                    dbContext.SaveChangesAsync();
                    Console.WriteLine("Başarılı şekilde düzenlendi.");
                }
            }
            else
            {
                Console.WriteLine("Belirtilen kimlik numarasına sahip todo bulunamadı.");
            }
        }
    }
    else
    {
        Console.WriteLine("Geçersiz kimlik numarası girişi! Lütfen bir tam sayı giriniz.");
    }
}

static async Task ListTodoItems(string kullaniciAdi)
{
    using (DBContext dbContext = new DBContext())
    {
        var user = dbContext.Users.FirstOrDefault(z => z.Username == kullaniciAdi);

        if (user == null)
        {
            Console.WriteLine("Kullanıcı bulunamadı!");
            return;
        }

        List<response>? af = dbContext.Tessts.Where(t => t.RowStatus & t.AssignedPerson == user.Username).Select(c => new response        
        {
            Title = c.Title,
        }).ToList();

        if (af.Count() == 0)
            Console.WriteLine("Henüz veri yok");

        for (int i = 0; i < af.Count(); i++)
            Console.WriteLine($"{i}-) {af[i].Title}");

        Console.ReadLine();
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

    ProcessUserChoice(Console.ReadKey(), kullaniciAdi);
}

static string Register()
{
    Console.Write("Kullanıcı Adı : ");
    string KA = Console.ReadLine();
    Console.Write("Şifre : ");
    string Sifre = Console.ReadLine();

    using (var dbContext = new DBContext())
    {
        Users newTodo = new Users
        {
            Username = KA,
            Password = Sifre

        };

        dbContext.Users.Add(newTodo);
        dbContext.SaveChanges();
    }

    return KA;
}

static string Login()
{
    Console.Write("Kullanıcı Adı: ");
    string KA = Console.ReadLine();
    Console.Write("Şifre: ");
    string Sifre = Console.ReadLine();

    bool basariliGiris = GirisYap(KA, Sifre);

    if (basariliGiris)
    {
        Console.Clear();
        Console.WriteLine("Başarılı şekilde giriş yapıldı!");
        return KA;
    }
    else
    {
        return "Kullanıcı adı veya şifre hatalı!";

    }

}

static bool GirisYap(string kullaniciAdi, string sifre)
{
    using (var dbContext = new DBContext())
    {
        // Veritabanında kullanıcının bilgilerini kontrol ediyoruz.
        bool kullaniciVarMi = dbContext.Users.Any(u => u.Username == kullaniciAdi && u.Password == sifre);

        return kullaniciVarMi;
    }
}
static void ProcessUserChoice(ConsoleKeyInfo keyInfo, string kullaniciAdi)
{
    switch (keyInfo.Key)
    {
        case ConsoleKey.D1:
            Console.Clear();
            AddTodoItem(kullaniciAdi);
            break;
        case ConsoleKey.D2:
            Console.Clear();
            DeleteTodoItem();
            break;
        case ConsoleKey.D3:
            Console.Clear();
            EditTodoItem(kullaniciAdi);
            break;
        case ConsoleKey.D4:
            Console.Clear();
            ListTodoItems(kullaniciAdi);
            break;
        default:
            Console.WriteLine("Geçersiz seçim!");
            break;
    }
}

string AccountService(ConsoleKeyInfo keyInfo)
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
            return "Hata";
    }
}
