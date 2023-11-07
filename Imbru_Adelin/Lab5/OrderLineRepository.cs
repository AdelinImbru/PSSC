using LanguageExt;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static Lab5.Cart;
using static LanguageExt.Prelude;
using static Lab5.OrderContext;
using static Lab5.CalculatedFinalPrice;

namespace Lab5
{
    public class OrderLineRepository: IOrderLineRepository
    {
        private readonly OrderContext dbContext;

        public OrderLineRepository(OrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<CalculatedFinalPrice>> TryGetExistingOrder() => async () => (await (
                          from oh in dbContext.OrderHeader
                          from ol in dbContext.OrderLine
                          join p in dbContext.Product on ol.ProductId equals p.ProductId
                          select new { p.Code, ol.OrderLineId, ol.Quantity, ol.Price, oh.Address})
                          .AsNoTracking()
                          .ToListAsync())
                          .Select(result => new CalculatedFinalPrice(
                                                    ProductCode: new(result.Code),
                                                    Price: new(result.Price),
                                                    Quantity: new(result.Quantity),
                                                    Address: new(result.Address),
                                                    FinalPrice: result.Price*result.Quantity)
                          { 
                            OrderLineId = result.OrderId
                          })
                          .ToList();

        public TryAsync<Unit> TrySaveOrder(PaidCart cart) => async () =>
        {
            var students = (await dbContext.Product.ToListAsync()).ToLookup(product=>product.Code);
            var newCart = cart.ProductList
                                    .Where(c => c.IsUpdated && c.CartId == 0)
                                    .Select(c => new OrderLineDto()
                                    {
                                        ProductId = product[c.ProductCode.Value].Single().ProductId,
                                        Price = c.Price.Value,
                                        Quantity = c.Quantity.Value,
                                    });
            var updatedCart = cart.ProductList.Where(c => c.IsUpdated && c.CartId > 0)
                                    .Select(c => new OrderLineDto()
                                    {
                                        OrderLineId = c.CartId,
                                        ProductId = product[c.ProductCode.Value].Single().ProductId,
                                        Price = c.Price.Value,
                                        Quantity = c.Quantity.Value,
                                    });

            dbContext.AddRange(newCart);
            foreach (var entity in updatedCart)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }

            await dbContext.SaveChangesAsync();

            return unit;
        };
    }
}
