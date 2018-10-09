using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory DBFactory;
        private MembershipContext MemberContext;

        public UnitOfWork(IDbFactory DbFactory)
        {
            this.DBFactory = DbFactory;
        }

        public MembershipContext DbContext
        {
            get { return MemberContext ?? (MemberContext = DBFactory.Init()); }
        }

        public void Commit()
        {
            MemberContext.Commit();
        }
    }
}
