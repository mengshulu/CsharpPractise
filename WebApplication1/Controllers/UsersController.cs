using System;
using System.Linq;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        // public ActionResult Index()
        // {
        //     return View();
        // }

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetUsers()
        {
            Console.WriteLine("Getting users...");
            var users = _context.Users.ToList();
            Console.WriteLine(users);
            return Ok(users);
        }
    }
}