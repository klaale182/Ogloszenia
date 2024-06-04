using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OgloszeniaOPraceXamarin.Models {
    public static class TypeOfWorkRepo {
        private static SQLiteAsyncConnection _connection;

        static TypeOfWorkRepo() {
            _connection = new SQLiteAsyncConnection(App.dbPath);
            _connection.CreateTableAsync<TypeOfWork>().Wait();
        }

        public static async Task Seed() {
            if (_connection.Table<TypeOfWork>().CountAsync().Result == 0) {
               await SeedAsync();
            }
        }

        public static async Task AddAsync(TypeOfWork typeOfWork) {
            await _connection.InsertAsync(typeOfWork);
        }

        public static async Task<TypeOfWork> GetAsync(int id) {
            return await _connection.Table<TypeOfWork>().FirstOrDefaultAsync(t => t.ID == id);
        }

        public static async Task<List<TypeOfWork>> GetAllAsync() {
            return await _connection.Table<TypeOfWork>().ToListAsync();
        }

        public static async Task UpdateAsync(TypeOfWork typeOfWork) {
            await _connection.UpdateAsync(typeOfWork);
        }

        public static async Task DeleteAsync(int id) {
            await _connection.DeleteAsync<TypeOfWork>(id);
        }

        private static async Task SeedAsync() {
            List<TypeOfWork> models = new List<TypeOfWork>() {
                new TypeOfWork() {
                    ID = 1,
                    Name="Zdalnie"
                },
                new TypeOfWork() {
                    ID=2,
                    Name="Hybrydowo"
                },
                new TypeOfWork() {
                    ID=3,
                    Name="Stacjonarnie"
                },
            };

            foreach (var model in models)
                await AddAsync(model);
        }
    }
}
