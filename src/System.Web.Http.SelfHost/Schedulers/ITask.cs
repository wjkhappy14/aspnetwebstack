
namespace System.Web.Http.SelfHost.Schedulers
{
    /// <summary>
    /// Interface that should be implemented by each task
    /// </summary>
    public partial interface ITask
    {
        /// <summary>
        /// Execute task
        /// </summary>
        void Execute();
    }
}
