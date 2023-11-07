using System;
using static Lab5.CartPaidEvent;
using static Lab5.CartOperation;
using static Lab5.Cart;
using static Lab5.ProductCode;
using static Lab5.PayCartWorkflow;
using static Lab5.IProductRepository;
using static Lab5.IOrderLineRepository;
using static Lab5.IOrderHeaderRepository;
using Microsoft.Extensions.Logging;
using static LanguageExt.Prelude;
using LanguageExt;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Lab5
{
    public class PayCartWorkflow
    {
        private readonly IProductRepository productRepository;
        private readonly IOrderLineRepository orderLineRepository;
        private readonly IOrderHeaderRepository orderHeaderRepository;
        private readonly ILogger<PayCartWorkflow> logger;

        public PayCartWorkflow(IProductRepository productRepository, IOrderLineRepository orderLineRepository, IOrderHeaderRepository orderHeaderRepository, ILogger<PayCartWorkflow> logger)
        {
            this.productRepository = productRepository;
            this.orderLineRepository = orderLineRepository;
            this.orderHeaderRepository = orderHeaderRepository;
            this.logger = logger;
        }

        public async Task<ICartPaidEvent> ExecuteAsync(PayCartCommand command)
        {
            EmptyCart emptyCart = new EmptyCart(command.InputCart);

            var result = from product in productRepository.TryGetExistingProduct(emptyCart.ProductList.Select(cart => cart.ProductCode))
                                          .ToEither(c => new InvalidatedCart(emptyCart.ProductList, c) as ICart)
                         from existingOrder in orderLineRepository.TryGetExistingOrder()
                                          .ToEither(c => new InvalidatedCart(emptyCart.ProductList, c) as ICart)
                         let checkProductExists = (Func<ProductCode, Option<ProductCode>>)(order => CheckProductExists(product, order))
                         from paidCart in ExecuteWorkflowAsync(emptyCart, existingOrder, checkProductExists).ToAsync()
                         from _ in orderLineRepository.TrySaveOrder(paidCart)
                                          .ToEither(c => new InvalidatedCart(emptyCart.ProductList, c) as ICart)
                         select paidCart;

            return await result.Match(
                    Left: cart => GenerateFailedEvent(cart) as ICartPaidEvent,
                    Right: paidCart => new CartPaySuccededEvent(paidCart.Csv, paidCart.PaymentDate)
                );
        }

        private async Task<Either<ICart, PaidCart>> ExecuteWorkflowAsync(EmptyCart emptyCart, 
                                                                                          IEnumerable<CalculatedFinalPrice> existingOrder, 
                                                                                          Func<ProductCode, Option<ProductCode>> checkProductExists)
        {
            
            ICart orders = await ValidateCart(checkProductExists, emptyCart);
            orders = CalculateFinalPrice(orders);
            orders = MergeProducts(orders, existingOrder);
            orders = PayCart(orders);

            return orders.Match<Either<ICart, PaidCart>>(
                whenEmptyCart: emptyCart => Left(emptyCart as ICart),
                whenCalculatedCart: calculatedCart => Left(calculatedCart as ICart),
                whenInvalidatedCart: invalidCart => Left(invalidCart as ICart),
                whenValidatedCart: validatedCart => Left(validatedCart as ICart),
                whenPaidCart: paidCart => Right(paidCart)
            );
        }

        private Option<ProductCode> CheckProductExists(IEnumerable<ProductCode> products, ProductCode productCode)
        {
            if(products.Any(p=>p == productCode))
            {
                return Some(productCode);
            }
            else
            {
                return None;
            }
        }

        private ICartPaidEvent GenerateFailedEvent(ICart orders) =>
            orders.Match<CartPayFailedEvent>(
                whenEmptyCart: emptyCart => new($"Invalid state {nameof(EmptyCart)}"),
                whenInvalidatedCart: invalidatedCart => new(invalidatedCart.Reason),
                whenValidatedCart: validatedCart => new($"Invalid state {nameof(ValidatedCart)}"),
                whenCalculatedCart: calculatedCart => new($"Invalid state {nameof(CalculatedCart)}"),
                whenPaidCart: paidCart => new($"Invalid state {nameof(PaidCart)}"));
    }
}
