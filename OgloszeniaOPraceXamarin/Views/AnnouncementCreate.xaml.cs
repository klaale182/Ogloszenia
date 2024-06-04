using OgloszeniaOPraceXamarin.Models;
using OgloszeniaOPraceXamarin.Repos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OgloszeniaOPraceXamarin.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnnouncementCreate : ContentPage {
        AnnouncementModel _Announcement;
        public AnnouncementCreate() {
            InitializeComponent();
            Setup();
            Title = "Dodaj Ogloszenie";
        }
        public AnnouncementCreate(AnnouncementModel model) {
            InitializeComponent();
            Title = "Edytuj Ogloszenie";
            ActionButton.Text = "Edytuj";
            _Announcement = model;
            Setup();
            ActionButton.Clicked -= ActionButton_Clicked;
            ActionButton.Clicked += ActionButtonEdit_Clicked;
            ATitle.Text = model.Title;
            Description.Text = model.Description;
            Position.Text = model.Position;
            MinWage.Text = model.MinWage.ToString();
            MaxWage.Text = model.MaxWage.ToString();
        }
        
        async void Setup() {
            Category.ItemsSource = await CategoryRepo.GetAllAsync();
            Category.SelectedIndex = 0;
            TypeOfWork.ItemsSource = await TypeOfWorkRepo.GetAllAsync();
            TypeOfWork.SelectedIndex = 0;
        }

        private async void ActionButtonEdit_Clicked(object sender, System.EventArgs e) {
            _Announcement.Title = ATitle.Text;
            _Announcement.Description = Description.Text;
            _Announcement.MinWage = decimal.Parse(MinWage.Text);
            _Announcement.MaxWage = decimal.Parse(MaxWage.Text);
            _Announcement.CategoryID = (int)(Category.SelectedItem as CategoryModel).ID;
            _Announcement.TypeOfWorkID = (int)(TypeOfWork.SelectedItem as TypeOfWork).ID;
            _Announcement.Position = Position.Text;
            await AnnouncementRepository.CreateAsync(_Announcement);
            await Navigation.PopToRootAsync();
        }

        private async void ActionButton_Clicked(object sender, System.EventArgs e) {
            AnnouncementModel announcement = new AnnouncementModel();
            announcement.Title = ATitle.Text;
            announcement.Description = Description.Text;
            announcement.CategoryID = (int)(Category.SelectedItem as CategoryModel).ID;
            announcement.TypeOfWorkID = (int)(TypeOfWork.SelectedItem as TypeOfWork).ID;
            announcement.Position = Position.Text;
            announcement.CompanyID = App.user.Company.ID;
            announcement.MinWage = decimal.Parse(MinWage.Text);
            announcement.MaxWage = decimal.Parse(MaxWage.Text);
            announcement.UserID = App.user.ID;

            announcement = await AnnouncementRepository.CreateAsync(announcement);

            await DisplayAlert("Powiadomienie", "Pomyslnie dodano ogloszenie", "OK");
            await Navigation.PopToRootAsync();
        }
    }
}