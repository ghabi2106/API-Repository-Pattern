using DLL.Models;
using DLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface ITransactionService
    {
        Task FinancialTransaction();
    }

    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task FinancialTransaction()
        {
            //var rand = new Random();
            //var amount = rand.Next(1000);
            //var transaction = new TransactionHistory()
            //{
            //    Amount = amount
            //};
            //var CustomerBalance = await _unitOfWork.CustomerBalanceRepository
            //    .FindAsync(x => x.Email == "ghabiassaad@gmail.com");
            //CustomerBalance.Balance += amount;
            //await _unitOfWork.TransactionHistoryRepository.CreateAsync(transaction);
            //_unitOfWork.CustomerBalanceRepository.Update(CustomerBalance);
            //await _unitOfWork.SaveAsync();


            var rand = new Random();
            var amount = rand.Next(1000);
            var transaction = new TransactionHistory()
            {
                Amount = amount
            };
            await _unitOfWork.TransactionHistoryRepository.CreateAsync(transaction);
            if (await _unitOfWork.SaveAsync())
            {
                await _unitOfWork.CustomerBalanceRepository.MustUpdateBalanceAsync("ghabiassaad@gmail.com", amount);
            };


        }
    }
}
