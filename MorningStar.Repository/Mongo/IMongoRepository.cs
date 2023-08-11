using MorningStar.Model;
using System.Linq.Expressions;

namespace MorningStar.Repository
{
    /// <summary>
    /// MongoDB 仓储接口类
    /// </summary>
    /// <typeparam name="T">泛型实体</typeparam>
    public interface IMongoRepository<T>
    {
        /// <summary>
        /// 根据主键匹配
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(long id);

        /// <summary>
        /// 根据筛选条件匹配
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        Task<List<T>> GetAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        Task<PageViewModel<T>> GetPageAsync(int pageIndex, int pageSize);

        /// <summary>
        /// 获取所有
        /// </summary>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">需新增的实体</param>
        /// <returns></returns>
        Task InsertAsync(T entity);

        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <param name="entities">需批量新增的实体</param>
        /// <returns></returns>
        Task InsertRangeAsync(List<T> entities);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="entity">需更新的实体</param>
        /// <returns></returns>
        Task UpdateAsync(long id, T entity);

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">需批量更新的实体</param>
        /// <param name="idSelector">使用泛型约束和泛型委托，使用示例：_=>_.ID </param>
        /// <returns></returns>
        Task UpdateRangeAsync(List<T> entities, Func<T, long> idSelector);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task DeleteAsync(long id);

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <returns></returns>
        Task DeleteRangeAsync(List<long> ids);
    }
}
