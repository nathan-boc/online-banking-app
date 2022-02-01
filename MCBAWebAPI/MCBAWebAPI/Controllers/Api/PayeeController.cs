using Microsoft.AspNetCore.Mvc;

using MvcBank.Models;
using MvcBank.Models.DataManager;

namespace s3717205_a2.Controllers.Api
{
	[ApiController]
	[Route("api/[controller]")]
	public class PayeeController : ControllerBase
	{
		private readonly PayeeManager _repo;

		public PayeeController(PayeeManager repo)
		{
			_repo = repo;
		}

		// GET: api/movies
		[HttpGet]
		public IEnumerable<Payee> Get()
		{
			return _repo.GetAll();
		}

		// GET api/movies/1
		[HttpGet("{id}")]
		public Payee Get(int payeeID)
		{
			return _repo.Get(payeeID);
		}

		// POST api/movies
		[HttpPost]
		public void Post([FromBody] Payee payee)
		{
			_repo.Add(payee);
		}

		// PUT api/movies
		[HttpPut]
		public void Put([FromBody] Payee payee)
		{
			_repo.Update(payee.PayeeID, payee);
		}

		// DELETE api/movies/1
		[HttpDelete("{id}")]
		public int Delete(int payeeID)
		{
			return _repo.Delete(payeeID);
		}
	}
}