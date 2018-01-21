using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.ProductRepo.Model
{
    public interface IProductRepoService : IService
    {
        Task<int> AddProductAync(Product prod);
        Task<IEnumerable<Product>> getAllProductAsync();
    }
}
