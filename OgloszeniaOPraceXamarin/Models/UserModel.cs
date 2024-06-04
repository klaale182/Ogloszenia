using SQLite;
using SQLitePCL;
namespace OgloszeniaOPraceXamarin.Models {
    public class UserModel {
        [PrimaryKey,AutoIncrement,NotNull]
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int? Permission { get; set; }
        public int? ProfileID { get; set; }
        public int? CompanyID { get; set; }
        //Join Models
        [Ignore]
        public ProfileModel Profile { get; set; }
        [Ignore]
        public Company Company { get; set; }

        public string Validate() {
            if (string.IsNullOrEmpty(Login) || Login.Length < 5 || Login.Length > 30 || string.IsNullOrEmpty(Login)) {
                return ("Login musi mieć od 5 do 30 znaków.");
            }
            if (string.IsNullOrEmpty(Password) || Password.Length < 5 || Password.Length > 40 || string.IsNullOrEmpty(Password)) {
                return ("Hasło musi mieć od 5 do 30 znaków.");
            }
            return null;
        }
    }
}
