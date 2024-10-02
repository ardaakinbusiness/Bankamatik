namespace bankamatik
{
    internal class Program
    {
        static void toMenu(double balance, string password, bool isCard)
        {
            while (true)
            {
                Console.WriteLine("Ana Menüye Dönmek için 9\nÇıkmak için 0 Tuşlayınız.");
                string choice1 = Console.ReadLine();
                switch (choice1)
                {
                    case "9":
                        mainMenu(balance, password, isCard);
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Lütfen Geçerli Bir Değer Giriniz!!");
                        break;
                }
            }
        }
        static double transferMethod(double balance)
        {

            int amount = 0;
            Console.WriteLine("Lütfen Göndermek İstediğiniz Tutarı Giriniz:");
            string s = Console.ReadLine();
            if (s.Length > 0)
            {
                amount = Convert.ToInt32(s);
            }
            else {
                Console.WriteLine("Geçersiz Bir Değer Girdiniz");
                return balance;
            }
            if (amount > 0 && balance >= amount)
            {
                balance -= amount;
                Console.WriteLine("Transfer İşlemi Başarılı.");
                Console.WriteLine("Kalan Bakiye:" + balance);

            }
            else if (amount > 0 && balance < amount)
            {
                Console.WriteLine("Bakiyeden Yüksek Değer Transfer Edilemez.");
            }
            return balance;
        }
        static double transfer(double balance, string isEFT)
        {
            string receiverID = " ";
            while (true)
            {
                if (isEFT == "1")
                {
                    Console.WriteLine("IBAN Giriniz:");
                }
                else
                {
                    Console.WriteLine("Hesap Numarası Giriniz:");
                }

                receiverID = Console.ReadLine().ToUpper();
                if (receiverID.StartsWith("TR") && receiverID.Length == 14 && isEFT == "1") // iban ile transfer
                {
                    balance = transferMethod((balance));
                    break;
                }
                else if (receiverID.Length != 14 && isEFT == "1")
                {
                    Console.WriteLine("Eksik yada Yanlış IBAN girdiniz!"); // iban numarası eksik yazıldığında verilecek hata mesajı
                }
                else if (receiverID.StartsWith("TR") == false && isEFT == "1")
                {
                    Console.WriteLine("IBAN \"TR\" ile Başlamalı");// iban tr ile başlamazsa verilecek hata mesajı
                }
                else if (isEFT == "2" && receiverID.Length == 11)
                {
                    balance = transferMethod((balance));
                    break;
                }
                else if (receiverID.Length != 11 && isEFT == "2")
                {
                    Console.WriteLine("Eksik yada Yanlış Hesap Numarası girdiniz!"); // hesap numarası eksik yazıldığında verilecek hata mesajı
                }
            }
            return balance;
        }
        static void login(double balance, string password, bool controller)
        {
            int attempt = 3;
            while (attempt > 0)
            {
                Console.WriteLine("Şifre Giriniz:");
                string passwordInput = Console.ReadLine();
                attempt--;
                if (passwordInput == password)
                {
                    Console.WriteLine("Giriş Başarılı!");
                    break;
                }
                else if (attempt == 0)
                {
                    Console.WriteLine("Giriş Hakkınız Kalmadı...");
                    if (controller) //şifre değiştirme ekranında hatalı giriş yapılırsa sistem kitlemek yerine anamenüye yönlendiriyor. ????
                    {
                        Console.WriteLine("3 Kez Hatalı Şifre Girdiniz!! \n Anamenüye Yönlendiriliyorsunuz.");
                        cardMenu(balance, password);
                    }
                    else
                    {
                        Console.WriteLine("Sistem kilitlendi");
                        Thread.Sleep(5000);
                        cardMenu(balance, password);
                    }
                }
                else
                {
                    Console.WriteLine("Giriş Başarısız!!");
                    Console.WriteLine("Tekrar Deneyiniz.");
                }
            }
        }
        static double withdraw(double balance, string password) // para çekimi
        {
            if (balance > 0)
            {

                Console.WriteLine("Çekilecek Tutarı Giriniz:");
                double withdrawAmount = 0;
                string s = Console.ReadLine();
                if (s.Length > 0)
                {
                    withdrawAmount = Convert.ToDouble(s);
                }
                else
                {
                    Console.WriteLine("Geçersiz Değer Girdiniz");
                    toMenu(balance, password, true);
                }
                if (withdrawAmount <= balance && withdrawAmount > 5)
                {
                    balance -= withdrawAmount;
                    Console.WriteLine("İşlem Başarılı\n Anamenüye Yönlendiriliyorsunuz.");

                    Console.WriteLine("Kalan Bakiye:" + balance);
                }
                else if (withdrawAmount <= balance && withdrawAmount < 5)
                    Console.WriteLine("En Az BEŞ TL Çekebilirsiniz!");
                else
                {
                    Console.WriteLine($"Bakiye Yetersiz!!\nBakiyeniz:{balance}");
                    toMenu(balance, password, true);
                }
            }
            else
            {
                Console.WriteLine("Bakiye Yetersiz!!");
                toMenu(balance, password, true);
            }
            return balance;
        }
        static void withdrawCepbank(double balance, string password)
        {
            string citizenID = "";
            string phoneNumber = "";
            for (int i = 3; i > 0; i--)
            {
                if (citizenID.Length != 11)
                {
                    Console.WriteLine("Lütfen T.C Kimlik Numaranızı Giriniz.");
                    citizenID = Console.ReadLine();
                }
                if (phoneNumber.Length != 10)
                {
                    Console.WriteLine("Lütfen Başında Sıfır Olmayacak Şekilde Telefon Numaranızı Giriniz.");
                    phoneNumber = Console.ReadLine();
                }
                if (phoneNumber.Length == 10 && citizenID.Length == 11)
                {
                    Console.WriteLine("Ödeme Başarılı");
                    toMenu(balance, password, false);
                }
                else if (phoneNumber.Length != 10 && citizenID.Length == 11)
                {
                    Console.WriteLine("Geçersiz Telefon Numarası!!");

                }
                else if (citizenID.Length != 11 && phoneNumber.Length == 10)
                {
                    Console.WriteLine("T.C Kimlik Numarası 11 Haneli Olmalıdır!");
                }
                else
                {
                    Console.WriteLine("Geçersiz T.C Kimlik ve Telefon Numarası!!");
                }
                if (i > 0)
                {
                    Console.WriteLine("Lütfen Tekrar Deneyiniz");
                }
                else
                {
                    Console.WriteLine("Çok Fazla Hatalı Giriş Yaptınız.");
                    toMenu(balance, password, false);
                }
            }
        }
        static void depositToOwnAccount(double balance, string password, bool isCard) // hesaba para yatırımı 
        {
            if (isCard == true)
            {
                double depositAmount = 0;
                Console.WriteLine("Yatırılacak Tutarı Giriniz:");
                string s=Console.ReadLine();
                if (s.Length>0)
                {
                    depositAmount = Convert.ToDouble(s);
                }
                else
                {
                    Console.WriteLine("Geçersiz Bir Değer Girdiniz");
                    toMenu(balance,password,isCard);
                }                
                if (depositAmount > 0)
                {
                    balance += depositAmount;
                    Console.WriteLine("Yeni Bakiyeniz:" + balance);
                    toMenu(balance, password, true);
                }
                else
                {
                    Console.WriteLine("Yatırılan Tutar Negatif Olamaz!");
                    toMenu(balance, password, true);
                }
            }
            else
            {
                string receiverID = "";
                while (true)
                {
                    Console.WriteLine("Hesap Numarası Giriniz:");
                    receiverID = Console.ReadLine();
                    if (receiverID.Length == 11)
                    {
                        Console.WriteLine("Yatırılacak Tutarı Giriniz:");
                        string depositAmount = Console.ReadLine();
                        if (depositAmount.Length>0&&Convert.ToInt32(depositAmount)>0)
                        Console.WriteLine($"Hesabınıza {depositAmount} TL Yatırılmıştır.");
                        else { Console.WriteLine("Geçersiz Bir Değer Girdiniz"); }
                        toMenu(balance, password, false);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Eksik yada Yanlış Hesap Numarası girdiniz!"); // hesap numarası eksik yazıldığında verilecek hata mesajı
                    }
                    toMenu(balance, password, false);
                }
            }
        }
        static void depositToCredit(double balance, string password, bool isCard) // karta para yatırımı 
        {
            Console.WriteLine("Kredi Kartı Numarası Giriniz.");
            string cardNumber = Console.ReadLine();
            if (cardNumber.Length == 12)
            {
                Console.WriteLine("Yatırılacak Tutarı Giriniz:");
                string amount = Console.ReadLine();
                if (amount.Length>0&&Convert.ToInt32(amount)>0)
                {
                    Console.WriteLine($"Kartınıza {amount} TL Yatırılmıştır.");
                }else
                {
                    Console.WriteLine("Geçersiz Bir Değer Girdiniz!!");
                }
                toMenu(balance, password, isCard);
            }
            else
            {
                Console.WriteLine("Kart Numarası 12 Haneli Olmalı!!");
                toMenu(balance, password, isCard);
            }
        }
        static void educationalPayments(double balance, string password, bool isCard) // eğitim ödemeleri ( ARIZALI)
        {
            Console.WriteLine("Sistemdeki Arıza Nedeniyle Gerçekleştirilemiyor!!");
            toMenu(balance, password, isCard);
        }
        static void payments(double balance, string password, bool isCard) // ödemeler 
        {
            Console.WriteLine("Elektrik Faturası İçin 1,\nTelefon Faturası İçin 2,\nİnternet Faturası İçin 3,\nSu Faturası İçin 4,\nOGS Ödemeleri İçin 5 Tuşlayınız.");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.Write("Elektrik");
                    break;
                case "2":
                    Console.Write("Telefon");
                    break;
                case "3":
                    Console.Write("İnternet");
                    break;
                case "4":
                    Console.Write("Su");
                    break;
                case "5":
                    Console.Write("OGS");
                    break;
                default:
                    Console.WriteLine("Lütfen Geçerli Bir Değer Giriniz!");
                    payments(balance, password, isCard);
                    break;
            }
            double bill = 0;
            Console.Write(" Faturası Tutarı Giriniz:\n");
            string s = Console.ReadLine();
            if (s.Length > 0)
            {
                bill = Convert.ToDouble(s);
                if (bill < 0) {
                    Console.WriteLine("Fatura Sıfırdan Düşük Olamaz!");
                }
            }
            else
            {
                Console.WriteLine("Geçersiz Bir Değer Girdiniz");
                toMenu(balance, password, isCard);
            }
            if (bill < balance && isCard)
            {
                balance -= bill;
                Console.WriteLine("Ödeme Başarılı\nYeni Bakiye:" + balance);
                toMenu(balance, password, isCard);
            }
            else if (isCard == false)
            {
                Console.WriteLine("Ödeme Başarılı");
                toMenu(balance, password, isCard);
            }
            else
            {
                Console.WriteLine("Bakiye Yetersiz!!");
                toMenu(balance, password, isCard);
            }
        }
        static string changePassword(double balance, string password)
        {
            login(balance, password, true);
            Console.WriteLine("Yeni Şifreyi Giriniz:");
            string tempPassword = Console.ReadLine();
            Console.WriteLine("Yeni Şifreyi Tekrar Giriniz:");
            string tempPassword2 = Console.ReadLine();
            if (tempPassword == tempPassword2)
            {
                if (tempPassword != password)
                {
                    Console.WriteLine("Şifreniz Değiştirildi.");
                    password = tempPassword;
                }
                else
                {
                    Console.WriteLine("Yeni Şifreniz Şuanki Şifrenizle Aynı Olamaz!!");
                }
            }
            else
            {
                Console.WriteLine("Girdiğiniz Şifreler Uyuşmuyor!!");
                toMenu(balance, password, true);
            }
            return password;
        }
        static void mainMenu(double balance, string password, bool isCard)
        {
            if (isCard == false)
            {
                Console.WriteLine(" CepBank Para Çekmek İçin 1,\n Para Yatırmak İçin 2,\n Eğitim Ödemeleri İçin 3,\n Ödemeler için 4 Tuşlayınız");
            }
            else
            {
                Console.WriteLine(" Para Çekmek İçin 1,\n Para Yatırmak İçin 2,\n Para Transferi İçin 3,\n Eğitim Ödemeleri İçin 4,\n Ödemeler için 5,\n Bilgi Güncellemek İçin 6 Tuşlayınız.");
            }
            int choice = 0;
            string s = Console.ReadLine();
            if (s.Length > 0)
            {
                choice = Convert.ToInt32(s);
            }
            if (choice > 2 && isCard == false)
            {
                choice++;
            }
            switch (choice)
            {
                case 1: //çekim
                    if (isCard)
                        balance = withdraw(balance, password);
                    else
                        withdrawCepbank(balance, password);
                    mainMenu(balance, password, isCard);
                    break;
                case 2: // yatırım
                    Console.WriteLine("Kredi Kartına Yatırmak İçin 1\nKendi Hesabınıza yatırmak için  2 Tuşlayınız.");
                    string value = Console.ReadLine();
                    switch (value)
                    {
                        case "1":
                            depositToCredit(balance, password, isCard);
                            break;
                        case "2":
                            depositToOwnAccount(balance, password, isCard);
                            break;
                        case "9":
                            mainMenu(balance, password, isCard);
                            break;
                        case "0":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Lütfen Geçerli Bir Değer Giriniz!");
                            break;
                    }
                    break;
                case 3: // transfer                    
                    Console.WriteLine("Başka Hesaba EFT için 1\nBaşka Hesaba Havale için 2 Tuşlayınız.");
                    string isEFT = Console.ReadLine();
                    if (isEFT == "1" || isEFT == "2")// 1 eft 2 hesap numarası için
                        balance = transfer(balance, isEFT);
                    else
                        Console.WriteLine("Lütfen Geçerli Bir Değer Giriniz.");
                    mainMenu(balance, password, isCard);
                    break;
                case 4: // eğitim
                    educationalPayments(balance, password, isCard);
                    mainMenu(balance, password, isCard);
                    break;
                case 5: //diğer ödemeler
                    payments(balance, password, isCard);
                    mainMenu(balance, password, isCard);
                    break;
                case 6: // şifre değiştirme
                    if (isCard == true)
                    {
                        password = changePassword(balance, password);
                        mainMenu(balance, password, isCard);

                    }
                    else
                    {
                        Console.WriteLine("Lütfen Geçerli Bir Değer Giriniz.");
                        mainMenu(balance, password, isCard);
                    }
                    break;
                default:
                    Console.WriteLine("Lütfen Geçerli Bir Değer Giriniz.");
                    mainMenu(balance, password, isCard);
                    break;
            }
        }
        static void cardMenu(double balance, string password)
        {
            Console.WriteLine("Kartlı İşlem için 1,");
            Console.WriteLine("Kartsız İşlem için 2 Tuşlayınız.");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":

                    login(balance, password, false);
                    mainMenu(balance, password, true);
                    break;
                case "2":
                    mainMenu(balance, password, false);
                    break;
                default:
                    Console.WriteLine("Lütfen Geçerli Bir Değer Giriniz.");
                    cardMenu(balance, password);
                    break;

            }
        }
        static void Main(string[] args)
        {
            double balance = 25000;
            string password = "ab18";
            cardMenu(balance, password);



        }
    }
}
