using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Infrastructure
{
    public class DBFactory : Disposable, IDbFactory
    {
        OJD OJDs;

        public OJD Init()
        {
            return OJDs ?? (OJDs = new OJD());
        }

        protected override void DisposeCore()
        {
            if (OJDs != null)
                OJDs.Dispose();
        }
    }
}
