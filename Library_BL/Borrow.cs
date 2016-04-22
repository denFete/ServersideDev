using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Library_BL
{
    public class Borrow
    {
        private string  _personID, _title;
        private int _barcode;
        private DateTime _borrowDate, _toBeReturnedDate;

  
        public Borrow() 
        {

        }
        public int Barcode
        {
            get { return _barcode; }
            set { _barcode = value; }
        }
        public string PersonID
        {
            get { return _personID; }
            set { _personID = value; }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public DateTime BorrowDate
        {
            get { return _borrowDate; }
            set { _borrowDate = value; }
        }
        public DateTime ToBeReturnedDate
        {
            get { return _toBeReturnedDate; }
            set { _toBeReturnedDate = value; }
        }

        public static List<Borrow> getBorrowedBooks(string personid)
        {
            List<Borrow> results = new List<Borrow>();
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM BORROW WHERE PersonId = '"+personid+"' AND ReturnDate IS NULL", con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    Borrow newBorrow = new Borrow();
                    newBorrow.Barcode= (int)dar["Barcode"];
                    newBorrow.BorrowDate = (DateTime)dar["BorrowDate"];
                    newBorrow.ToBeReturnedDate = (DateTime)dar["ToBeReturnedDate"];
                    string isbn = Convert.ToString(Borrow.GetISBN(newBorrow.Barcode));
                    newBorrow.Title = Borrow.GetTitle(isbn);
                    results.Add(newBorrow);
                }
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                con.Close();
            }
            return results;
        }
        public static string GetISBN(int barcode)
        {
            string ISBN = "";
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT ISBN FROM COPY WHERE Barcode = "+barcode, con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    ISBN = dar["ISBN"] as string;
                }
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                con.Close();
            }
            return ISBN;
        }
        public static string GetTitle(string isbn)
        {
            string Title = "";
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT Title FROM BOOK WHERE ISBN = '" + isbn + "'", con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    Title = dar["Title"] as string;
                }
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                con.Close();
            }
            return Title;

        }
        public static void Reborrow(int barcode)
        {
            DateTime time = DateTime.Now;
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("UPDATE BORROW Set ToBeReturnedDate= @time WHERE Barcode="+ barcode+" AND ReturnDate IS NULL" , con);
            cmd.Parameters.AddWithValue("@time", time.AddMonths(1));
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                con.Close();
            }
            
        }
    }
}
