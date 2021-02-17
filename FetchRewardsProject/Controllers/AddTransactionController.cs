using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FetchRewardsProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddTransactionController : ControllerBase
    {
        [HttpPost]
        public ActionResult<string> PostTransaction([FromBody] object transaction)
        {

            using StreamWriter streamWriter = new StreamWriter("Transactions.txt", append: true);
            streamWriter.WriteLineAsync(transaction.ToString());
            if (System.IO.File.Exists("Balances.txt"))
            {
                System.IO.File.Delete("Balances.txt");
            }
            return "Transaction Added";
            
        }
    }
}