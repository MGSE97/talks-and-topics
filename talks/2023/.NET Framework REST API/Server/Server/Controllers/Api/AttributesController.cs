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

        // GET api/attributes
        /// <summary>
        /// Gonna
        /// </summary>
        [HttpGet]
        public IEnumerable<string> Gonna()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/attributes/5
        /// <summary>
        /// Give
        /// </summary>
        [HttpGet]
        public string Give(int id)
        {
            return "value";
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
        public void Up(int id, [FromBody] string value)
        {
        }
    }
}
