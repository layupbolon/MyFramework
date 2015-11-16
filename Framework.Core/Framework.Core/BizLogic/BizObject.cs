using Framework.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Framework.Core.BizLogic
{
    /// <summary>
    /// 业务逻辑对象基础基类
    /// </summary>
    /// <typeparam name="TContext">数据库上下文对象类型</typeparam>
    /// <typeparam name="TEntity">实体对象类型</typeparam>
    [DataObject]
    public class BizObject<TContext, TEntity>
        where TContext : DbContext, new()
        where TEntity : class 
    {

        /// <summary>
        /// 构建业务逻辑对象基础基类
        /// </summary>
        public BizObject()
        {
            this.Context = new TContext();
            this.Data = this.Context.Set<TEntity>();
        }


        /// <summary>
        /// 构建业务逻辑对象基础基类
        /// </summary>
        /// <param name="context">数据库上下文对象</param>
        public BizObject(TContext context)
        {
            this.Context = context;
            this.Data = this.Context.Set<TEntity>();
        }

        /// <summary>
        /// 业务逻辑对象对应的数据库上下文对象
        /// </summary>
        internal TContext Context { get; set; }
        /// <summary>
        /// 业务逻辑对象对应的实体集合
        /// </summary>
        protected DbSet<TEntity> Data { get; set; }
        /// <summary>
        /// 主键名
        /// </summary>
        protected virtual string PrimaryKey { get { return "SysNo"; } }
        /// <summary>
        /// 是否允许脏读，目前默认不允许
        /// </summary>
        public bool AllowNolockQuery { get; set; }

        /// <summary>
        /// 获取记录数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            if (this.AllowNolockQuery)
            {
                int rlt = 0;
                QueryWithNoLock(() =>
                {
                    rlt = this.Data.Count();
                });
                return rlt;
            }
            else
            {
                return this.Data.Count();
            }
        }

        /// <summary>
        /// 根据给定条件获取符合条件的记录数量
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public int Count(Expression<Func<TEntity, bool>> where)
        {
            return this.Count(where, null);
        }

        /// <summary>
        /// 根据给定条件获取符合条件的记录数量
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public int Count(Expression<Func<TEntity, bool>> where, string orderBy)
        {
            var query = this.Data as IQueryable<TEntity>;
            if (where != null)
            {
                query = query.Where(where);
            }

            if (this.AllowNolockQuery)
            {
                int rlt = 0;
                QueryWithNoLock(() =>
                {
                    rlt = query.Count();
                });
                return rlt;
            }
            else
            {
                return query.Count();
            }
        }

        /// <summary>
        /// 根据给定条件获取符合条件的记录数量
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public int Count(string strWhere)
        {
            return this.Count(strWhere, null);
        }

        /// <summary>
        /// 根据给定条件获取符合条件的记录数量
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public int Count(string strWhere, string orderBy)
        {
            var query = this.Data as IQueryable<TEntity>;
            if (!string.IsNullOrEmpty(strWhere))
            {
                query = query.Where(strWhere);
            }

            if (this.AllowNolockQuery)
            {
                int rlt = 0;
                QueryWithNoLock(() =>
                {
                    rlt = query.Count();
                });
                return rlt;
            }
            else
            {
                return query.Count();
            }
        }


        /// <summary>
        /// 判断是否存在任何记录
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            return this.Exists(null);
        }

        /// <summary>
        /// 根据给定条件判断是否存在符合条件的记录
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public bool Exists(Expression<Func<TEntity, bool>> where)
        {
            var query = this.Data as IQueryable<TEntity>;
            if (where != null)
            {
                query = query.Where(where);
            }


            if (this.AllowNolockQuery)
            {
                bool rlt = false;
                QueryWithNoLock(() =>
                {
                    rlt = query.Any();
                });
                return rlt;
            }
            else
            {
                return query.Any();
            }
        }


        /// <summary>
        /// 获取数据实体列表
        /// </summary>
        /// <returns></returns>
        public virtual List<TEntity> GetAllList()
        {
            if (this.AllowNolockQuery)
            {
                List<TEntity> rlt = null;
                QueryWithNoLock(() =>
                {
                    rlt = this.Data.ToList();
                });
                return rlt;
            }
            else
            {
                return this.Data.ToList();
            }
        }

        /// <summary>
        /// 根据给定条件获取数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> where)
        {
            return GetAllList(where, null);
        }

        /// <summary>
        /// 根据给定条件获取数据实体列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public virtual List<TEntity> GetAllList(string strWhere)
        {
            return GetAllList(strWhere, null);
        }


        /// <summary>
        /// 根据给定条件获取数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <returns></returns>
        public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> where, string orderBy)
        {
            var query = this.Data as IQueryable<TEntity>;

            if (where != null)
            {
                query = query.Where(where);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                query = query.OrderBy(orderBy);
            }

            if (this.AllowNolockQuery)
            {
                List<TEntity> rlt = null;
                QueryWithNoLock(() =>
                {
                    rlt = query.ToList();
                });
                return rlt;
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// 获取数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <returns></returns>
        public virtual List<TEntity> GetAllList(string strWhere, string orderBy)
        {
            var query = this.Data as IQueryable<TEntity>;

            if (!string.IsNullOrEmpty(strWhere))
            {
                query = query.Where(strWhere);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                query = query.OrderBy(orderBy);
            }

            if (this.AllowNolockQuery)
            {
                List<TEntity> rlt = null;
                QueryWithNoLock(() =>
                {
                    rlt = query.ToList();
                });
                return rlt;
            }
            else
            {
                return query.ToList();
            }
        }



        /// <summary>
        /// 分页获取数据实体列表
        /// </summary>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public virtual List<TEntity> GetPagedList(int startRowIndex, int maximumRows)
        {

            startRowIndex = startRowIndex < 0 ? 0 : startRowIndex;

            var query = this.Data.Skip(startRowIndex).Take(maximumRows);

            if (this.AllowNolockQuery)
            {
                List<TEntity> rlt = null;
                QueryWithNoLock(() =>
                {
                    rlt = query.ToList();
                });
                return rlt;
            }
            else
            {
                return query.ToList();
            }

        }

        /// <summary>
        /// 根据给定条件分页获取数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public virtual List<TEntity> GetPagedList(Expression<Func<TEntity, bool>> where, int startRowIndex, int maximumRows)
        {
            return GetPagedList(where, null, startRowIndex, maximumRows);
        }

        /// <summary>
        /// 根据给定条件分页获取数据实体列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public virtual List<TEntity> GetPagedList(string strWhere, int startRowIndex, int maximumRows)
        {
            return GetPagedList(strWhere, null, startRowIndex, maximumRows);
        }


        /// <summary>
        /// 根据给定条件分页获取数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public virtual List<TEntity> GetPagedList(Expression<Func<TEntity, bool>> where, string orderBy, int startRowIndex, int maximumRows)
        {
            var query = this.Data as IQueryable<TEntity>;
            if (where != null)
            {
                query = query.Where(where);
            }

            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = string.Format("{0} DESC", this.PrimaryKey);
            }

            query = query.OrderBy(orderBy);

            startRowIndex = startRowIndex < 0 ? 0 : startRowIndex;

            query = query.Skip(startRowIndex).Take(maximumRows);

            if (this.AllowNolockQuery)
            {
                List<TEntity> rlt = null;
                QueryWithNoLock(() =>
                {
                    rlt = query.ToList();
                });
                return rlt;
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// 根据给定条件分页获取数据实体列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public virtual List<TEntity> GetPagedList(string strWhere, string orderBy, int startRowIndex, int maximumRows)
        {
            var query = this.Data as IQueryable<TEntity>;
            if (!string.IsNullOrEmpty(strWhere))
            {
                query = query.Where(strWhere);
            }

            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = string.Format("{0} DESC", this.PrimaryKey);
            }

            query = query.OrderBy(orderBy);

            startRowIndex = startRowIndex < 0 ? 0 : startRowIndex;

            query = query.Skip(startRowIndex).Take(maximumRows);

            if (this.AllowNolockQuery)
            {
                List<TEntity> rlt = null;
                QueryWithNoLock(() =>
                {
                    rlt = query.ToList();
                });
                return rlt;
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// 获取指定个数排名靠前的实体列表
        /// </summary>
        /// <param name="top">个数</param>
        /// <returns></returns>
        public virtual List<TEntity> Top(int top)
        {
            return this.Top(null, null, top);
        }

        /// <summary>
        /// 获取指定个数排名靠前的实体列表
        /// </summary>
        /// <param name="top">个数</param>
        /// <returns></returns>
        public virtual List<TEntity> Top(Expression<Func<TEntity, bool>> where, int top)
        {
            return this.Top(where, null, top);
        }

        /// <summary>
        /// 根据给定条件获取符合条件指定个数排名靠前的实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="top">个数</param>
        /// <returns></returns>
        public virtual List<TEntity> Top(Expression<Func<TEntity, bool>> where, string orderBy, int top)
        {
            var query = this.Data as IQueryable<TEntity>;
            if (where != null)
            {
                query = query.Where(where);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                query = query.OrderBy(orderBy);
            }

            query = query.Take(top);

            if (this.AllowNolockQuery)
            {
                List<TEntity> rlt = null;
                QueryWithNoLock(() =>
                {
                    rlt = query.ToList();
                });
                return rlt;
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// 根据给定条件获取符合条件的第一个实体
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public virtual TEntity TopOne(Expression<Func<TEntity, bool>> where)
        {
            return this.TopOne(where, null);
        }

        /// <summary>
        /// 根据给定条件获取符合条件的首个实体
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <returns></returns>
        public virtual TEntity TopOne(Expression<Func<TEntity, bool>> where, string orderBy)
        {

            var query = this.Data as IQueryable<TEntity>;
            if (where != null)
            {
                query = query.Where(where);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                query = query.OrderBy(orderBy);
            }
            query = query.Take(1);

            if (this.AllowNolockQuery)
            {
                TEntity rlt = null;
                QueryWithNoLock(() =>
                {
                    rlt = query.FirstOrDefault();
                });
                return rlt;
            }
            else
            {
                return query.FirstOrDefault();
            }

        }

        /// <summary>
        /// 根据主键编号获取实体
        /// </summary>
        /// <param name="sysNo">主键编号</param>
        /// <returns></returns>
        public virtual TEntity GetBySysNo(int sysNo)
        {
            if (this.AllowNolockQuery)
            {
                TEntity rlt = null;
                QueryWithNoLock(() =>
                {
                    rlt = this.Data.Find(sysNo);
                });
                return rlt;
            }
            else
            {
                return this.Data.Find(sysNo);
            }
        }

        /// <summary>
        /// 通过指定实体插入数据;
        /// 完成后修改仅存在于Context中，如需提交请调用SaveChanges方法
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual void Insert(TEntity entity)
        {
            this.Data.Add(entity);
        }

        /// <summary>
        /// 通过指定实体修改数据;
        /// 完成后修改仅存在于Context中，如需提交请调用SaveChanges方法
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual void Update(TEntity entity)
        {
            this.Context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// 通过指定实体删除数据;
        /// 完成后修改仅存在于Context中，如需提交请调用SaveChanges方法
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual void Delete(TEntity entity)
        {

            var s = entity as IStatusFlag;
            if (s==null)
            {
                this.Data.Remove(entity); 
            }
            else
            {
                s.Status = -1;//逻辑删除
                this.Context.Entry(entity).State = EntityState.Modified;
            }
            

        }

        /// <summary>
        /// 通过指定主键编号删除数据;
        /// 完成后修改仅存在于Context中，如需提交请调用SaveChanges方法
        /// </summary>
        /// <param name="sysNo">主键编号</param>
        public virtual void Delete(int sysNo)
        {
            var entity = this.Data.Find(sysNo);
            var e = entity as IStatusFlag;
            if (e == null)
            {
                this.Data.Remove(entity);
            }
            else
            {
                e.Status = -1;//逻辑删除
                this.Context.Entry(entity).State = EntityState.Modified;
            }
            
        }

        /// <summary>
        /// 通过指定条件批量删除数据;
        /// 完成后修改仅存在于Context中，如需提交请调用SaveChanges方法
        /// </summary>
        /// <param name="where"></param>
        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            if (where != null)
            {
                var entities = this.Data.Where(where);
                var type = typeof(IStatusFlag);
                if (type.IsAssignableFrom(typeof(TEntity)))
                {
                    foreach (var entity in entities)
                    {
                        var e = entity as IStatusFlag;
                        e.Status = -1;//逻辑删除
                        this.Context.Entry(entity).State = EntityState.Modified;
                    }
                }
                else
                {
                    this.Data.RemoveRange(entities);
                }
            }
        }


        /// <summary>
        /// 向数据库提交数据变更
        /// </summary>
        /// <returns>返回影响记录</returns>
        public int SaveChanges()
        {
            return this.Context.SaveChanges();
        }

        /// <summary>
        /// 采用ReadUncommitted类型的事务执行指定数据库操作
        /// </summary>
        /// <param name="action">指定数据库操作</param>
        public static void QueryWithNoLock(Action action)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required
                , new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                action();
                scope.Complete();
            }
        }

    }
}
