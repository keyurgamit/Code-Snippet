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
    public class RegisterRepository : DapperRepositoryBase, IRegisterRepository
    {
        public RegisterRepository(IDbConnection conn, string conString)
        {
            _connection = conn;
            _connection.ConnectionString = conString;
        }

        public int AddUpdateRegisterCustomer(RegisterCustomer objRegister)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", objRegister.Id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("UserName", objRegister.UserName, DbType.String, ParameterDirection.Input);
            parameters.Add("UserRoleId", objRegister.UserRoleId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("EmailId", objRegister.EmailId, DbType.String, ParameterDirection.Input);
            parameters.Add("PhoneNo", objRegister.PhoneNo, DbType.String, ParameterDirection.Input);
            parameters.Add("Password", objRegister.Password, DbType.String, ParameterDirection.Input);
            parameters.Add("IpAddress", objRegister.IpAddress, DbType.String, ParameterDirection.Input);
            parameters.Add("Browser", objRegister.Browser, DbType.String, ParameterDirection.Input);
            parameters.Add("IsNewsLetter", objRegister.AgreeToReceiverEmailCommunication, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("ReturnValue", null, DbType.Int32, ParameterDirection.Output);
            parameters.Add("ErrorCode", null, DbType.Int32, ParameterDirection.Output);

            Execute("AddUpdateUserMaster", parameters);
            return parameters.Get<int>("ReturnValue");
        }

        #region user details
        public RegisterCustomer GetLoginDetails(RegisterCustomer reg)
        {
            return Query<RegisterCustomer>("UserMasterDetails", new { ActionId = 4, EmailId = reg.EmailId, Password = reg.Password }).FirstOrDefault();
        }

        public RegisterCustomer GetUserDetails(string reg)
        {
            return Query<RegisterCustomer>("UserMasterDetails", new { ActionId = 5, EmailId = reg }).FirstOrDefault();
        }

        public List<UserRole> GetUserRole()
        {
            return Query<UserRole>("UserMasterDetails", new { ActionId = 7 }).ToList();
        }

        public void updatelastLoginDetail(int Id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", Id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("ActionId", 6, DbType.Int32, ParameterDirection.Input);

            Execute("UserMasterDetails", parameters);
        }

        public void ActivateAccountByUserEmail(string emailId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ActionId", 15, DbType.Int32, ParameterDirection.Input);
            parameters.Add("EmailId", emailId, DbType.String, ParameterDirection.Input);

            Execute("UserMasterDetails", parameters);
        }
        #endregion

        #region User profile
        public UserProfile GetUserProfile(int UserId)
        {
            return Query<UserProfile>("UserMasterDetails", new { ActionId = 9, UserId = UserId }).FirstOrDefault();
        }

        public void UpdateUserProfile(string Username, string EmailId, string PhoneNo, int UserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ActionId", 10, DbType.Int32, ParameterDirection.Input);
            parameters.Add("Name", Username, DbType.String, ParameterDirection.Input);
            parameters.Add("EmailId", EmailId, DbType.String, ParameterDirection.Input);
            parameters.Add("PhoneNo", PhoneNo, DbType.String, ParameterDirection.Input);
            parameters.Add("UserId", UserId, DbType.Int32, ParameterDirection.Input);

            Execute("UserMasterDetails", parameters);
        }

        public void UpdateUserPassword(string Password, int UserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ActionId", 11, DbType.Int32, ParameterDirection.Input);
            parameters.Add("UserId", UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("Password", Password, DbType.String, ParameterDirection.Input);

            Execute("UserMasterDetails", parameters);
        }
        #endregion

        #region user management
        public List<RegisterCustomer> GetAllUsersByRoleId(int UserRoleId)
        {
            return Query<RegisterCustomer>("UserMasterDetails", new { ActionId = 12, UserRoleId = UserRoleId }).ToList();
        }

        public List<RegisterCustomer> GetAllUsersByRoleIdWithPaging(int UserRoleId, int PageSize, int PageIndex)
        {
            return Query<RegisterCustomer>("UserMasterDetails", new { ActionId = 14, UserRoleId = UserRoleId, PageSize = PageSize, PageIndex = PageIndex }).ToList();
        }

        public void UpdateUserRecord(int UserId, string Username, string Email, string Phonenumber, bool IsPublished, bool IsDeleted)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ActionId", 13, DbType.Int32, ParameterDirection.Input);
            parameters.Add("UserId", UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("Name", Username, DbType.String, ParameterDirection.Input);
            parameters.Add("EmailId", Email, DbType.String, ParameterDirection.Input);
            parameters.Add("PhoneNo", Phonenumber, DbType.String, ParameterDirection.Input);
            parameters.Add("IsPublished", IsPublished, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("IsDeleted", IsDeleted, DbType.Boolean, ParameterDirection.Input);

            Execute("UserMasterDetails", parameters);
        }
        #endregion
    }
}