using Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Model.Base;

namespace Data.Infrastructure
{
    public class RepositoryBase<T> where T : class
    {
        #region Properties
        private MembershipContext MemberContext;
        private readonly IDbSet<T> DBSet;
        enum ExecType { List, Single, NoExecRecord };

        protected IDbFactory DBFactory
        {
            get;
            private set;
        }

        protected MembershipContext DbContext
        {
            get { return MemberContext ?? (MemberContext = DBFactory.Init()); }
        }

        protected RepositoryBase(IDbFactory DbFactory)
        {
            DBFactory = DbFactory;
            DBSet = DbContext.Set<T>();
        }

        public virtual void Add(T Entity)
        {
            DBSet.Add(Entity);
        }

        public virtual void Update(T Entity)
        {
            DBSet.Attach(Entity);
            MemberContext.Entry(Entity).State = EntityState.Modified;
        }

        public virtual void Delete(T Entity)
        {
            DBSet.Remove(Entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> Objects = DBSet.Where<T>(where).AsEnumerable();
            foreach (T OBJ in Objects)
                DBSet.Remove(OBJ);
        }

        public virtual T GetById(int ID)
        {
            return DBSet.Find(ID);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return DBSet.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return DBSet.Where(where).ToList();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return DBSet.Where(where).FirstOrDefault<T>();
        }


        public virtual IEnumerable<T> ExecSPToList(string SPName, SqlParameter[] Param = null)
        {
            if (Param == null)
            {
                return DbContext.Database.SqlQuery<T>(SPName).ToList<T>();
            }
            else
            {
                return DbContext.Database.SqlQuery<T>(SPName, Param).ToList<T>();
            }
        }


        public virtual async Task<IEnumerable<T>> ExecSPToListAsync(string SPName, SqlParameter[] Param = null)
        {
            if (Param == null)
            {
                var Query = DbContext.Database.SqlQuery<T>(SPName);
                return await Query.ToListAsync();
            }
            else
            {
                try
                {
                    var Query = DbContext.Database.SqlQuery<T>(SPName, Param);
                    var a = Query.ToListAsync();
                    return await a;
                }
                catch (Exception e)
                {
                    var Query = DbContext.Database.SqlQuery<T>(SPName, Param);
                    return await Query.ToListAsync();
                }
            }
        }


        public virtual T ExecSPToSingle(string SPName, object[] Param = null)
        {
            if (Param != null)
            {
                return DbContext.Database.SqlQuery<T>(SPName, Param).FirstOrDefault<T>();
            }
            else
            {
                return DbContext.Database.SqlQuery<T>(SPName).FirstOrDefault<T>();
            }
        }


        public virtual async Task<T> ExecSPToSingleAsync(string SPName, object[] Param = null)
        {
            if (Param != null)
            {
                var Query = DbContext.Database.SqlQuery<T>(SPName, Param);
                return await Query.FirstOrDefaultAsync();
            }
            else
            {
                var Query = DbContext.Database.SqlQuery<T>(SPName);
                return await Query.FirstOrDefaultAsync();
            }
        }


        public virtual async Task<ExecuteResult> ExecMultipleSPWithTransaction(List<StoredProcedure> SP)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            ReturnValue.Status = null;
            ReturnValue.ReturnVariable = 0;
            using (var dbContextTransaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (StoredProcedure SPItem in SP)
                    {
                        if (SPItem.SQLParam == null)
                        {
                            //SQLCommand yang dicomment adalah untuk menjalankan SP tapi return valuenya adalah jumlah row yang affected
                            //ReturnValue.ReturnVariable = await DbContext.Database.ExecuteSqlCommandAsync("EXEC " + SPItem.SPName);

                            //SQLCommand ini digunakan untuk mengambil return value yang berupa int
                            var Query = DbContext.Database.SqlQuery<Int32>(SPItem.SPName);
                            ReturnValue.ReturnVariable = await Query.FirstOrDefaultAsync();
                        }
                        else
                        {
                            //ReturnValue.ReturnVariable = await DbContext.Database.ExecuteSqlCommandAsync("EXEC " + SPItem.SPName, SPItem.SQLParam);
                            var Query = DbContext.Database.SqlQuery<Int32>(SPItem.SPName, SPItem.SQLParam);
                            ReturnValue.ReturnVariable = await Query.FirstOrDefaultAsync();
                        }
                    }
                    DbContext.SaveChanges();
                    dbContextTransaction.Commit();
                    ReturnValue.Status = true;

                }
                catch (Exception EX)
                {
                    dbContextTransaction.Rollback();
                    ReturnValue.Exception = EX;
                    ReturnValue.Message = EX.Message;
                    ReturnValue.Status = false;
                }
                return ReturnValue;
            }

        }
        #endregion
    }
}
