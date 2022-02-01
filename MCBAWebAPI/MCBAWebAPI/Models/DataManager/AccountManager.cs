using MvcBank.Data;
using MvcBank.Models.Repository;

namespace MvcBank.Models.DataManager;

public class AccountManager : IDataRepository<Account, int>
{
    private readonly MvcBankContext _context;

    public AccountManager(MvcBankContext context)
    {
        _context = context;
    }

    public Account Get(int accountNumber)
    {
        return _context.Account.Find(accountNumber);
    }

    public IEnumerable<Account> GetAll()
    {
        return _context.Account.ToList();
    }

    public int Add(Account account)
    {
        _context.Account.Add(account);
        _context.SaveChanges();

        return account.AccountNumber;
    }

    public int Delete(int accountNumber)
    {
        _context.Account.Remove(_context.Account.Find(accountNumber));
        _context.SaveChanges();

        return accountNumber;
    }

    public int Update(int accountNumber, Account account)
    {
        _context.Update(account);
        _context.SaveChanges();
            
        return accountNumber;
    }
}
