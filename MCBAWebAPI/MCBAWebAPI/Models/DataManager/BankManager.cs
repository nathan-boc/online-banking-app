using MvcBank.Data;
using MvcBank.Models.Repository;

namespace MvcBank.Models.DataManager;

public class BankManager : IDataRepository<Customer, int>
{
    private readonly MvcBankContext _context;

    public BankManager(MvcBankContext context)
    {
        _context = context;
    }

    public Customer Get(int id)
    {
        return _context.Customer.Find(id);
    }

    public IEnumerable<Customer> GetAll()
    {
        return _context.Customer.ToList();
    }

    public int Add(Customer customer)
    {
        _context.Customer.Add(customer);
        _context.SaveChanges();

        return customer.CustomerID;
    }

    public int Delete(int id)
    {
        _context.Customer.Remove(_context.Customer.Find(id));
        _context.SaveChanges();

        return id;
    }

    public int Update(int id, Customer customer)
    {
        _context.Update(customer);
        _context.SaveChanges();
            
        return id;
    }
}
