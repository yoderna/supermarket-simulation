//////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//        Project:    Supermarket Simulation
//
//      File Name:    EventType.cs
//
//         Author:    Nicholas Yoder
//
//    Description:    This file contains the EventType enumeration declaration, which indicates
//                    whether an event is of type ENTER or EXIT.
//
//                      ENTER: Occurs when a new customer enters the back of a checkout line.
//
//                      EXIT: Occurs when a customer exits a checkout line.
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace SupermarketSimulation
{
    /// <summary>
    /// Enumerated data type that indicates whether an event is of type ENTER or EXIT
    /// </summary>
    public enum EVENTTYPE
    {
        /// <summary>
        /// A customer enters the back of a checkout line
        /// </summary>
        ENTER,

        /// <summary>
        /// A customer exits a checkout line
        /// </summary>
        EXIT
    }
}
