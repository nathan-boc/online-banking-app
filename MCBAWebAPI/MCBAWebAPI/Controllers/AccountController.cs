using Microsoft.AspNetCore.Mvc;

using MvcBank.Models;
using MvcBank.Models.DataManager;

namespace s3717205_a2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountManager _repo;

        public AccountController(AccountManager repo)
        {
            _repo = repo;
        }

        // GET: api/movies
        [HttpGet]
        public IEnumerable<Account> Get()
        {
            return _repo.GetAll();
        }

        // GET api/movies/1
        [HttpGet("{id}")]
        public Account Get(int accountNumber)
        {
            return _repo.Get(accountNumber);
        }

        // POST api/movies
        [HttpPost]
        public void Post([FromBody] Account account)
        {
            _repo.Add(account);
        }

        // PUT api/movies
        [HttpPut]
        public void Put([FromBody] Account account)
        {
            _repo.Update(account.AccountNumber, account);
        }

        // DELETE api/movies/1
        [HttpDelete("{id}")]
        public long Delete(int accountNumber)
        {
            return _repo.Delete(accountNumber);
        }
    }
}
