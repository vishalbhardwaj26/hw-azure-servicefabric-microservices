using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketplace.ProductRepo.Model;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System.Threading;

namespace Marketplace.ProductRepo
{
    public class ProductDB : IProductDB
    {
        private IReliableStateManager _stateManager;

        public ProductDB(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }
       

        public async Task<int> AddProductAync(Product prod)
        {
            IReliableDictionary<Guid, Product> dict = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid,Product>>("produts");

            using (var tx = _stateManager.CreateTransaction())
            {
                await dict.AddOrUpdateAsync(tx, prod.Id, prod, (id, value) => prod);
                await tx.CommitAsync();
            }

                return 0;
        }
         
        public async Task<IEnumerable<Product>> getAllProductAsync()
        {
            IReliableDictionary<Guid, Product> dict = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>("produts");
            var result = new List<Product>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allProducts = await dict.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumaerator = allProducts.GetAsyncEnumerator())
                {
                    while(await enumaerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, Product> current = enumaerator.Current;
                        result.Add(current.Value);

                    }
                }
            }
            return result;
        }
    }
}
