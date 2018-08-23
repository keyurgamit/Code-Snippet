using BasementRenting.Controllers;
using Dapper;
using DataAccess.Abstract;
using DataAccess.Interface;
using EntityModel.DomainModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataAccess.Repository.Dapper.MSSQL
{
    public class CommonRepository : DapperRepositoryBase, ICommonRepository
    {
        public CommonRepository(IDbConnection conn, string conString)
        {
            _connection = conn;
            _connection.ConnectionString = conString;
        }

        public RegisterCustomer getUserRolebyId(int Userid)
        {
            return Query<RegisterCustomer>("UserMasterDetails", new { ActionId = 8, Id = Userid }).FirstOrDefault();
        }

        #region Homepage CMS
        public ContentManagement getHomePageCms()
        {
            return Query<ContentManagement>("ContentManagement", new { ActionId = 1, ContentName = "HomepageWelcomeInfo" }).FirstOrDefault();
        }

        public void UpdateHomePageCms(ContentManagement objContentManagement)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ActionId", 2, DbType.Int32, ParameterDirection.Input);
            parameters.Add("ContentName", "HomepageWelcomeInfo", DbType.String, ParameterDirection.Input);
            parameters.Add("ContentValue", objContentManagement.Value, DbType.String, ParameterDirection.Input);

            Execute("ContentManagement", parameters);
        }
        #endregion

        #region Terms & Conditions CMS
        public ContentManagement getTermsConditionsPageCms()
        {
            return Query<ContentManagement>("ContentManagement", new { ActionId = 1, ContentName = "TermsAndConditions" }).FirstOrDefault();
        }
        public void UpdateTermsConditionsPageCms(ContentManagement objContentManagement)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ActionId", 2, DbType.Int32, ParameterDirection.Input);
            parameters.Add("ContentName", "TermsAndConditions", DbType.String, ParameterDirection.Input);
            parameters.Add("ContentValue", objContentManagement.Value, DbType.String, ParameterDirection.Input);

            Execute("ContentManagement", parameters);
        }
        #endregion
    }
}