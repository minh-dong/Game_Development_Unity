using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1_Base
{
    class Program
    {
        /**
        * <summary>
        * The factorization will find the prime numbers, place it in the list
        * and repeat until factorization can no longer be done. After that, return
        * the list so it can be displayed.
        * </summary>
        * \par Pseudo Code
        * \par
        * If there are any 2s, take it out.
        * \par
        * If there are any other primes, take it out.
        * \par
        * If the number is not 1, then whatever is left is prime.
        * 
        * <param name="num">Integer list</param>
        * 
        * <returns>Returns the List</returns>
        */
        public static List<int> Factorization(int num)
        {
            List<int> result = new List<int>();

            // Take out the 2s
            while (num % 2 == 0)
            {
                result.Add(2);
                num /= 2;
            }

            // Take out other primes.
            int factor = 3;
            while (factor * factor <= num)
            {
                if (num % factor == 0)
                {
                    result.Add(factor);
                    num /= factor;
                }
                else factor += 2;
            }

            // If num is not 1, then whatever is left is prime
            if (num > 1)
                result.Add(num);

            return result;

        } // end of Factorization function

        /**
        * <summary>
        * The greatest common demonitor (GCD) will take in the numbers and will
        * find the GCD value, then return it to display later.
        * </summary>
        * \par Pseudo Code
        * \par
        * If a is greater than b, a = a % b
        * \par
        * If b is greater than a, b = b % a
        *
        * <param name="a">Integer A Value</param>
        * <param name="b">Integer B Value</param>
        * 
        * <returns>Returns values of a and b</returns>
        */
        public static int GCD(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a == 0 ? b : a;
        }// End GCD

        /**
        * <summary>
        * The least common denominator (LCD) will take in the numbers and will
        * find the LCD value, then return it to display later.
        * \par Pseudo Code
        * \par
        * Store the values if a > b or b > a for num1 and num2 respecitvely (both)
        * \par
        * Keep looping for the lowest common deonimator until the LCD is satisfied
        * 
        * <param name="a">Integer A value</param>
        * <param name="b">Integer B value</param>
        * 
        * <returns>Returns the number for LCD</returns>
        */
        public static int LCD(int a, int b)
        {
            int num1, num2;

            if (a > b)
            {
                num1 = a;
                num2 = b;
            }
            else
            {
                num1 = b;
                num2 = a;
            }

            for (int i = 1; i < num2; i++)
            {
                if ((num1 * i) % num2 == 0)
                    return i * num1;
            }
            return num1 * num2;
        }

        /**
        * <summary>
        * The comomn factors will be found when two numbers are input into this
        * </summary>
        * \par Pseudo Code
        * \par
        * Continue to loop until the highest common factor is found
        * 
        * <param name="a">Integer A Value</param>
        * <param name="b">Integer B Value</param>
        * 
        * <returns>Returns the List</returns>
        */
        public static int CF(int a, int b)
        {
            int i, j, hcf = 1;

            j = (a < b) ? a : b;
            for (i = 1; i <= j; i++)
                if (a % i == 0 && b % i == 0)
                    hcf = i;

            return hcf;
        }

        /**
        * <summary>
        * The main function to run the entire program. This program will ask the user to
        * input two numbers, then the program will calculate the GCF and LCD automatically.
        * Also, it will display to the user with some output.
        * </summary>
        * 
        * <param name="args"></param>
        * 
        * <returns>Loops if Y/y, Stops if N/n</returns>
        */
        static void Main(string[] args)
        {
            int a = -1, b = -1;
            string sa, sb;
            int[] primes = {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 
                           47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97};

            bool isContinue = true;

            while (isContinue)
            {
                bool aValid = false, bValid = false;
                Console.WriteLine("Enter the first number:");
                while (!aValid)
                {
                    sa = Console.ReadLine();
                    try
                    {
                        a = Int32.Parse(sa);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} is not a valid integer.", sa);
                    }
                    if (a < 1 || a > 100)
                        Console.WriteLine("Please enter a number between 1 and 100.");
                    else
                        aValid = true;
                }

                Console.WriteLine("Enter the second number:");
                while (!bValid)
                {
                    sb = Console.ReadLine();
                    try
                    {
                        b = Int32.Parse(sb);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} is not a valid integer.", sb);
                    }
                    if (b < 1 || b > 100)
                        Console.WriteLine("Please enter a number between 1 and 100.");
                    else
                        bValid = true;
                }

                //                           \\
                // **Enter your code here.** \\
                //                           \\
                Console.WriteLine(""); // to add a space

                // Get the first factor text and display it
                Console.WriteLine("The factors of {0} are:", a);
                var factorsA = string.Join(" ", Factorization(a));
                Console.WriteLine(factorsA);

                // Get the second factor text and display it
                Console.WriteLine("The factors of {0} are:", b);
                var factorsB = string.Join(" ", Factorization(b));
                Console.WriteLine(factorsB);

                // Find the highest common number then factor and display it
                Console.WriteLine("The common factors of {0} and {1} are:", a, b);
                int newCommonFactor = CF(a, b);
                string primeCommonFactors = string.Join(" ", Factorization(newCommonFactor));
                Console.WriteLine(primeCommonFactors);

                // Get the GCF and LCD and display it
                Console.WriteLine("The GCF of {0} and {1} is: {2}", a, b, GCD(a, b));
                Console.WriteLine("The LCD of {0} and {1} is: {2}", a, b, LCD(a, b));

                //                          \\
                // **End writing own code** \\
                //                          \\

                Console.WriteLine("\nDo you want to continue? Y/N");
                string newLoop = Console.ReadLine();
                if (newLoop[0] == 'Y' || newLoop[0] == 'y')
                {
                    Console.WriteLine();
                    isContinue = true;
                }
                else
                    isContinue = false;
            }
        }
    }
}
