using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Library_BL
{
    public class Book
    {

        static Book()
        {
        }

        private string _title,
                       _isbn,
                       _oldIsbn,
                       _publicationinfo
                       ;
                        
        private int _aid,
                    _barcode,
                    _statusid,
                    _pages;
        public int Pages
        {
            get { return this._pages; }
            set { this._pages = value; }
        }

        public string PublicationInfo
        {
            get { return this._publicationinfo; }
            set { this._publicationinfo = value; }
        }
        public string Title
        {
            get { return this._title; }
            set { this._title = value; }
        }
        public string ISBN
        {
            get { return this._isbn; }
            set { this._isbn = value; }
        }
        public string OLDISBN
        {
            get { return this._oldIsbn; }
            set { this._oldIsbn = value; }
        }
        public int Barcode
        {
            get { return this._barcode; }
            set { this._barcode = value; }
        }
        public int StatusId
        {
            get { return this._statusid; }
            set { this._statusid = value; }
        }
        public int Aid
        {
            get { return _aid; }
            set { _aid = value; }
        }
        public static List<Book> search(string query)
        {
            string SQL = "SELECT BOOK.* FROM BOOK WHERE Title LIKE '%" + query + "%'";
            List<Book> results = new List<Book>();
            SqlConnection con = new SqlConnection(Library_BL.Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand(SQL, con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    Book book = new Book();
                    book._title = dar["Title"] as string;
                    book._isbn = dar["ISBN"] as string;
                  /* book._publicationinfo = dar["publicationinfo"] as string;*/
                    results.Add(book);
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
        public static List<Book> getAll()
        {
            string SQL = "SELECT BOOK.* FROM BOOK";
            List<Book> results = new List<Book>();
            SqlConnection con = new SqlConnection(Library_BL.Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand(SQL, con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    Book book = new Book();
                    book._title = dar["Title"] as string;
                    book._isbn = dar["ISBN"] as string;
                    results.Add(book);
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

        public static List<Book> getByAuthor(int authorID)
        {
            string SQL = "SELECT BOOK.* FROM BOOK " +
                "INNER JOIN BOOK_AUTHOR ON BOOK_AUTHOR.ISBN=BOOK.ISBN " +
                "WHERE BOOK_AUTHOR.Aid=" + authorID;
            List<Book> results = new List<Book>();
            SqlConnection con = new SqlConnection(Library_BL.Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand(SQL, con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    Book book = new Book();
                    book._title = dar["Title"] as string;
                    book._isbn = dar["ISBN"] as string;
                    results.Add(book);
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
        public static List<Book> getBarCode(string isbn)
        {

            string SQL = "SELECT Barcode,StatusId FROM COPY WHERE ISBN='" + isbn + "' AND StatusId=1";
            List<Book> results = new List<Book>();
            SqlConnection con = new SqlConnection(Library_BL.Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand(SQL, con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    Book book = new Book();
                    book._barcode =(int)dar["Barcode"];
                    book._statusid = (int)dar["StatusId"];
                    results.Add(book);
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
        public static void changeStatus(int barcode, int status)
        {
            string SQL = "UPDATE COPY set StatusId =" + status + " WHERE Barcode=" + barcode;
            List<Book> results = new List<Book>();
            SqlConnection con = new SqlConnection(Library_BL.Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand(SQL, con);
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
        public void save()
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd;
            cmd = new SqlCommand("INSERT INTO BOOK (ISBN, Title) VALUES ('" + this.ISBN+ "','" + this.Title + "'); SELECT SCOPE_IDENTITY()", con);
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
                saveBookAuthor();
                saveIntoCopy();
            }
        
        }

        public void update()
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd;
            
            cmd = new SqlCommand("UPDATE BOOK set ISBN='" + this.ISBN + "', Title='" + this.Title+ "' WHERE ISBN= '"+this.OLDISBN+"'", con);
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

        public void saveBookAuthor()
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd;
            cmd = new SqlCommand("INSERT INTO BOOK_AUTHOR (ISBN, Aid) VALUES ('" + this.ISBN + "','" + this.Aid + "');", con);
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

        public void saveIntoCopy()
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd;
            cmd = new SqlCommand("INSERT INTO COPY (StatusId,ISBN) VALUES ('" +StatusId + "', '" + ISBN +"');", con);
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
        public  Book getByISBN(string isbn,string title)
        {
            Book newBook = null;
            SqlConnection con = new SqlConnection(Settings.ConnectionString);

            
             SqlCommand cmd = new SqlCommand("SELECT * FROM BOOK WHERE ISBN=@ISBN", con);
          
            
            SqlParameter paramAid = new SqlParameter("ISBN", SqlDbType.NVarChar);
            paramAid.Value = isbn;
            cmd.Parameters.Add(paramAid);

            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                if (dar.Read())
                {
                    newBook = new Book();
                    newBook.OLDISBN = dar["ISBN"] as string;
                    newBook.Title = dar["Title"] as string;
                    newBook.ISBN = dar["ISBN"] as string;
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
            return newBook;
        }

        public static void delete(string isbn)
        {


            string SQL0 = "DELETE FROM BOOK_AUTHOR WHERE ISBN='" + isbn + "'";
            string SQL1 = "DELETE FROM COPY WHERE ISBN='" + isbn + "'";
            string SQL2 = "DELETE FROM BOOK WHERE ISBN='" + isbn + "'";


            SqlConnection con = new SqlConnection(Library_BL.Settings.ConnectionString);
            SqlCommand cmd0 = new SqlCommand(SQL0, con);
            SqlCommand cmd1 = new SqlCommand(SQL1, con);
            SqlCommand cmd2 = new SqlCommand(SQL2, con);
            try
            {
                con.Open();
                cmd0.ExecuteNonQuery();
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
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