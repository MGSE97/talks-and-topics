using System.Collections.Generic;
using System.Web.Http;

namespace Server.Controllers
{
    public class AttributesController : ApiController
    {
        // DELETE api/attributes/5
        /// <summary>
        /// Never
        /// </summary>
        [HttpDelete]
        public void Never(int id)
        {
        }

        // GET api/attributes/5
        /// <summary>
        /// Gonna
        /// </summary>
        [HttpGet]
        public string Gonna(int id)
        {
            return "value";
        }

        // GET api/attributes
        /// <summary>
        /// Give
        /// </summary>
        [HttpGet]
        public IEnumerable<string> Give()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/attributes
        /// <summary>
        /// You
        /// </summary>
        [HttpPost]
        public void You([FromBody] string value)
        {
        }

        // PUT api/attributes/5
        /// <summary>
        /// Up
        /// </summary>
        [HttpPut]
        public void Up([FromBody] string value)
        {
        }
    }
}
