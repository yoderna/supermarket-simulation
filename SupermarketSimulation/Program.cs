//////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//        Project:    Supermarket Simulation
//
//      File Name:    Program.cs
//
//         Course:    CSCI 2210-001: Data Structures
//
//         Author:    Nicholas Yoder
//
//        Contact:    yoderna@etsu.edu
//
//    Description:    This file contains the entry point of the application. It presents the user with
//                    a console menu interface to set variables related to the simulation.
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace SupermarketSimulation
{
    /// <summary>
    /// Contains the entry point of the program and functions to
    /// provide a console window menu interface to the user
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Entry point of the program
        /// </summary>
        /// <remarks>
        /// Contains code for menu-based interaction with the user.
        /// Here, the user can configure the simulation prior to running it.
        /// </remarks>
        static void Main()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Blue;

            Supermarket supermarket = new Supermarket();

            char menuChoice;

            do
            {
                Console.Clear();

                // Display all menu options and receive the user's choice
                DisplayMenu(supermarket);

                menuChoice = Console.ReadKey().KeyChar;

                Console.Clear();

                // Handle the user's menu choice
                HandleMenuChoice(supermarket, menuChoice);

            } while (menuChoice != '6');

        }

        /// <summary>
        /// Displays all menu options and current supermarket configuration
        /// </summary>
        /// <param name="supermarket">The Supermarket object from which supermarket configuration is obtained</param>
        private static void DisplayMenu(Supermarket supermarket)
        {
            Console.WriteLine("Menu".PadLeft(38));

            Console.WriteLine("----".PadLeft(38));

            Console.WriteLine("\n\t1. Enter the expected number of customers (currently " + supermarket.NumCustomers + ")");

            Console.WriteLine("\t2. Enter the number of checkout lines (currently " + supermarket.NumLines + ")");

            Console.WriteLine("\t3. Enter the expected checkout time (currently " + supermarket.ExpectedCheckoutTime + " minutes)");

            Console.WriteLine("\t4. Enter the number of hours open (currently " + supermarket.HoursOpen + ")");

            Console.WriteLine("\t5. Run the simulation");

            Console.WriteLine("\t6. Exit");

            Console.Write("\nEnter menu choice: ");
        }

        /// <summary>
        /// Reacts to the user's chosen menu option
        /// </summary>
        /// <remarks>
        /// If the user chose options 1-4, they will be prompted to enter a value for some supermarket
        /// configuration variable. The user will continue to be prompted until valid data is entered.
        /// </remarks>
        /// <param name="supermarket">The Supermarket object to configure or run the simulation with</param>
        /// <param name="menuChoice">The user's chosen menu option</param>
        public static void HandleMenuChoice(Supermarket supermarket, char menuChoice)
        {
            switch (menuChoice)
            {
                // 1 -- Receive input for number of customers to serve in the simulation
                case '1':
                    Console.Write("Enter the expected number of customers: ");

                    int numCustomers;

                    while (!int.TryParse(Console.ReadLine(), out numCustomers) || numCustomers <= 0)
                    {
                        Console.WriteLine("\nInvalid input; must be an integer greater than 0.\n");
                        Console.Write("Enter the expected number of customers: ");
                    }

                    supermarket.NumCustomers = numCustomers;
                    break;

                // 2 -- Receive input for the number of checkout lines in the simulation
                case '2':
                    Console.Write("Enter the number of checkout lines: ");

                    int numLines;

                    while (!int.TryParse(Console.ReadLine(), out numLines) || numLines <= 0 || numLines > 8)
                    {
                        Console.WriteLine("\nInvalid input; must be an integer between 1 and 8.\n");
                        Console.Write("Enter the number of checkout lines: ");
                    }

                    supermarket.NumLines = numLines;
                    break;

                // 3 -- Receive input for the expected checkout time of customers
                // This is the amount of time a customer must wait upon reaching
                // the front of a line, NOT the total time spent waiting in line
                case '3':
                    Console.Write("Enter the expected checkout time of each customer: ");

                    double checkoutTime;

                    while (!double.TryParse(Console.ReadLine(), out checkoutTime) || checkoutTime <= 2)
                    {
                        Console.WriteLine("\nInvalid input; must be a floating-point number greater than 2.\n");
                        Console.Write("Enter the expected checkout time of each customer: ");
                    }

                    supermarket.ExpectedCheckoutTime = checkoutTime;
                    break;

                // 4 -- Receive input for the number of hours to be open during the simulation
                case '4':
                    Console.Write("Enter the number of hours the supermarket will be open: ");

                    int hoursOpen;

                    while (!int.TryParse(Console.ReadLine(), out hoursOpen) || hoursOpen <= 0 || hoursOpen > 24)
                    {
                        Console.WriteLine("\nInvalid input; must be an integer between 1 and 24.\n");
                        Console.Write("Enter the number of hours the supermarket will be open: ");
                    }

                    supermarket.HoursOpen = hoursOpen;
                    break;

                // 5 -- Run the simulation
                case '5':
                    supermarket.Run();
                    break;
            }
        }
    }
}
