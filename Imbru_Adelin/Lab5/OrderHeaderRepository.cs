using LanguageExt;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lab5
{
    public class OrderHeaderRepository: IOrderHeaderRepository
    {
        private readonly OrderContext orderContext;

        public OrderHeaderRepository(OrderContext orderContext)
        {
            this.orderContext = orderContext;  
        }

        public TryAsync<List<Address>> TryGetExistingOrderHeader(IEnumerable<string> orderHeaderToCheck) => async () =>
        {
            var orderHeader = await orderContext.OrderHeader
                                              .Where(orderHeader => orderHeaderToCheck.Contains(orderHeader.Address))
                                              .AsNoTracking()
                                              .ToListAsync();
            return orderHeader.Select(orderHeader => new Address(orderHeader.Address))
                           .ToList();
        };
    }
}
