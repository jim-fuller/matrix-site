using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Collections;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace matrix_site.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        public ArticlesController (
            IConfiguration configuration)
        {
            Configuration = configuration;

        }
    
    /// <summary>
    /// Gets the application configuration.
    /// </summary>
    protected IConfiguration Configuration { get; }
    // GET: api/<ArticlesController>
    [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "", "" };
        }
       

        // GET api/<ArticlesController>/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(Configuration.GetValue<string>("CCdb")))
                {
                    System.Diagnostics.Debug.WriteLine("id:" + id);
                    NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM ARTICLE WHERE lower(article_headline) like lower('%" + id + "%')", connection);
                    connection.Open();
                    System.Diagnostics.Debug.WriteLine(command.CommandText);
                    try
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            ArrayList data = new ArrayList();
                            while (reader.Read())
                            {
                                data.Add(new
                                {
                                    id = reader[0],
                                    author = reader[1],
                                    created = reader[2],
                                    headline = reader[3],
                                    permalink = reader[4],
                                    published = reader[5]

                                }); ;
                            }
                            return JsonConvert.SerializeObject(data);
                        }
                    }
                    catch (Exception e)
                    {
                        Exception p = e;
                        return string.Empty;
                    }
                }
            }
        }

        // POST api/<ArticlesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ArticlesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ArticlesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
