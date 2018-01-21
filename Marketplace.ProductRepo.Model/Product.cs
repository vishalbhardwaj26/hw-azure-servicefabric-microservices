using System;

namespace Marketplace.ProductRepo.Model
{
    public class Product
    {
        public Guid Id {get; set;}
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public int Price { get; set; }
    }
}
