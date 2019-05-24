using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using BinaryParkingClientServerNiverovskyi.Models.Transaction;

namespace BinaryParkingClientServerNiverovskyi.Controllers
{
    public class TransactionController : Controller
    {
        private TransactionContext _transactionContext;
        private readonly IHostingEnvironment _appEnvironment;

        public TransactionController(IHostingEnvironment appEnvironment, TransactionContext transactionContext)
        {
            _appEnvironment = appEnvironment;
            _transactionContext = transactionContext;
        }

        private void AddToFile(Transaction transaction)
        {
            using (var file =
                new System.IO.StreamWriter(@"Transactions.log", true))
            {
                file.WriteLine($"{transaction._dateTime} - ID:{transaction.Id} - Tariff: {transaction._tariff}");
            }
        }

        public void Add(Transaction transaction)
        {
            _transactionContext.Transactions.Add(transaction);
            _transactionContext.MinuteTransactions.Add(transaction);
            _transactionContext.SaveChanges();
            AddToFile(transaction);
        }

        private void UpdateMinuteTransactions()
        {
            if (!_transactionContext.MinuteTransactions.Any()) return;
            while ((DateTime.Now - _transactionContext.MinuteTransactions.ToList()[0]._dateTime).TotalMinutes > 1)
            {
                _transactionContext.MinuteTransactions.Remove(_transactionContext.MinuteTransactions.ToList()[0]);
            }

            _transactionContext.SaveChanges();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Transaction>> GetMinuteTransactions()
        {
            UpdateMinuteTransactions();
            return _transactionContext.MinuteTransactions.ToList();
        }

        [HttpGet]
        public IActionResult GetFile()
        {
            return PhysicalFile(Path.Combine(_appEnvironment.ContentRootPath, "Files/Transactions.log"),
                "application/log",
                "Transactions.log");
        }

        [HttpGet]
        public double MinuteSumOfIncome()
        {
            UpdateMinuteTransactions();
            return Enumerable.Sum(_transactionContext.MinuteTransactions, transaction => transaction._tariff);
        }
    }
}