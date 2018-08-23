using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EntityModel.DomainModel
{
    public class Property
    {
        public Property()
        {
            lstProvince = new List<SelectListItem>();
            lstCity = new List<SelectListItem>();
            lstNoOfBedrooms = new List<SelectListItem>();
            lstNoOfBathrooms = new List<SelectListItem>();
            lstPropertyImages = new List<Image>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }

        [Display(Name = "Property Title")]
        public string PropertyTitle { get; set; }

        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        public int ProvinceId { get; set; }

        public int CityId { get; set; }

        public string CityName { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "No. Of Bedrooms")]
        public int NoOfBedroom { get; set; }

        [Display(Name = "No. Of Bathroom")]
        public int NoOfBathroom { get; set; }

        [Display(Name = "Furnished")]
        public bool IsFurnished { get; set; }
        public bool IsFavourite { get; set; }

        [Display(Name = "Featured")]
        public bool IsFeatured { get; set; }

        [Display(Name = "Property Information")]
        public string PropertyInformation { get; set; }

        [Display(Name = "Monthly Rent ($)")]
        public int MonthlyRent { get; set; }

        [Display(Name = "Approx Area (sq ft)")]
        public string Area { get; set; }
        public bool Availability { get; set; }


        [Display(Name = "Available From Date")]
        public string AvailableFromDate { get; set; }

        public bool Published { get; set; }
        public bool IsApproved { get; set; }

        [Display(Name = "Approx Dist From School (mtr.)")]
        public string ApproxDistanceFromSchool { get; set; }

        [Display(Name = "Major Intersection")]
        public string MajorIntersection { get; set; }

        [Display(Name = "Separate Entrance")]
        public bool IsSeparateEntrance { get; set; }

        [Display(Name = "Separate Laundry")]
        public bool IsSeparateLaundry { get; set; }

        [Display(Name = "Internet")]
        public bool IsInternet { get; set; }

        [Display(Name = "TV")]
        public bool IsTV { get; set; }

        [Display(Name = "Parking Available")]
        public bool IsParkingAvailable { get; set; }

        [Display(Name = "Smoke Friendly")]
        public bool IsSmokeFriendly { get; set; }

        [Display(Name = "Pet Friendly")]
        public bool IsPetFriendly { get; set; }

        [Display(Name = "Walkout Or No")]
        public bool IsWalkOutOrNot { get; set; }

        [Display(Name = "Vegetarian Only?")]
        public bool IsVegetarian { get; set; }

        [Display(Name = "First And Last Month's Rent Required?")]
        public bool IsFirstAndLastMonthRentRequired { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ImagePath { get; set; }
        public string OwnerEmailId { get; set; }
        public string OwnerPhoneNumber { get; set; }

        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int ROWNUMBER { get; set; }

        [Display(Name = "Province")]
        public List<SelectListItem> lstProvince { get; set; }

        [Display(Name = "City")]
        public List<SelectListItem> lstCity { get; set; }

        [Display(Name = "No. Of Bedrooms")]
        public List<SelectListItem> lstNoOfBedrooms { get; set; }

        [Display(Name = "No. Of Bathrooms")]
        public List<SelectListItem> lstNoOfBathrooms { get; set; }

        public List<Image> lstPropertyImages { get; set; }
    }

    public class Image
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int PropertyId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class UserFavouriteMapping
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PropertyIds { get; set; }
    }
}