using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    [AsChoice]
    public static partial class CartPaidEvent
    {
        public interface ICartPaidEvent { }

        public record CartPaySuccededEvent : ICartPaidEvent 
        {
            public string Csv{ get;}
            public DateTime PaymentDate { get; }

            internal CartPaySuccededEvent(string csv, DateTime paymentDate)
            {
                Csv = csv;
                PaymentDate = paymentDate;
            }
        }

        public record CartPayFailedEvent : ICartPaidEvent 
        {
            public string Reason { get; }

            internal CartPayFailedEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}
