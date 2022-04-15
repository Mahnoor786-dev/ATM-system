using Customer_BusinessObj;
using DataAcessLayer;
namespace BusinessLogicLayer
{
    public class customerBLL
    {
        customerDAL data = new customerDAL();
        //verify credentials of user from data tier
        public bool verifyLogin(Customer_BO obj)
        {
            Customer_BO encryptedObj = encrypt_decrypt(obj);
            return data.verifyLogindata(encryptedObj);
        }
        public Customer_BO encrypt_decrypt(Customer_BO AccDetails)
        {
            for (int j = 0; j < 2; j++)
            {
                string optionStr;
                if (j==0) //encrypt userid(login)
                    optionStr = AccDetails.UserId;
                else //encrypt PIN
                    optionStr=(AccDetails.Pin).ToString();
                optionStr=optionStr.ToUpper();
                int[] ascii = new int[optionStr.Length];
                char[] encr = new char[optionStr.Length];
                int remainder;
                char caseOfCharacter = 'u'; //to notify that a character was in upper case or lower case
                for (int i = 0; i < optionStr.Length; i++)
                {
                    ascii[i]=(int)optionStr[i]; //get ascii value of character
                    //encrypting alphabets
                    if ((ascii[i]<=90)&&((ascii[i]>=65)))
                    {
                        if ((ascii[i]<=122)&&((ascii[i]>=97))) //if character was lowercase
                            caseOfCharacter = 'l';
                        remainder = 90 % ascii[i];
                        ascii[i] = remainder+65;
                        //to keep the case of character same after encryption
                        if (caseOfCharacter=='l')
                            ascii[i] = ascii[i]+32;
                        encr[i]=(char)ascii[i]; //convert ascii to character
                    }
                    else //encrypting numbers
                    {
                        ascii[i]=(int)optionStr[i]; //digit character to ascii value
                        remainder = 57 % ascii[i];
                        ascii[i] = remainder+48;
                        encr[i]=(char)ascii[i];  //ascii value to digit character
                    }
                }
                string encryptedU_Id = new string(encr);
                if (j==0) //store encrypted login id in object
                    AccDetails.UserId=encryptedU_Id;
                else //store encrypted PIN in object
                    AccDetails.Pin=int.Parse(encryptedU_Id);
            }
            return AccDetails;
        }

        public void disableUser(string loginId)
        {
            data.disableUser(loginId);
        }
        public int fastCashAmountList(int option)
        {
            int amount = 0;
            switch (option)
            {
                case 1:
                    amount=500;
                    break;
                case 2:
                    amount=1000;
                    break;
                case 3:
                    amount=2000;
                    break;
                case 4:
                    amount = 5000;
                    break;
                case 5:
                    amount=10000;
                    break;
                case 6:
                    amount=15000;
                    break;
                case 7:
                    amount=20000;
                    break;
            }
            return amount;
        }
        public Customer_BO withdrawCash(Customer_BO b_Obj)
        {
            //get available balance of customer
            int currentBalance = (data.checkBalance(b_Obj)).balance;
            int acc = (data.checkBalance(b_Obj)).accountNo;
            b_Obj.balance = currentBalance;
            b_Obj.accountNo = acc;
            int withdrawals = (data.withdrawToday(b_Obj)).withdrawalToday;
            //withdraw amount if balance is sufficient AND limit of withdawal is not reached
            if ((b_Obj.requestedWithdraw <= currentBalance) && ((withdrawals + b_Obj.requestedWithdraw) <=20000))
            {
                data.updateBalance(b_Obj);
                b_Obj.balance = currentBalance-b_Obj.requestedWithdraw; //update balance in object as well
                Console.WriteLine(" Cash Successfully Withdrawn ");
            }
            else if (b_Obj.requestedWithdraw > currentBalance)
            {
                Console.WriteLine("Unsufficient balance for this Withdrawal");
                b_Obj.requestedWithdraw = 0; //if balance is insufficient, withdrawal=0
            }
            else
            { 
                Console.WriteLine("You have reached max limit of withdrawal for today");
                b_Obj.requestedWithdraw = 0;
            }
            return b_Obj;
        }

        //get name of a account holder against a given account number
        public cashReceiver_BO getNameOfAccountHolder(cashReceiver_BO receiver)
        {
            return data.getNameFromDB(receiver);
        }

        public bool checkExistenceOfAccount(int accountNum)
        {
            return data.checkExistenceOfAccount(accountNum);
        }

        public Customer_BO transferCash(Customer_BO b_Obj, cashReceiver_BO receiver)
        {
            Customer_BO senderObj = data.checkBalance(b_Obj);
            if (b_Obj.requestedTransaction <= senderObj.balance)
            {
                data.transferCash(senderObj, receiver);
                b_Obj.balance = b_Obj.balance - b_Obj.requestedTransaction; //update balance in B.object after cash transfer
                Console.WriteLine("Transaction confirmed");
            }
            else
            {
                Console.WriteLine("\nYou have Unsufficient amount for this transaction!");
                b_Obj.requestedTransaction = 0; //if balance is insufficient transferred amount = 0
            }
            return senderObj;
        }

        public Customer_BO addCashInAccount(Customer_BO b_Obj)
        {
            b_Obj.balance = b_Obj.balance + b_Obj.requestedDeposit;//update balance in B.object after cash deposit
            data.addCashInAccount(b_Obj);
            return b_Obj;
        }

        public Customer_BO getBalance(Customer_BO b_Obj)
        {
            return data.checkBalance(b_Obj);
        }



    }
}