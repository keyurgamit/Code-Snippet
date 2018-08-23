using EntityModel.DomainModel;
using System.Collections.Generic;

namespace DataAccess.Interface
{
    public partial interface ISavedSearchRepository : IRepositoryBase
    {
        int SaveSearch(SearchProperty objSearchProperty);

        SearchProperty SearchResultsByUserId(int UserId);
    }
}