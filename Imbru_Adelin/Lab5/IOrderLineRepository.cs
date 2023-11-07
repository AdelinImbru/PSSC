using LanguageExt;
using System.Collections.Generic;
using static Lab5.Cart;

namespace Lab5
{
    public interface IOrderLineRepository
    {
        TryAsync<List<CalculatedFinalPrice>> TryGetExistingOrder();

        TryAsync<Unit> TrySaveOrder(PaidCart cart);
    }
}
