using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace dining_philosophers
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>();
            Random rnd = new Random();
            long total = 0;
            int n = 0;

            for (int taskCtr = 0; taskCtr < 10; taskCtr++)
                tasks.Add(Task.Run(() => {
                    int[] values = new int[10000];
                    int taskTotal = 0;
                    int taskN = 0;
                    int ctr = 0;
                    Monitor.Enter(rnd);
                    // Generate 10,000 random integers
                    for (ctr = 0; ctr < 10000; ctr++)
                        values[ctr] = rnd.Next(0, 1001);
                    Monitor.Exit(rnd);
                    taskN = ctr;
                    foreach (var value in values)
                        taskTotal += value;

                    Console.WriteLine("Mean for task {0,2}: {1:N2} (N={2:N0})",
                                      Task.CurrentId, (taskTotal * 1.0) / taskN,
                                      taskN);
                    Interlocked.Add(ref n, taskN);
                    Interlocked.Add(ref total, taskTotal);
                }));
            try
            {
                Task.WaitAll(tasks.ToArray());
                Console.WriteLine("\nMean for all tasks: {0:N2} (N={1:N0})",
                                  (total * 1.0) / n, n);

            }
            catch (AggregateException e)
            {
                foreach (var ie in e.InnerExceptions)
                    Console.WriteLine("{0}: {1}", ie.GetType().Name, ie.Message);
            }

            Console.WriteLine("Press any key to continue:");
            Console.ReadLine();
        }
    }
}
