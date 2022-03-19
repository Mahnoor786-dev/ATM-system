using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_BusinessObj
{
    //account no and name of person to which someone transfers cash
    public class cashReceiver_BO
    {
        public int accountNo { get; set; }
        public string name { get; set; }
        public cashReceiver_BO(int acc, string n=" ")
        {
            accountNo = acc;
            name = n;
        }
    }
}
