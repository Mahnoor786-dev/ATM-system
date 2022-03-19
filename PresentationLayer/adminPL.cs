﻿using System;
using Customer_BusinessObj;
using BusinessLogicLayer;

namespace PresentationLayer
{
    public class adminPL
    {

        public static void Login()
        {
            adminBLL busLogic = new adminBLL();
            int pinNumeric;
            bool isValid = false;
            string pin;
            int wrongAttempts = 0; //counts no of wrong Attempts at login
            admin_BO busnsObject = new admin_BO();
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
                    adminMenu();
                else
                    Console.WriteLine("\nWRONG PIN.!!");
            }

            void adminMenu()
            {
                adminBLL adminLogic = new adminBLL();
                Console.WriteLine("\n1----Create New Account.\n2----Delete Existing Account.\n3----Update Account Information.\n4----Search for Account.\n5----View Reports\n6----Exit ");
                string optionStr = Console.ReadLine();
                int option = 0;
                bool isNumeric = int.TryParse(optionStr, out option);
                while (!isNumeric)
                {
                    Console.WriteLine("\nPlease chose from 1 - 7): ");
                    optionStr = Console.ReadLine();
                    isNumeric = int.TryParse(optionStr, out option);
                }
                switch (option)
                {
                    case 1:
                        Customer_BO accountInfo = new Customer_BO();
                        Console.WriteLine("\n========  Create New Account ========\n");
                        Console.WriteLine("\nLogin: ");
                        accountInfo.UserId = Console.ReadLine();
                        Console.WriteLine("\nPin Code: ");
                        string pin = Console.ReadLine();
                        //check if pincode is numeric, if not then get pin again
                        int pinNumeric = 0;
                        isNumeric = int.TryParse(pin, out pinNumeric);
                        while ((!isNumeric) || (pin.Length!=5))
                        {
                            Console.WriteLine("\nPlease enter valid Pin Code(5 digit only): ");
                            pin = Console.ReadLine();
                            isNumeric = int.TryParse(pin, out pinNumeric);
                        }
                        accountInfo.Pin = pinNumeric;
                        Console.WriteLine("\nHolders Name: ");
                        accountInfo.holderName = Console.ReadLine();
                        Console.WriteLine("\nType(savings, current): ");
                        string type = Console.ReadLine();
                        type=type.ToLower();
                        if (type=="saving")
                            type="savings";
                        while ((type!="savings")&&(type!="current"))
                        {
                            Console.WriteLine("\nPlease Enter valid Type(Savings/Current): ");
                            type = Console.ReadLine();
                            type=type.ToLower();
                            if (type=="saving")
                                type="savings";
                        }
                        accountInfo.accountType = type;
                        Console.WriteLine("\nStarting Balance: ");
                        string balanc = Console.ReadLine();
                        int balanceNumeric = 0;
                        isNumeric = int.TryParse(balanc, out balanceNumeric);
                        while (!isNumeric)
                        {
                            Console.WriteLine("\nPlease enter valid balance(numbers only): ");
                            balanc = Console.ReadLine();
                            isNumeric = int.TryParse(balanc, out balanceNumeric);
                        }
                        accountInfo.balance = balanceNumeric;
                        Console.WriteLine("\nStatus: ");
                        string status = Console.ReadLine();
                        status=status.ToLower();
                        while ((status!="active")&&(status!="disabled"))
                        {
                            Console.WriteLine("\nPlease Enter status(active/disabled): ");
                            status = Console.ReadLine();
                            status=status.ToLower();
                        }
                        accountInfo.status = status;
                        int accountNO = adminLogic.CreateNewAccount(accountInfo);
                        Console.WriteLine("\n *******Account Successfully Created *********\n Account number assigned is: ", accountNO);
                        break;
                    case 2:
                        Console.WriteLine("\n======== Delete Existing Account ========\n");
                        Console.WriteLine("\nEnter the account number which you want to delete: ");
                        string accountNoStr = Console.ReadLine();
                        int accountNum = 0;
                        //check if account no entered is numeric? if not then get input again and typecaste to int
                        bool IsNumeric = int.TryParse(accountNoStr, out accountNum);
                        while (!IsNumeric)
                        {
                            Console.WriteLine("\nPlease enter valid account number(numbers only): ");
                            accountNoStr = Console.ReadLine();
                            IsNumeric = int.TryParse(accountNoStr, out accountNum);
                        }
                        string name = adminLogic.getAccountHolderName(accountNum);
                        Console.WriteLine("\nYou wish to delete the account held by "+ accountNum + ". If this information is correct please re-enter the account number: ");
                        string accountNoStr2 = Console.ReadLine();
                        int accountNum2 = 0;
                        //check if account no entered is numeric? if not then get input again and typecaste to int
                        bool IsNumeric2 = int.TryParse(accountNoStr2, out accountNum2);
                        while (!IsNumeric2)
                        {
                            Console.WriteLine("\nPlease enter valid account number(numbers only): ");
                            accountNoStr2 = Console.ReadLine();
                            IsNumeric2 = int.TryParse(accountNoStr2, out accountNum2);
                        }
                        if (accountNum==accountNum2)
                        {
                            bool isExist = adminLogic.DeleteExistingAccount(accountNum2);
                            if (isExist)
                                Console.WriteLine("\n----Account Deleted Successfully----");
                            else
                                Console.WriteLine("\n---- DELETION FAILED!! Account Doesn't Exist ----");
                        }
                        else
                            Console.WriteLine("\n------DELETION FAILED!! Second entered account no. does not match with first input-----");
                        break;
                    case 3:
                        Console.WriteLine("\n======== Update Account Information ========\n");


                        break;
                    case 4:
                        Customer_BO searchAccount = new Customer_BO();
                        Console.WriteLine("\n======== Search for Account========\n");
                        Console.WriteLine("SEARCH MENU:\n Account ID: ");
                        string id = Console.ReadLine();
                        if (id=="")
                            searchAccount.accountNo=-1;
                        else
                        {
                            searchAccount.accountNo = int.Parse(id);
                        }
                        Console.WriteLine("User ID: ");
                        searchAccount.UserId = Console.ReadLine();
                        Console.WriteLine("Holders Name: ");
                        searchAccount.holderName = Console.ReadLine();
                        Console.WriteLine("Type(Savings Current): ");
                        string accType = Console.ReadLine();
                        if (accType=="")
                            searchAccount.accountType = "";
                        else
                        {
                            accType=accType.ToLower();
                            if (accType=="saving")
                                accType="savings";
                            while ((accType!="savings")&&(accType!="current"))
                            {
                                Console.WriteLine("\nType(Savings/Current): ");
                                accType = Console.ReadLine();
                                accType=accType.ToLower();
                                if (accType=="saving")
                                    accType="savings";
                            }
                            searchAccount.accountType= accType;
                        }
                        Console.WriteLine("Balance: ");
                        string balancee = Console.ReadLine();
                        if (balancee=="")
                            searchAccount.balance= -1;
                        else
                        {
                            int balanceNum = 0;
                            isNumeric = int.TryParse(balancee, out balanceNum);
                            while (!isNumeric)
                            {
                                Console.WriteLine("\nPlease enter valid balance(numbers only): ");
                                balancee = Console.ReadLine();
                                isNumeric = int.TryParse(balancee, out balanceNum);
                            }
                            searchAccount.balance= balanceNum;
                        }
                        Console.WriteLine("Status:: ");
                        string statuss = Console.ReadLine();
                        if (statuss=="")
                            searchAccount.status="";
                        else
                        {
                            statuss= statuss.ToLower();
                            while ((statuss!="active")&&(statuss!="disabled"))
                            {
                                Console.WriteLine("\nPlease Enter status(active/disabled): ");
                                statuss = Console.ReadLine();
                                statuss= statuss.ToLower();
                            }
                            searchAccount.status = statuss;
                        }
                        busLogic.searchAcounts(searchAccount);


                        break;
                    case 5:
                        Console.WriteLine("\n======== View Reports ========\n");
                        break;
                    case 6:
                        break;
                }
            }
        }
    }
}