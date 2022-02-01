using Microsoft.AspNetCore.Mvc;

using MvcBank.Models;
using MvcBank.Models.DataManager;

namespace s3717205_a2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillPayController : ControllerBase
    {
        private readonly BillPayManager _repo;

        public BillPayController(BillPayManager repo)
        {
            _repo = repo;
        }

        // GET: api/movies
        [HttpGet]
        public IEnumerable<BillPay> Get()
        {
            return _repo.GetAll();
        }

        // GET api/movies/1
        [HttpGet("{id}")]
        public BillPay Get(int billPayID)
        {
            return _repo.Get(billPayID);
        }

        // POST api/movies
        [HttpPost]
        public void Post([FromBody] BillPay billPay)
        {
            _repo.Add(billPay);
        }

        // PUT api/movies
        [HttpPut]
        public void Put([FromBody] BillPay billPay)
        {
            _repo.Update(billPay.BillPayID, billPay);
        }

        // DELETE api/movies/1
        [HttpDelete("{id}")]
        public long Delete(int billPayID)
        {
            return _repo.Delete(billPayID);
        }
    }
}
