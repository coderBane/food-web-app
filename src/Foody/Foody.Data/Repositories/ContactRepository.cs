using System;
using Foody.Data.Data;
using Foody.Data.Interfaces;
using Foody.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Foody.Data.Repositories
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(FoodyDbContext context) : base(context)
        {
        }

        public override Task<Contact?> Get(int id)
        {
            return _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

