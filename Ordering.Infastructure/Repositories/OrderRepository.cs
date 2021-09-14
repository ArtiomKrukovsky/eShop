﻿using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregateModels.Ordering;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ordering.Domain.Seedwork;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderingContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public OrderRepository(OrderingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetAsync(int orderId)
        {
            var order = await _context.Orders.Include(x => x.Address)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                order = _context.Orders.Local
                    .FirstOrDefault(o => o.Id == orderId);
            }

            if (order != null)
            {
                await _context.Entry(order)
                    .Collection(i => i.OrderItems).LoadAsync();
                await _context.Entry(order)
                    .Reference(i => i.OrderStatus).LoadAsync();
            }

            return order;
        }

        public void Update(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
