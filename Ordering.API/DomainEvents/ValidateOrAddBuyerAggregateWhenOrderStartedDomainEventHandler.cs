using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.AggregateModels.Buyer;
using Ordering.Domain.Events;

namespace Ordering.API.DomainEvents
{
    public class ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler 
        : INotificationHandler<OrderStartedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IBuyerRepository _buyerRepository;

        public ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler(IBuyerRepository buyerRepository, ILoggerFactory logger)
        {
            _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(OrderStartedDomainEvent orderStartedEvent, CancellationToken cancellationToken)
        {
            var cardTypeId = (orderStartedEvent.CardTypeId != 0) ? orderStartedEvent.CardTypeId : 1;
            var buyer = await _buyerRepository.FindAsync(orderStartedEvent.UserId);
            var buyerOriginallyExisted = (buyer != null);

            if (!buyerOriginallyExisted)
            {
                buyer = new Buyer(orderStartedEvent.UserId, orderStartedEvent.UserName);
            }

            buyer.VerifyOrAddPaymentMethod(cardTypeId,
                $"Payment Method on {DateTime.UtcNow}",
                orderStartedEvent.CardNumber,
                orderStartedEvent.CardSecurityNumber,
                orderStartedEvent.CardHolderName,
                orderStartedEvent.CardExpiration,
                orderStartedEvent.Order.Id);

            var buyerUpdated = buyerOriginallyExisted ?
                _buyerRepository.Update(buyer) :
                _buyerRepository.Add(buyer);

            await _buyerRepository.UnitOfWork
                .SaveEntitiesAsync(cancellationToken);

            _logger.CreateLogger<ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler>()
                .LogTrace("Buyer {BuyerId} and related payment method were validated or updated for orderId: {OrderId}.",
                    buyerUpdated.Id, orderStartedEvent.Order.Id);
        }
    }
}