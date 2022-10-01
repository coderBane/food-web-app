using System;
using Foody.Data.Data;
using Foody.Data.Interfaces;
using Foody.Entities.Models;

namespace Foody.Data.Repositories
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(FoodyDbContext context) : base(context)
        {
        }
    }
}

