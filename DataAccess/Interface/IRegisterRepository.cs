using EntityModel.DomainModel;
using System.Collections.Generic;

namespace DataAccess.Interface
{
    public partial interface IRegisterRepository : IRepositoryBase
    {
        int AddUpdateRegisterCustomer(RegisterCustomer objRegister);

        #region user details
        RegisterCustomer GetLoginDetails(RegisterCustomer reg);

        RegisterCustomer GetUserDetails(string reg);

        List<UserRole> GetUserRole();

        void updatelastLoginDetail(int Id);

        void ActivateAccountByUserEmail(string emailId);
        #endregion

        #region user profile
        UserProfile GetUserProfile(int UserId);

        void UpdateUserProfile(string Username, string EmailId, string PhoneNo, int UserId);

        void UpdateUserPassword(string Password, int UserId);
        #endregion

        #region user management
        List<RegisterCustomer> GetAllUsersByRoleId(int UserRoleId);

        List<RegisterCustomer> GetAllUsersByRoleIdWithPaging(int UserRoleId, int PageSize, int PageIndex);

        void UpdateUserRecord(int UserId, string Username, string Email, string Phonenumber, bool IsPublished, bool IsDeleted);
        #endregion
    }
}