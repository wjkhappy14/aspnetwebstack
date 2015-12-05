using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Web.Http.SelfHost.Schedulers
{
    /// <summary>
    /// Task service
    /// </summary>
    public partial class ScheduleTaskService : IScheduleTaskService
    {
        #region Fields

        private readonly IList<ScheduleTask> _taskRepository;

        #endregion

        #region Ctor

        public ScheduleTaskService(IList<ScheduleTask> taskRepository)
        {
            this._taskRepository = taskRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a task
        /// </summary>
        /// <param name="task">Task</param>
        public virtual void DeleteTask(ScheduleTask task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            _taskRepository.Remove(task);
        }

        /// <summary>
        /// Gets a task
        /// </summary>
        /// <param name="taskId">Task identifier</param>
        /// <returns>Task</returns>
        public virtual ScheduleTask GetTaskById(int taskId)
        {
            if (taskId == 0)
                return null;

            return _taskRepository.Single(x=>x.Id== taskId);
        }

        /// <summary>
        /// Gets a task by its type
        /// </summary>
        /// <param name="type">Task type</param>
        /// <returns>Task</returns>
        public virtual ScheduleTask GetTaskByType(string type)
        {
            if (String.IsNullOrWhiteSpace(type))
                return null;
            

            var task = _taskRepository.FirstOrDefault();
            return task;
        }

        /// <summary>
        /// Gets all tasks
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Tasks</returns>
        public virtual IList<ScheduleTask> GetAllTasks(bool showHidden = false)
        {
            var query = _taskRepository.AsQueryable();
            if (!showHidden)
            {
                query = query.Where(t => t.Enabled);
            }
            query = query.OrderByDescending(t => t.Seconds);

            var tasks = query.ToList();
            return tasks;
        }

        /// <summary>
        /// Inserts a task
        /// </summary>
        /// <param name="task">Task</param>
        public virtual void InsertTask(ScheduleTask task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            _taskRepository.Add(task);
        }

        /// <summary>
        /// Updates the task
        /// </summary>
        /// <param name="task">Task</param>
        public virtual void UpdateTask(ScheduleTask task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

           // _taskRepository.Update(task);
        }

        #endregion
    }
}
