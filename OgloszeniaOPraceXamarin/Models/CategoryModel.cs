using SQLite;

namespace OgloszeniaOPraceXamarin.Models {
    public class CategoryModel {
        [PrimaryKey, AutoIncrement, NotNull]
        public int? ID { get; set; }
        public string Name { get; set; }

    }
}
