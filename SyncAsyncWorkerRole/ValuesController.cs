using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace SyncAsyncDotnet.Controllers
{
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        private const int NCalls = 2;
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("sync/{id}")]
        public string Get(int id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                for (int i = 0; i < NCalls; i++)
                {
                    httpClient.GetAsync(
                        "https://img.rl0.ru/666c5c4cea9fb7637d4b9dac0809e3a3/c143x90q75/news.rambler.ru/img/2017/03/07202409.114892.5113.jpg")
                        .Result
                        .Content.ReadAsStreamAsync().Result
                        .ReadByte();
                }
            }

            return "sync";
        }

        [Route("async/{id}")]
        public async Task<string> GetAsync(int id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                for (int i = 0; i < NCalls; i++)
                {
                    var response =
                        await
                            httpClient.GetAsync(
                                "https://img.rl0.ru/666c5c4cea9fb7637d4b9dac0809e3a3/c143x90q75/news.rambler.ru/img/2017/03/07202409.114892.5113.jpg");
                    var stream = await response.Content.ReadAsStreamAsync();
                    stream.ReadByte();
                }
            }

            return "async";
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
