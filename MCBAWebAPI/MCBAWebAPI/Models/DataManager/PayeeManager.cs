using MvcBank.Data;
using MvcBank.Models.Repository;

namespace MvcBank.Models.DataManager;

public class PayeeManager : IDataRepository<Payee, int>
{
    private readonly MvcBankContext _context;

    public PayeeManager(MvcBankContext context)
    {
        _context = context;
    }

    public Payee Get(int payeeID)
    {
        return _context.Payee.Find(payeeID);
    }

    public IEnumerable<Payee> GetAll()
    {
        return _context.Payee.ToList();
    }

    public int Add(Payee payee)
    {
        _context.Payee.Add(payee);
        _context.SaveChanges();

        return payee.PayeeID;
    }

    public int Delete(int payeeID)
    {
        _context.Payee.Remove(_context.Payee.Find(payeeID));
        _context.SaveChanges();

        return payeeID;
    }

    public int Update(int payeeID, Payee payee)
    {
        _context.Update(payee);
        _context.SaveChanges();
            
        return payeeID;
    }
}
