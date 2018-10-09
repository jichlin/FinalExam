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
        MembershipContext MemberContext;

        public MembershipContext Init()
        {
            return MemberContext ?? (MemberContext = new MembershipContext());
        }

        protected override void DisposeCore()
        {
            if (MemberContext != null)
                MemberContext.Dispose();
        }
    }
}
