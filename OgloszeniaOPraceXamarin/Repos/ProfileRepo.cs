using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OgloszeniaOPraceXamarin.Models {
    public static class ProfileRepo {
        private static SQLiteAsyncConnection database;

        static ProfileRepo() {
            database = new SQLiteAsyncConnection(App.dbPath);
            database.CreateTableAsync<ProfileModel>().Wait();
        }

        public static async Task Seed() {
            if (database.Table<ProfileModel>().CountAsync().Result == 0) {
                await SeedAsync();
            }
        }

        public static async Task<ProfileModel> AddAsync(ProfileModel person) {
            await database.InsertAsync(person);
            return person;
        }

        public static async Task<ProfileModel> GetAsync(int id) {
            return await database.Table<ProfileModel>().FirstOrDefaultAsync(p => p.ID == id);
        }

        public static async Task<List<ProfileModel>> GetAllAsync() {
            return await database.Table<ProfileModel>().ToListAsync();
        }

        public static async Task<ProfileModel> UpdateAsync(ProfileModel person) {
            await database.UpdateAsync(person);
            return person;
        }

        public static async Task DeleteAsync(int id) {
            await database.DeleteAsync<ProfileModel>(id);
        }

        private static async Task SeedAsync() {
            List<ProfileModel> profiles = new List<ProfileModel>
            {
                new ProfileModel { Name = "John", Surname = "Doe", Email = "john.doe@example.com" },
                new ProfileModel { Name = "Jane", Surname = "Smith", Email = "jane.smith@example.com" },
                new ProfileModel { Name = "Bob", Surname = "Johnson", Email = "bob.johnson@example.com" }
            };

            foreach (ProfileModel profile in profiles)
                await AddAsync(profile);
        }
    }
}
