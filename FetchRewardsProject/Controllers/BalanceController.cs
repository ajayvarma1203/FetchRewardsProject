using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FetchRewardsProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetBalance()
        {
            string readText = System.IO.File.ReadAllText("Balances.txt");
            string[] balances = readText.Split('\n');
            List<PayerAccount> allAccounts = new List<PayerAccount>();
            for(int i=0; i<balances.Length-1; i++)
            {
                string[] currentBalance = balances[i].Substring(1, balances[i].Length-2).Split(", ");
                PayerAccount payerAccount = new PayerAccount { payer = currentBalance[0], points = Int32.Parse(currentBalance[1]) };
                allAccounts.Add(payerAccount);
            }
            if (System.IO.File.Exists("Transactions.txt"))
            {
                System.IO.File.Delete("Transactions.txt");
            }
            return JsonConvert.SerializeObject(allAccounts);
        }
    }
}