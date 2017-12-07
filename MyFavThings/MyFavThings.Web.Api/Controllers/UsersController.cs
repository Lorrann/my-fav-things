using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MyFavThings.Web.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {

        /// <summary>
        /// get user
        /// </summary>
        /// <returns>Returns user</returns>
        /// <response code="200">Returns user</response>
        [ProducesResponseType(typeof(string), 200)]  
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("UserName");
        }
    }
}
