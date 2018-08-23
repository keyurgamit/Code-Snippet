using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace EntityModel.DomainModel
{
    public class SearchProperty
    {
        ////public int Id { get; set; }
        //public string Location { get; set; }
        //public string SearchKeyword { get; set; }
        //public int FromPrice { get; set; }
        //public int ToPrice { get; set; }
        //public int NoOfBedrooms { get; set; }
        //public int MinNoOfBathrooms { get; set; }
        //public string IsFurnished { get; set; }
        //public List<string> CommunityAmenities { get; set; }
        //public List<string> BuildingAmenities { get; set; }

        public SearchProperty()
        {
            lstProvince = new List<SelectListItem>();
            lstCity = new List<SelectListItem>();
        }

        public virtual int UserId { get; set; } // to save Searches which are done by users

        public int Id { get; set; }

        [Display(Name = "Keyword")]
        public string Keyword { get; set; }

        [Display(Name = "Province")]
        public List<SelectListItem> lstProvince { get; set; }

        [Display(Name = "Province")]
        public int ProvinceId { get; set; }

        [Display(Name = "City")]
        public List<SelectListItem> lstCity { get; set; }

        [Display(Name = "City")]
        public int CityId { get; set; }

        [Display(Name = "From Price")]
        public int FromPrice { get; set; }

        [Display(Name = "To Price")]
        public int ToPrice { get; set; }

        [Display(Name = "Available From Date")]
        //public DateTime? AvailableFromDate { get; set; }
        public string AvailableFromDate { get; set; }

        [Display(Name = "No of Bedroom")]
        public int NoOfBedRooms { get; set; }

        [Display(Name = "No of Bathroom")]
        public int NoOfBathRooms { get; set; }

        [Display(Name = "Distance From School (mtr.)")]
        public int DistanceFromSchool { get; set; }

        [Display(Name = "Separate Entrance")]
        public bool SeparateEntrance { get; set; }

        [Display(Name = "Separate Laundry")]
        public bool SeparateLaundry { get; set; }
        public bool Internet { get; set; }
        public bool TV { get; set; }
        public bool Furnished { get; set; }

        [Display(Name = "Smoke Friendly")]
        public bool SmokeFriendly { get; set; }

        [Display(Name = "Pet Friendly")]
        public bool PetFriendly { get; set; }
        public bool WalkOut { get; set; }

        [Display(Name = "Parking Available")]
        public bool ParkingAvailable { get; set; }

        [Display(Name = "Vegetarian Only?")]
        public bool VegetarianOnly { get; set; }

        virtual public int CurrentPageIndex { get; set; }
    }
}