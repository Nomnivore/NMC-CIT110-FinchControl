using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control - Menu Starter
    // Description: Starter solution with the helper methods,
    //              opening and closing screens, and the menu
    // Application Type: Console
    // Author: Velis, John
    // Dated Created: 1/22/2020
    // Last Modified: 1/25/2020
    //
    // **************************************************

    class Program
    {
        // ************************************
        // Title: Finch Control
        // Application Type: Console
        // Description: Demonstrate Finch's capabilities combining light, sound, and movement in a Talent Show.
        // Author: Kyle Warner
        // Date Created:  6/2/2021
        // Last Modified: 6/3/2021
        // ************************************


        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        TalentShowDisplayMenuScreen(finchRobot);
                        break;

                    case "c":
                        DataRecorderDisplayMenuScreen(finchRobot);
                        break;

                    case "d":
                        AlarmSystemDisplayMenuScreen(finchRobot);
                        break;

                    case "e":
                        UserProgrammingDisplayMenuScreen(finchRobot);
                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void TalentShowDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light and Sound");
                Console.WriteLine("\tb) Dance");
                Console.WriteLine("\tc) Mixing It Up");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower().Trim();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        TalentShowDisplayLightAndSound(finchRobot);
                        break;

                    case "b":
                        TalentShowDisplayDance(finchRobot);
                        break;

                    case "c":
                        TalentShowDisplayMixingItUp(finchRobot);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Light and Sound                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void TalentShowDisplayLightAndSound(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light and Sound");

            Console.WriteLine("\tThe Finch robot will now show off its glowing talent!");
            DisplayContinuePrompt();

            for (int lightSoundLevel = 0; lightSoundLevel < 255; lightSoundLevel++)
            {
                finchRobot.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
                finchRobot.noteOn(lightSoundLevel * 100);
            }

            finchRobot.setLED(0, 0, 0);

            DisplayMenuPrompt("Talent Show");
        }

        /// <summary>
        /// Talent Show > Dance
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void TalentShowDisplayDance(Finch finchRobot)
        {
            DisplayScreenHeader("Dance");

            Console.WriteLine("\tThe Finch robot will now do a spinning dance!");
            DisplayContinuePrompt();

            int speed = GetSpeedFromUser(100, 255);

            finchRobot.setMotors(speed/2, speed/2);
            finchRobot.wait(2000);

            finchRobot.setMotors(speed, -speed);
            finchRobot.wait(4000);

            finchRobot.setMotors(0, 0);
            finchRobot.wait(500);

            finchRobot.setMotors(speed/2, speed/2);
            finchRobot.wait(2000);

            finchRobot.setMotors(-speed, speed);
            finchRobot.wait(4000);

            finchRobot.setMotors(0, 0);

            DisplayMenuPrompt("Talent Show");
        }

        /// <summary>
        /// Prompts the user for a speed input & validates
        /// </summary>
        /// <param name="min">minimum value allowed for entry</param>
        /// <param name="max">maximum value allowed for entry</param>
        /// <returns>The speed entered by the user</returns>
        static int GetSpeedFromUser(int min, int max)
        {
            int userSpeed;
            bool validSpeed = false;
            Console.WriteLine($"\tEnter a number between {min} and {max} for the speed.");
            do
            {
                Console.Write("\t> ");
                if (int.TryParse(Console.ReadLine(), out userSpeed) && userSpeed >= min && userSpeed <= max)
                    validSpeed = true;
                else
                {
                    Console.WriteLine($"\tOops! It must be a single integer number between {min} and {max}.");
                }
            } while (!validSpeed);
            return userSpeed;
        }

        static void TalentShowDisplayMixingItUp(Finch finchRobot)
        {
            DisplayScreenHeader("Mixing It Up");

            Console.WriteLine("\tThe robot will now show you what it can do!");
            Console.WriteLine("\t  - Song: Overwatch Theme Jingle");
            DisplayContinuePrompt();

            // overwatch theme jingle
            // notes will be played with randomized LED colors
            // we'll alternate the notes with different movement patterns
            // wait commands are handled by the PlayNote* methods

            // curve forward
            finchRobot.setMotors(255, 100);
            PlayNoteWithLED(finchRobot, 698, 1);
            PlayNoteWithLED(finchRobot, 659, .5);

            // sharp turn
            finchRobot.setMotors(-255, 255);
            PlayNoteWithLED(finchRobot, 698, .5);

            // reverse
            finchRobot.setMotors(-255, -255);
            PlayNoteWithLED(finchRobot, 784, 1);
            PlayNoteWithLED(finchRobot, 659, .5);

            //forward
            finchRobot.setMotors(255, 255);
            PlayNoteWithLED(finchRobot, 784, .5);

            // spin
            finchRobot.setMotors(-255, 255);
            PlayNoteWithLED(finchRobot, 988, 2);

            finchRobot.setMotors(0, 0);
        }

        /// <summary>
        /// Helper method to play a single note
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <param name="freq">frequency (hz) of the note</param>
        /// <param name="seconds">time (in seconds) to play the note for</param>
        static void PlayNote(Finch finchRobot, int freq, double seconds)
        {
            int noteTime = Convert.ToInt32(seconds * 1000);
            finchRobot.noteOn(freq);
            finchRobot.wait(noteTime);
            finchRobot.noteOff();
        }

        /// <summary>
        /// Helper method to play a single note, with LED.
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <param name="freq">frequency (hz) of the note</param>
        /// <param name="seconds">time (in seconds) to play the note for</param>
        /// <param name="r">red color, default random if not set</param>
        /// <param name="g">green color, default random if not set</param>
        /// <param name="b">blue color, default random if not set</param>
        static void PlayNoteWithLED(Finch finchRobot, int freq, double seconds, int r = -1, int g = -1, int b = -1)
        {
            // convert default values to random if not specified in method call
            Random rand = new Random();
            if (r == -1)
                r = rand.Next(0, 255);
            if (g == -1)
                g = rand.Next(0, 255);
            if (b == -1)
                b = rand.Next(0, 255);

            finchRobot.setLED(r, g, b);
            PlayNote(finchRobot, freq, seconds);
            finchRobot.setLED(0, 0, 0);
        }

        #endregion

        #region DATA RECORDER

        static void DataRecorderDisplayMenuScreen(Finch finchRobot)
        {
            DisplayUnderDevelopment("Data Recorder Menu");
        }

        #endregion

        #region ALARM SYSTEM

        static void AlarmSystemDisplayMenuScreen(Finch finchRobot)
        {
            DisplayUnderDevelopment("Alarm System Menu");
        }

        #endregion

        #region USER PROGRAMMING

        static void UserProgrammingDisplayMenuScreen(Finch finchRobot)
        {
            DisplayUnderDevelopment("User Programming Menu");
        }

        #endregion

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnected.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            do
            {
                DisplayContinuePrompt();
                robotConnected = finchRobot.connect();

                if (!robotConnected)
                    Console.WriteLine("\n\tCould not connect to the Finch Robot. Please check the connections and try again.");

            } while (!robotConnected);

            // test connection and provide user feedback - text, lights, sounds

            Console.WriteLine("\n\tConnected! Your robot should be lighting up green.");
            finchRobot.setLED(0, 255, 0);

            PlayNote(finchRobot, 523, .5);


            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        static void DisplayUnderDevelopment(string headerText)
        {
            DisplayScreenHeader(headerText);
            Console.WriteLine("\tSorry, this module is still under development.");
            Console.WriteLine("\tCome back later!");
            DisplayContinuePrompt();
        }

        #endregion
    }
}
