using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    [AsChoice]
    public static partial class CartPaidEvent
    {
        public interface ICartPaidEvent { }

        public record CartPaySucceededEvent : ICartPaidEvent 
        {
            public string Csv{ get;}
            public DateTime PaymentDate { get; }

            internal CartPaySucceededEvent(string csv, DateTime paymentDate)
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
