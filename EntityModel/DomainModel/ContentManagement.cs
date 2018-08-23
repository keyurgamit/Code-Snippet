namespace EntityModel.DomainModel
{
    public class ContentManagement
    {
        #region Content Management
        public int Id { get; set; }

        public string Name { get; set; }

        //[AllowHtml]
        public string Value { get; set; }
        #endregion
    }
}