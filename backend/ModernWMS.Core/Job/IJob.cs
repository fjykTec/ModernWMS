/*
 * date：2023-02-08
 * developer：AMo
 */

namespace ModernWMS.Core.Job
{
    /// <summary>
    /// base interface
    /// </summary>
    public interface IJob
    {
        /// <summary>
        /// cron 
        /// </summary>
        string CronExpression { get; }

        /// <summary>
        /// Execute
        /// </summary>
        /// <returns></returns>
        Task Execute();
    }
}
