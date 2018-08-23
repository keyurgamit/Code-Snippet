using EntityModel.DomainModel;
using System.Collections.Generic;

namespace DataAccess.Interface
{
    public partial interface IRegionRepository : IRepositoryBase
    {
        #region Province
        List<ProvinceMaster> GetAllProvince();
        ProvinceMaster GetProvinceById(int Id);
        #endregion

        #region City
        List<CityMaster> GetAllCity();
        CityMaster GetCityById(int Id);
        List<CityMaster> GetCityByProvinceId(int Id);
        List<CityMaster> GetCitylistByName(string CityName);
        #endregion
    }
}
