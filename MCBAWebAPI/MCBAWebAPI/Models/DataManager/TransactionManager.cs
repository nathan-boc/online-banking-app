using MvcBank.Data;
using MvcBank.Models.Repository;

namespace MvcBank.Models.DataManager;

public class TransactionManager : IDataRepository<Transaction, int>
{
    private readonly MvcBankContext _context;

    public TransactionManager(MvcBankContext context)
    {
        _context = context;
    }

    public Transaction Get(int transactionID)
    {
        return _context.Transaction.Find(transactionID);
    }

    public IEnumerable<Transaction> GetAll()
    {
        return _context.Transaction.ToList();
    }

    public int Add(Transaction transaction)
    {
        _context.Transaction.Add(transaction);
        _context.SaveChanges();

        return transaction.TransactionID;
    }

    public int Delete(int transactionID)
    {
        _context.Transaction.Remove(_context.Transaction.Find(transactionID));
        _context.SaveChanges();

        return transactionID;
    }

    public int Update(int transactionID, Transaction transaction)
    {
        _context.Update(transaction);
        _context.SaveChanges();
            
        return transactionID;
    }
}
