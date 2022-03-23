using System;
using BusinessLogicLayer;
using Customer_BusinessObj;

namespace PresentationLayer
{
    public class customerPL
    {
        customerBLL busLogic = new customerBLL();
        public bool Login()
        {
            int pinNumeric = 0;
            bool isValid = false;
            string pin;
            int wrongAttempts = 0; //counts no of wrong Attempts at login
            Customer_BO busnsObject = new Customer_BO();

            while ((wrongAttempts<3)&&(!isValid))
            {
                Console.WriteLine("Enter userID login:  ");
                busnsObject.UserId = Console.ReadLine();
                Console.WriteLine("Enter Pin code:  ");
                pin = Console.ReadLine();
                //check if pincode is numeric, if not then get pin again
                bool isNumeric = int.TryParse(pin, out pinNumeric);
                while ((!isNumeric) || (pin.Length!=5))
                {
                    Console.WriteLine("\nPlease enter valid Pin Code(5 digit only): ");
                    pin = Console.ReadLine();
                    isNumeric = int.TryParse(pin, out pinNumeric);
                }
                busnsObject.Pin = pinNumeric;
                isValid = busLogic.verifyLogin(busnsObject);
                if (isValid)
                {
                    customerMenu(busnsObject, true);
                    return true;
                }
                else
                {
                    Console.WriteLine("\nWRONG CREDENTIALS.!!");
                    wrongAttempts++;
                }
            }
            if (wrongAttempts==3)
            {
                busLogic.disableUser(busnsObject.UserId);
                Console.WriteLine("\nYou have entered wrong pin 3 times, you are now disabled to login");
                return false;
            }
            return false;
        }
        public void customerMenu(Customer_BO b_Obj, bool isLogIn)
        {
            //menu will be shown only when user is login 
            if (isLogIn)
            {
                Console.WriteLine("1----Withdraw Cash \n2----Cash Transfer\n3----Deposit Cash\n4----Display Balance\n5----Exit\n Please select one of the above options by entering corresponding Number: ");
                string optionStr = Console.ReadLine();
                int option = 0;
                bool isNumeric = int.TryParse(optionStr, out option);
                while (!isNumeric)
                {
                    Console.WriteLine("\nPlease enter valid Pin Code(numbers only): ");
                    optionStr = Console.ReadLine();
                    isNumeric = int.TryParse(optionStr, out option);
                }
                switch (option)
                {
                    case 1:
                        Console.WriteLine("\n========  Withdraw Cash ========\n");
                        int mode = 0;
                        string modeStr;
                        Console.WriteLine("1) Fast Cash\n2) Normal Cash.\nPlease select a mode of withdrawal by entering corresponding Number:");
                        while ((mode!=1) && (mode!=2))
                        {
                            modeStr = Console.ReadLine();
                            isNumeric = int.TryParse(modeStr, out mode);
                            while (!isNumeric)
                            {
                                Console.WriteLine("\nPlease enter valid option from above menu(1/2): ");
                                modeStr = Console.ReadLine();
                                isNumeric = int.TryParse(modeStr, out mode);
                            }
                            if ((mode!=1) || (mode!=2))
                                Console.WriteLine("Please Enter valid choice:  ");
                        }
                        if (mode==1)
                            fastCash(ref b_Obj);
                        else
                            normalCash(ref b_Obj);
                        break;

                    case 2:
                        Console.WriteLine("\n========  Cash Transfer ========\n");
                        transferCash(ref b_Obj);
                        break;
                    case 3:
                        Console.WriteLine("\n========  Deposit Cash ========\n");
                        depositCash(ref b_Obj);
                        break;
                    case 4:
                        Console.WriteLine("\n========  Display balance ========\n");
                        displayBalance(ref b_Obj);
                        break;
                    case 5:
                        break;
                    default:
                        Console.WriteLine("*** Invalid choice ***");
                        break;
                }
            }
            else
                Console.WriteLine("\n---- NOTHING TO SHOW\nYOU HAVE BEEN DISABLED TO LOGIN DUE TO WRONG ATTEMPTS. ----");
        }

        //ask user to print receipt, print if needed
        public void receipt(ref Customer_BO b_Obj, int receiptType)
        {
            string choicee = "g";
            while ((choicee != "Y")&&(choicee != "N"))
            {
                Console.WriteLine("\nDo you wish to print a receipt(Y/N) ?");
                choicee = Console.ReadLine();
                choicee = choicee.ToUpper();
            }
            char choice = choicee[0];
            if (choice=='Y')
            {
                Console.WriteLine("\n*********** RECEIPT ***********\nAccount#: "+ b_Obj.accountNo+ "\nDate: ", DateTime.Today);
                //show relevant result according to operation performed
                switch(receiptType)
                {
                    case 1:
                        Console.WriteLine("\nWithdrawn: Rs."+ b_Obj.requestedWithdraw);
                        break;
                    case 2:
                        Console.WriteLine("\nAmount transferred: Rs."+ b_Obj.requestedTransaction );
                        break;
                    case 3:
                        Console.WriteLine("\nDeposited: Rs."+ b_Obj.requestedDeposit);
                        break;
                    case 4:
                        break; //nothing to print in case of display balance request
                }
                Console.WriteLine("\nBalance: Rs."+ b_Obj.balance);
            }
            else
                Console.WriteLine("\nOk! Have a good day");
            Console.WriteLine("\n*********************************\n");
        }

