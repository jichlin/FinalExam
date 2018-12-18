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
        private OJD OJDs;

        public UnitOfWork(IDbFactory DbFactory)
        {
            this.DBFactory = DbFactory;
        }

        public OJD DbContext
        {
            get { return OJDs ?? (OJDs = DBFactory.Init()); }
        }

        public void Commit()
        {
            OJDs.Commit();
        }
    }
}
