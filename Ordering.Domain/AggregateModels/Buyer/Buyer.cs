using System;
using System.Collections.Generic;
using System.Linq;
using Ordering.Domain.Events;
using Ordering.Domain.Seedwork;

namespace Ordering.Domain.AggregateModels.Buyer
{
    public class Buyer : Entity, IAggregateRoot
    {
        public string IdentityGuid { get; private set; }
        public string Name { get; private set; }

        private readonly List<PaymentMethod> _paymentMethods;
        public IReadOnlyCollection<PaymentMethod> PaymentMethods => _paymentMethods;

        protected Buyer()
        {
            _paymentMethods = new List<PaymentMethod>();
        }

        public Buyer(string identity, string name)
        {
            IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
        }

        public PaymentMethod VerifyOrAddPaymentMethod(int cardTypeId, string alias, string cardNumber, 
            string securityNumber, string cardHolder, DateTime expiration, int orderId)
        {
            var existingPayment = _paymentMethods.SingleOrDefault(p =>
                p.IsEqualTo(cardTypeId, cardNumber, expiration));

            if (existingPayment != null)
            {
                AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, existingPayment, orderId));

                return existingPayment;
            }

            var payment = new PaymentMethod(alias, cardNumber, securityNumber, cardHolder, expiration, cardTypeId);

            _paymentMethods.Add(payment);

            AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, payment, orderId));

            return payment;
        }
    }
}