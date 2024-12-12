using System.Text.Json.Serialization;

namespace InventoryManagementSystem.Domain.Entities
{

    public class Product
    {
        public int Id { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public Product() { }

        public Product(int id, string name, string description, decimal price, int quantity, int categoryId)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
            CategoryId = categoryId;
        }
    }
}
