using CSP.Domain.Entities;
using CSP.Domain.IRepositories;
using CSP.Domain.Logics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Domain.Logics
{
    public class ExpenseLogic :IExpenseLogic
    {
        public readonly IExpenseRepository _expensesRepository;

        public ExpenseLogic(IExpenseRepository expensesRepository)
        {
            _expensesRepository = expensesRepository;
        }
        #region create expense
        public async Task CreateExpenseAsync(Expense expense) 
        {
            if (expense.ExpensesId == 0 && expense != null) 
            {
                await _expensesRepository.AddAsync(expense);
                await _expensesRepository.SaveAsync();
            }
        }
        #endregion        
        #region get all expenses by order id 
        public async Task<List<Expense>> GetExpensesByOrderIdAsync(int orderId) 
        {
            if (orderId != 0) 
            {
                List<Expense> expenses = await _expensesRepository.FindByAsync(x=>x.OrderId == orderId);
                if (expenses != null) 
                {
                    return expenses;
                }
                return null;
            }
            return null;
        }
        #endregion
        #region Delete Expense
        public async Task<bool> DeleteExpenseAsync(int expenseId) 
        {
            if (expenseId != 0) 
            {
                var expense = await _expensesRepository.GetAsync(x=>x.ExpensesId == expenseId);
                if (expense != null) 
                {
                    _expensesRepository.Delete(expense);
                    await _expensesRepository.SaveAsync();
                    return true;
                }
                return false;
            }
            return false;
        }
        #endregion
        #region get all expenses
        public async Task<List<Expense>> GetAllExpensesAsync() 
        {
            return await _expensesRepository.GetAllAsync();
        }
        #endregion
        #region get all company expenses
        public async Task<List<Expense>> GetAllCompanyExpenses() 
        {
            return await _expensesRepository.FindByAsync(x=>x.IsCompanyExpenses);
        }
        #endregion
    }
}
