using Dapper;
using DataAccess.Abstract;
using DataAccess.Interface;
using EntityModel.DomainModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataAccess.Repository.Dapper.MSSQL
{
    public class PropertyRepository : DapperRepositoryBase, IPropertyRepository
    {
        public PropertyRepository(IDbConnection conn, string conString)
        {
            _connection = conn;
            _connection.ConnectionString = conString;
        }

        public List<Property> getPropertiesByCityId(int CityId, string sortby, int pageindex)
        {
            return Query<Property>("PropertyDetails", new { ActionId = 1, CityId = CityId, SortBy = sortby, PageIndex = pageindex }).ToList();
        }

        public List<Property> getFeaturedProperties()
        {
            return Query<Property>("PropertyDetails", new { ActionId = 2 }).ToList();
        }

        public List<Property> getSearchedProperties(SearchProperty objSearchProperty)
        {
            ////return Query<Property>("PropertyDetails", new { ActionId = 3 }).ToList();

            //var objDynamicParameters = new DynamicParameters();
            ////objDynamicParameters.Add("Location", objSearchProperty.Location);
            ////objDynamicParameters.Add("SearchKeyword", objSearchProperty.SearchKeyword);
            ////objDynamicParameters.Add("FromPrice", objSearchProperty.FromPrice);
            ////objDynamicParameters.Add("ToPrice", objSearchProperty.ToPrice);
            ////objDynamicParameters.Add("NoOfBedrooms", objSearchProperty.NoOfBedrooms);
            ////objDynamicParameters.Add("MinNoOfBathrooms", objSearchProperty.MinNoOfBathrooms);
            ////objDynamicParameters.Add("IsFurnished", objSearchProperty.IsFurnished);
            ////objDynamicParameters.Add("CommunityAmenities", objSearchProperty.CommunityAmenities);
            ////objDynamicParameters.Add("BuildingAmenities", objSearchProperty.BuildingAmenities);

            //objDynamicParameters.Add("Keyword", objSearchProperty.Keyword);
            //objDynamicParameters.Add("FromPrice", objSearchProperty.FromPrice);
            //objDynamicParameters.Add("ToPrice", objSearchProperty.ToPrice);
            //objDynamicParameters.Add("AvailableFromDate", objSearchProperty.AvailableFromDate);
            //objDynamicParameters.Add("NoOfBedRooms", objSearchProperty.NoOfBedRooms);
            //objDynamicParameters.Add("NoOfBathRooms", objSearchProperty.NoOfBathRooms);
            //objDynamicParameters.Add("DistanceFromSchool", objSearchProperty.DistanceFromSchool);
            //objDynamicParameters.Add("SeperateEntrance", objSearchProperty.SeparateEntrance);
            //objDynamicParameters.Add("SeperateLaundry", objSearchProperty.SeparateLaundry);
            //objDynamicParameters.Add("Internet", objSearchProperty.Internet);
            //objDynamicParameters.Add("TV", objSearchProperty.TV);
            //objDynamicParameters.Add("Furnished", objSearchProperty.Furnished);
            //objDynamicParameters.Add("SmokeFriendly", objSearchProperty.SmokeFriendly);
            //objDynamicParameters.Add("PetFriendly", objSearchProperty.PetFriendly);
            //objDynamicParameters.Add("WalkOut", objSearchProperty.WalkOut);
            //objDynamicParameters.Add("ParkingAvailable", objSearchProperty.ParkingAvailable);
            //objDynamicParameters.Add("VegetarianOnly", objSearchProperty.VegetarianOnly);

            ////Execute("SearchProperties", objDynamicParameters);

            ////return objDynamicParameters.Get<int>("ReturnValue");
            ////return null;

            return Query<Property>("SearchProperties", new { Keyword = objSearchProperty.Keyword,
                ProvinceId = objSearchProperty.ProvinceId, CityId = objSearchProperty.CityId,
                FromPrice = objSearchProperty.FromPrice, ToPrice = objSearchProperty.ToPrice,
                AvailableFromDate = objSearchProperty.AvailableFromDate, NoOfBedRooms = objSearchProperty.NoOfBedRooms,
                NoOfBathRooms = objSearchProperty.NoOfBathRooms, DistanceFromSchool = objSearchProperty.DistanceFromSchool,
                SeparateEntrance = objSearchProperty.SeparateEntrance, SeparateLaundry = objSearchProperty.SeparateLaundry,
                Internet = objSearchProperty.Internet, TV = objSearchProperty.TV, Furnished = objSearchProperty.Furnished,
                SmokeFriendly = objSearchProperty.SmokeFriendly, PetFriendly = objSearchProperty.PetFriendly, WalkOut = objSearchProperty.WalkOut,
                ParkingAvailable = objSearchProperty.ParkingAvailable, VegetarianOnly = objSearchProperty.VegetarianOnly,
                PAGEINDEX = objSearchProperty.CurrentPageIndex }).ToList();
        }

        public List<Property> getLatestProperties()
        {
            return Query<Property>("PropertyDetails", new { ActionId = 4 }).ToList();
        }

        public int InsertUpdateProperty(Property objProperty)
        {
            try
            {
                var objDynamicParameters = new DynamicParameters();
                //objDynamicParameters.Add("ActionId", 5, DbType.Int32, ParameterDirection.Input);
                objDynamicParameters.Add("PropertyId", objProperty.Id, DbType.Int32, ParameterDirection.Input);
                objDynamicParameters.Add("UserId", objProperty.UserId, DbType.Int32, ParameterDirection.Input);
                objDynamicParameters.Add("PropertyTitle", objProperty.PropertyTitle, DbType.String, ParameterDirection.Input);
                objDynamicParameters.Add("StreetAddress", objProperty.StreetAddress, DbType.String, ParameterDirection.Input);
                objDynamicParameters.Add("CityId", objProperty.CityId, DbType.Int32, ParameterDirection.Input);
                objDynamicParameters.Add("PostalCode", objProperty.PostalCode, DbType.String, ParameterDirection.Input);
                objDynamicParameters.Add("NoOfBedroom", objProperty.NoOfBedroom, DbType.Int32, ParameterDirection.Input);
                objDynamicParameters.Add("NoOfBathroom", objProperty.NoOfBathroom, DbType.Int32, ParameterDirection.Input);
                objDynamicParameters.Add("IsFurnished", objProperty.IsFurnished, DbType.Boolean, ParameterDirection.Input);
                objDynamicParameters.Add("IsFeatured", objProperty.IsFeatured, DbType.Boolean, ParameterDirection.Input);
                objDynamicParameters.Add("PropertyInformation", objProperty.PropertyInformation, DbType.String, ParameterDirection.Input);
                objDynamicParameters.Add("MonthlyRent", objProperty.MonthlyRent, DbType.Int32, ParameterDirection.Input);
                objDynamicParameters.Add("Area", objProperty.Area, DbType.String, ParameterDirection.Input);
                objDynamicParameters.Add("Availability", objProperty.Availability, DbType.Boolean, ParameterDirection.Input);
                objDynamicParameters.Add("AvailableFromDate", objProperty.AvailableFromDate, DbType.Date, ParameterDirection.Input);

                //objDynamicParameters.Add("Published", objProperty.Published, DbType.Boolean, ParameterDirection.Input);
                objDynamicParameters.Add("Published", true, DbType.Boolean, ParameterDirection.Input);

                objDynamicParameters.Add("IsApproved", false, DbType.Boolean, ParameterDirection.Input);
                objDynamicParameters.Add("ApproxDistanceFromSchool", objProperty.ApproxDistanceFromSchool, DbType.String, ParameterDirection.Input);
                objDynamicParameters.Add("MajorIntersection", objProperty.MajorIntersection, DbType.String, ParameterDirection.Input);
                objDynamicParameters.Add("IsSeparateEntrance", objProperty.IsSeparateEntrance, DbType.Boolean, ParameterDirection.Input);
                objDynamicParameters.Add("IsSeparateLaundry", objProperty.IsSeparateLaundry, DbType.Boolean, ParameterDirection.Input);
                objDynamicParameters.Add("IsInternet", objProperty.IsInternet, DbType.Boolean, ParameterDirection.Input);
                objDynamicParameters.Add("IsTV", objProperty.IsTV, DbType.Boolean, ParameterDirection.Input);
                objDynamicParameters.Add("IsParkingAvailable", objProperty.IsParkingAvailable, DbType.Boolean, ParameterDirection.Input);
                objDynamicParameters.Add("IsSmokeFriendly", objProperty.IsSmokeFriendly, DbType.Boolean, ParameterDirection.Input);
                objDynamicParameters.Add("IsPetFriendly", objProperty.IsPetFriendly, DbType.Boolean, ParameterDirection.Input);
                objDynamicParameters.Add("IsWalkOutOrNot", objProperty.IsWalkOutOrNot, DbType.Boolean, ParameterDirection.Input);
                objDynamicParameters.Add("IsVegetarian", objProperty.IsVegetarian, DbType.Boolean, ParameterDirection.Input);
                objDynamicParameters.Add("IsFirstAndLastMonthRentRequired", objProperty.IsFirstAndLastMonthRentRequired, DbType.Boolean, ParameterDirection.Input);
                //objDynamicParameters.Add("CreatedDate", objProperty.CreatedDate, DbType.DateTime, ParameterDirection.Input);
                //objDynamicParameters.Add("UpdatedDate", objProperty.UpdatedDate, DbType.DateTime, ParameterDirection.Input);
                objDynamicParameters.Add("ReturnValue", null, DbType.Int32, ParameterDirection.Output);
                objDynamicParameters.Add("ErrorCode", null, DbType.Int32, ParameterDirection.Output);

                Execute("AddUpdateProperty", objDynamicParameters);
                var PropertyId = objDynamicParameters.Get<int>("ReturnValue");
                var ErrorCode = objDynamicParameters.Get<int>("ErrorCode");

                return PropertyId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool InsertPropertyImages(List<Image> lstImages, int PropertyId)
        {
            try
            {
                foreach (var item in lstImages)
                {
                    var objDynamicParameters = new DynamicParameters();
                    objDynamicParameters.Add("ActionId", 6, DbType.Int32, ParameterDirection.Input);
                    objDynamicParameters.Add("INS_ImagePath", item.ImagePath, DbType.String, ParameterDirection.Input);
                    objDynamicParameters.Add("INS_PropertyId", PropertyId, DbType.Int32, ParameterDirection.Input);
                    objDynamicParameters.Add("IsDeleted", item.IsDeleted, DbType.Boolean, ParameterDirection.Input);
                    //objDynamicParametersForImage.Add("ReturnValue", null, DbType.Int32, ParameterDirection.Output);

                    Execute("PropertyDetails", objDynamicParameters);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Property GetPropertyById(int PropertyId)
        {
            return Query<Property>("PropertyDetails", new { ActionId = 7, PropertyId = PropertyId }).FirstOrDefault();
        }

        public List<Image> getImagesByPropertyId(int PropertyId)
        {
            return Query<Image>("PropertyDetails", new { ActionId = 8, PropertyId = PropertyId }).ToList();
        }

        public List<Property> getSimilarProperties(int NoOfBedrooms, int NoOfBathrooms)
        {
            return Query<Property>("PropertyDetails", new { ActionId = 9, NoOfBedrooms = NoOfBedrooms, MinNoOfBathrooms = NoOfBathrooms }).ToList();
        }

        public UserFavouriteMapping getFavouritePropertyIdsByUserId(int UserId)
        {
            return Query<UserFavouriteMapping>("PropertyDetails", new { ActionId = 10, @UserId = UserId }).FirstOrDefault();
        }

        public bool UpdateFavouritePropertiesByUserId(string PropertyIds, int UserId)
        {
            try
            {
                var objDynamicParameters = new DynamicParameters();
                objDynamicParameters.Add("ActionId", 11, DbType.Int32, ParameterDirection.Input);
                objDynamicParameters.Add("PropertyIds", PropertyIds, DbType.String, ParameterDirection.Input);
                objDynamicParameters.Add("UserId", UserId, DbType.Int32, ParameterDirection.Input);
                //objDynamicParametersForImage.Add("ReturnValue", null, DbType.Int32, ParameterDirection.Output);

                Execute("PropertyDetails", objDynamicParameters);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Property> getFavouritePropertiesByUserId(int UserId)
        {
            return Query<Property>("PropertyDetails", new { ActionId = 12, UserId = UserId }).ToList();
        }

        public List<Property> getPropertyListByLandlord(int LandlordId)
        {
            return Query<Property>("PropertyDetails", new { ActionId = 13, UserId = LandlordId }).ToList();
        }

        public void deletePropertyImageOnEdit(int ImageId)
        {
            var objDynamicParameters = new DynamicParameters();
            objDynamicParameters.Add("ActionId", 5, DbType.Int32);
            objDynamicParameters.Add("ImageId", ImageId, DbType.Int32);

            Execute("PropertyDetails", objDynamicParameters);
            //var errorCode = objDynamicParameters.Get<int>("ErrorCode");
            //return errorCode == 0;
        }

        public bool deleteProperty(int PropertyId)
        {
            try
            {
                var objDynamicParameters = new DynamicParameters();
                objDynamicParameters.Add("ActionId", 14, DbType.Int32);
                objDynamicParameters.Add("PropertyId", PropertyId, DbType.Int32);

                Execute("PropertyDetails", objDynamicParameters);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Property> getPropertiesByCityName(string CityName)
        {
            return Query<Property>("PropertyDetails", new { ActionId = 17, SearchKeyword = CityName }).ToList();
        }

        #region Property Management
        public List<Property> GetAllPropertiesByLandlordWithPaging(int SeletecedLandlordId, bool IsApproved, int pageindex, int pageSize)
        {
            return Query<Property>("PropertyDetails", new { ActionId = 15, UserId = SeletecedLandlordId, IsApproved = IsApproved, PageIndex = pageindex, PageSize = pageSize }).ToList();
        }

        public bool approveProperty(int PropertyId)
        {
            try
            {
                var objDynamicParameters = new DynamicParameters();
                objDynamicParameters.Add("ActionId", 16, DbType.Int32);
                objDynamicParameters.Add("PropertyId", PropertyId, DbType.Int32);

                Execute("PropertyDetails", objDynamicParameters);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}