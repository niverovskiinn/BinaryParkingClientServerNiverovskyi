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
        private static List<Transaction> _transactions = new List<Transaction>();
        private static List<Transaction> _minuteTransactions = new List<Transaction>();

        private readonly IHostingEnvironment _appEnvironment;

        public TransactionController(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        private static void AddToFile(Transaction transaction)
        {
            using (var file =
                new StreamWriter(@"Transactions.log", true))
            {
                file.WriteLine($"{transaction._dateTime} - ID:{transaction.Id} - Tariff: {transaction._tariff}");
            }
        }

        public static void Add(Transaction transaction)
        {
            _transactions.Add(transaction);
            _minuteTransactions.Add(transaction);
            AddToFile(transaction);
        }

        private void UpdateMinuteTransactions()
        {
            if (!_minuteTransactions.Any()) return;
            while ((DateTime.Now - _minuteTransactions[0]._dateTime).TotalMinutes > 1)
            {
                _minuteTransactions.Remove(_minuteTransactions.ToList()[0]);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Transaction>> GetMinuteTransactions()
        {
            UpdateMinuteTransactions();
            return _minuteTransactions;
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
            return _minuteTransactions.Sum(transaction => transaction._tariff);
        }
    }
}