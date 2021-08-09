using FluentValidation;
using MediatR;
using Ordering.API.Models;
using Ordering.Domain.AggregateModels.Ordering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Commands
{
    [DataContract]
    public class CreateOrderCommand : IRequest<bool>
    {
        [DataMember]
        private readonly List<OrderItemModel> _orderItems;

        [DataMember]
        public string UserId { get; private set; }

        [DataMember]
        public string UserName { get; private set; }

        [DataMember]
        public string City { get; private set; }

        [DataMember]
        public string Street { get; private set; }

        [DataMember]
        public string State { get; private set; }

        [DataMember]
        public string Country { get; private set; }

        [DataMember]
        public string ZipCode { get; private set; }

        [DataMember]
        public string CardNumber { get; private set; }

        [DataMember]
        public string CardHolderName { get; private set; }

        [DataMember]
        public DateTime CardExpiration { get; private set; }

        [DataMember]
        public string CardSecurityNumber { get; private set; }

        [DataMember]
        public int CardTypeId { get; private set; }

        [DataMember]
        public IEnumerable<OrderItemModel> OrderItems => _orderItems;

        public CreateOrderCommand()
        {
            _orderItems = new List<OrderItemModel>();
        }

        public CreateOrderCommand(List<OrderItemModel> orderItems, string userId, string userName, string city, string street, string state, string country, string zipcode,
            string cardNumber, string cardHolderName, DateTime cardExpiration, string cardSecurityNumber, int cardTypeId): this()
        {
            _orderItems = orderItems;
            UserId = userId;
            UserName = userName;
            City = city;
            Street = street;
            State = state;
            Country = country;
            ZipCode = zipcode;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
            CardSecurityNumber = cardSecurityNumber;
            CardTypeId = cardTypeId;
            CardExpiration = cardExpiration;
        }

        public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
        {
            public CreateOrderCommandValidator()
            {
                RuleFor(command => command.City).NotEmpty();
                RuleFor(command => command.Street).NotEmpty();
                RuleFor(command => command.State).NotEmpty();
                RuleFor(command => command.Country).NotEmpty();
                RuleFor(command => command.ZipCode).NotEmpty();
                RuleFor(command => command.CardNumber).NotEmpty().Length(12, 19);
                RuleFor(command => command.CardHolderName).NotEmpty();
                RuleFor(command => command.CardExpiration).NotEmpty().Must(BeValidExpirationDate).WithMessage("Please specify a valid card expiration date");
                RuleFor(command => command.CardSecurityNumber).NotEmpty().Length(3);
                RuleFor(command => command.CardTypeId).NotEmpty();
                RuleFor(command => command.OrderItems).Must(ContainOrderItems).WithMessage("No order items found");
            }

            private bool BeValidExpirationDate(DateTime dateTime)
            {
                return dateTime >= DateTime.UtcNow;
            }

            private bool ContainOrderItems(IEnumerable<OrderItemModel> orderItems)
            {
                return orderItems.Any();
            }
        }

        public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
        {
            private readonly IOrderRepository _orderRepository;

            public CreateOrderCommandHandler(IOrderRepository orderRepository)
            {
                _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            }

            public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                var address = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode);
                var order = new Order(request.UserId, request.UserName, address, request.CardTypeId, request.CardNumber, request.CardSecurityNumber, request.CardHolderName, request.CardExpiration);

                foreach (var item in request.OrderItems)
                {
                    order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
                }

                await _orderRepository.AddAsync(order);

                return true;
            }
        }
    }
}