        //mode of withdrawals: fastcash, normal cash view
        public void fastCash(ref Customer_BO b_Obj)
        {
            int amountChoice = 0;
            string choiceStr;
            bool isNumeric;
            while ((amountChoice>=7) || (amountChoice<=0))
            {
                Console.WriteLine("\n1----Rs 500\n2----Rs 1000\n3----Rs 2000\n4----Rs 5000\n5----Rs 10000\n6----Rs 15000\n7----Rs 20000\nSelect one of the denominations of money: ");
                choiceStr = Console.ReadLine();
                isNumeric = int.TryParse(choiceStr, out amountChoice);
                while(!isNumeric)
                {
                    Console.WriteLine("\nPlease chose valid option from above menu(1-7): ");
                    choiceStr = Console.ReadLine();
                    isNumeric = int.TryParse(choiceStr, out amountChoice);
                }
                if ((amountChoice>=7) || (amountChoice<=0))
                    Console.WriteLine("Please Enter valid choice:  ");
            }
            b_Obj.requestedWithdraw = busLogic.fastCashAmountList(amountChoice);
            string choicee = " ";
            Console.WriteLine("Are you sure you want to withdraw Rs." + b_Obj.requestedWithdraw + "(Y/N) ?");
            while ((choicee != "Y")&&(choicee != "N"))
            {
                Console.WriteLine("Please enter either 'Y' or 'N': ");
                choicee = Console.ReadLine();
                choicee = choicee.ToUpper();
            }
            char choice = choicee[0];   //take only first character of input string
            if (choice=='N')
                fastCash(ref b_Obj); //repeat the process again, if selected amount is not verified by user
            else
            {
                Customer_BO updatedObj = busLogic.withdrawCash( b_Obj);
                Console.WriteLine("Cash Successfully Withdrawn!");
                receipt(ref updatedObj, 1);
            }
        }
        public void normalCash(ref Customer_BO b_Obj)
        {
            Console.WriteLine("\nEnter the withdrawal amount: Rs? ");
            string amount = Console.ReadLine();
            //check if amount is numeric, if not then get input again
            int amountNumeric = 0;
            bool isNumeric = int.TryParse(amount, out amountNumeric);
            while (!isNumeric)
            {
                Console.WriteLine("\nPlease enter valid(numeric only) withdrawal amount: Rs?: ");
                amount = Console.ReadLine();
                isNumeric = int.TryParse(amount, out amountNumeric);
            }
            b_Obj.requestedWithdraw = amountNumeric; //set requested withdrawal amount
            Customer_BO updatedObj = busLogic.withdrawCash(b_Obj); //validate requested amount (<balance) and then withdraw
            Console.WriteLine(" Cash Successfully Withdrawn ");
            receipt(ref updatedObj, 1);
        }

        public void transferCash(ref Customer_BO b_Obj)
        {
            int amount = 1;
            int option = 0;
            bool isNumeric;
            string optionStr;
            //checks: amount should be multiple of 500, input must be numeric
            while (amount%500 != 0)
            {
                Console.WriteLine("\nEnter amount in multiples of 500: ");
                optionStr = Console.ReadLine();
                isNumeric = int.TryParse(optionStr, out option);
                while (!isNumeric)
                {
                    Console.WriteLine("\nPlease enter valid amount (numbers only).. ");
                    optionStr = Console.ReadLine();
                    isNumeric = int.TryParse(optionStr, out option);
                }
                if (amount%500 != 0)
                    Console.WriteLine("--- Please Enter valid amount --- \n");
            }
            b_Obj.requestedTransaction = amount; //save the requested transfer amount in BO
            Console.WriteLine("Enter the account number to which you want to transfer: ");
            int accountNo = int.Parse(Console.ReadLine());
            //if the asked account no is invalid(doesn't exist), transfer can't happen definitely
            if (busLogic.checkExistenceOfAccount(accountNo))
            {
                cashReceiver_BO receiver = busLogic.getNameOfAccountHolder(new cashReceiver_BO(accountNo));
                Console.WriteLine("You wish to deposit Rs " + amount + "in account held by ", receiver.name, "; If this information is correct please re-enter the account number: ");
                int accountNo2 = int.Parse(Console.ReadLine());
                if (accountNo==accountNo2)
                {
                    Customer_BO updatedObj = busLogic.transferCash(b_Obj, receiver); //pass business objects of sender and receiver to transfer cash
                    Console.WriteLine("Transaction confirmed");
                    receipt(ref updatedObj, 2); //argument 2 will result in displaying "amount transferred" in receipt
                }
                else
                    Console.WriteLine("Transfer failed! Confirmed Account no does not match first input. ");
            }
            else
                Console.WriteLine("\n---- TRANSFER FAILED!! Account Doesn't Exist ----");
        }

        public void depositCash(ref Customer_BO b_Obj)
        {
            Console.WriteLine("\nEnter the cash amount to deposit: ");
            int amount = int.Parse(Console.ReadLine());

            b_Obj.requestedDeposit= amount; //save the requested deposit amount in BO
            Customer_BO updatedObj = busLogic.addCashInAccount(b_Obj);
            Console.WriteLine("\nCash Deposited Successfully.");
            receipt(ref updatedObj, 3);
        }

        public void displayBalance(ref Customer_BO b_Obj)
        {
            Customer_BO customer_obj = busLogic.getBalance(b_Obj);
            receipt(ref customer_obj, 4); 
        }

    }
}
        