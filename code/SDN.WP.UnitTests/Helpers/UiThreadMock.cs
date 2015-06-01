using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SDN.WP.UnitTests.Helpers
{
    internal sealed class UiThreadMock
    {
        private readonly object syncRoot = new object();
        private readonly Queue<Task> tasks = new Queue<Task>();

        public Task Enqueue(Action action)
        {
            var task = new Task(action);

            lock (syncRoot)
            {
                tasks.Enqueue(task);
            }

            return task;
        }

        public void ExecuteAll()
        {
            var currentTask = TryDequeue();

            while (currentTask != null)
            {
                currentTask.RunSynchronously();

                currentTask = TryDequeue();
            }
        }

        private Task TryDequeue()
        {
            lock (syncRoot)
            {
                if (tasks.Count == 0)
                {
                    Thread.Sleep(100);

                    if (tasks.Count == 0)
                    {
                        return null;
                    }
                }

                return tasks.Dequeue();
            }
        }
    }
}