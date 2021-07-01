using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinchAPI;
using System.Text.RegularExpressions;

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

    /// <summary>
    /// User Commands
    /// </summary>
    public enum Command
    {
        NONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        WAIT,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF,
        GETTEMPERATURE,
        SOUNDON,
        SOUNDOFF,
        RANDLED, 
        RANDSOUND,
        RANDLIGHTSOUND,
        DISCO, // spin, with random light and random sound
        DONE
    }

    class Program
    {
        // ************************************
        // Title: Finch Control
        // Application Type: Console
        // Description: S1) Demonstrate Finch's capabilities combining light, sound, and movement in a Talent Show.
        //              S2) Record ambient light and temperature, displaying results in a neatly organized table.
        //              S3) Monitor light and/or temperature levels, sounding an alarm when a threshold is exceeded.
        //              S4) Add, read, display and execute user defined commands for the Finch robot.
        //              S5) Save and Load command lists from text files. Data persistance using File IO
        // Author: Kyle Warner
        // Date Created:  6/2/2021
        // Last Modified: 6/22/2021
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
                Console.Write("\t\tEnter Choice: ");
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
                Console.Write("\t\tEnter Choice: ");
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
            DisplayScreenHeader("Data Recorder Menu");

            bool quitMenu = false;
            string menuChoice;

            // variables for data collection
            int numberOfDataPoints = 0;
            double dataPointFrequency = 0;
            double[] temperatures = null;
            int[] lightLevels = null;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Number of Data Points");
                Console.WriteLine("\tb) Frequency of Data Points");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Show Data");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice: ");
                menuChoice = Console.ReadLine().ToLower().Trim();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        break;

                    case "c":
                        (temperatures, lightLevels) = DataRecorderDisplayGetData(numberOfDataPoints, dataPointFrequency, finchRobot);
                        break;

                    case "d":
                        DataRecorderDisplayData(temperatures, lightLevels);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            DisplayScreenHeader("Number of Data Points Input");
            Console.WriteLine("\tPlease enter the number of data points\n" +
                "\tyou'd like the Finch robot to record: ");
            bool validResponse = false;
            int numPoints;
            do
            {
                Console.Write("\t#: ");
                if (int.TryParse(Console.ReadLine(), out numPoints) && numPoints > 0)
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine("\tPlease enter a positive integer.");
                }
            } while (!validResponse);

            Console.WriteLine("\tGot it! The Finch will record {0} data points.", numPoints);
            DisplayContinuePrompt();
            return numPoints;
        }

        static double DataRecorderDisplayGetDataPointFrequency()
        {
            DisplayScreenHeader("Data Point Frequency Input");
            Console.WriteLine("\tPlease enter the time interval (in seconds)\n" +
                "\tbetween each data point recording.");
            bool validResponse = false;
            double pointFrequency;
            do
            {
                Console.Write("\t#: ");
                if (double.TryParse(Console.ReadLine(), out pointFrequency) && pointFrequency > 0)
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine("Please enter a positive number (in seconds).");
                }
            } while (!validResponse);

            Console.WriteLine("\tGot it! The Finch will record the data points at\n" +
                $"\ta frequency of 1 every {pointFrequency:F2} second(s).");
            DisplayContinuePrompt();
            return pointFrequency;
        }

        static (double[] temperatures, int[] lightLevels) DataRecorderDisplayGetData(int numberOfDataPoints, double dataPointFrequency, Finch finchRobot)
        {
            DisplayScreenHeader("Get Data");
            double[] temperatures = new double[numberOfDataPoints];
            int[] lightLevels = new int[numberOfDataPoints];
            int leftLightReading;
            int rightLightReading;

            Console.WriteLine("\t{0} data points will be recorded at a frequency of 1 every {1} second(s).",
                numberOfDataPoints, dataPointFrequency);
            Console.WriteLine("\tThe Finch Robot is ready to begin recording.");
            DisplayContinuePrompt();

            // display table headers
            Console.WriteLine();
            string[] headers = { "Point", "Temp(F)", "Light" };
            foreach (string header in headers)
            {
                Console.Write(header.PadLeft(10));
            }
            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat("-", headers.Length * 10)));

            for (int i = 0; i < numberOfDataPoints; ++i)
            {
                // get data using finch's sensors
                temperatures[i] = CelsiusToFahrenheit(finchRobot.getTemperature());
                leftLightReading = finchRobot.getLeftLightSensor();
                rightLightReading = finchRobot.getRightLightSensor();
                lightLevels[i] = (leftLightReading + rightLightReading) / 2;

                // echo data to user in table
                Console.WriteLine(
                    $"{i + 1}".PadLeft(10) +
                    $"{temperatures[i]:F1}".PadLeft(10) +
                    $"{lightLevels[i]}".PadLeft(10)
                    );
                finchRobot.wait(Convert.ToInt32(dataPointFrequency * 1000));
            }

            Console.WriteLine("\n\tThe Finch robot has completed the data recording.");
            DisplayContinuePrompt();

            return (temperatures: temperatures, lightLevels: lightLevels);
        }
        static void DataRecorderDisplayData(double[] temperatures, int[] lightLevels)
        {
            DisplayScreenHeader("Display Data");
            DataRecorderDisplayDataTable(temperatures, lightLevels);
            DisplayContinuePrompt();
        }

        static void DataRecorderDisplayDataTable(double[] temperatures, int[] lightLevels)
        {
            // display table headers
            string[] headers = { "Point", "Temp(F)", "Light" };
            foreach (string header in headers)
            {
                Console.Write(header.PadLeft(10));
            }
            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat("-", headers.Length * 10)));

            // display data from tables
            // they will both be the same length, so only one loop is necessary.
            for (int i = 0; i < temperatures.Length; ++i)
            {
                Console.WriteLine(
                    $"{i + 1}".PadLeft(8) +
                    $"{temperatures[i]:F1}".PadLeft(8) +
                    $"{lightLevels[i]}".PadLeft(8)
                    );
            }
            Console.WriteLine(string.Concat(Enumerable.Repeat("-", headers.Length * 10)));
            // demonstrate some array methods, summarizing the data
            // display headers
            Console.WriteLine();
            string[] headers2 = { "Data", "Sum", "Average", "Median"};
            foreach (string header in headers2)
            {
                Console.Write(header.PadLeft(10));
            }
            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat("-", headers2.Length * 10)));

            // calculate values
            Array.Sort(temperatures);
            Array.Sort(lightLevels);
            double tempAvg = temperatures.Average();
            double tempSum = temperatures.Sum();
            double tempMedian = temperatures[temperatures.Length / 2];
            double lightAvg = lightLevels.Average();
            int lightSum = lightLevels.Sum();
            int lightMedian = lightLevels[lightLevels.Length / 2];

            // display those values in the summary table
            Console.WriteLine(
                "Temp(F)".PadLeft(10) +
                $"{tempSum}".PadLeft(10) +
                $"{tempAvg:F1}".PadLeft(10) +
                $"{tempMedian}".PadLeft(10)
                );
            Console.WriteLine(
                "Light".PadLeft(10) +
                $"{lightSum}".PadLeft(10) +
                $"{lightAvg:F1}".PadLeft(10) +
                $"{lightMedian}".PadLeft(10)
                );
        }


        static double CelsiusToFahrenheit(double celsius)
        {
            return (celsius * 9) / 5 + 32;
        }
        



        #endregion

        #region ALARM SYSTEM

        static void AlarmSystemDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            // variables for alarm configuration
            string monitorType = "both"; // "temp" || "light" || "both"
            string sensorsToMonitor = "both"; // "left" || "right" || "both"
            string rangeType = "max"; // "min" || "max"
            int tempThresholdValue = 0;
            int lightThresholdValue = 0;
            int timeToMonitor = 3; // in seconds


            // menu loop
            do
            {
                DisplayScreenHeader("Alarm System Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Temperature or Light");
                Console.WriteLine("\tb) Set Sensors to Monitor");
                Console.WriteLine("\tc) Set Range Type");
                Console.WriteLine("\td) Set Maximum/Minimum Threshold Value");
                Console.WriteLine("\te) Set Time to Monitor");
                Console.WriteLine("\tf) Set Alarm");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice: ");
                menuChoice = Console.ReadLine().ToLower().Trim();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        monitorType = AlarmSystemDisplaySetMonitorType();
                        break;

                    case "b":
                        sensorsToMonitor = AlarmSystemDisplaySetSensorsToMonitor(finchRobot, monitorType);
                        break;

                    case "c":
                        rangeType = AlarmSystemDisplaySetRangeType();
                        break;

                    case "d":
                        (tempThresholdValue, lightThresholdValue) = AlarmSystemDisplaySetThreshold(monitorType);
                        break;

                    case "e":
                        timeToMonitor = AlarmSystemDisplaySetTimeToMonitor();
                        break;

                    case "f":
                        AlarmSystemDisplaySetAlarm(finchRobot, monitorType, sensorsToMonitor, rangeType,
                            tempThresholdValue, lightThresholdValue, timeToMonitor);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static string AlarmSystemDisplaySetMonitorType()
        {
            bool validResponse = false;
            string choices = "[temp, light, both]";
            string userChoice;

            do
            {
                DisplayScreenHeader("Set Monitor Type");

                Console.WriteLine("\tThe alarm can monitor temperature, light, or both at once.");
                Console.Write("\tPlease enter the type of monitoring you'd like {0}: ", choices);
                userChoice = Console.ReadLine().Trim().ToLower();

                switch (userChoice)
                {

                    case "temp":
                    case "light":
                    case "both":
                        validResponse = true;
                        break;

                    default:
                        Console.WriteLine("\n\tPlease enter one of {0} for your choice.", choices);
                        DisplayContinuePrompt();
                        break;
                }

            } while (!validResponse);

            Console.WriteLine("\n\tGot it! The Finch will monitor {0} sensors.", userChoice);
            DisplayMenuPrompt("Alarm System");
            return userChoice;
        }

        static string AlarmSystemDisplaySetSensorsToMonitor(Finch finchRobot, string monitorType)
        {
            bool validResponse = false;
            string choices = "[left, right, both]";
            string userChoice;

            // guard clause for monitorType, temp only has one sensor
            if (monitorType == "temp")
            {
                DisplayScreenHeader("Set Sensors to Monitor");
                Console.WriteLine("\tYou don't have any light sensors selected, so choosing left/right will have no impact.");
                DisplayMenuPrompt("Alarm System");
                return "both";
            }

            // 'light' or 'both' selected
            do
            {
                DisplayScreenHeader("Set Sensors to Monitor");

                Console.WriteLine("\tThe light reading can be taken from the left, right, or an average of both sides.");
                Console.WriteLine("\tCurrent Sensor Readings:");

                // table header
                Console.WriteLine("\t\t" +
                    "Left".PadLeft(8) +
                    "Right".PadLeft(8) +
                    "Both".PadLeft(8)
                    );

                // current readings
                int leftSensor = finchRobot.getLeftLightSensor();
                int rightSensor = finchRobot.getRightLightSensor();
                Console.WriteLine("\t\t" +
                    $"{leftSensor}".PadLeft(8) +
                    $"{rightSensor}".PadLeft(8) +
                    $"{(leftSensor + rightSensor) / 2}".PadLeft(8)
                    );
                

                Console.Write("\tPlease enter the sensors you'd like to monitor {0}: ", choices);
                userChoice = Console.ReadLine().Trim().ToLower();

                switch (userChoice)
                {

                    case "left":
                    case "right":
                    case "both":
                        validResponse = true;
                        break;

                    default:
                        Console.WriteLine("\n\tPlease enter one of {0} for your choice.", choices);
                        DisplayContinuePrompt();
                        break;
                }

            } while (!validResponse);

            Console.WriteLine("\n\tGot it! The Finch will monitor {0} sensors.", userChoice);
            DisplayMenuPrompt("Alarm System");
            return userChoice;
        }

        static string AlarmSystemDisplaySetRangeType()
        {
            bool validResponse = false;
            string choices = "[min, max]";
            string userChoice;

            do
            {
                DisplayScreenHeader("Set Monitor Type");

                Console.WriteLine("\tThe alarm will sound when a certain threshold is met." +
                    "\n\tThis can be a minimum value, or a maximum value.");
                Console.Write("\tPlease enter the type of threshold you'd like {0}: ", choices);
                userChoice = Console.ReadLine().Trim().ToLower();

                switch (userChoice)
                {

                    case "min":
                    case "max":
                        validResponse = true;
                        break;

                    default:
                        Console.WriteLine("\n\tPlease enter one of {0} for your choice.", choices);
                        DisplayContinuePrompt();
                        break;
                }

            } while (!validResponse);

            Console.WriteLine("\n\tGot it! The Finch will watch for a {0} reading.", userChoice);
            DisplayMenuPrompt("Alarm System");
            return userChoice;
        }

        static (int tempThresholdValue, int lightThresholdValue) AlarmSystemDisplaySetThreshold(string monitorType)
        {

            bool validResponse = false;
            int userTempValue = 0;
            int userLightValue = 0; // default values in case they are not needed
            
            if (monitorType == "temp" || monitorType == "both")
            {
                // get temp threshold
                do
                {
                    DisplayScreenHeader("Set Temp Threshold");

                    Console.WriteLine("\tTemperature will be read as Fahrenheit (F).");
                    // a specified range here will keep inputs sane, while still allowing extremes beyond what the Finch
                    //     could even withstand.
                    Console.Write("\tFor the temp threshold, please enter an integer between -20 and 200: ");
                    if (int.TryParse(Console.ReadLine(), out userTempValue) && userTempValue >= -20 && userTempValue <= 200)
                    {
                        validResponse = true;

                    } else
                    {
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter an integer between -20 and 200.");
                        DisplayContinuePrompt();
                    }

                } while (!validResponse);

                validResponse = false; // reset variable for reuse in next operation
                Console.WriteLine();
                Console.WriteLine("\tOK! The threshold for temperature will be {0}.", userTempValue);
                DisplayContinuePrompt();
            }

            if (monitorType == "light" || monitorType == "both")
            {
                // get light threshold
                do
                {
                    DisplayScreenHeader("Set Light Threshold");

                    Console.WriteLine("\tThe light sensors can read light values up to 255.");
                    // a specified range here will keep inputs within Finch's capabilities
                    Console.Write("\tFor the light threshold, please enter an integer between 0 and 255: ");
                    if (int.TryParse(Console.ReadLine(), out userLightValue) && userTempValue >= 0 && userTempValue <= 255)
                    {
                        validResponse = true;

                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter an integer between -20 and 200.");
                        DisplayContinuePrompt();
                    }

                } while (!validResponse);

                Console.WriteLine();
                Console.WriteLine("\tOK! The threshold for light will be {0}.", userLightValue);
                DisplayContinuePrompt();
            }

            return (tempThresholdValue: userTempValue, lightThresholdValue: userLightValue);
        }

        static int AlarmSystemDisplaySetTimeToMonitor()
        {
            bool validResponse = false;
            int userTime;

            do
            {
                DisplayScreenHeader("Set Time to Monitor");

                Console.WriteLine("\tThe alarm will run up to specified time unless a threshold is reached.");
                Console.Write("\tPlease enter the time to monitor (in whole seconds): ");
                // the only requirement here is a positive time, so > 0
                if (int.TryParse(Console.ReadLine(), out userTime) && userTime > 0)
                {
                    validResponse = true;
                } else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a positive whole number for the time.");
                    DisplayContinuePrompt();
                }
            } while (!validResponse);

            Console.WriteLine();
            Console.WriteLine("\tUnderstood. The Finch Alarm will operate for {0} second(s).", userTime);
            DisplayMenuPrompt("Alarm System");
            return userTime;
        }

        static void AlarmSystemDisplaySetAlarm(Finch finchRobot, string monitorType, string sensorsToMonitor, string rangeType, int tempThresholdValue, int lightThresholdValue, int timeToMonitor)
        {

            DisplayScreenHeader("Set Alarm");


            Console.Write("\tMonitor Type: ");
            if (monitorType == "both")
                Console.WriteLine("Temp & Light");
            else
                Console.WriteLine(Capitalize(monitorType));
            if (monitorType != "temp")
                Console.WriteLine("\tLight sensors to Monitor: {0}", Capitalize(sensorsToMonitor));
            Console.WriteLine("\tRange Type: {0}", Capitalize(rangeType));
            if (monitorType != "light")
                Console.WriteLine("\tTemperature Threshold (F): {0}", tempThresholdValue);
            if (monitorType != "temp")
                Console.WriteLine("\tLight Level Threshold: {0}", lightThresholdValue);
            Console.WriteLine("\tTime to monitor (in seconds): {0}", timeToMonitor);

            Console.WriteLine("\n\tThe Finch Robot is ready to begin monitoring.");
            DisplayContinuePrompt();

            Console.CursorVisible = false;

            // build the interface for displaying current values
            int[] timeElapsedPos = { 0, 0 };
            int[] tempPos = { 0, 0 };
            int[] lightPos = { 0, 0 };
            int[] continuePos = { 0, 0 };

            Console.WriteLine();
            Console.Write("\t\t\tTime elapsed: ");
            timeElapsedPos[0] = Console.CursorLeft;
            timeElapsedPos[1] = Console.CursorTop;
            if (monitorType != "light")
            {
                Console.WriteLine();
                Console.Write("\t\t\tCurrent Temp: ");
                tempPos[0] = Console.CursorLeft;
                tempPos[1] = Console.CursorTop;
            }
            if (monitorType != "temp")
            {
                Console.WriteLine();
                Console.Write("\t\t\tCurrent Light: ");
                lightPos[0] = Console.CursorLeft;
                lightPos[1] = Console.CursorTop;
            }
            Console.WriteLine("\n");
            continuePos[0] = Console.CursorLeft;
            continuePos[1] = Console.CursorTop;

            int timeElapsed = 0;
            int thresholdExceeded = 0; // 0 = not exceeded, 1 = temp exceeded, 2 = light exceeded
            int currentTemp = 0;
            int currentLight = 0;
            while (thresholdExceeded == 0 && timeElapsed < timeToMonitor)
            {
                timeElapsed += 1;
                Console.SetCursorPosition(timeElapsedPos[0], timeElapsedPos[1]);
                Console.Write(timeElapsed);

                if (monitorType == "temp" || monitorType == "both")
                {
                    // check, process, and display temperature
                    // check
                    currentTemp = (int)CelsiusToFahrenheit(finchRobot.getTemperature());
                    
                    //process
                    switch (rangeType)
                    {
                        case "min":
                            if (currentTemp <= tempThresholdValue)
                                thresholdExceeded = 1;
                            break;

                        case "max":
                            if (currentTemp >= tempThresholdValue)
                                thresholdExceeded = 1;
                            break;
                    }

                    // display
                    Console.SetCursorPosition(tempPos[0], tempPos[1]);
                    Console.Write(currentTemp);
                }
                if (monitorType == "light" || monitorType == "both")
                {
                    // check, process, and display light
                    // check
                    switch (sensorsToMonitor)
                    {
                        case "left":
                            currentLight = finchRobot.getLeftLightSensor();
                            break;
                        case "right":
                            currentLight = finchRobot.getRightLightSensor();
                            break;

                        case "both":
                            currentLight = (finchRobot.getLeftLightSensor() + finchRobot.getRightLightSensor()) / 2;
                            break;
                    }

                    //process
                    switch (rangeType)
                    {
                        case "min":
                            if (currentLight <= lightThresholdValue)
                                thresholdExceeded = 2;
                            break;

                        case "max":
                            if (currentLight >= lightThresholdValue)
                                thresholdExceeded = 2;
                            break;
                    }

                    // display
                    Console.SetCursorPosition(lightPos[0], lightPos[1]);
                    Console.Write(currentLight);
                }
                // terminate without delay if a threshold is exceeded
                if (thresholdExceeded == 0)
                    finchRobot.wait(1000);
            }
            // now out of monitoring
            Console.SetCursorPosition(continuePos[0], continuePos[1]);

            if (thresholdExceeded > 0)
            {
                // stuff for when threshold exceeded
                // display which value, the range type
                string whichThreshold;
                int minMaxExceeded;
                int exceededValue;
                if (thresholdExceeded == 1)
                {
                    whichThreshold = "Temp";
                    minMaxExceeded = tempThresholdValue;
                    exceededValue = currentTemp;
                } else
                {
                    whichThreshold = "Light";
                    minMaxExceeded = lightThresholdValue;
                    exceededValue = currentLight;
                }

                Console.WriteLine($"\tThe {whichThreshold} threshold with a {rangeType} of {minMaxExceeded} " +
                    $"was exceeded by a value of {exceededValue}.");
                PlayNote(finchRobot, 500, 1.5);
            } else
            {
                // alarm timed out
                Console.WriteLine("\tThreshold(s) not exceeded.");
            }

            Console.CursorVisible = true;
            DisplayMenuPrompt("Alarm System");
        }

        static string Capitalize(string word)
        {
            return char.ToUpper(word[0]) + word.Substring(1);
        }

        #endregion

        #region USER PROGRAMMING

        static void UserProgrammingDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            // create directory for FileIO (will skip if already exists)
            Directory.CreateDirectory("Data");

            // variables for user programming module configuration
            (int motorSpeed, int ledBrightness, double waitSeconds, int soundFrequency) 
                commandParameters = (255, 255, 1.0, 300);
            List<(Command action, int value)> commands = new List<(Command action, int value)>();

            // menu loop
            do
            {
                DisplayScreenHeader("User Programming Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Command Parameters");
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Execute Commands");
                Console.WriteLine("\te) Reset Commands List");
                Console.WriteLine("\tf) Save Commands to File");
                Console.WriteLine("\tg) Load commands from File");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice: ");
                menuChoice = Console.ReadLine().ToLower().Trim();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        commandParameters = UserProgrammingDisplaySetCommandParameters();
                        break;

                    case "b":
                        commands = UserProgrammingDisplayAddCommands(commands, commandParameters);
                        break;

                    case "c":
                        UserProgrammingDisplayViewCommands(commands);
                        break;

                    case "d":
                        UserProgrammingDisplayExecuteCommands(finchRobot, commands);
                        break;

                    case "e":
                        commands = UserProgrammingDisplayResetCommands();
                        break;

                    case "f":
                        FileIODisplaySaveCommands(commands);
                        break;

                    case "g":
                        commands = FileIODisplayLoadCommands(commands);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static (int motorSpeed, int ledBrightness, double waitSeconds, int soundFrequency) UserProgrammingDisplaySetCommandParameters()
        {
            DisplayScreenHeader("Command Parameters");

            // create defaults
            (int motorSpeed, int ledBrightness, double waitSeconds, int soundFrequency)
                commandParameters = (255, 255, 1.0, 300);

            GetValidInteger("Enter Motor Speed", 1, 255, out commandParameters.motorSpeed);
            GetValidInteger("Enter LED Brightness", 1, 255, out commandParameters.ledBrightness);
            GetValidDouble("Enter Wait in Seconds", 1, 10, out commandParameters.waitSeconds);
            GetValidInteger("Enter Sound Frequency (Hz)", 50, 2000, out commandParameters.soundFrequency);

            // echo values back
            Console.WriteLine("\n");
            Console.WriteLine($"\tMotor speed: {commandParameters.motorSpeed}");
            Console.WriteLine($"\tLED Brightness: {commandParameters.ledBrightness}");
            Console.WriteLine($"\tWait Command Duration: {commandParameters.waitSeconds}");
            Console.WriteLine($"\tSound Frequency (Hz): {commandParameters.soundFrequency}");

            DisplayMenuPrompt("User Programming");
            return commandParameters;
        }

        static List<(Command action, int value)> UserProgrammingDisplayAddCommands(List<(Command action, int value)> commands, (int motorSpeed, int ledBrightness, double waitSeconds, int soundFrequency) commandParameters)
        {
            DisplayScreenHeader("Add Commands");

            Command command = Command.NONE;

            // list available commands
            int commandCount = 1;
            Console.WriteLine("\tList of Available Commands\n");
            Console.Write("\t-");
            foreach (string commandName in Enum.GetNames(typeof(Command)))
            {
                Console.Write($"- {commandName.ToLower()}  -");
                if (commandCount % 5 == 0)
                    Console.Write("-\n\t-");
                ++commandCount;
            }
            Console.WriteLine();

            while (command != Command.DONE)
            {
                Console.Write("\tEnter Command: ");

                if (Enum.TryParse(Console.ReadLine().ToUpper(), out command))
                {
                    // command success

                    switch (command)
                    {
                        // commands that are controlled by param motorSpeed
                        case Command.MOVEFORWARD:
                        case Command.MOVEBACKWARD:
                        case Command.TURNRIGHT:
                        case Command.TURNLEFT:
                            commands.Add((action: command, value: commandParameters.motorSpeed));
                            break;

                        case Command.WAIT:
                            commands.Add((action: command, value: (int)(commandParameters.waitSeconds * 1000)));
                            break;

                        case Command.LEDON:
                            commands.Add((action: command, value: commandParameters.ledBrightness));
                            break;

                        case Command.SOUNDON:
                            commands.Add((action: command, value: commandParameters.soundFrequency));
                            break;

                        // commands not controlled by commandParameters
                        default:
                            commands.Add((action: command, value: -1));
                            break;
                    }
                } else
                {
                    // command fail
                    Console.WriteLine("\tPlease enter a valid command from the list above.\n");
                }
            }

            return commands;
        }

        static void UserProgrammingDisplayViewCommands(List<(Command action, int value)> commands)
        {
            DisplayScreenHeader("View Commands");

            foreach ((Command action, int value) command in commands)
            {
                if (command.value != -1)
                    Console.WriteLine($"\t\t{command.action} : {command.value}");
                else
                    Console.WriteLine($"\t\t{command.action}");
            }

            DisplayMenuPrompt("User Programming");
        }

        static void UserProgrammingDisplayExecuteCommands(Finch finchRobot, List<(Command action, int value)> commands)
        {
            DisplayScreenHeader("Execute Commands");

            Console.WriteLine("\tThe Finch will now execute your list of commands!");
            DisplayContinuePrompt();
            Console.WriteLine();

            double temp;
            Random rand = new Random();
            // iterate through commands
            foreach ((Command action, int value) command in commands)
            {
                // echo command to user as performed
                if (command.value != -1)
                    Console.WriteLine($"\t\t{command.action} : {command.value}");
                else
                    Console.WriteLine($"\t\t{command.action}");

                switch (command.action)
                {
                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(command.value, command.value);
                        break;

                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(-command.value, -command.value);
                        break;

                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        break;

                    case Command.WAIT:
                        finchRobot.wait(command.value);
                        break;

                    case Command.TURNRIGHT:
                        finchRobot.setMotors(command.value, -command.value/2);
                        break;

                    case Command.TURNLEFT:
                        finchRobot.setMotors(-command.value/2, command.value);
                        break;

                    case Command.LEDON:
                        finchRobot.setLED(command.value, command.value, command.value);
                        break;

                    case Command.LEDOFF:
                        finchRobot.setLED(0, 0, 0);
                        break;

                    case Command.GETTEMPERATURE:
                        temp = CelsiusToFahrenheit(finchRobot.getTemperature());
                        Console.WriteLine($"\t\tCURRENT TEMP: {temp:F1}");
                        break;

                    case Command.SOUNDON:
                        finchRobot.noteOn(command.value);
                        break;

                    case Command.SOUNDOFF:
                        finchRobot.noteOff();
                        break;

                    case Command.RANDLED:
                        finchRobot.setLED(rand.Next(255), rand.Next(255), rand.Next(255));
                        break;

                    case Command.RANDSOUND:
                        finchRobot.noteOn(rand.Next(2000));
                        break;

                    case Command.RANDLIGHTSOUND:
                        finchRobot.setLED(rand.Next(255), rand.Next(255), rand.Next(255));
                        finchRobot.noteOn(rand.Next(2000));
                        break;

                    case Command.DISCO:
                        // this one lasts 4 seconds total
                        finchRobot.setMotors(255, -255);
                        for (int i = 0; i <= 3; ++i)
                        {
                            if (i == 2)
                                finchRobot.setMotors(-255, 255);

                            finchRobot.setLED(rand.Next(255), rand.Next(255), rand.Next(255));
                            finchRobot.noteOn(rand.Next(2000));
                            finchRobot.wait(1000);
                        }
                        finchRobot.noteOff();
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.setMotors(0, 0);
                        break;
                }
            }

            // reset the finch after executing all commands
            finchRobot.noteOff();
            finchRobot.setLED(0, 0, 0);
            finchRobot.setMotors(0, 0);

            DisplayMenuPrompt("User Programming");
        }

        static List<(Command action, int value)> UserProgrammingDisplayResetCommands()
        {
            DisplayScreenHeader("Reset Commands List");

            Console.WriteLine("\tList of commands has been reset.");

            DisplayMenuPrompt("User Programming");
            return new List<(Command action, int value)>();
        }

        /// <summary>
        /// Helper method for getting user input as integer
        /// </summary>
        /// <param name="prompt">Console prompt the user will see</param>
        /// <param name="min">Minimum value for validation</param>
        /// <param name="max">Maximum value for validation</param>
        /// <param name="variable">Variable to store the validated integer</param>
        static void GetValidInteger(string prompt, int min, int max, out int variable)
        {
            bool validInput = false;

            do
            {
                Console.Write($"\t{prompt} [{min} - {max}]: ");
                if (int.TryParse(Console.ReadLine(), out variable) && variable >= min && variable <= max)
                {
                    validInput = true;
                } else
                {
                    Console.WriteLine($"\tYou must enter an integer value between {min} and {max}.");
                    Console.WriteLine();
                }
            } while (!validInput);
        }

        /// <summary>
        /// Helper method for getting user input as double
        /// </summary>
        /// <param name="prompt">Console prompt the user will see</param>
        /// <param name="min">Minimum value for validation</param>
        /// <param name="max">Maximum value for validation</param>
        /// <param name="variable">Variable to store the validated double</param>
        static void GetValidDouble(string prompt, double min, double max, out double variable)
        {
            bool validInput = false;

            do
            {
                Console.Write($"\t{prompt} [{min} - {max}]: ");
                if (double.TryParse(Console.ReadLine(), out variable) && variable >= min && variable <= max)
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine($"\tYou must enter a decimal value between {min} and {max}.");
                    Console.WriteLine();
                }
            } while (!validInput);
        }

        #endregion

        #region FILE I/O

        static void FileIODisplaySaveCommands(List<(Command action, int value)> commands)
        {
            bool fileSaved = false;
            bool saveSuccess;
            string filename;
            do
            {
                DisplayScreenHeader("Save Commands");

                Console.WriteLine("\tThis will save your current list of commands to a text file for later.");
                Console.WriteLine("\tFiles will be overwritten if they exist already.");
                Console.WriteLine("\tHere is a list of of your existing files:\n");
                FileIOListAllFiles();
                Console.WriteLine("\n\tWhat would you like to save this file as?");
                Console.WriteLine("\t(Press enter with no text to cancel)");
                Console.Write("\t\t- ");
                filename = Console.ReadLine();

                // guard clause to exit method if nothing is entered
                if (filename.Length < 1)
                {
                    Console.WriteLine("\tThe commands will not be saved at this time.");
                    DisplayMenuPrompt("User Programming");
                    return;
                }

                if (Regex.IsMatch(filename, "^[a-zA-Z0-9]*$"))
                {
                    saveSuccess = FileIOSaveCommandList(filename, commands);
                    if (saveSuccess)
                    {
                        Console.WriteLine("\tFile saved.");
                        fileSaved = true;
                    } else
                    {
                        Console.WriteLine($"\tThere was an error writing this file. Please try again.");
                        DisplayContinuePrompt();
                    }
                } else
                {
                    // filename entered not alphanumeric
                    Console.WriteLine("\tYou must enter a name using only letters and numbers.");
                    DisplayContinuePrompt();
                }

            } while (!fileSaved);

            DisplayMenuPrompt("User Programming");
        }

        static bool FileIOSaveCommandList(string filename, List<(Command action, int value)> commands)
        {
            try
            {
                string filepath = $"Data\\{filename}.txt";
                //File.Create(filepath);
                File.WriteAllLines(filepath, FileIOListToString(commands));
            } catch (Exception)
            {
                return false;
            }

            return true;
        }

        static string[] FileIOListToString(List<(Command action, int value)> commands)
        {
            string[] cmdArray = new string[commands.Count];
            int index = 0;
            foreach ((Command action, int value) command in commands)
            {
                cmdArray[index] = $"{command.action},{command.value}";
                ++index;
            }

            return cmdArray;
        }

        static FileInfo[] FileIOListAllFiles()
        {
            DirectoryInfo folder = new DirectoryInfo("Data");
            FileInfo[] files = folder.GetFiles("*.txt");
            int index = 1;
            foreach(FileInfo file in files)
            {
                Console.WriteLine($"\t\t{index}) {file.Name}");
                ++index;
            }

            return files;
        }

        static List<(Command action, int value)> FileIODisplayLoadCommands(List<(Command action, int value)> commands)
        {
            bool loadSuccess = false;
            FileInfo[] files;
            int userFileChoice;
            string loadStatus;
            List<(Command action, int value)> newCommands;
            do
            {
                DisplayScreenHeader("Load Commands");
                Console.WriteLine("\tThis will load a list of commands from a file.");
                Console.WriteLine("\tHere is a list of your previously saved files:\n");
                files = FileIOListAllFiles();
                // exit if no files found
                if (files.Length < 1)
                {
                    Console.WriteLine("\tIt appears you do not have any files saved.");
                    Console.WriteLine("Your current commands will not be overwritten.");
                    DisplayMenuPrompt("User Programming");
                    // return without overwriting existing commands
                    return commands;
                }
                Console.WriteLine("\n\tWould you like to continue loading a file? ([y]es/[n]o)");
                // get yes or no
                string userContinue;
                bool validResponse = false;
                do
                {
                    Console.Write("\t? ");
                    userContinue = Console.ReadLine().Trim().ToLower();
                    switch (userContinue)
                    {
                        case "y":
                        case "yes":
                            validResponse = true;
                            break;

                        case "n":
                        case "no":
                            validResponse = true;
                            // return current commands
                            Console.WriteLine("\tYour current commands will not be overwritten.");
                            DisplayMenuPrompt("User Programming");
                            return commands;

                        default:
                            Console.WriteLine("\tPlease type 'yes' or 'no'.");
                            break;
                    }
                } while (!validResponse);

                // int validation for file selection
                GetValidInteger("Enter a number", 1, files.Length, out userFileChoice);
                newCommands = FileIOLoadCommandList(files[userFileChoice - 1].Name, out loadStatus);

                if (loadStatus == "Success")
                {
                    // load success, return new commands
                    Console.WriteLine("\tCommands successfully loaded.");

                    // exit the loop
                    loadSuccess = true;
                } else
                {
                    Console.WriteLine($"\tError: {loadStatus}");
                    DisplayContinuePrompt();
                }

            } while (!loadSuccess);


            DisplayMenuPrompt("User Programming");
            return newCommands;
        }

        static List<(Command action, int value)> FileIOLoadCommandList(string filename, out string status)
        {
            string filepath = $"Data\\{filename}";
            List<(Command action, int value)> commands = new List<(Command action, int value)>();

            try
            {
                // load and iterate through command file
                string[] lines = File.ReadAllLines(filepath);
                string[] lineData;
                Command lineAction;
                int lineValue;
                foreach (string line in lines)
                {
                    // make sure line isn't empty
                    if (line.Length > 0)
                    {
                        // separate line by comma for command,action
                        lineData = line.Split(',');

                        if (Enum.TryParse(lineData[0], out lineAction) && int.TryParse(lineData[1], out lineValue))
                        {
                            commands.Add((action: lineAction, value: lineValue));
                        }
                    }
                }

                status = "Success";
            } catch (DirectoryNotFoundException)
            {
                status = "Could not locate the Data folder.";
            } catch (FileNotFoundException)
            {
                status = $"Could not locate the file - {filename}";
            } catch (Exception)
            {
                status = "Could not read the command file.";
            }

            return commands;
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
