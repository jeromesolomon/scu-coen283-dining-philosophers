using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



/*
 * 
 * Author: Jerome Solomon
 * Description: Dining Philosophers' C# solution using C# monitor
 * Class: COEN283 Operating Systems
 * Professor: Amr Elkady
 * 
 * Homework 2 question/assignment.  Solve the diners philosphers problem using monitors
 * 
 */




namespace dining_philosophers
{
    class Program
    {

        static object monitorObject = new object();

        const int nPhilosophers = 5;
        static bool[] chopsticksAvailable = new bool[nPhilosophers];

        static void Eat(Object arg)
        {

            int p = (int)arg;

            Random rnd = new Random();
            int waitMiliseconds;

            // Console.WriteLine("philosopher " + p + " is hungry and trying to eat.");

            // assume both left and right chopstick are not available
            bool leftAvailable = false;
            bool rightAvailable = false;


            // wait until both left and right chopsticks are available
            while ((!leftAvailable) || (!rightAvailable))
            {

                // check the left chopsticks
                leftAvailable = chopsticksAvailable[p];

                if (leftAvailable)
                {
                    chopsticksAvailable[p] = false;
                }

                // check the right chopsticks
                rightAvailable = chopsticksAvailable[(p + 1) % nPhilosophers];

                if (rightAvailable)
                {
                    chopsticksAvailable[(p + 1) % nPhilosophers] = false;
                }

                if (leftAvailable || rightAvailable)
                {
                    string msg = "philosopher " + p + " left available = " + leftAvailable + "\tright available = " + rightAvailable;
                    Console.WriteLine(msg);
                }
            }

            // simulate time to eat
            waitMiliseconds = rnd.Next(100, 1000);
            Thread.Sleep(waitMiliseconds);

            // release the chopsticks
            chopsticksAvailable[p] = true;
            chopsticksAvailable[(p + 1) % nPhilosophers] = true;


            Console.WriteLine("philosopher " + p + " ate food.");

        }

        static void Main(string[] args)
        {

            Console.WriteLine("Initializing the structures");

            // initialize the structuures
            for (int i = 0; i < nPhilosophers; i++)
            {
                chopsticksAvailable[i] = true;
            }

            Thread[] threadArray = new Thread[nPhilosophers];

            for (int i = 0; i < nPhilosophers; i++)
            {
          
                threadArray[i] = new Thread(new ParameterizedThreadStart(Eat));

                threadArray[i].Name = "Thread #" + i.ToString();
                threadArray[i].Start(i);

            }

            // wait for all of the threads to complete
            for (int i = 0; i < nPhilosophers; i++)
            {
                threadArray[i].Join();
            }

            Console.WriteLine("Press any key to continue:");
            Console.Read();

        }
    }
}
