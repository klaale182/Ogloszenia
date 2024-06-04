using OgloszeniaOPraceXamarin.Models;
using OgloszeniaOPraceXamarin.Repos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OgloszeniaOPraceXamarin.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnnouncementView : ContentPage {
        AnnouncementModel _announcement;
        public AnnouncementView(AnnouncementModel announcement) {
            InitializeComponent();
            Title = announcement.Title;
            BindingContext = announcement;
            _announcement = announcement;
            if (App.user != null && App.user.ID == announcement.User.ID) {
                Toolbar.IsVisible = true;
            }
            if (App.user != null && App.user.ID != announcement.User.ID) {
                ApplicationButton.IsVisible = true;
                Quantity.IsVisible = false;
                CheckApplication();
            }
            Setup();
        }
        async void Setup() {
            int count = await ApplicationForAdvertisementRepo.getCount(_announcement.ID);
            ApplicationQuantity.Text = Quantity.Text = "Liczba aplikacji: " + count;

        }
        async void CheckApplication() {
            if (await ApplicationForAdvertisementRepo.isApplicating(App.user.ID, _announcement.ID)) {
                Applicate.Text = "Anuluj aplikacje";
            } else {
                Applicate.Text = "Aplikuj";
            }
        }
        private async void Edit_Clicked(object sender, System.EventArgs e) {
            await Navigation.PushAsync(new AnnouncementCreate(_announcement));
        }

        private async void Delete_Clicked(object sender, System.EventArgs e) {
            if (await DisplayAlert("Ostrzezenie", "Czy na pewno chcesz usunac to ogloszenie?", "Tak", "Nie")) {
                await AnnouncementRepository.DeleteAsync(_announcement);
                await Navigation.PopToRootAsync();
            }
        }

        private async void Applicate_Clicked(object sender, System.EventArgs e) {
            if (Applicate.Text == "Aplikuj") {
                await ApplicationForAdvertisementRepo.AddAsync(new ApplicationForAdvertisement() {
                    AnnouncementID = _announcement.ID,
                    UserID = App.user.ID,
                });
                Applicate.Text = "Anuluj aplikacje";
                Setup();
            } else {
                await ApplicationForAdvertisementRepo.DeleteAsync(App.user.ID, _announcement.ID);
                Applicate.Text = "Aplikuj";
                Setup();
            }
        }
    }
}