using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FetchRewardsProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpendController : ControllerBase
    {
        [HttpPost]
        public ActionResult<string> Spend([FromBody] object points)
        {
            int p = (int)JObject.Parse(points.ToString())["points"];

            string readText = System.IO.File.ReadAllText("Transactions.txt");
            string[] transactions = readText.Split('\n');
            Dictionary<string, int> totalPoints = new Dictionary<string, int>();
            for (int i = 0; i < transactions.Length - 1; i++)
            {
                var a = JObject.Parse(transactions[i]);
                if (!totalPoints.ContainsKey((string)a["payer"]))
                    totalPoints.Add((string)a["payer"], (int)a["points"]);
                else
                    totalPoints[(string)a["payer"]] = totalPoints[(string)a["payer"]] + (int)a["points"];
            }
            Dictionary<DateTime, int> toSort1 = new Dictionary<DateTime, int>();
            
            for(int i=0; i<transactions.Length-1; i++) 
            {
                var a = JObject.Parse(transactions[i]);
                toSort1.Add(DateTime.Parse((string)a["timestamp"]), i);
            }

            // toSort is the final sorted dictionary which guides the transactions in the order of their timestamp
            var toSort = toSort1.OrderBy(p => p.Key);
            
            Dictionary<string, int> payerAccounts = new Dictionary<string, int>();
            
            foreach(var x in toSort)
            {
                var a = JObject.Parse(transactions[x.Value]);
                if (p >= (int)a["points"])
                {
                    /*If the payer isn't already on records, if the points value is positive,
                     * it should go in, else it is a bad request*/
                    if (!payerAccounts.ContainsKey((string)a["payer"]))
                    {
                        int sub = (int)a["points"];
                        if(sub<0)
                        {
                            if (System.IO.File.Exists("Transactions.txt"))
                            {
                                System.IO.File.Delete("Transactions.txt");
                            }
                            return BadRequest();
                        }
                        payerAccounts.Add((string)a["payer"], -1 * sub);
                        p = p - sub;
                    }

                    /* If the payer already exists in the dictionary, value should be updated
                     * if the result value is still positive, else it will be a bad request*/
                    else
                    {
                        int sub = (int)a["points"];
                        int already = payerAccounts[(string)a["payer"]];
                        if((-1*already)+sub<0)
                        {
                            if (System.IO.File.Exists("Transactions.txt"))
                            {
                                System.IO.File.Delete("Transactions.txt");
                            }
                            return BadRequest();
                        }
                        p = p - sub;
                        payerAccounts[(string)a["payer"]] = already - sub;
                    }
                }
                else
                {
                    payerAccounts.Add((string)a["payer"], -1 * p);
                    break;
                }
            }
            Dictionary<string, int> balances = new Dictionary<string, int>();
            
            using StreamWriter streamWriter = new StreamWriter("Balances.txt", append: true);
            // Calculating final balances upfront and storing them in the text file.
            foreach (var x in totalPoints)
            {
                balances.Add(x.Key, totalPoints[x.Key] + payerAccounts[x.Key]);
            }
            foreach (var x in balances)
            {
                streamWriter.WriteLineAsync(x.ToString());
            }
            List<PayerAccount> catalog = new List<PayerAccount>();
            foreach(var x in payerAccounts)
            {
                PayerAccount payer = new PayerAccount { payer = x.Key, points = x.Value };
                catalog.Add(payer);
            }

            return JsonConvert.SerializeObject(catalog);
        }
    }
}