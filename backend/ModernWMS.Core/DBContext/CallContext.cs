using System.Collections.Concurrent;
using System.Threading;

namespace ModernWMS.Core.DBContext
{
    /// <summary>
    /// 当前连接的上下文
    /// </summary>
    public static class CallContext
    {
        /// <summary>
        /// 静态字典
        /// </summary>
        static ConcurrentDictionary<string, AsyncLocal<object>> state = new ConcurrentDictionary<string, AsyncLocal<object>>();
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="name">键</param>
        /// <param name="data">值</param>
        public static void SetData(string name, object data) =>
            state.GetOrAdd(name, _ => new AsyncLocal<object>()).Value = data;
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="name">键</param>
        /// <returns></returns>
        public static object GetData(string name) =>
            state.TryGetValue(name, out AsyncLocal<object> data) ? data.Value : null;
    }
}
