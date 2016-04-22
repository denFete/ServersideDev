using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Web;
using System.Data;


namespace Library_BL
{
    public class User
    {
        private string _firstName,_lastName,_password,_personId,_userName,_salt;
        private int _id,_isAdmin;

        public User(string firstname, string lastname,string password,string personid,string username)
        {
            this._firstName = firstname;
            this._lastName = lastname;
            this._password = password;
            this._personId = personid;
            this._userName = username;
        }

        public User() { }
        public string UserName { 
            get { return _userName; }
            set {_userName = value ;}
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        public string PersonId
        {
            get { return _personId; }
            set { _personId = value; }
        }
        public string Salt
        {
            get { return _salt; }
            set { _salt = value; }
        }
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public int isAdmin
        {
            get { return _isAdmin; }
            set { _isAdmin = value; }
        }
        public int save()
        {
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd;
            int retVal = -1;
            bool existing;
            cmd = new SqlCommand("SELECT COUNT (*) FROM BORROWER WHERE PersonId='"+this._personId+"'", con);
            string salt = Settings.GenerateSalt();
            string passSalt = Settings.SecureString(this._password+salt);
           

         try
         {
            con.Open();
            object o = cmd.ExecuteScalar();
            if( this._id==0 && Convert.ToUInt16(o) == 0)
            {
                existing = false;
                cmd = new SqlCommand("INSERT INTO BORROWER (FirstName, LastName, PersonId, username, password,salt) VALUES ('" + this._firstName + "','" + this._lastName + "','" + this._personId + "','" + this._userName + "','" + passSalt + "','"+salt+"')", con);
            }
            else
            {
                existing = true;
                passSalt = Settings.SecureString(this.Password+salt);
                cmd = new SqlCommand("UPDATE BORROWER set FirstName='" + this.FirstName + "', LastName='" + this.LastName + "', username='" + this.UserName + "', password='" + passSalt + "', salt='" + salt+"' WHERE ID=" + this.ID, con);
            }            
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

        public static List<User> getAll()
        {
            List<User> results = new List<User>();
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM BORROWER", con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    User newUser = new User();
                    newUser.PersonId = dar["PersonId"] as string;
                    newUser.UserName = dar["username"] as string;
                    newUser.FirstName = dar["FirstName"] as string;
                    newUser.LastName = dar["LastName"] as string;
                    newUser.Password = dar["password"] as string;
                    results.Add(newUser);
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
        public static User getByPersonId(string PersonId)
        {
            User newUser = null;
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM BORROWER WHERE PersonId=@PersonId", con);
            SqlParameter paramPersonId = new SqlParameter("PersonId", SqlDbType.NVarChar);
            paramPersonId.Value = PersonId;
            cmd.Parameters.Add(paramPersonId);

            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                if (dar.Read())
                {
                    newUser = new User();
                    newUser.PersonId = dar["PersonId"] as string;
                    newUser.UserName = dar["username"] as string;
                    newUser.FirstName = dar["FirstName"] as string;
                    newUser.LastName = dar["LastName"] as string;
                    newUser.Password = dar["password"] as string;
                    newUser.ID = (int)dar["ID"];
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
            return newUser;
        }

        public static User getByusername (string username)
        {
            User newUser = null;
            SqlConnection con = new SqlConnection(Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM BORROWER WHERE username='"+username+"'", con);
            SqlParameter paramUsername = new SqlParameter("username", SqlDbType.NVarChar);
            paramUsername.Value = username;
            cmd.Parameters.Add(paramUsername);

            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                if (dar.Read())
                {
                    newUser = new User();
                    newUser.PersonId = dar["PersonId"] as string;
                    newUser.UserName = dar["username"] as string;
                    newUser.FirstName = dar["FirstName"] as string;
                    newUser.LastName = dar["LastName"] as string;
                    newUser.Password = dar["password"] as string;
                    newUser.Salt = dar["salt"] as string;
                    newUser.isAdmin = (int)dar["isAdmin"];
                    newUser.ID = (int)dar["ID"];
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
            return newUser;
        }

        public static List<User> search(string query)
        {
            string SQL = "SELECT * FROM BORROWER WHERE FirstName LIKE '%" + query + "%' OR LastName LIKE '%" + query + "%'";
            List<User> results = new List<User>();
            SqlConnection con = new SqlConnection(Library_BL.Settings.ConnectionString);
            SqlCommand cmd = new SqlCommand(SQL, con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                while (dar.Read())
                {
                    User newUser = new User();
                    newUser.PersonId = dar["PersonId"] as string;
                    newUser.UserName = dar["username"] as string;
                    newUser.Password = dar["password"] as string;  
                    newUser.FirstName = dar["FirstName"] as string;
                    newUser.LastName = dar["LastName"] as string;
                    results.Add(newUser);
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
        public static void borrowBook(string personId, string isbn)
        {
            List <Book> books = new List<Book>();
            books = Book.getBarCode(isbn);
            int barcode=0;
            DateTime borrowDate = DateTime.Now;
            if( books.Count() > 0)
            {
                barcode = Convert.ToInt32(books[0].Barcode);
                SqlConnection con = new SqlConnection(Library_BL.Settings.ConnectionString);
                SqlCommand cmd = new SqlCommand("INSERT INTO BORROW (Barcode, PersonId, BorrowDate, ToBeReturnedDate) VALUES (" + barcode + ",'" + personId + "', @time, @time2)" , con);
                cmd.Parameters.AddWithValue("@time", borrowDate);
                cmd.Parameters.AddWithValue("@time2", borrowDate.AddMonths(1));
                List<User> results = new List<User>();
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
                    Library_BL.Book.changeStatus(barcode, 2);
                    con.Close();
                }
            }
            else 
            {
                //FÅR EJ LÅNA, finns ej böcker kvar
            }
            
        }
        

        

        
    }

   
}

