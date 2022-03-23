using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer
{
    public class mainMenu
    {
        public char mainView()
        {
            Console.WriteLine("\nYou want to login as: \n1- Admin \n2- Customer\n Enter 1 or 2? ");
            string userTypeStr = Console.ReadLine();
            int userType = 0;
            bool isNumeric = int.TryParse(userTypeStr, out userType);
            //to ensure must be either 1 or 2
            while ((!isNumeric)|| ((userType!=1) && (userType!=2) ) )
            {
                Console.WriteLine("\nPlease enter valid entry(1 or 2 only): ");
                userTypeStr = Console.ReadLine();
                isNumeric = int.TryParse(userTypeStr, out userType);
            }
            //return a: for admin , or c: for customer
            if (userType==1)
                return 'a';
            return 'c';
        }
    }
}
