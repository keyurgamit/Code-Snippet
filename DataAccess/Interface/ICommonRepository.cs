using EntityModel.DomainModel;
using System.Collections.Generic;

namespace DataAccess.Interface
{
    public partial interface ICommonRepository : IRepositoryBase
    {
        RegisterCustomer getUserRolebyId(int Userid);

        #region Homepage CMS
        ContentManagement getHomePageCms();

        void UpdateHomePageCms(ContentManagement objContentManagement);
        #endregion

        #region Terms & Conditions
        ContentManagement getTermsConditionsPageCms();
        void UpdateTermsConditionsPageCms(ContentManagement objContentManagement);
        #endregion
    }
}