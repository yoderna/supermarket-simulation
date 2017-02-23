//////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//        Project:    Supermarket Simulation
//
//      File Name:    Supermarket.cs
//
//         Author:    Nicholas Yoder
//
//    Description:    This file contains the Supermarket class declaration, which represents the
//                    supermarket in the simulation. It contains various public properties related
//                    to the simulation configuration and private properties representing simulation
//                    statistics. It also contains functions involved in updating and drawing the
//                    simulation as each event is processed.
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Threading;

namespace SupermarketSimulation
{
    /// <summary>
    /// Represents the supermarket in the simulation
    /// </summary>
    public class Supermarket
    {
        #region Properties for Simulation Configuration
        /// <summary>
        /// The number of customers that will be served while open
        /// </summary>
        public int NumCustomers { get; set; }

        /// <summary>
        /// The number of hours the supermarket will be open during the simulation
        /// </summary>
        public int HoursOpen { get; set; }

        /// <summary>
        /// The number of checkout lines in the supermarket
        /// </summary>
        public int NumLines { get; set; }

        /// <summary>
        /// The expected amount of time to serve a customer upon reaching the front of a checkout line
        /// </summary>
        public double ExpectedCheckoutTime { get; set; }
        #endregion

        #region Private Fields For Simulation Statistics
        /// <summary>
        /// The longest amount of time between a customer entering and exiting a checkout line
        /// </summary>
        private TimeSpan longestServiceTime;

        /// <summary>
        /// The shortest amount of time between a customer entering and exiting a checkout line
        /// </summary>
        private TimeSpan shortestServiceTime;

        /// <summary>
        /// The average amount of time between a customer entering and exiting a checkout line
        /// </summary>
        private TimeSpan averageServiceTime;

        /// <summary>
        /// The longest checkout line (in number of customers) encountered during the simulation
        /// </summary>
        private int longestQueueLength;
        #endregion

        /// <summary>
        /// A random number generator used for determining both ENTER event times and the
        /// amount of time it takes to serve a customer upon reaching the front of a line
        /// </summary>
        private Random r;

        /// <summary>
        /// A priority queue of Event objects that orders by the time of the event, where earlier events are at the front of the queue
        /// </summary>
        private PriorityQueue<Event> events;

        /// <summary>
        /// Default constructor that sets supermarket configuration to default values
        /// </summary>
        public Supermarket()
        {
            NumCustomers = 400;
            HoursOpen = 8;
            NumLines = 6;
            ExpectedCheckoutTime = 6.25;

            r = new Random();
        }

        /// <summary>
        /// Runs the entire Supermarket simulation, including all setup and post-simulation functions
        /// </summary>
        public void Run()
        {
            events = new PriorityQueue<Event>();

            shortestServiceTime = TimeSpan.MaxValue;
            longestServiceTime = TimeSpan.FromTicks(0);
            averageServiceTime = TimeSpan.FromTicks(0);

            longestQueueLength = 0;

            GenerateEventsAndCustomers();   // Generate all Customer objects and ENTER events upfront

            DoSimulation();                 // Perform the simulation

            DisplayStats();                 // Display all relevant statistics for the simulation
        }

        /// <summary>
        /// Generates all ENTER events and Customer objects for the simulation
        /// </summary>
        private void GenerateEventsAndCustomers()
        {
            // Create ENTER and EXIT events for all customers
            for (int i = 1; i <= NumCustomers; i++)
            {
                // Determine a random time at which a new customer enters a checkout line
                DateTime enterQueueTime = DateTime.Today + new TimeSpan(8, 0, r.Next(3600 * HoursOpen));

                // Determine the length of checkout once the customer reaches
                // the front of the line; minimum checkout time is 2 minutes
                double checkoutTime = 2.0 + NegExp(ExpectedCheckoutTime - 2.0);

                TimeSpan checkoutDuration = TimeSpan.FromMinutes(checkoutTime);

                // Create a new Customer object with this information
                // and enqueue a new ENTER event
                Customer customer = new Customer(i, checkoutDuration, enterQueueTime);

                events.Enqueue(new Event(enterQueueTime, EVENTTYPE.ENTER, customer));
            }
        }

