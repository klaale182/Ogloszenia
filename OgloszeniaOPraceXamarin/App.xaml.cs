using System;
using Xamarin.Forms;
using System.IO;
using OgloszeniaOPraceXamarin.Repos;
using OgloszeniaOPraceXamarin.Models;
using System.Diagnostics;
using OgloszeniaOPraceXamarin.Views;

namespace OgloszeniaOPraceXamarin {
    public partial class App : Application {
        public static string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "baza_111.db3");
        public static UserModel user=null;
        public App() {
            InitializeComponent();
            if(!File.Exists(dbPath)) 
                File.Create(dbPath);
            Setup();
            MainPage = new NavigationPage(new MainPage());
        }
        static async void Setup() {
            await CategoryRepo.Seed();
            await CompanyRepo.Seed();
            await ProfileRepo.Seed();
            await TypeOfWorkRepo.Seed();
            await UserRepo.Seed();
            await AnnouncementRepository.Seed();
            await ApplicationForAdvertisementRepo.getAsyncByUserID(0);
        }
    }
}
