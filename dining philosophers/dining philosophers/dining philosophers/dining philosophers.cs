﻿
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

        // monitor lock object
        static object monitorObject = new object();

        // philosphers structures
        const int nPhilosophers = 5;
        static bool[] chopsticksAvailable = new bool[nPhilosophers];

        // max seconds a philospher can wait without eating (before marked as starving (dead lock))
        const int maxWaitSeconds = 6;

        static void Eat(Object arg)
        {

            int p = (int)arg;

            Random rnd = new Random();
            int waitMiliseconds;

            bool starving = false;

            // Console.WriteLine("philosopher " + p + " is hungry and trying to eat.");

            // assume both left and right chopstick are not available
            bool leftAvailable = false;
            bool rightAvailable = false;


            // create a timestamp to gauge how long philosopher is without food
            DateTime start = DateTime.Now;
            int secondsStart = start.Second;

            // wait until both left and right chopsticks are available
            while (((!leftAvailable) || (!rightAvailable))  && (!starving))
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

                // if you have one chopstick or more, report it
                if (leftAvailable || rightAvailable)
                {
                    string msg = "philosopher " + p + " has a chopstick:" + "\tleft = " + leftAvailable + "\tright = " + rightAvailable;
                    Console.WriteLine(msg);
                }

                // if you dont have both chopsticks and you have been hungry for a long while, you are starving
                if ((!leftAvailable) || (!rightAvailable))
                {
                    DateTime curr = DateTime.Now;

                    int secondsCurr = curr.Second;

                    if (secondsCurr - secondsStart > maxWaitSeconds)
                    {
                        starving = true;
                    }
                }
            }

            if (starving)
            {
                Console.WriteLine("philosopher " + p + " starved to death.  Deadlock!");
            }
            else
            {
                // simulate time to eat
                waitMiliseconds = rnd.Next(100, 1000);
                Thread.Sleep(waitMiliseconds);

                // release the chopsticks
                chopsticksAvailable[p] = true;
                chopsticksAvailable[(p + 1) % nPhilosophers] = true;


                Console.WriteLine("philosopher " + p + " ate food.");
            }

        }

        static void Main(string[] args)
        {

            // Console.WriteLine("Initializing the structures");

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
