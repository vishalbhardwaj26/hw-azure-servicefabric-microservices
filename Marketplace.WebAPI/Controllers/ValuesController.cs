using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Marketplace.WebAPI.Model;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Client;
using Marketplace.ProductRepo.Model;

namespace Marketplace.WebAPI.Controllers
{
   
       [Route("api/[controller]")]
    public class ProductController : Controller
    {
        // GET api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        private readonly IProductRepoService _prodService;

        public ProductController()
        {
            _prodService = ServiceProxy.Create<IProductRepoService>(new Uri("fabric:/Marketplace/ProductRepo"),new ServicePartitionKey(0));
        }
        

        [HttpGet]
        public async Task<IEnumerable<APIProduct>> GetAsync()
        {
            IEnumerable<Product> allProducts = await _prodService.getAllProductAsync();

            return allProducts.Select(p => new APIProduct
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                IsAvailable = p.IsAvailable
            });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody]APIProduct product)
        {
            var newProduct = new Product()
            {
                Id = Guid.NewGuid(),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                IsAvailable = true
            };

            await _prodService.AddProductAync(newProduct);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
