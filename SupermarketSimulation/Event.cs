//////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//        Project:    Supermarket Simulation
//
//      File Name:    Event.cs
//
//         Author:    Nicholas Yoder
//
//    Description:    This file contains the Event class declaration, which represents an event in the
//                    simulation. It can be one of two types:
//
//                      ENTER: Occurs when a new customer enters the back of a checkout line.
//
//                      EXIT: Occurs when a customer exits a checkout line, after having been served.
//
//                    Along with this information, the Event class contains the time at which the event
//                    occurred and a reference to the Customer object that caused the event.
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace SupermarketSimulation
{
    /// <summary>
    /// Represents an ENTER queue or EXIT queue event in the supermarket simulation
    /// </summary>
    public class Event : IComparable
    {
        /// <summary>
        /// The type of event; either ENTER or EXIT
        /// </summary>
        public EVENTTYPE Type { get; set; }

        /// <summary>
        /// The time at which the event occurs
        /// </summary>
        public DateTime TimeOfEvent { get; set; }

        /// <summary>
        /// The customer that triggers the event
        /// </summary>
        public Customer Patron { get; set; }

        /// <summary>
        /// Constructor that initializes class properties
        /// </summary>
        /// <param name="timeOfEvent">The time at which the event occurs</param>
        /// <param name="eventType">The type of event</param>
        /// <param name="patron">The customer that triggers the event</param>
        public Event(DateTime timeOfEvent, EVENTTYPE eventType, Customer patron)
        {
            TimeOfEvent = timeOfEvent;
            Type = eventType;
            Patron = patron;
        }

        /// <summary>
        /// Compares two Event objects based on the time at which they occur
        /// </summary>
        /// <param name="other">The event object being compared to</param>
        /// <returns>-1 if this event occurs before the other, 1 if it occurs after, and 0 if they are the same</returns>
        public int CompareTo(object other)
        {
            if (other == null)
            {
                throw new ArgumentNullException();
            }
            else if (!(other is Event))
            {
                throw new ArgumentException("Event objects can only be compared to other Event objects.");
            }
            else
            {
                Event otherEvent = other as Event;

                return TimeOfEvent.CompareTo(otherEvent.TimeOfEvent);
            }
        }
    }
}
