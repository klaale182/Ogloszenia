using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OgloszeniaOPraceXamarin.Models
{
    public class ApplicationForAdvertisement {
        [PrimaryKey,AutoIncrement,NotNull]
        public int? ID { get; set; }
        public int UserID { get; set; }
        public int AnnouncementID { get; set; }
        //Join Models
        [Ignore]
        public UserModel User { get; set; }
        [Ignore]
        public AnnouncementModel Announcement { get; set; }
    }
}
