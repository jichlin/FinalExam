using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Data.Context
{
    public class MembershipContext : DbContext
    {
        public MembershipContext() : base("MembershipContext")
        {

        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

    }
}
