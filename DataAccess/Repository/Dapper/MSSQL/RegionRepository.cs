using DataAccess.Abstract;
using DataAccess.Interface;
using EntityModel.DomainModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataAccess.Repository.Dapper.MSSQL
{
    public class RegionRepository : DapperRepositoryBase, IRegionRepository
    {
        public RegionRepository(IDbConnection conn, string conString)
        {
            _connection = conn;
            _connection.ConnectionString = conString;
        }

        #region Province
        public List<ProvinceMaster> GetAllProvince()
        {
            return Query<ProvinceMaster>("CountryDetails", new { ActionId = 1 }).ToList();
        }
        public ProvinceMaster GetProvinceById(int Id)
        {
            return Query<ProvinceMaster>("CountryDetails", new { ActionId = 2, ProvinceId = Id }).FirstOrDefault();
        }
        #endregion

        #region City
        public List<CityMaster> GetAllCity()
        {
            return Query<CityMaster>("CountryDetails", new { ActionId = 3 }).ToList();
        }
        public CityMaster GetCityById(int Id)
        {
            return Query<CityMaster>("CountryDetails", new { ActionId = 4, CityId = Id }).FirstOrDefault();
        }
        public List<CityMaster> GetCityByProvinceId(int Id)
        {
            return Query<CityMaster>("CountryDetails", new { ActionId = 5, ProvinceId = Id }).ToList();
        }

        public List<CityMaster> GetCitylistByName(string CityName)
        {
            return Query<CityMaster>("CountryDetails", new { ActionId = 6, CityName = CityName }).ToList();
        }
        #endregion
    }
}
