namespace API.DTOs.CategoryDTOs
{
    public class UpsertCategoryDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public int[] Picture { get; set; }
    }
}
