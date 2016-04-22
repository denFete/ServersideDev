using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Library_BL;


namespace MyAPI.Controllers
{
    public class ValuesController : ApiController
    {
        
        // GET api/values
        public IEnumerable<Library_BL.Book> Get(string query)
        {
            List<Library_BL.Book> bookList = new List<Library_BL.Book>();
            bookList = Library_BL.Book.search(query);
            return bookList;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}