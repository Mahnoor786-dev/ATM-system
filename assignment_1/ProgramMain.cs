// See https://aka.ms/new-console-template for more information
using PresentationLayer;
namespace Lecture17_EMS
{
    class Program
    {
        static void Main(string[] args)
        {
            customerPL customerMenu = new customerPL();
            mainMenu menu = new mainMenu();
            char userType = menu.mainView();
            if (userType=='c')  //for customer login
                customerMenu.Login();
            else if (userType=='a') //for admin login
                adminPL.Login();
        }
    }
}