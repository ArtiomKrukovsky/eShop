using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Ordering.API.DTOs;
using Ordering.API.Interfaces;

namespace Ordering.API.Queries
{
    [DataContract]
    public class GetOrderQuery : IQuery<OrderDTO>
    {
        [DataMember]
        public int OrderId { get; set; }

        public GetOrderQuery(int orderId)
        {
            OrderId = orderId;
        }
    }

    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderDTO>
    {
        private readonly string _connectionString;

        public GetOrderQueryHandler(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }

        public async Task<OrderDTO> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var result = await connection.QueryAsync<dynamic>(
                @"select o.[Id] as ordernumber,o.OrderDate as date, o.Description as description,
                        o.Address_City as city, o.Address_Country as country, o.Address_State as state, o.Address_Street as street, o.Address_ZipCode as zipcode,
                        os.Name as status, 
                        oi.ProductName as productname, oi.Units as units, oi.UnitPrice as unitprice, oi.PictureUrl as pictureurl
                        FROM ordering.Orders o
                        LEFT JOIN ordering.Orderitems oi ON o.Id = oi.orderid 
                        LEFT JOIN ordering.orderstatus os on o.OrderStatusId = os.Id
                        WHERE o.Id=@id"
                , new { request.OrderId }
            );

            if (result.AsList().Count == 0)
                throw new KeyNotFoundException();

            return MapOrderItems(result);
        }

        private static OrderDTO MapOrderItems(dynamic result)
        {
            var order = new OrderDTO
            {
                OrderNumber = result[0].ordernumber,
                Date = result[0].date,
                Status = result[0].status,
                Description = result[0].description,
                Street = result[0].street,
                City = result[0].city,
                Zipcode = result[0].zipcode,
                Country = result[0].country,
                OrderItems = new List<OrderItemDTO>(),
                Total = 0
            };

            foreach (var item in result)
            {
                var orderItem = new OrderItemDTO
                {
                    ProductName = item.productname,
                    Units = item.units,
                    UnitPrice = (double)item.unitprice,
                    PictureUrl = item.pictureurl
                };

                order.Total += item.units * item.unitprice;
                order.OrderItems.Add(orderItem);
            }

            return order;
        }
    }
}