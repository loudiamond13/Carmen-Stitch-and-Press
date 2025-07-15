using CSP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Domain.Logics.Interfaces
{
    public interface IExpenseLogic
    {
        Task CreateExpenseAsync(Expense expense);
        Task<List<Expense>> GetExpensesByOrderIdAsync(int orderId);
        Task<bool> DeleteExpenseAsync(int expenseId);
        Task<List<Expense>> GetAllExpensesAsync();

        Task<List<Expense>> GetAllCompanyExpenses();

    }
}
