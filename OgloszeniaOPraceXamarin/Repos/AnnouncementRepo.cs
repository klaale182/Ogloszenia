using OgloszeniaOPraceXamarin.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OgloszeniaOPraceXamarin.Repos {
    public static class AnnouncementRepository {
        static readonly SQLiteAsyncConnection database;

        static AnnouncementRepository() {
            database = new SQLiteAsyncConnection(App.dbPath);
            database.CreateTableAsync<AnnouncementModel>().Wait();
            
        }
        public static async Task Seed() {
            if (database.Table<AnnouncementModel>().CountAsync().Result == 0) {
               await SeedAsync();
            }
        }
        public static async Task<List<AnnouncementModel>> GetAllAsync() {
            var announcements = await database.Table<AnnouncementModel>().ToListAsync();

            foreach (var announcement in announcements) {
                await LoadAssociatedModelsAsync(announcement);
            }

            return announcements;
        }

        public static async Task<AnnouncementModel> GetByIdAsync(int id) {
            var announcement = await database.FindAsync<AnnouncementModel>(id);
            await LoadAssociatedModelsAsync(announcement);
            return announcement;
        }

        public static async Task<AnnouncementModel> CreateAsync(AnnouncementModel announcement) {
            if (announcement.ID != 0) {
                await database.UpdateAsync(announcement);
            } else {
                await database.InsertAsync(announcement);
            }
            return announcement;
        }

        public static async Task<int> DeleteAsync(AnnouncementModel announcement) {
            return await database.DeleteAsync(announcement);
        }

        private static async Task LoadAssociatedModelsAsync(AnnouncementModel announcement) {
            if (announcement != null) {
                announcement.Category = await database.Table<CategoryModel>().FirstOrDefaultAsync(c => c.ID == announcement.CategoryID);
                announcement.Company = await database.Table<Company>().FirstOrDefaultAsync(c => c.ID == announcement.CompanyID);
                announcement.TypeOfWork = await database.Table<TypeOfWork>().FirstOrDefaultAsync(t => t.ID == announcement.TypeOfWorkID);
                announcement.User = await database.Table<UserModel>().FirstOrDefaultAsync(u => u.ID == announcement.UserID);
            }
        }
        public static async Task<List<AnnouncementModel>> GetAllByUserID(int userID) {
            List<AnnouncementModel> list = await database.Table<AnnouncementModel>().Where(a => a.UserID == userID).ToListAsync();
            foreach (AnnouncementModel announcement in list) {
                await LoadAssociatedModelsAsync(announcement);
            }
            return list;
        }
        private static async Task SeedAsync() {
            var count = await database.Table<AnnouncementModel>().CountAsync();

            if (count == 0) {
                List<AnnouncementModel> announcements = new List<AnnouncementModel>() {
                new AnnouncementModel {
                    Title = "Robota 1",
                    Description = "We are hiring a skilled software engineer for our development team.",
                    CategoryID = 1,
                    CompanyID = 2,
                    TypeOfWorkID = 3,
                    Position = "Software Engineer",
                    MinWage = 50000,
                    MaxWage = 70000,
                    UserID=1,
                },
                new AnnouncementModel {
                    Title = "Robota 2",
                    Description = "We are hiring a skilled software engineer for our development team.",
                    CategoryID = 3,
                    CompanyID = 2,
                    TypeOfWorkID = 1,
                    Position = "Software Engineer",
                    MinWage = 60000,
                    MaxWage = 70000,
                    UserID=1,
                },
                new AnnouncementModel {
                    Title = "Robota 3",
                    Description = "We are hiring a skilled software engineer for our development team.",
                    CategoryID = 2,
                    CompanyID = 1,
                    TypeOfWorkID = 3,
                    Position = "Software Engineer",
                    MinWage = 70000,
                    MaxWage = 70000,
                    UserID=1,
                },
            };

                foreach (AnnouncementModel item in announcements)
                    await CreateAsync(item);
            }
        }
    }
}
