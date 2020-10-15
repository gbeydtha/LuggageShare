using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryContext repositoryContext): base(repositoryContext)
        {

        }

        public IEnumerable<Account> AccountsByOwner (Guid ownerId)
        {
            return FindByCondition(acc => acc.OwnerId.Equals(ownerId)).ToList(); 
        }
    }
}
