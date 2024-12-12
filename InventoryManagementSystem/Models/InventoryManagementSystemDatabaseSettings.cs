namespace InventoryManagementSystem.Models
{
    public class InventoryManagementSystemDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ProductCollectionName { get; set; } = null!;
        public string CategoryCollectionName { get; set; } = null!;
    }
}