        /// <summary>
        /// Performs the main part of the simulation until all events have been processed
        /// </summary>
        /// <remarks>
        /// The priority queue of events has already been filled by the GenerateEventsAndCustomers() function.
        /// This function will loop until that queue has been fully emptied. At first, it will consist entirely
        /// of ENTER events, but EXIT events will be enqueued as customers reach the front of a line.
        /// </remarks>
        private void DoSimulation()
        {
            // A list of all checkout lines, which Customers wait in
            List<Queue<Customer>> checkoutLines = new List<Queue<Customer>>(NumLines);

            for (int i = 0; i < NumLines; i++)
                checkoutLines.Add(new Queue<Customer>());

            int numArrivals = 0,        // The number of customers who have entered a line so far
                numDepartures = 0,      // The number of customers who have checked out so far
                eventsProcessed = 0;    // The total number of events processed so far

            // Loop until all events have been processed
            while (!events.IsEmpty)
            {
                Console.Clear();

                Event nextEvent = events.Dequeue();
                eventsProcessed++;

                // Process enter event
                if (nextEvent.Type == EVENTTYPE.ENTER)
                {
                    numArrivals++;

                    HandleEnterEvent(nextEvent, checkoutLines);
                }
                // Process exit event
                else
                {
                    numDepartures++;

                    HandleExitEvent(nextEvent, checkoutLines);
                }

                // Draw the current state of the simulation and pause before continuing
                Draw(checkoutLines, numArrivals, numDepartures);
                Thread.Sleep(100);
            }

            averageServiceTime = TimeSpan.FromMilliseconds(averageServiceTime.TotalMilliseconds / NumCustomers);
        }

        /// <summary>
        /// Processes an ENTER event
        /// </summary>
        /// <remarks>
        /// Inserts the appropriate customer specified by the event into the shortest
        /// available checkout line.
        /// 
        /// If the line is empty, an EXIT event is immediately enqueued at the current
        /// event's time, plus the length of service time.
        /// 
        /// If the line is not empty, the EXIT event will be created later, after all
        /// customers in front have exited.
        /// </remarks>
        /// <param name="enterEvent">The ENTER event being processed</param>
        /// <param name="checkoutLines">The list of checkout lines in the supermarket</param>
        private void HandleEnterEvent(Event enterEvent, List<Queue<Customer>> checkoutLines)
        {
            // Find the smallest checkout line, and enqueue the appropriate
            // customer there as specified by the event
            Queue<Customer> smallestQueue = checkoutLines[0];

            for (int i = 1; i < checkoutLines.Count; i++)
            {
                if (checkoutLines[i].Count < smallestQueue.Count)
                    smallestQueue = checkoutLines[i];
            }

            Customer newCustomer = enterEvent.Patron;
            newCustomer.CheckoutLine = smallestQueue;
            smallestQueue.Enqueue(newCustomer);

            // If the customer entered an empty line, immediately enqueue an exit event at time of entering queue + length of service
            // Otherwise an EXIT event will be enqueued later when the customer reaches the front of the line
            if (smallestQueue.Count == 1)
            {
                Event checkoutEvent = new Event(
                    enterEvent.TimeOfEvent + newCustomer.CheckoutLength,
                    EVENTTYPE.EXIT,
                    enterEvent.Patron);

                events.Enqueue(checkoutEvent);
            }
        }

