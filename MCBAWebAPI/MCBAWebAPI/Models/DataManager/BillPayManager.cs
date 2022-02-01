using MvcBank.Data;
using MvcBank.Models.Repository;

namespace MvcBank.Models.DataManager;

public class BillPayManager : IDataRepository<BillPay, int>
{
    private readonly MvcBankContext _context;

    public BillPayManager(MvcBankContext context)
    {
        _context = context;
    }

    public BillPay Get(int billPayID)
    {
        return _context.BillPay.Find(billPayID);
    }

    public IEnumerable<BillPay> GetAll()
    {
        return _context.BillPay.ToList();
    }

    public int Add(BillPay billPay)
    {
        _context.BillPay.Add(billPay);
        _context.SaveChanges();

        return billPay.BillPayID;
    }

    public int Delete(int billPayID)
    {
        _context.BillPay.Remove(_context.BillPay.Find(billPayID));
        _context.SaveChanges();

        return billPayID;
    }

    public int Update(int billPayID, BillPay billPay)
    {
        _context.Update(billPay);
        _context.SaveChanges();
            
        return billPayID;
    }
}
