using System;
using Customer_BusinessObj;
using DataAcessLayer;

namespace BusinessLogicLayer
{
    public class adminBLL
    {
        adminDAL adminData = new adminDAL();
        public bool verifyLogin(admin_BO obj)
        {
            return adminData.verifyLogindata(obj);
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
        public int CreateNewAccount(Customer_BO customerAccDetails)
        {
            //encrypt login details before saving to database
            Customer_BO encryptedData = encrypt_decrypt(customerAccDetails);
            return adminData.CreateNewAccount(encryptedData);
        }
        public string getAccountHolderName(int accountNum)
        {
            return adminData.readAccountHolderName(accountNum);
        }

        public bool DeleteExistingAccount(int accountNo)
        {
            return adminData.DeleteExistingAccount(accountNo);
        }
        
        public List<Customer_BO> searchAccounts(Customer_BO searchCriteria)
        {
            return adminData.searchAccounts(searchCriteria);
        }

        public Customer_BO getCustomerDetails(int accNo)
        {
            return adminData.getCustomerDetails(accNo);
        }
        public void updateCustomer(Customer_BO customer)
        {

        } 
    }
}
