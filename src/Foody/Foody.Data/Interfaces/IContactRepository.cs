namespace Foody.Data.Interfaces;

public interface IContactRepository : IRepository<Contact> 
{ 
    Task<bool> ToggleRead(int id);
}

