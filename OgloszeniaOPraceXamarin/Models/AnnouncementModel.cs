using SQLite;

namespace OgloszeniaOPraceXamarin.Models {
    public class AnnouncementModel {
        [PrimaryKey, AutoIncrement, NotNull]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public int? CompanyID { get; set; }
        public int? TypeOfWorkID { get; set; }
        public string Position { get; set; }
        public decimal? MinWage { get; set; }
        public decimal? MaxWage { get; set; }
        public int? UserID { get; set; }
        //Join Models
        [Ignore]
        public CategoryModel Category { get; set; }
        [Ignore]
        public Company Company { get; set; }
        [Ignore]
        public TypeOfWork TypeOfWork { get; set; }
        [Ignore]
        public UserModel User { get; set; }
    }
}
