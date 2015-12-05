using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Wintellect.PowerCollections;

namespace System.Web.Http.SelfHost.Controllers
{




    /// <summary>
    /// Threading in C#  http://www.albahari.com/threading/
    /// </summary>
    public class ThreadingController : ApiController
    {


        private BackgroundWorker bgWorker;

        private string _progressPercentage;
        /// <summary>
        /// Threading
        /// </summary>
        public ThreadingController()
        {

        }
        Action<Task> nonTrivial = delegate
        {
            Thread.SpinWait(5);
        };
        LazyValue<int> func = new LazyValue<int>(() => { return 10; }, 1000);


        /// <summary>
        /// 使多个任务能够采用并行方式依据某种算法在多个阶段中协同工作。
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Barrier()
        {


            var t1 = new Thread((x) =>
            {

            });
            t1.Start();
            t1.Join();

            Barrier barrier = new Threading.Barrier(100, (x) =>
            {


                x.SignalAndWait();

            });

            return Json(barrier);

        }
        [HttpGet]
        public IHttpActionResult ReferenceCount()
        {
            Barrier barrier = new Threading.Barrier(100, (x) =>
            {

                x.SignalAndWait();
            });


            ReferenceCounted<Barrier> counter = new ReferenceCounted<Threading.Barrier>(barrier);
            return Json(counter);


        }
        [HttpGet]
        public IHttpActionResult Mutex()
        {
            ParameterizedThreadStart pt = (x) =>
            {


            };
            pt.Invoke(DateTime.Now);
            Mutex mutex = new Threading.Mutex();
            return Json(mutex);
        }





        /// <summary>
        /// 在单独的线程上运行操作
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public IHttpActionResult BackgroundWorker()
        {
            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.Disposed += BgWorker_Disposed;
            bgWorker.ProgressChanged += BgWorker_ProgressChanged;
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
            bgWorker.RunWorkerAsync();
            return Json(bgWorker);
        }

        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _progressPercentage = (e.ProgressPercentage.ToString() + "%");
        }

        private void BgWorker_Disposed(object sender, EventArgs e)
        {
            

        }

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            for (int i = 1; i < int.MaxValue; i++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    Thread.Sleep(500);
                    worker.ReportProgress(i * 10);
                }
            }

        }
    }
}
