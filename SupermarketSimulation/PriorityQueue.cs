//////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//        Project:    Supermarket Simulation
//
//      File Name:    PriorityQueue.cs
//
//    Description:    This file contains a linked list implementation of a generic priority queue
//
//         Course:    CSCI 2210-001: Data Structures
//
//         Author:    Nicholas Yoder
//
//        Contact:    yoderna@etsu.edu
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace SupermarketSimulation
{
    /// <summary>
    /// Linked list implementation of a priority queue
    /// </summary>
    /// <typeparam name="T">The type of object to use in the priority queue</typeparam>
    public class PriorityQueue<T> where T : IComparable
    {
        /// <summary>
        /// Indicates whether the priority queue is empty (true) or not (false)
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return head == null;
            }
        }

        /// <summary>
        /// The first node in the priority queue
        /// (the node with the highest priority)
        /// </summary>
        private Node head;
        
        /// <summary>
        /// Represents a node in the linked list
        /// </summary>
        private class Node
        {
            /// <summary>
            /// The value contained in this node
            /// </summary>
            public T value { get; set; }

            /// <summary>
            /// Reference to the next node in the linked list
            /// </summary>
            public Node next { get; set; }

            /// <summary>
            /// Node constructor that initializes class properties
            /// </summary>
            /// <param name="value">The generic value contained in this node</param>
            /// <param name="nextNode">The next node in the linked list</param>
            public Node(T value, Node nextNode)
            {
                this.value = value;
                next = nextNode;
            }
        }

        /// <summary>
        /// Enqueues a new item in the priority queue
        /// </summary>
        /// <param name="newValue">The value being added to the priority queue</param>
        public void Enqueue(T newValue)
        {
            // If the PQ is empty, the new value will be the head node
            if (head == null)
            {
                head = new Node(newValue, null);
            }
            // Otherwise, iterate through the linked list until its appropriate place is found
            else
            {
                Node currNode = head;
                Node prevNode = null;

                while (currNode != null && newValue.CompareTo(currNode.value) > 0)
                {
                    prevNode = currNode;
                    currNode = currNode.next;
                }

                // Insert the new item into the linked list
                Node newNode = new Node(newValue, currNode);

                if (prevNode == null)
                {
                    head = newNode;
                }
                else
                {
                    prevNode.next = newNode;
                }
            }
        }

        /// <summary>
        /// Dequeues and returns the item with the highest priority
        /// </summary>
        /// <returns>The item at the front of the priority queue</returns>
        public T Dequeue()
        {
            T value = head.value;

            head = head.next;

            return value;
        }
    }
}