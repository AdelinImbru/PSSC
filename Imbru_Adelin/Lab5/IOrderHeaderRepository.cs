using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    public interface IOrderHeaderRepository
    {
        TryAsync<List<Address>> TryGetExistingOrderHeader(IEnumerable<string> orderHeaderToCheck);
    }
}
