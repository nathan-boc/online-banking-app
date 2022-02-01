using MvcBank.Data;
using MvcBank.Models.Repository;

namespace MvcBank.Models.DataManager;

public class LoginManager : IDataRepository<Login, string>
{
    private readonly MvcBankContext _context;

    public LoginManager(MvcBankContext context)
    {
        _context = context;
    }

    public Login Get(string loginID)
    {
        return _context.Login.Find(loginID);
    }

    public IEnumerable<Login> GetAll()
    {
        return _context.Login.ToList();
    }

    public string Add(Login login)
    {
        _context.Login.Add(login);
        _context.SaveChanges();

        return login.LoginID;
    }

    public string Delete(string loginID)
    {
        _context.Login.Remove(_context.Login.Find(loginID));
        _context.SaveChanges();

        return loginID;
    }

    public string Update(string loginID, Login login)
    {
        _context.Update(login);
        _context.SaveChanges();
            
        return loginID;
    }
}
