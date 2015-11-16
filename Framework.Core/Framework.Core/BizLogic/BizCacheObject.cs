using Framework.Core.Caching;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.BizLogic
{
    /// <summary>
    /// 基于缓存的业务逻辑对象基础基类
    /// </summary>
    /// <typeparam name="TContext">数据库上下文对象类型</typeparam>
    /// <typeparam name="TEntity">实体对象类型</typeparam>
    [DataObject]
    public class BizCacheObject<TContext, TEntity> : BizObject<TContext, TEntity>
        where TContext : DbContext, new()
        where TEntity : class
    {

        /// <summary>
        /// 构建基于缓存的业务逻辑对象
        /// </summary>
        public BizCacheObject()
            : base()
        {

        }

        /// <summary>
        /// 构建基于缓存的业务逻辑对象
        /// </summary>
        /// <param name="context">数据库上下文对象</param>
        public BizCacheObject(TContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 缓存索引
        /// </summary>
        protected virtual string CacheKey
        {
            get
            {
                return this.GetType().FullName;
            }
        }

        /// <summary>
        /// 获取缓存记录数量
        /// </summary>
        /// <returns></returns>
        public int TryCacheCount()
        {
            var list = this.TryCacheAllList();
            var count = 0;
            if (list != null)
            {
                count = list.Count;
            }
            return count;
        }

        /// <summary>
        /// 根据给定条件获取符合条件的缓存记录数量
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public int TryCacheCount(Expression<Func<TEntity, bool>> where)
        {
            return this.TryCacheCount(where, null);
        }

        /// <summary>
        /// 根据给定条件获取符合条件的缓存记录数量
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public int TryCacheCount(Expression<Func<TEntity, bool>> where, string orderBy)
        {
            var list = this.TryCacheAllList();
            var count = 0;
            if (list != null)
            {
                var query = list.AsQueryable();
                if (where != null)
                {
                    query = query.Where(where);
                }

                count = query.Count();
            }
            return count;
        }

        /// <summary>
        /// 根据给定条件获取符合条件的缓存记录数量
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public int TryCacheCount(string strWhere)
        {
            return this.Count(strWhere, null);
        }

        /// <summary>
        /// 根据给定条件获取符合条件的缓存记录数量
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public int TryCacheCount(string strWhere, string orderBy)
        {
            var list = this.TryCacheAllList();
            int count = 0;
            if (list == null)
            {
                var query = list.AsQueryable();
                if (!string.IsNullOrEmpty(strWhere))
                {
                    query = query.Where(strWhere);
                }
                count= query.Count();
            }
            return count;
        }


        /// <summary>
        /// 判断是否存在任何缓存记录
        /// </summary>
        /// <returns></returns>
        public bool TryCacheExists()
        {
            return this.TryCacheExists(null);
        }

        /// <summary>
        /// 根据给定条件判断是否存在符合条件的缓存记录
        /// 
        /// 修改功能：原来判断条件是list == null，很明显应该是错误了。现修改如下：list != null
        /// 修改人：davidshu 2015-05-14
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public bool TryCacheExists(Expression<Func<TEntity, bool>> where)
        {
            var list = this.TryCacheAllList();
            bool exist = false;
            if (list != null)
            {
                var query = list.AsQueryable();
                if (where != null)
                {
                    query = query.Where(where);
                }
                exist = query.Any();
            }
            return exist;
        }

        /// <summary>
        /// 获取缓存数据实体列表，不存在缓存则调用GetAllList方法从数据库获取。
        /// </summary>
        /// <returns></returns>
        public virtual List<TEntity> TryCacheAllList()
        {
            var list = CacheManager.Default.Get<List<TEntity>>(this.CacheKey);
            if (list == null)
            {
                list = this.GetAllList();
                if (list != null)
                {
                    CacheManager.Default.Add(this.CacheKey, list);
                }
            }

            return list;
        }

        /// <summary>
        /// 根据给定条件获取缓存数据实体列表，不存在缓存则调用GetAllList方法从数据库获取。
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public virtual List<TEntity> TryCacheAllList(Expression<Func<TEntity, bool>> where)
        {
            return TryCacheAllList(where, null);
        }

        /// <summary>
        /// 根据给定条件获取缓存数据实体列表，不存在缓存则调用GetAllList方法从数据库获取。
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <returns></returns>
        public virtual List<TEntity> TryCacheAllList(Expression<Func<TEntity, bool>> where, string orderBy)
        {
            var list = this.TryCacheAllList();
            if (list != null)
            {
                var query = list.AsQueryable();
                if (where != null)
                    query = query.Where(where);
                if (!string.IsNullOrEmpty(orderBy))
                    query = query.OrderBy(orderBy);
                list = query.ToList();
            }
            return list;
        }

        /// <summary>
        /// 根据给定条件获取缓存数据实体列表，不存在缓存则调用GetAllList方法从数据库获取。
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public virtual List<TEntity> TryCacheAllList(string strWhere)
        {
            return TryCacheAllList(strWhere, null);
        }

        /// <summary>
        /// 根据给定条件获取缓存数据实体列表，不存在缓存则调用GetAllList方法从数据库获取。
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <returns></returns>
        public virtual List<TEntity> TryCacheAllList(string strWhere, string orderBy)
        {
            var list = this.TryCacheAllList();
            if (list != null)
            {
                var query = list.AsQueryable();
                if (!string.IsNullOrEmpty(strWhere))
                    query = query.Where(strWhere);
                if (!string.IsNullOrEmpty(orderBy))
                    query = query.OrderBy(orderBy);
                list = query.ToList();
            }
            return list;
        }

        /// <summary>
        /// 分页获取缓存数据实体列表
        /// </summary>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public virtual List<TEntity> TryCachePagedList(int startRowIndex, int maximumRows)
        {
            var list = this.TryCacheAllList();
            if (list != null)
            {
                startRowIndex = startRowIndex < 0 ? 0 : startRowIndex;
                list = list.Skip(startRowIndex).Take(maximumRows).ToList();
            }
            return list;
        }

        /// <summary>
        /// 根据给定条件分页获取缓存数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public virtual List<TEntity> TryCachePagedList(Expression<Func<TEntity, bool>> where, int startRowIndex, int maximumRows)
        {
            return TryCachePagedList(where, null, startRowIndex, maximumRows);
        }


        /// <summary>
        /// 根据给定条件分页获取缓存数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public virtual List<TEntity> TryCachePagedList(Expression<Func<TEntity, bool>> where, string orderBy, int startRowIndex, int maximumRows)
        {
            var list = this.TryCacheAllList();
            if (list != null)
            {
                var query = list.AsQueryable();
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

                list = query.Skip(startRowIndex).Take(maximumRows).ToList();
            }
            return list;
        }


        /// <summary>
        /// 根据给定条件分页获取缓存数据实体列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public virtual List<TEntity> TryCachePagedList(string strWhere, int startRowIndex, int maximumRows)
        {
            return TryCachePagedList(strWhere, null, startRowIndex, maximumRows);
        }

        /// <summary>
        /// 根据给定条件分页获取缓存数据实体列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public virtual List<TEntity> TryCachePagedList(string strWhere, string orderBy, int startRowIndex, int maximumRows)
        {
            var list = this.TryCacheAllList();
            if (list != null)
            {
                var query = list.AsQueryable();
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

                list = query.Skip(startRowIndex).Take(maximumRows).ToList();
            }

            return list;
        }

        /// <summary>
        /// 获取指定个数排名靠前的缓存实体列表
        /// </summary>
        /// <param name="top">个数</param>
        /// <returns></returns>
        public virtual List<TEntity> TryCacheTop(int top)
        {
            return this.TryCacheTop(null, null, top);
        }

        /// <summary>
        /// 获取指定个数排名靠前的缓存实体列表
        /// </summary>
        /// <param name="top">个数</param>
        /// <returns></returns>
        public virtual List<TEntity> TryCacheTop(Expression<Func<TEntity, bool>> where, int top)
        {
            return this.TryCacheTop(where, null, top);
        }

        /// <summary>
        /// 根据给定条件获取符合条件指定个数排名靠前的缓存实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="top">个数</param>
        /// <returns></returns>
        public virtual List<TEntity> TryCacheTop(Expression<Func<TEntity, bool>> where, string orderBy, int top)
        {
            var list = this.TryCacheAllList();
            if (list != null)
            {
                var query = list.AsQueryable();
                if (where != null)
                {
                    query = query.Where(where);
                }

                if (!string.IsNullOrEmpty(orderBy))
                {
                    query = query.OrderBy(orderBy);
                }

                list = query.Take(top).ToList();
            }
            return list;
        }

        /// <summary>
        /// 根据给定条件获取符合条件的第一个缓存实体
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public virtual TEntity TryCacheTopOne(Expression<Func<TEntity, bool>> where)
        {
            return this.TryCacheTopOne(where, null);
        }

        /// <summary>
        /// 根据给定条件获取符合条件的首个缓存实体
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <returns></returns>
        public virtual TEntity TryCacheTopOne(Expression<Func<TEntity, bool>> where, string orderBy)
        {
            var list = this.TryCacheAllList();
            TEntity rlt = null;
            if (list != null)
            {
                var query = list.AsQueryable();
                if (where != null)
                {
                    query = query.Where(where);
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    query = query.OrderBy(orderBy);
                }

                rlt = query.Take(1).FirstOrDefault();
            }
            return rlt;
        }

        /// <summary>
        /// 根据主键编号获取缓存实体
        /// </summary>
        /// <param name="sysNo">主键编号</param>
        /// <returns></returns>
        public virtual TEntity TryCacheBySysNo(int sysNo)
        {
            var list = this.TryCacheAllList();
            TEntity rlt = null;
            if (list != null)
            {
                var query = list.AsQueryable();
                query = query.Where(string.Format("{0}={1}", this.PrimaryKey, sysNo));
                rlt = query.SingleOrDefault();
            }
            return rlt;
        }


    }
}
