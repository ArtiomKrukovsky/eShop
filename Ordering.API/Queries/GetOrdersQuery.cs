using System;
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
    public class GetOrdersQuery : IQuery<OrderSummaryDTO>
    {
        public GetOrdersQuery()
        {
        }
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, OrderSummaryDTO>
    {
        private readonly string _connectionString;

        public GetOrdersQueryHandler(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }

        public async Task<OrderSummaryDTO> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_connectionString);
            connection.Open();

            return await connection.QueryAsync<OrderSummaryDTO>(
                @"SELECT o.[Id] as ordernumber,
                  o.[OrderDate] as [date],os.[Name] as [status],
                  SUM(oi.units*oi.unitprice) as total
                  FROM [ordering].[Orders] o
                  LEFT JOIN[ordering].[orderitems] oi ON  o.Id = oi.orderid
                  LEFT JOIN[ordering].[orderstatus] os on o.OrderStatusId = os.Id
                  GROUP BY o.[Id], o.[OrderDate], os.[Name]
                  ORDER BY o.[Id]") as OrderSummaryDTO;
        }
    }
}