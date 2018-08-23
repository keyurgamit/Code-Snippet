using DataAccess.Abstract;
using DataAccess.Interface;
using EntityModel.DomainModel;
using System.Data;
using System.Linq;

namespace DataAccess.Repository.Dapper.MSSQL
{
    public class SettingRepository : DapperRepositoryBase, ISettingRepository
    {
        public SettingRepository(IDbConnection conn, string conString)
        {
            _connection = conn;
            _connection.ConnectionString = conString;
        }

        public string GetValueBySettingName(string SettingName)
        {
            return Query<Setting>("SettingsManagement", new { ActionId = 1, SettingName = SettingName }).FirstOrDefault().Value;
        }
    }
}
