using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_BusinessObj
{
    public class Customer_BO
    {
        public string UserId { get; set; }
        public int Pin { get; set; }
        public int requestedWithdraw { get; set; }
        public int requestedTransaction { get; set; }
        public int requestedDeposit { get; set; }
        public int withdrawalToday { get; set; }
        public int accountNo { get; set; }
        public int balance { get; set; }
        public string holderName { get; set; }
        public string accountType { get; set; }
        public string status { get; set; }
        public Customer_BO(int p, string id)
        {
            Pin=p;
            UserId=id;
        }
        public Customer_BO()
        {
            Pin = 0;
            UserId ="";
        }
    }
}
