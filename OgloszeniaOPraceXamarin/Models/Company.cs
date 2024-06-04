using SQLite;

namespace OgloszeniaOPraceXamarin.Models {
    public class Company {
        [PrimaryKey, AutoIncrement, NotNull]
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? NIP { get; set; }
        public string Location { get; set; }
        public string ImageLink { get; set; }
    }
}
