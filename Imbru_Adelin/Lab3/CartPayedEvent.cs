using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    [AsChoice]
    public static partial class CartPayedEvent
    {
        public interface ICartPayedEvent { }

        public record CartPaySucceededEvent : ICartPayedEvent 
        {
            public string Csv{ get;}
            public DateTime PaymentDate { get; }

            internal CartPaySucceededEvent(string csv, DateTime paymentDate)
            {
                Csv = csv;
                PaymentDate = paymentDate;
            }
        }

        public record CartPayFailedEvent : ICartPayedEvent 
        {
            public string Reason { get; }

            internal CartPayFailedEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}
