//////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//        Project:    Supermarket Simulation
//
//      File Name:    Customer.cs
//
//         Author:    Nicholas Yoder
//
//    Description:    This file contains the Customer class declaration, which represents a customer in
//                    the simulation. 
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////

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
        /// The unique ID of the customer
        /// </summary>
        public int PatronId { get; private set; }

        /// <summary>
        /// The time at which the customer enters a checkout line
        /// </summary>
        public DateTime EnqueueTime { get; private set; }

        /// <summary>
        /// The amount of time it takes a customer to exit after reaching the front of a checkout line
        /// </summary>
        public TimeSpan CheckoutLength { get; private set; }

        /// <summary>
        /// Reference to the checkout queue that the customer is in
        /// </summary>
        public Queue<Customer> CheckoutLine { get; set; }

        /// <summary>
        /// Constructor for Customer objects that initializes properties
        /// </summary>
        /// <param name="id">The unique ID of the customer</param>
        /// <param name="checkoutLength">The amount of time it will take the customer to exit after reaching the front of a checkout line</param>
        /// <param name="enqueueTime">The time at which the customer will enter a checkout line</param>
        public Customer(int id, TimeSpan checkoutLength, DateTime enqueueTime)
        {
            PatronId = id;
            CheckoutLength = checkoutLength;
            EnqueueTime = enqueueTime;
        }

        /// <summary>
        /// Returns the customer's ID as a string
        /// </summary>
        /// <returns>The customer's ID as a string</returns>
        public override string ToString()
        {
            return PatronId.ToString("000");   
        }
    }
}