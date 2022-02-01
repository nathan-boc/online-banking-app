using Microsoft.AspNetCore.Mvc;

using MvcBank.Models;
using MvcBank.Models.DataManager;

namespace s3717205_a2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginManager _repo;

        public LoginController(LoginManager repo)
        {
            _repo = repo;
        }

        // GET: api/movies
        [HttpGet]
        public IEnumerable<Login> Get()
        {
            return _repo.GetAll();
        }

        // GET api/movies/1
        [HttpGet("{id}")]
        public Login Get(string loginID)
        {
            return _repo.Get(loginID);
        }

        // POST api/movies
        [HttpPost]
        public void Post([FromBody] Login login)
        {
            _repo.Add(login);
        }

        // PUT api/movies
        [HttpPut]
        public void Put([FromBody] Login login)
        {
            _repo.Update(login.LoginID, login);
        }

        // DELETE api/movies/1
        [HttpDelete("{id}")]
        public string Delete(string loginID)
        {
            return _repo.Delete(loginID);
        }
    }
}
