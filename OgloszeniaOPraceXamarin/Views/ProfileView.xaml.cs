using OgloszeniaOPraceXamarin.Models;
using OgloszeniaOPraceXamarin.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OgloszeniaOPraceXamarin.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileView : ContentPage {
        public ProfileView() {
            InitializeComponent();
            Setup();
            if (App.user != null && App.user.Company != null) {
                CompanyForm.IsVisible = false;
            }
            if(App.user!=null&&App.user.Company == null) {
                Applications.IsVisible = false;
            }
            Title = $"Profil uzytkownika: {App.user.Login}";
        }
        private async void Setup() {
            Login.Text = App.user.Login;
            Name.Text = App.user.Profile.Name;
            Surname.Text = App.user.Profile.Surname;
            Email.Text = App.user.Profile.Email;

            List<ApplicationForAdvertisement> l = await ApplicationForAdvertisementRepo.getAsyncByUserID(App.user.ID);

            ApplicationsList.ItemsSource = await ApplicationForAdvertisementRepo.getAsyncByUserID(App.user.ID);
            UserAnnouncementList.ItemsSource = await AnnouncementRepository.GetAllByUserID(App.user.ID);
        }

        private async void Delete_Clicked(object sender, EventArgs e) {
            if ((sender as Button).CommandParameter is AnnouncementModel announcement) {
                bool alert = await DisplayAlert("Potwierdzenie", "Czy napewno chcesz usunac to ogloszenie?", "Tak", "Nie");
                if (alert) {
                    await AnnouncementRepository.DeleteAsync(announcement);
                    UserAnnouncementList.ItemsSource = await AnnouncementRepository.GetAllByUserID(App.user.ID);
                }
            }
        }

        private async void Edit_Clicked(object sender, EventArgs e) {
            if ((sender as Button).CommandParameter is AnnouncementModel announcement) {
                await Navigation.PushAsync(new AnnouncementCreate(announcement));
            }
        }

        private async void LogOut_Clicked(object sender, EventArgs e) {
            App.user=null;
            await Navigation.PopToRootAsync();
        }

        private async void CompanyAddButton_Clicked(object sender, EventArgs e) {
            Company company = new Company() {
                Name = CompanyName.Text,
                Description = CompanyDescription.Text,
                NIP = int.Parse(NIP.Text),
                ImageLink = CompanyImage.Text,
            };
            company = await CompanyRepo.AddAsync(company);
            App.user.CompanyID = company.ID;
            App.user.Company = company;
            App.user =await UserRepo.UpdateAsync(App.user);
            await DisplayAlert("Powiadomienie", "Pomyslnie zaaktualizowano profil","OK");
            CompanyForm.IsVisible = true;
        }

        private async void Accept_Clicked(object sender, EventArgs e) {
            if((sender as Button).CommandParameter is ApplicationForAdvertisement data) {
                bool alert=await DisplayAlert("Aplikacja", $"Czy na pewno chcesz zaakceptowac aplikacje uzytkownika {data.User.Login}","Tak","Nie");
                if (alert) {
                    await ApplicationForAdvertisementRepo.DeleteAsync(data.UserID, data.AnnouncementID);
                    ApplicationsList.ItemsSource = await ApplicationForAdvertisementRepo.getAsyncByUserID(App.user.ID);
                }
            }
        }

        private async void Denied_Clicked(object sender, EventArgs e) {
            if ((sender as Button).CommandParameter is ApplicationForAdvertisement data) {
                bool alert = await DisplayAlert("Aplikacja", $"Czy na pewno chcesz odrzucic aplikacje uzytkownika {data.User.Login}", "Tak", "Nie");
                if (alert) {
                    await ApplicationForAdvertisementRepo.DeleteAsync(data.UserID, data.AnnouncementID);
                    ApplicationsList.ItemsSource = await ApplicationForAdvertisementRepo.getAsyncByUserID(App.user.ID);
                }
            }
        }
    }
}