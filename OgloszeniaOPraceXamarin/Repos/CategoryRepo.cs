using OgloszeniaOPraceXamarin.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OgloszeniaOPraceXamarin.Repos {
    public static class CategoryRepo {
        private static SQLiteAsyncConnection database;

        static CategoryRepo() {
            database = new SQLiteAsyncConnection(App.dbPath);
            database.CreateTableAsync<CategoryModel>().Wait();
            
        }
        public static async Task Seed() {
            if (database.Table<CategoryModel>().CountAsync().Result == 0) {
                SeedAsync();
            }
        }
        public static async Task<CategoryModel> AddAsync(CategoryModel category) {
            await database.InsertAsync(category);
            return category;
        }

        public static async Task<CategoryModel> GetAsync(int id) {
            return await database.Table<CategoryModel>().FirstOrDefaultAsync(c => c.ID == id);
        }

        public static async Task<List<CategoryModel>> GetAllAsync() {
            return await database.Table<CategoryModel>().ToListAsync();
        }

        public static async Task<CategoryModel> UpdateAsync(CategoryModel category) {
            await database.UpdateAsync(category);
            return category;
        }

        public static async Task DeleteAsync(int id) {
            await database.DeleteAsync<CategoryModel>(id);
        }

        private static async Task SeedAsync() {
            List<CategoryModel> models = new List<CategoryModel>() {
                new CategoryModel() {
                    ID = 1,
                    Name="IT"
                },
                new CategoryModel() {
                    ID=2,
                    Name="Hotelarstwo"
                },
                new CategoryModel() {
                    ID=3,
                    Name="Edukacja"
                },
                new CategoryModel() {
                    ID=4,
                    Name="Mechanika pojazów"
                },
            };

            foreach (var model in models)
                await AddAsync(model);
        }
    }
}
