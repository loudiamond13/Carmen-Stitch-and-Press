using CSP.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.DAL.Interceptors
{
    public class OrderAuditAndTotalInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderAuditAndTotalInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCurrentUser()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null)
                return await base.SavingChangesAsync(eventData, result, cancellationToken);

            var now = DateTime.UtcNow;
            var user = GetCurrentUser();

            int? orderId = null;
            decimal deletedPaymentAmt = 0;

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                switch (entry.Entity)
                {
                    case Order order:
                        if (entry.State == EntityState.Added)
                        {
                            order.CreatedBy = user;
                            order.OrderDate = now;
                            order.IsOpen = true;
                            order.IsDeleted = false;
                        }
                        else if (entry.State == EntityState.Modified)
                        {
                            order.UpdatedBy = user;
                        }

                        orderId = order.OrderId;
                        break;

                    case OrderItem item:
                        if (entry.State == EntityState.Added)
                        {
                            item.CreatedBy = user;
                            item.IsDone = false;
                        }

                        orderId = item.OrderId;
                        break;

                    case Payment payment:
                        if (entry.State == EntityState.Added )
                        {
                            payment.CreatedBy = user;
                            payment.PaymentDate = now;
                        }
                        else if (entry.State == EntityState.Modified && payment.IsDeleted == true)
                        {
                            payment.DeletedBy = user;
                            deletedPaymentAmt = payment.Amount;
                        }
                        else if (entry.State == EntityState.Modified)
                        {
                            payment.UpdatedBy = user;
                        }

                        orderId = payment.OrderId;
                        break;

                    case Discount discount:
                        if (entry.State == EntityState.Added)
                        {
                            discount.DiscountedBy = user;
                        }
                        else if (entry.State == EntityState.Modified)
                        {
                            discount.UpdatedBy = user;
                        }

                        orderId = discount.OrderId;
                        break;

                    case Expense expense:
                        if (entry.State == EntityState.Added)
                        {
                            expense.SpendDate = now;
                        }
                        orderId = expense.OrderId;
                        break;
                    case MoneyTransfer moneyTransfer:
                        if (entry.State == EntityState.Added)
                        { 
                            moneyTransfer.TransferDate = now;
                        }
                        break;
                }
            }

            if (orderId.HasValue)
            {
                var orderEntry = context.ChangeTracker.Entries<Order>()
                                        .FirstOrDefault(e => e.Entity.OrderId == orderId.Value);

                Order? order;

                if (orderEntry != null && orderId == 0)
                {
                    order = orderEntry.Entity;

                    //get related tracked child entities for this new order
                    var orderItems = context.ChangeTracker.Entries<OrderItem>()
                        .Where(e => e.Entity.OrderId == order.OrderId)
                        .Select(e => e.Entity);

                    var payments = context.ChangeTracker.Entries<Payment>()
                        .Where(e => e.Entity.OrderId == order.OrderId && !e.Entity.IsDeleted)
                        .Select(e => e.Entity);

                    var discounts = context.ChangeTracker.Entries<Discount>()
                        .Where(e => e.Entity.OrderId == order.OrderId)
                        .Select(e => e.Entity);

                    var expenses = context.ChangeTracker.Entries<Expense>()
                        .Where(e => e.Entity.OrderId == order.OrderId)
                        .Select(e => e.Entity);

                    //calculate totals from tracked entities
                    order.TotalDiscount = discounts.Sum(d => d.Amount);
                    order.TotalAmount = orderItems.Sum(i => i.Price * i.Quantity) - order.TotalDiscount;
                    order.PaidAmount = payments.Sum(p => p.Amount);
                    order.TotalBalance = order.TotalAmount  - order.PaidAmount;
                    order.TotalExpenses = expenses.Sum(i => i.Amount);

                    order.UpdatedBy = user;

                }
                else
                {
                    // fallback: load from DB for existing orders
                    order = await context.Set<Order>()
                        .Include(o => o.OrderItems)
                        .Include(o => o.Payments.Where(p => !p.IsDeleted))
                        .Include(o => o.Discounts)
                        .Include(o => o.Expenses)
                        .FirstOrDefaultAsync(o => o.OrderId == orderId.Value, cancellationToken);

                    if (order != null)
                    {
                        order.TotalDiscount = order.Discounts.Sum(d => d.Amount);
                        order.TotalAmount = order.OrderItems.Sum(i => i.Price * i.Quantity) - order.TotalDiscount;
                        order.PaidAmount = order.Payments.Sum(p => p.Amount) - deletedPaymentAmt; 
                        order.TotalBalance = order.TotalAmount - order.PaidAmount;
                        order.TotalExpenses = order.Expenses.Sum(i => i.Amount);

                        order.UpdatedBy = user;

                    }
                }
            }
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
