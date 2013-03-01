using System;
using System.Collections.Generic;
using System.Media;

public class MathQuiz
{
    static void Main()
    {

    #region Local Declarations

        const int ArraySize = 10;
        int SwapSpace = 0;
        bool ToggleTimer = true;
        DateTime startTime = DateTime.Now;
        int UserAnswer = 0;
        double AverageTime = 0.0;

        //create a 10 x 10 2-Dimensional Array 0 to 9, for Elapsed Time
        double[,] MultiTable = new double[ArraySize, ArraySize];

        // Create 2, one dimensional arrays for Multiplicands, Multiplyers
        int[] Multiplicands = new int[ArraySize * ArraySize];
        int[] Multiplyers = new int[ArraySize * ArraySize];

        List<int> myQueue = new List<int>();

        // Declare an Instance of Random Class outside the Loop
        Random MyRandNumb = new Random();

        #endregion

    #region Intro

        Console.WriteLine("\n\nWelcome to Dillinger's Math Quiz program.\n");

        System.Threading.Thread.Sleep(3000);

        Console.WriteLine("First let's go through the multiplication tables");
        Console.WriteLine("and calculate your average response time.\n");

        System.Threading.Thread.Sleep(4000);

        Console.WriteLine("Press the Enter key to Begin.");
        Console.ReadLine();

        #endregion

    #region Seed the Arrays

        // Fill Multiplicands and Multiplyers Arrays with seed numbers
        for (int hCounter = 0; hCounter < ArraySize; hCounter++)
        {
            for (int iCounter = (hCounter * ArraySize); iCounter < ((hCounter * ArraySize) + ArraySize); iCounter++)
            {  //  000000000, 111111111, 222... etc.
                Multiplicands[iCounter] = hCounter;
            }
        }

        for (int hCounter = 0; hCounter < ArraySize; hCounter++)
        {   // 123456789, 123456789, 123... etc.
            for (int iCounter = 0; iCounter < (ArraySize); iCounter++)
            {
                Multiplyers[iCounter + (hCounter * ArraySize)] = iCounter;
            }
        }

        // Number the Queue
        for (int iCounter = 0; iCounter < ArraySize * ArraySize; iCounter++)
        {
            myQueue.Add(iCounter); //Enqueue
        } // end Seed Arrays

        #endregion

    #region Shuffle Loosely
        //Shuffle the Queue in 4 separate overlapping segments so it starts easy and gets harder
        // 0-27  18-45 36-63 54-81
        // 0 - 3   2 - 5   4 - 7    6 - 9      ( * 9 or SizeOfArray )
        for (int iCounter = 0; iCounter < 9; iCounter++)
        {
            for (int kCounter = ((iCounter * 9) + 1); kCounter < (((iCounter + 3) * 9) + 1); kCounter++)
            {
                // Generate Random number within current boundaries
                int Swap = MyRandNumb.Next((1 + (iCounter * 9)), ((iCounter + 3) * 9));

                SwapSpace = myQueue[kCounter]; // Store each cell temporarily in SwapSpace Variable
                myQueue[kCounter] = myQueue[Swap]; // Swap two random cells
                myQueue[Swap] = SwapSpace; // Put SwapSpace Variable back in leftover cell
            }
        } // end Shuffle
        #endregion

    #region Multiplication Quiz
        //Use a For Loop to Quiz the user based on the Current Queue Number
        // and print it to the screen
        for (int jCounter = 0; jCounter < (myQueue.Count); jCounter++)
        {
            if (ToggleTimer == true)   // Don't reset the timer for Retry
            {
                startTime = DateTime.Now; // Get the current time
            }
        StartHere:
            Console.Write("\n{0} X {1} = ",
                Multiplicands[myQueue[jCounter]], Multiplyers[myQueue[jCounter]]);
            try
            {   // Convert User input to numerical Integer
                UserAnswer = int.Parse(Console.ReadLine());
            }
            catch
            {
                SystemSounds.Exclamation.Play();
                Console.WriteLine("\nThat does not compute!\n ");
                Console.WriteLine("Expected whole number input. ");
                Console.WriteLine("Please try again.\n ");
                goto StartHere;
            }

            if (UserAnswer == (Multiplicands[myQueue[jCounter]] * Multiplyers[myQueue[jCounter]]))
            {   // if user answer is correct...
                DateTime stopTime = DateTime.Now; // get the current time
                TimeSpan TimeElapsed = (stopTime - startTime); // Calculate elapsed time
                Console.Write("Elapsed Time:  ");
                Console.WriteLine(TimeElapsed.TotalSeconds);
                ToggleTimer = true;
                //Store the elapsed time of each answer in the MultiTable Array
                MultiTable[Multiplicands[myQueue[jCounter]], Multiplyers[myQueue[jCounter]]] = TimeElapsed.TotalSeconds;
            }
            else
            {
                SystemSounds.Beep.Play();
                Console.Beep(100, 500);
                Console.WriteLine("\nNope, try again");
                ToggleTimer = false;
                jCounter--;
            }
        } // end Multiplication Quiz

        #endregion

    #region Average Reaction Time

        // Calculate the average answer time
        AverageTime = 0.0;  // Should be zeroed but let's make sure
        foreach (double elementValue in MultiTable) // Read entire 2 Dim Array using Foreach
        {
            AverageTime = AverageTime + elementValue;  // Add all the values together
        }

        SystemSounds.Asterisk.Play();
        Console.WriteLine("\n\nGood!\n");

        System.Threading.Thread.Sleep(2000);

        Console.Write("\nYour average response time was: ");
        // Divide total by the number of Array Cells and Voila!
        AverageTime = AverageTime / (ArraySize * ArraySize); // Store the Average
        Console.WriteLine("{0} seconds.\n", AverageTime); // Print it

        System.Threading.Thread.Sleep(3000);

        Console.WriteLine("Press the Enter key to view your Reaction Times.\n");
        Console.ReadLine();

        // end Average
        #endregion

    #region UnShuffle the Queue

        for (int iCounter = 0; iCounter < ArraySize * ArraySize; iCounter++)
        {
            myQueue.Add(iCounter); // Enqueue
        }

        // Remove Dividing Zeros
        for (int iCounter = 0; iCounter < (myQueue.Count); iCounter++)
        {
            if (Multiplicands[myQueue[iCounter]] == 0)
            {
                myQueue.RemoveAt(iCounter);
                iCounter--;
            }
        }
        #endregion

    #region Print Reaction Times
        Console.WriteLine("\nList of Reaction Times \n");
        for (int iCounter = 0; iCounter < (myQueue.Count); iCounter++)
        {
            Console.WriteLine("{0} X {1}: {2} ",
        Multiplicands[myQueue[iCounter]], Multiplyers[myQueue[iCounter]], MultiTable[Multiplicands[myQueue[iCounter]], Multiplyers[myQueue[iCounter]]]);
        }
        #endregion

    #region Intro Part 2
        // Begin new quiz using slower than average times
        System.Threading.Thread.Sleep(6000);
        Console.WriteLine("\nLet's focus on the ones that took longer than average: \n");

        System.Threading.Thread.Sleep(3000);

        Console.WriteLine("Press the Enter key to continue");
        Console.ReadLine(); 
        #endregion

    #region Remove the Easy Questions

            for (int iCounter = 0; iCounter < (myQueue.Count); iCounter++)
            {
                // Step through the MultiTable
                // If Elapsed time is Less than Average Time, Delete the reference to it
                if ((MultiTable[Multiplicands[myQueue[iCounter]], Multiplyers[myQueue[iCounter]]] < AverageTime))
                {
                    myQueue.RemoveAt(iCounter);
                    iCounter--;
                }
            }
            // end free lunch
            #endregion

            while (myQueue.Count > 0)
            {
            #region Shuffle Random

            // Shuffle the Queue
            for (int iCounter = 0; iCounter < myQueue.Count; iCounter++)
            {
                // Generate Random number within current boundaries
                int Swap = MyRandNumb.Next(myQueue.Count);

                SwapSpace = myQueue[iCounter]; // Store each cell temporarily in SwapSpace Variable
                myQueue[iCounter] = myQueue[Swap]; // Swap two random cells
                myQueue[Swap] = SwapSpace; // Put SwapSpace Variable back in leftover cell

            } // end Shuffle
            #endregion

            #region Retry Multiplication Quiz

            for (int iCounter = 0; iCounter < (myQueue.Count); iCounter++)
            {

            StartHere2:
                Console.Write("\n\nWhat is {0} X {1} ? ",
                    Multiplicands[myQueue[iCounter]], Multiplyers[myQueue[iCounter]]);

                // Quiz the user on iCounter
                if (ToggleTimer == true)
                {
                    startTime = DateTime.Now;
                }
                try
                {
                    UserAnswer = int.Parse(Console.ReadLine());
                }
                catch
                {
                    SystemSounds.Beep.Play();
                    Console.Beep(100, 500);
                    Console.WriteLine("\nThat does not compute!\n ");
                    Console.WriteLine("Expected whole number input. ");
                    Console.WriteLine("Please try again.\n ");
                    goto StartHere2;
                }

                if (UserAnswer ==
                    (Multiplicands[myQueue[iCounter]] * Multiplyers[myQueue[iCounter]]))
                {
                    DateTime stopTime = DateTime.Now;
                    TimeSpan TimeElapsed = (stopTime - startTime);
                    Console.Write("Elapsed Time:  ");
                    Console.WriteLine(TimeElapsed.TotalSeconds);
                    Console.Write("Previous Elapsed Time:  ");
                    Console.WriteLine(MultiTable[Multiplicands[myQueue[iCounter]], Multiplyers[myQueue[iCounter]]]);
                    ToggleTimer = true;
                    // Subtract the difference from user's previous answer
                    if (TimeElapsed.TotalSeconds < MultiTable[Multiplicands[myQueue[iCounter]], Multiplyers[myQueue[iCounter]]])
                    {
                        SystemSounds.Asterisk.Play();
                        Console.Write("You were {0} seconds faster this time!\n",
                        MultiTable[Multiplicands[myQueue[iCounter]], Multiplyers[myQueue[iCounter]]] -
                        TimeElapsed.TotalSeconds);
                        myQueue.RemoveAt(iCounter);
                    }
                }
                else
                {
                    SystemSounds.Beep.Play();
                    Console.Beep(100, 500);
                    Console.WriteLine("\nNope, try again");
                    ToggleTimer = false;
                    goto StartHere2;
                }
            } // end if User Answer Correct

            #endregion // end Retry Multiplication Tables 
        }

    #region The End
                SystemSounds.Asterisk.Play();
                Console.WriteLine("\nYou have succesfully beat all of your intial reaction times! :)");
                Console.WriteLine("Press the Enter key to terminate.");
                Console.ReadLine(); 
                #endregion // The end of The End

    }
}
