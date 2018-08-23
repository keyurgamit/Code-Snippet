namespace EntityModel.DomainModel
{
    public class CityMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProvinceId { get; set; }
        public bool IsPublished { get; set; }
    }
}
