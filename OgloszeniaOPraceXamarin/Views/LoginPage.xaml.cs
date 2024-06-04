using OgloszeniaOPraceXamarin.Models;
using OgloszeniaOPraceXamarin.Repos;
using OgloszeniaOPraceXamarin.Supports;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OgloszeniaOPraceXamarin.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : TabbedPage {
        public LoginPage() {
            InitializeComponent();
            LoginEntry.Text = "Login";
            PasswordEntry.Text = "Password";
        }

        private async void LoginAction_Clicked(object sender, EventArgs e) {
            UserModel user = await UserRepo.Login(LoginEntry.Text, PasswordEntry.Text);
            if (user != null) {
                App.user = user;
                await Navigation.PopToRootAsync();
            } else
                await DisplayAlert("Blad logowania", "Niepoprawny login lub haslo", "OK");
        }

        private async void RegisterAction_Clicked(object sender, EventArgs e) {
            UserModel user = new UserModel() {
                CompanyID = null,
                Login = RLogin.Text,
                Password = RPassword.Text,
                Permission=1,
                Profile = new ProfileModel() {
                    Name = RName.Text,
                    Surname = RName.Text,
                    Email = REmail.Text,
                }

            };

            if(user.Validate()!=null) {
                await DisplayAlert("Blad podczas rejestracji",user.Validate(), "OK");
                return;
            }
            if(user.Profile.Validate()!=null) {
                await DisplayAlert("Blad podczas rejestracji", user.Profile.Validate(), "OK");
                return;
            }
            if (RPassword.Text != RPassword2.Text) {
                await DisplayAlert("Blad podczas rejestracji","Hasla musza byc identyczne","OK");
                return;
            }
            user.Password = PasswordHandling.HashPassword(user.Password);
            UserModel _user=await UserRepo.AddAsync(user);
            App.user = _user;
            await Navigation.PopToRootAsync(true);
        }
    }
}