        /// <summary>
        /// Processes an EXIT event
        /// </summary>
        /// <param name="exitEvent">The EXIT event being processed</param>
        /// <param name="checkoutLines">The list of checkout lines in the supermarket</param>
        private void HandleExitEvent(Event exitEvent, List<Queue<Customer>> checkoutLines)
        {
            // Dequeue from the checkout line that the customer is in
            Customer leavingCustomer = exitEvent.Patron;
            Queue<Customer> checkoutLine = leavingCustomer.CheckoutLine;
            checkoutLine.Dequeue();

            // If there is a new customer at the front of the line, create a new EXIT event that
            // takes place at the time of the current event, plus that customer's checkout length
            if (checkoutLine.Count >= 1)
            {
                Customer frontCustomer = checkoutLine.Peek();

                Event nextExitEvent = new Event(
                    exitEvent.TimeOfEvent + frontCustomer.CheckoutLength,
                    EVENTTYPE.EXIT,
                    frontCustomer);

                events.Enqueue(nextExitEvent);
            }

            // Determine the customer's entire length of wait in queue
            TimeSpan serviceLength = exitEvent.TimeOfEvent - exitEvent.Patron.EnqueueTime;

            if (serviceLength > longestServiceTime)
            {
                longestServiceTime = serviceLength;
            }
            else if (serviceLength < shortestServiceTime)
            {
                shortestServiceTime = serviceLength;
            }

            // Add service length to average service time, which will later be 
            // divided by the number of customers to find the correct average
            averageServiceTime += serviceLength;
        }

        /// <summary>
        /// Draws the current state of the simulation
        /// </summary>
        /// <param name="checkoutLines">All checkout lines in the simulation</param>
        /// <param name="numArrivals">The number of ENTER events processed so far</param>
        /// <param name="numDepartures">The number of EXIT events processed so far</param>
        private void Draw(List<Queue<Customer>> checkoutLines, int numArrivals, int numDepartures)
        {
            Console.WriteLine("Checkout Lines".PadLeft(46));
            Console.WriteLine("--------------".PadLeft(46) + "\n");

            // Loop through all checkout lines and draw the customers in them
            for (int i = 0, currentLinePosition = 16; i < NumLines; i++, currentLinePosition += 8)
            {
                Console.SetCursorPosition(currentLinePosition, 4);
                Console.Write("L " + (i + 1));

                // Draw each customer in the line
                int j = 0;
                foreach (Customer c in checkoutLines[i])
                {
                    Console.SetCursorPosition(currentLinePosition, 6 + j);
                    Console.WriteLine(c);
                    j++;
                }

                if (checkoutLines[i].Count > longestQueueLength)
                    longestQueueLength = checkoutLines[i].Count;
            }

            // Draw a line indicating the longest queue length so far
            Console.SetCursorPosition(0, 6 + longestQueueLength);
            Console.WriteLine("Longest So Far".PadRight(80, '-'));

            // Display stats on number of events processed so far
            Console.WriteLine("\nEvents Processed So Far: " + (numArrivals + numDepartures) +
                "\tArrivals: " + numArrivals + "\tDepartures: " + numDepartures);
        }

        /// <summary>
        /// Displays statistics related to the simulation
        /// </summary>
        public void DisplayStats()
        {
            Console.WriteLine("\n Average Service Time: {0}", averageServiceTime.ToString(@"hh\:mm\:ss"));
            Console.WriteLine(" Longest Service Time: {0}", longestServiceTime.ToString(@"hh\:mm\:ss"));
            Console.WriteLine("Shortest Service Time: {0}", shortestServiceTime.ToString(@"hh\:mm\:ss"));

            Console.WriteLine("\n Longest Queue Length: {0} customers\n", longestQueueLength);

            Console.Write("Press any key to return to the menu...");

            Console.ReadKey();
        }

        /// <summary>
        /// Provides a random number around an expected value according to a negative exponential distribution
        /// </summary>
        /// <param name="expectedValue">An estimated value</param>
        /// <returns>A random number according to a negative exponential distribution</returns>
        private double NegExp(double expectedValue)
        {
            return -expectedValue * Math.Log(r.NextDouble(), Math.E);
        }
    }
}
