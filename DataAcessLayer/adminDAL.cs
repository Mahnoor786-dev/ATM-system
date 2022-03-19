using System;
using Microsoft.Data.SqlClient;
using Customer_BusinessObj;

namespace DataAcessLayer
{
    public class adminDAL
    {
        public bool verifyLogindata(admin_BO obj)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlParameter p1 = new SqlParameter("u", obj.UserId);
            SqlParameter p2 = new SqlParameter("p", obj.Pin);
            string query = "SELECT * FROM admins WHERE userId=@u AND pinCode =@p ";
            SqlCommand comand = new SqlCommand(query, con);
            comand.Parameters.Add(p1);
            comand.Parameters.Add(p2);
            SqlDataReader dr = comand.ExecuteReader();
            if (dr.HasRows)
            {
                con.Close();
                return true;
            }
            con.Close();
            return false;
        }
        public int CreateNewAccount(Customer_BO customerAccDetails)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            //save new customer account details in database
            SqlParameter p1 = new SqlParameter("u", customerAccDetails.UserId);
            SqlParameter p2 = new SqlParameter("p", customerAccDetails.Pin);
            SqlParameter p3 = new SqlParameter("n", customerAccDetails.holderName);
            SqlParameter p4 = new SqlParameter("t", customerAccDetails.accountType);
            SqlParameter p5 = new SqlParameter("b", customerAccDetails.balance);
            SqlParameter p6 = new SqlParameter("s", customerAccDetails.status);
            string query = "INSERT INTO customers (userId, pinCode, name, accountType, balance, status) VALUES (@u, @p, @n, @t, @b, @s)";
            SqlCommand comand = new SqlCommand(query, con);
            comand.Parameters.Add(p1);
            comand.Parameters.Add(p2);
            comand.Parameters.Add(p3);
            comand.Parameters.Add(p4);
            comand.Parameters.Add(p5);
            comand.Parameters.Add(p6);
            comand.ExecuteNonQuery();
            SqlParameter p7 = new SqlParameter("i", customerAccDetails.UserId);
            query = "SELECT accountNo FROM customers WHERE userId=@i";
            SqlCommand comand2 = new SqlCommand(query, con);
            comand2.Parameters.Add(p7);
            SqlDataReader dr = comand2.ExecuteReader();
            int accountNo = int.Parse(dr["accountNo"].ToString());
            return accountNo;
        }
        public string readAccountHolderName(int accountNum)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlParameter p1 = new SqlParameter("a",accountNum);
            string query = "SELECT name FROM customers WHERE accountNo=@a";
            SqlCommand comand = new SqlCommand(query, con);
            comand.Parameters.Add(p1);
            SqlDataReader dr = comand.ExecuteReader();
            string name = dr["name"].ToString();
            con.Close();
            return name;
        }

        public bool checkExistenceOfAccount(int accountNum)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlParameter p1 = new SqlParameter("a", accountNum);
            string query = "SELECT * FROM customers WHERE accountNo=@a";
            SqlCommand comand = new SqlCommand(query, con);
            comand.Parameters.Add(p1);
            SqlDataReader dr = comand.ExecuteReader();
            if (dr.HasRows)
            {
                con.Close();
                return true;
            }
            return false;
        }

        //delete account from database having specified accountNo
        public bool DeleteExistingAccount(int accountNum)
        {
            //if the account of such account no does not exist, it will definitely not deleted
            if (checkExistenceOfAccount(accountNum))
            {
                string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                SqlConnection con = new SqlConnection(conString);
                con.Open();
                SqlParameter p1 = new SqlParameter("a", accountNum);
                string query = "DELETE FROM customers WHERE accountNo=@a";
                SqlCommand comand = new SqlCommand(query, con);
                comand.Parameters.Add(p1);
                comand.ExecuteNonQuery();
                con.Close();
            }
            return false;
        }

        public List<Customer_BO> searchAcounts()
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlParameter p1 = new SqlParameter("a", accountNum);
            string query = "SELECT * FROM customers WHERE accountNo=@a";
            SqlCommand comand = new SqlCommand(query, con);
            comand.Parameters.Add(p1);
            SqlDataReader dr = comand.ExecuteReader();
        }


    }
}
