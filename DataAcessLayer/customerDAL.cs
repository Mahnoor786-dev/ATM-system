using Customer_BusinessObj;
using Microsoft.Data.SqlClient;
namespace DataAcessLayer
{
    public class customerDAL
    {
        public bool verifyLogindata(Customer_BO obj)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlParameter p1 = new SqlParameter("u", obj.UserId);
            SqlParameter p2 = new SqlParameter("p", obj.Pin);
            string query = "SELECT * FROM customers WHERE userId=@u AND pinCode =@p ";
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

        public void disableUser(string loginId)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = "UPDATE customers SET status = 'disabled'  WHERE userId=loginId";
            SqlCommand comand = new SqlCommand(query, con);
            comand.ExecuteNonQuery();
            con.Close();
        }

        public Customer_BO checkBalance(Customer_BO obj)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlParameter p1 = new SqlParameter("u", obj.UserId);
            string query = "SELECT balance, accountNo FROM customers WHERE userId=@u";
            SqlCommand comand = new SqlCommand(query, con);
            comand.Parameters.Add(p1);
            SqlDataReader dr = comand.ExecuteReader();
            obj.accountNo = int.Parse(dr["accountNo"].ToString());
            obj.balance = int.Parse(dr["balance"].ToString());
            con.Close();
            return obj;
        }

        //updates balance in database after withdrawal of money
        public void updateBalance(Customer_BO obj)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = "UPDATE customers SET balance = balance-obj.requestedWithdraw WHERE userId=obj.UserId";
            SqlCommand comand = new SqlCommand(query, con);
            comand.ExecuteNonQuery();
            DateTime cur_date = DateTime.Today;
            string id = obj.UserId;
            query="UPDATE cust_Transactions SET withdrawal=obj.requestedWithdraw, date=cur_date WHERE userId=id";
            SqlCommand comand2 = new SqlCommand(query, con);
            comand2.ExecuteNonQuery();
            con.Close();
        }

        //sum all the withdrawals of specified customer on current day
        public Customer_BO withdrawToday(Customer_BO obj)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string id = obj.UserId;
            string query = "SELECT withdrawal FROM cust_Transactions WHERE userId=id";
            SqlCommand comand = new SqlCommand(query, con);
            SqlDataReader dr = comand.ExecuteReader();
            int withdrawT = 0;
            while (dr.Read())
                withdrawT+= int.Parse(dr["withdrawal"].ToString()); //sum of todays' withdrawals 
            con.Close();
            obj.withdrawalToday=withdrawT;
            return obj;
        }

        //reads the name of account holder to whom sender transfers cash
        public cashReceiver_BO getNameFromDB(cashReceiver_BO receiver)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            int accountNum = receiver.accountNo;
            string query = "SELECT name FROM customers WHERE accountNo=accountNum";
            SqlCommand comand = new SqlCommand(query, con);
            SqlDataReader dr = comand.ExecuteReader();
            receiver.name = dr["name"].ToString();
            con.Close();
            return receiver;
        }
        public bool checkExistenceOfAccount(int accountNum)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = "SELECT * FROM customers WHERE accountNo=accountNum";
            SqlCommand comand = new SqlCommand(query, con);
            SqlDataReader dr = comand.ExecuteReader();
            if (dr.HasRows)
            {
                con.Close();
                return true;
            }
            return false;
        }

        //transfer cash operation: updates balance of both sender and receiver in database
        public void transferCash(Customer_BO senderObj, cashReceiver_BO receiver)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            // deduct amount from sender's account
            int senderBalnc = senderObj.balance;
            int transaction = senderObj.requestedTransaction;
            string id = senderObj.UserId;
            string query = "UPDATE customers SET balance=senderBalnc - transaction WHERE userId=id";
            SqlCommand comand = new SqlCommand(query, con);
            comand.ExecuteNonQuery();
            // transfer sender's amount to receiver account
            transaction = senderObj.requestedTransaction;
            int accountNum = receiver.accountNo;
            query="UPDATE customers SET balance = balance + transaction WHERE accountNo = accountNum";
            SqlCommand comand2 = new SqlCommand(query, con);
            comand2.ExecuteNonQuery();
            con.Close();
        }

        public void addCashInAccount(Customer_BO b_Obj)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            int depositAmount = b_Obj.requestedDeposit;
            string id = b_Obj.UserId;
            string query = "UPDATE customers SET balance = balance + depositAmount WHERE userId=id";
            SqlCommand comand = new SqlCommand(query, con);
            comand.ExecuteNonQuery();
            con.Close();
        }

    }
}