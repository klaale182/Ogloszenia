using OgloszeniaOPraceXamarin.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using OgloszeniaOPraceXamarin.Supports;

namespace OgloszeniaOPraceXamarin.Repos {
    public static class UserRepo {
        private static SQLiteAsyncConnection database;

        static UserRepo() {
            database = new SQLiteAsyncConnection(App.dbPath);
            database.CreateTableAsync<UserModel>().Wait();

        }
        public static async Task Seed() {
            if (database.Table<UserModel>().CountAsync().Result == 0) {
                await SeedAsync();
            }
        }
        public static async Task<UserModel> AddAsync(UserModel user) {

            if (user.Profile != null) {
                user.Profile = await ProfileRepo.AddAsync(user.Profile);
                user.ProfileID = user.Profile.ID;
            }

            await database.InsertAsync(user);
            return user;
        }

        public static async Task<UserModel> GetAsync(int id) {
            var user = await database.Table<UserModel>().FirstOrDefaultAsync(u => u.ID == id);

            if (user != null) {
                user.Profile = await ProfileRepo.GetAsync(user.ProfileID.GetValueOrDefault());
                user.Company = await CompanyRepo.GetAsync(user.CompanyID.GetValueOrDefault());
            }

            return user;
        }

        public static async Task<List<UserModel>> GetAllAsync() {
            var users = await database.Table<UserModel>().ToListAsync();

            foreach (var user in users) {
                user.Profile = await ProfileRepo.GetAsync(user.ProfileID.GetValueOrDefault());
                user.Company = await CompanyRepo.GetAsync(user.CompanyID.GetValueOrDefault());
            }

            return users;
        }

        public static async Task<UserModel> UpdateAsync(UserModel user) {

            if (user.Profile != null) {
                user.Profile = await ProfileRepo.UpdateAsync(user.Profile);
            }
            await database.UpdateAsync(user);
            return user;
        }

        public static async Task DeleteAsync(int id) {
            var user = await GetAsync(id);

            if (user != null) {
                await ProfileRepo.DeleteAsync(user.ProfileID.GetValueOrDefault());
                //await CompanyRepo.DeleteAsync(user.CompanyID.GetValueOrDefault());
                await database.DeleteAsync<UserModel>(id);
            }
        }

        public static async Task<UserModel> Login(string login, string password) {
            UserModel user = await database.Table<UserModel>()
                                       .Where(u => u.Login == login)
                                       .FirstOrDefaultAsync();
            if (user != null && PasswordHandling.VerifyPassword(password, user.Password)) {
                user.Profile = await ProfileRepo.GetAsync((int)user.ProfileID);
                user.Company = await CompanyRepo.GetAsync(user.CompanyID.GetValueOrDefault());
                return user;
            }
            return null;
        }
        private static async Task SeedAsync() {
            List<UserModel> userModels = new List<UserModel>() {
                new UserModel {
                    ID = 1,
                    Login="Login",
                    Password=PasswordHandling.HashPassword("Password"),
                    CompanyID=1,
                    Permission=2,
                    ProfileID=1,
                },
                new UserModel {
                    ID = 2,
                    Login="Login1",
                    Password=PasswordHandling.HashPassword("Password1"),
                    CompanyID=1,
                    Permission=1,
                    ProfileID=1,
                },
                new UserModel {
                    ID = 2,
                    Login="Login2",
                    Password=PasswordHandling.HashPassword("Password2"),
                    CompanyID=1,
                    Permission=1,
                    ProfileID=1,
                }
            };

            foreach (UserModel userModel in userModels)
                await AddAsync(userModel);
        }
    }
}
