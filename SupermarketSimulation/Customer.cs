using System;
using System.Collections.Generic;

namespace SupermarketSimulation
{
    /// <summary>
    /// Represents a customer in the supermarket simulation
    /// </summary>
    public class Customer
    {   
        /// <summary>
        /// The ID of the customer
        /// </summary>
        public int PatronId { get; private set; }

        /// <summary>
        /// The time at which the customer enters a checkout line
        /// </summary>
        public DateTime EnterQueueTime { get; private set; }

        /// <summary>
        /// The amount of time it takes a customer to exit after reaching the front of a queue
        /// </summary>
        public TimeSpan CheckoutLength { get; private set; }

        /// <summary>
        /// The checkout queue that the customer entered
        /// </summary>
        public Queue<Customer> CheckoutLine { get; set; }

        /// <summary>
        /// Constructor for Customer objects that initializes properties
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <param name="checkoutLength">The time it will take to checkout upon reaching the front of the queue</param>
        /// <param name="enterQueueTime">The time at which the customer will enter a checkout line</param>
        public Customer(int id, TimeSpan checkoutLength, DateTime enterQueueTime)
        {
            PatronId = id;
            CheckoutLength = checkoutLength;
            EnterQueueTime = enterQueueTime;
        }

        /// <summary>
        /// Overridden ToString function that returns the customer's ID as a 3-character string
        /// </summary>
        /// <returns>The customer's ID as a string</returns>
        public override string ToString()
        {
            return PatronId.ToString("000");   
        }
    }
}