using Microsoft.AspNetCore.Mvc;

using MvcBank.Models;
using MvcBank.Models.DataManager;

namespace s3717205_a2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionManager _repo;

        public TransactionController(TransactionManager repo)
        {
            _repo = repo;
        }

        // GET: api/movies
        [HttpGet]
        public IEnumerable<Transaction> Get()
        {
            return _repo.GetAll();
        }

        // GET api/movies/1
        [HttpGet("{id}")]
        public Transaction Get(int transactionID)
        {
            return _repo.Get(transactionID);
        }

        // POST api/movies
        [HttpPost]
        public void Post([FromBody] Transaction transaction)
        {
            _repo.Add(transaction);
        }

        // PUT api/movies
        [HttpPut]
        public void Put([FromBody] Transaction transaction)
        {
            _repo.Update(transaction.TransactionID, transaction);
        }

        // DELETE api/movies/1
        [HttpDelete("{id}")]
        public long Delete(int transactionID)
        {
            return _repo.Delete(transactionID);
        }
    }
}
