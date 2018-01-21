using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Marketplace.ProductRepo.Model;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace Marketplace.ProductRepo
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class ProductRepo : StatefulService, IProductRepoService
    {
        private IProductDB _store;

        public ProductRepo(StatefulServiceContext context)
            : base(context)
        { }

        public async Task<int> AddProductAync(Product prod)
        {
            await _store.AddProductAync(prod);
            return 0;
        }

        public async Task<IEnumerable<Product>> getAllProductAsync()
        {
            return await _store.getAllProductAsync();
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
           {
                new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context))
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            _store = new ProductDB(this.StateManager);

            var prod1 = new Product() {Id = Guid.NewGuid(), Name="Choclate", Description="Nestle choclate", IsAvailable= true,Price=10 };
            var prod2 = new Product() { Id = Guid.NewGuid(), Name = "SHoes", Description = "Adidas", IsAvailable = true, Price = 10 };
            var prod3 = new Product() { Id = Guid.NewGuid(), Name = "T-Shirt", Description = "Benetton", IsAvailable = false, Price = 10 };

            await _store.AddProductAync(prod1);
            await _store.AddProductAync(prod2);
            await _store.AddProductAync(prod3);

            IEnumerable<Product> prodts = await _store.getAllProductAsync();
        }
    }
}
