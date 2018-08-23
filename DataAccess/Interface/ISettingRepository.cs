namespace DataAccess.Interface
{
    public partial interface ISettingRepository : IRepositoryBase
    {
        string GetValueBySettingName(string SettingName);
    }
}