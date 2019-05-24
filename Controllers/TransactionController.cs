using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using BinaryParkingClientServerNiverovskyi.Models.Transaction;

namespace BinaryParkingClientServerNiverovskyi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        private static List<Transaction> _transactions = new List<Transaction>();
        private static List<Transaction> _minuteTransactions = new List<Transaction>();

        public TransactionController()
        {
        }

        private static void AddToFile(Transaction transaction)
        {
            using (var file =
                new StreamWriter(@"FileTransactions/Transactions.log", true))
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

        [HttpGet("GetMinuteTransactions")]
        public ActionResult<IEnumerable<Transaction>> GetMinuteTransactions()
        {
            UpdateMinuteTransactions();
            return _minuteTransactions;
        }

        [HttpGet("GetTransactions")]
        public string GetTransactions()
        {
            var str = System.IO.File.ReadAllText(@"FileTransactions/Transactions.log");
            return str;
        }

        [HttpGet("MinuteSumOfIncome")]
        public double MinuteSumOfIncome()
        {
            UpdateMinuteTransactions();
            return _minuteTransactions.Sum(transaction => transaction._tariff);
        }
    }
}