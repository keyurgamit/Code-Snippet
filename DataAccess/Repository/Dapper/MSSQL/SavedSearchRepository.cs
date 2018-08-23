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
    public class SavedSearchRepository : DapperRepositoryBase, ISavedSearchRepository
    {
        public SavedSearchRepository(IDbConnection conn, string conString)
        {
            _connection = conn;
            _connection.ConnectionString = conString;
        }

        public int SaveSearch(SearchProperty objSearchProperty)
        {
            var objDynamicParameters = new DynamicParameters();
            objDynamicParameters.Add("Id", objSearchProperty.Id, DbType.Int32, ParameterDirection.Input);
            objDynamicParameters.Add("UserId", objSearchProperty.UserId, DbType.Int32, ParameterDirection.Input);
            objDynamicParameters.Add("Keyword", objSearchProperty.Keyword, DbType.String, ParameterDirection.Input);
            objDynamicParameters.Add("ProvinceId", objSearchProperty.ProvinceId, DbType.Int32, ParameterDirection.Input);
            objDynamicParameters.Add("CityId", objSearchProperty.CityId, DbType.Int32, ParameterDirection.Input);
            objDynamicParameters.Add("FromPrice", objSearchProperty.FromPrice, DbType.Int32, ParameterDirection.Input);
            objDynamicParameters.Add("ToPrice", objSearchProperty.ToPrice, DbType.Int32, ParameterDirection.Input);
            objDynamicParameters.Add("AvailableFromDate", objSearchProperty.AvailableFromDate, DbType.DateTime, ParameterDirection.Input);
            objDynamicParameters.Add("NoOfBedrooms", objSearchProperty.NoOfBedRooms, DbType.Int32, ParameterDirection.Input);
            objDynamicParameters.Add("NoOfBathrooms", objSearchProperty.NoOfBathRooms, DbType.Int32, ParameterDirection.Input);
            objDynamicParameters.Add("DistanceFromSchool", objSearchProperty.DistanceFromSchool, DbType.Int32, ParameterDirection.Input);
            objDynamicParameters.Add("SeparateEntrance", objSearchProperty.SeparateEntrance, DbType.Boolean, ParameterDirection.Input);
            objDynamicParameters.Add("Internet", objSearchProperty.Internet, DbType.Boolean, ParameterDirection.Input);
            objDynamicParameters.Add("Furnished", objSearchProperty.Furnished, DbType.Boolean, ParameterDirection.Input);
            objDynamicParameters.Add("PetFriendly", objSearchProperty.PetFriendly, DbType.Boolean, ParameterDirection.Input);
            objDynamicParameters.Add("ParkingAvailable", objSearchProperty.ParkingAvailable, DbType.Boolean, ParameterDirection.Input);
            objDynamicParameters.Add("SeparateLaundry", objSearchProperty.SeparateLaundry, DbType.Boolean, ParameterDirection.Input);
            objDynamicParameters.Add("TV", objSearchProperty.TV, DbType.Boolean, ParameterDirection.Input);
            objDynamicParameters.Add("SmokeFriendly", objSearchProperty.SmokeFriendly, DbType.Boolean, ParameterDirection.Input);
            objDynamicParameters.Add("WalkOut", objSearchProperty.WalkOut, DbType.Boolean, ParameterDirection.Input);
            objDynamicParameters.Add("VegetarianOnly", objSearchProperty.VegetarianOnly, DbType.Boolean, ParameterDirection.Input);

            objDynamicParameters.Add("ReturnValue", 0, DbType.Int32, ParameterDirection.Output);
            objDynamicParameters.Add("ErrorCode", 0, DbType.Int32, ParameterDirection.Output);

            Execute("AddUpdateSaveSearch", objDynamicParameters);
            var RecordId = objDynamicParameters.Get<int>("ReturnValue");
            var ErrorCode = objDynamicParameters.Get<int>("ErrorCode");

            return RecordId;
        }
        
        public SearchProperty SearchResultsByUserId(int UserId)
        {
            return Query<SearchProperty>("SaveSearchManagement", new { ActionId = 1, UserId = UserId }).FirstOrDefault();
        }
    }
}