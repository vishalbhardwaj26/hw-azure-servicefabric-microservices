using Marketplace.ProductRepo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.ProductRepo
{
    interface IProductDB
    {
        Task<int> AddProductAync(Product prod);
        Task<IEnumerable<Product>> getAllProductAsync();
    }
}
