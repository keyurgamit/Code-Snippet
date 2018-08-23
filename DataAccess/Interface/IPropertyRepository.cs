using EntityModel.DomainModel;
using System.Collections.Generic;

namespace DataAccess.Interface
{
    public partial interface IPropertyRepository : IRepositoryBase
    {
        List<Property> getPropertiesByCityId(int CityId, string sortby, int pageindex);

        List<Property> getFeaturedProperties();

        List<Property> getSearchedProperties(SearchProperty objSearchProperty);

        List<Property> getLatestProperties();

        int InsertUpdateProperty(Property objProperty);

        bool InsertPropertyImages(List<Image> lstImages, int PropertyId);

        Property GetPropertyById(int PropertyId);

        List<Image> getImagesByPropertyId(int PropertyId);

        List<Property> getSimilarProperties(int NoOfBedrooms, int NoOfBathrooms);

        UserFavouriteMapping getFavouritePropertyIdsByUserId(int UserId);

        bool UpdateFavouritePropertiesByUserId(string PropertyIds, int UserId);

        List<Property> getFavouritePropertiesByUserId(int UserId);

        List<Property> getPropertyListByLandlord(int LandlordId);

        void deletePropertyImageOnEdit(int Id);

        bool deleteProperty(int PropertyId);

        List<Property> getPropertiesByCityName(string CityName);

        #region Property Management
        List<Property> GetAllPropertiesByLandlordWithPaging(int SeletecedLandlordId, bool IsApproved, int pageindex, int pageSize);

        bool approveProperty(int PropertyId);
        #endregion
    }
}