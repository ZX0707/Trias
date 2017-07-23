using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Trias.Models;

namespace Trias.Service
{
    public class BaseService<T> where T : class
    {
        protected static readonly TriasEntities db = DbContextFactory.GetCurrentThreadInstance();
        protected static readonly DbSet<T> service = db.Set<T>();

        #region 添加

        /// <summary>
        /// 添加一个实体
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public T Add(T Model)
        {
            return service.Add(Model);
        }

        /// <summary>
        /// 添加多个实体
        /// </summary>
        /// <param name="Models"></param>
        /// <returns></returns>
        public List<T> AddList(IEnumerable<T> Models)
        {
            var result = new List<T>();
            var enumerator = Models.GetEnumerator();
            while (enumerator.MoveNext())
            {
                result.Add(Add(enumerator.Current));
            }
            return result;
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除一个实体，实体必须在上下文中
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public T Remove(T Model)
        {
            return service.Remove(Model);
        }

        /// <summary>
        /// 删除多个实体
        /// </summary>
        /// <param name="Models"></param>
        /// <returns></returns>
        public List<T> RemoveList(IEnumerable<T> Models)
        {
            var result = new List<T>();
            var enumerator = Models.GetEnumerator();
            while (enumerator.MoveNext())
            {
                result.Add(Remove(enumerator.Current));
            }
            return result;
        }

        /// <summary>
        /// 通过条件删除实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<T> RemoveWhere(Expression<Func<T, bool>> predicate)
        {
            return RemoveList(Where(predicate).ToList());
        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改一个实体
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public T Edit(T Model, object obj)
        {
            var objType = obj.GetType();
            var objProperties = objType.GetProperties();

            foreach (var property in objProperties)
            {
                Model.GetType().GetProperty(property.Name).SetValue(Model, property.GetValue(obj, null), null);
            }
            return Model;
        }

        /// <summary>
        /// 通过条件删除实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<T> EditWhere(Expression<Func<T, bool>> predicate, object obj)
        {
            return Where(predicate).ToList().Select(x => Edit(x, obj)).ToList();
        }

        #endregion

        #region 查询

        /// <summary>
        /// 查询所有实体
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> Where()
        {
            return Where(x => true);
        }

        /// <summary>
        /// 通过条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return service.Where(predicate);
        }

        /// <summary>
        /// 通过主键查找实体
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public T Find(params object[] keyValues)
        {
            return service.Find(keyValues);
        }

        /// <summary>
        /// 根据查询第一个实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return Where(predicate).FirstOrDefault();
        }

        #endregion

        #region 其他

        /// <summary>
        /// 判定是否存在
        /// </summary>
        /// <param name="predicate">判断条件</param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return Where(predicate).Any();
        }

        #endregion

        #region 保存

        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        #endregion
    }
}