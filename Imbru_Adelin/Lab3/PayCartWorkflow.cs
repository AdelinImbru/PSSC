using System;
using static Lab3.CartPaidEvent;
using static Lab3.CartOperation;
using static Lab3.Cart;

namespace Lab3
{
    public class PayCartWorkflow
    {
        public ICartPaidEvent Execute(PayCartCommand command, Func<ProductCode, bool> checkProductExists)
        {
            EmptyCart emptyCart = new EmptyCart(command.InputCart);
            ICart products = ValidateCart(checkProductExists, emptyCart);
            products = CalculateFinalPrice(products);
            products = PayCart(products);

            return products.Match(
                    whenEmptyCart: emptyCart => new CartPayFailedEvent("Unexpected unvalidated state") as ICartPaidEvent,
                    whenInvalidatedCart: invalidCart => new CartPayFailedEvent(invalidCart.Reason),
                    whenValidatedCart: validatedCart => new CartPayFailedEvent("Unexpected validated state"),
                    whenCalculatedCart: calculatedCart => new CartPayFailedEvent("Unexpected calculated state"),
                    whenPaidCart: paidCart => new CartPaySucceededEvent(paidCart.Csv, paidCart.PaymentDate)
                );
        }
    }
}
