using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Remotely.Server.Auth;
using Remotely.Server.Hubs;
using Remotely.Server.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Remotely.Server.API
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ApiAuthorizationFilter))]
    public class SessionController : ControllerBase
    {
        
        // GET: api/<SessionController>
        [HttpGet]
        public IEnumerable<RCSessionInfo> Get()
        {
            return CasterHub.SessionInfoList.Select(a=> a.Value).AsEnumerable();
        }

        //// GET api/<SessionController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<SessionController>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<SessionController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<SessionController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
