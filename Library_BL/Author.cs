using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Library_BL
{
    public class Author
    {
        private int _Aid, _BirthYear;
        private string _FirstName, _LastName;
        public Author(string firstname, string lastnamne, int birthyear)
        {
           this._FirstName = firstname;
           this._LastName = lastnamne;
           this._BirthYear = birthyear;
       
        }
        public Author() { }

        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }        
        }
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        public int Aid
        {   get { return _Aid; }
            set { _Aid = value; }
        }
        public int BirthYear
        {   get { return _BirthYear; }
            set { _BirthYear = value; }
        }
        public int save()
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd;
            int retVal = -1;
            bool existing;
            if (this._Aid == 0)
            {
                existing = false;
                cmd = new SqlCommand("INSERT INTO AUTHOR (FirstName, LastName, BirthYear) VALUES ('" + this._FirstName + "','" + this._LastName + "'," + this._BirthYear + "); SELECT SCOPE_IDENTITY()", con);
            }
            else
            {
                existing = true;
                cmd = new SqlCommand("UPDATE AUTHOR set FirstName='" + this.FirstName + "', LastName='" + this.LastName + "', BirthYear=" + this.BirthYear + " WHERE Aid=" + this.Aid, con);
            }
            try
            {
                con.Open();
                if (!existing)
                {

                    retVal = cmd.ExecuteNonQuery();
                }
                else
                {
                    retVal = -1;
                    cmd.ExecuteScalar();
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
            return retVal;
        }

        public static List<Author> getAll()
        {
            List<Author> results = new List<Author>();
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM AUTHOR", con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    Author newAuthor = new Author();
                    newAuthor.Aid = (int)dar["Aid"];
                    newAuthor.BirthYear = (dar["BirthYear"] == DBNull.Value) ? 0 : Convert.ToInt32(dar["BirthYear"].ToString());
                    newAuthor.FirstName = dar["FirstName"] as string;
                    newAuthor.LastName = dar["LastName"] as string;
                    results.Add(newAuthor);
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
        public static Author getByAid(int aid)
        {
            Author newAuthor = null;
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM AUTHOR WHERE Aid=@Aid", con);
            SqlParameter paramAid = new SqlParameter("Aid", SqlDbType.Int);
            paramAid.Value = aid;
            cmd.Parameters.Add(paramAid);

            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                if (dar.Read())
                {
                    newAuthor = new Author();
                    newAuthor.Aid = (int)dar["Aid"];
                    newAuthor.BirthYear = (dar["BirthYear"] == DBNull.Value) ? 0 : Convert.ToInt32(dar["BirthYear"].ToString());
                    newAuthor.FirstName = dar["FirstName"] as string;
                    newAuthor.LastName = dar["LastName"] as string;
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
            return newAuthor;
        }
       
        public static string GetBookISBN(int id)
        {
                Library_BL.Author author = Library_BL.Author.getByAid(id);
                string queryGetBooks = "select * from BOOK_AUTHOR where Aid=" + id;
                SqlConnection con = new SqlConnection(Settings.ConnectionString);
                SqlCommand cmd = new SqlCommand(queryGetBooks, con);
                List<string> isbnList = new List<string>();
                try
                {
                    con.Open();
                    SqlDataReader dar = cmd.ExecuteReader();
                    while (dar.Read())
                    {
                        string isbn = Convert.ToString( dar["ISBN"]);             
                        isbnList.Add(isbn);
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
            int antalBocker = isbnList.Count();
            string query="";
            for(int i = 0 ; i < antalBocker; i++)
            {
                if (i != 0)
                    query += ", ";
                
                query += "'" + isbnList[i] + "'";
            }
            return query;
        }
        public static void DeleteFromBookAuthor(string isbnBooks)
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("Delete from BOOK_AUTHOR where ISBN IN (" + isbnBooks + ")", con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
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
        public static void DeleteFromCopy(string isbnBooks)
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("Delete from COPY where ISBN IN (" + isbnBooks + ")", con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
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
        public static void DeleteFromBook(string isbnBooks)
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("Delete from BOOK Where ISBN IN (" + isbnBooks + ")", con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
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
        public static void DeleteAuthor(int id)
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("Delete from AUTHOR Where Aid="+id, con);
            string books = GetBookISBN(id);
            if (books == "")
            {
                try
                {
                    con.Open();
                    SqlDataReader dar = cmd.ExecuteReader();
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
            else
            {
                DeleteFromBookAuthor(books);
                DeleteFromCopy(books);
                DeleteFromBook(books);
                DeleteAuthor(id);
            }

        }

        public static List<Author> search(string query)
        {
            string SQL = "SELECT * FROM AUTHOR WHERE FirstName LIKE '%" + query + "%' OR LastName LIKE '%" + query + "%'";
            List<Author> results = new List<Author>();
            SqlConnection con = new SqlConnection(Library_BL.Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand(SQL, con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    Author newAuthor = new Author();
                    newAuthor.Aid = (int)dar["Aid"];
                    newAuthor.BirthYear = (dar["BirthYear"] == DBNull.Value) ? 0 : Convert.ToInt32(dar["BirthYear"].ToString());
                    newAuthor.FirstName = dar["FirstName"] as string;
                    newAuthor.LastName = dar["LastName"] as string;
                    results.Add(newAuthor);
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

        public static List<Author> getBookDetails(string isbn)
        {
            List<Author> result = new List<Author>(); 
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT Aid from BOOK_AUTHOR WHERE ISBN ='" + isbn + "'", con);

            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read()) 
                {
                    Author newAuthor = new Author();
                    newAuthor.Aid = (int)dar["Aid"];
                    result.Add(newAuthor);
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
            return result;
        }
       
      

        

    }


}


   