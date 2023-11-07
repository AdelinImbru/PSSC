using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    public interface IProductRepository
    {
        TryAsync<List<ProductCode>> TryGetExistingProduct(IEnumerable<string> productToCheck);
    }
}
