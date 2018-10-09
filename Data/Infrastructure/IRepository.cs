using Model.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        void Add(T Entity);
        void Update(T Entity);
        void Delete(T Entity);
        void Delete(Expression<Func<T, bool>> where);

        T GetById(int Id);
        T Get(Expression<Func<T, bool>> where);
        T ExecSPToSingle(string SPName, object[] Param = null);
        Task<T> ExecSPToSingleAsync(string SPName, object[] Param = null);

        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        IEnumerable<T> ExecSPToList(string SPName, SqlParameter[] Param = null);
        Task<IEnumerable<T>> ExecSPToListAsync(string SPName, SqlParameter[] Param = null);
        Task<ExecuteResult> ExecMultipleSPWithTransaction(List<StoredProcedure> SP);
    }
}
