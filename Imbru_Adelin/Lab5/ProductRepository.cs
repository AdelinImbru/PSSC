using LanguageExt;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lab5
{
    public class ProductRepository: IProductRepository
    {
        private readonly OrderContext orderContext;

        public ProductRepository(OrderContext orderContext)
        {
            this.orderContext = orderContext;  
        }

        public TryAsync<List<ProductCode>> TryGetExistingProduct(IEnumerable<string> productToCheck) => async () =>
        {
            var product = await orderContext.Product
                                              .Where(product => productToCheck.Contains(product.Code))
                                              .AsNoTracking()
                                              .ToListAsync();
            return product.Select(product => new ProductCode(product.Code))
                           .ToList();
        };
    }
}
