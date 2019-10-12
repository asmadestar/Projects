using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ParallelProcessingE9
{
    class ParallelSamplesE9
    {
        private long m_FinishCounter = 0;

        /// <summary>
        /// Starts all jobs in a single sequence.
        /// </summary>
        /// <param name="sequences"></param>
        /// <param name="func"></param>
        public void StartSequenced(int sequences, Action<object> func)
        {
            Thread.CurrentThread.Name = "SingleThread";

            for (int i = 0; i < sequences; i++)
            {
                func(null);
            }
        }


        /// <summary>
        /// Starts all job in parallel.
        /// </summary>
        /// <param name="threads"></param>
        /// <param name="func"></param>
        public void StartMultithreadedNative(int threads, Action<object> func)
        {
            for (int i = 0; i < threads; i++)
            {
                var t = new Thread(new ParameterizedThreadStart(func));
                t.Name = i.ToString();
                t.Start(new Action<string>(onThreadFinished));
            }

            while (true)
            {
                if (Interlocked.Read(ref m_FinishCounter) == threads)
                    break;
                else
                    Thread.Sleep(500);
            }
        }


        public void StartMultithreadedNativeV2(int threads, Action<object> func)
        {
            List<Thread> tList = new List<Thread>();

            for (int i = 0; i < threads; i++)
            {
                var thread = new Thread(new ParameterizedThreadStart(func));
                thread.Name = i.ToString();
                tList.Add(thread);
            }

            foreach (var item in tList)
            {
                item.Start(new Action<string>(onThreadFinished));
            }

            while (true)
            {
                if (Interlocked.Read(ref m_FinishCounter) == threads)
                    break;
                else
                    Thread.Sleep(500);
            }
        }

        public void StartWithTpl(int threads, Action<object> func)
        {
            List<Task> tList = new List<Task>();

            for (int i = 0; i < threads; i++)
            {
                var t = new Task(func, new Action<string>(onThreadFinished));
                tList.Add(t);
            }

            foreach (var t in tList)
            {
                t.Start();
            }

            Task.WaitAll(tList.ToArray());
        }

        private void onThreadFinished(string threadName)
        {
            Interlocked.Increment(ref m_FinishCounter);
        }
    }
}
