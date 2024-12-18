//---------------------------------------------------------------------------------------
// TASK: Text file "L1_31.txt" stores information about cars.
// Create a program to solve the following tasks and
// display results to screen (console):
// 1. Find which car has least consumption of the fuel.
// If multiple cars with least consumption of fuel exist,
// display all information about the first one.
// 2. Find which car can carry the largest number of
// passengers. If multiple cars with largest number of
// passengers exist, display all information about the last one.
// 3. Decrease fuel consumption by 7% to cars that can
// carry number of passengers less than y
// (value entered from keyboard). Display contents
// of both collections after decrease procedure.
// 4. Find which student found a car that can carry
// the largest number of passengers. If multiple such
// students are found, display names
// and surnames of both students.
// 5. Create a new collection (deep copy) of cars from
// both lists, which have number of passengers lower
// than P(entered from keyboard).

using System;
using System.IO;

namespace LW1
{
    /// <summary>
    /// Primary class to execute required calculations tasks 
    /// </summary>
    internal class Program
    {
        const string file1 = "L1_31.txt";  // Filename for input data text file
        const string file2 = "L1_32.txt";  // Filename for input data text file
        const string fileToWrite = "Result.txt"; // Filename result text file
        const int arraySize = 100;  // Maximum number of cars
        static void Main(string[] args)
        {
            // Clear result file
            if (File.Exists(fileToWrite))
                File.Delete(fileToWrite);

            Car[] Car1 = new Car[arraySize];   // Object collection to store
                                               // data about cars
                                               // from first file
            Car[] Car2 = new Car[arraySize];   // Object collection to store
                                               // data about cars
                                               // from second file

            int numberOfCars1, numberOfCars2;   // Number of cars
                                                // in object collections
            string studentName1, studentName2;  // Students names

            ReadFile(file1, Car1, out numberOfCars1, out studentName1);
            ReadFile(file2, Car2, out numberOfCars2, out studentName2);

            PrintToFile(fileToWrite, "Initial data from file 1",
                Car1, numberOfCars1, studentName1);
            PrintToFile(fileToWrite, "Initial data from file 2",
                Car2, numberOfCars2, studentName2);

            PrintOneCarToFile(fileToWrite,
                "Information about the first car with least fuel consumption: ",
                Car1, LeastFuelConsumption(Car1, numberOfCars1));
            PrintOneCarToFile(fileToWrite,
                "Information about the last car with maximum number" +
                " of passengers: ",
                Car1, MaxNumPassengers(Car1, numberOfCars1));

            Console.WriteLine("");
            Console.WriteLine("Enter the value of y: ");
            string value = Console.ReadLine();  // Reading input data
                                                // y from the keyboard
            int yNumOfPass = int.Parse(value);  // Y value
            DecreasingFuelConsumption(Car1, yNumOfPass, numberOfCars1);
            PrintToFile(fileToWrite, "Decreased fuel consumptions in file 1",
                Car1, numberOfCars1);
            DecreasingFuelConsumption(Car2, yNumOfPass, numberOfCars2);
            PrintToFile(fileToWrite, "Decreased fuel consumptions in file 2",
                Car2, numberOfCars2);

            string name = TheLargestNumOfPass(file1, file2,  // Student's    
                Car1, Car2, numberOfCars1, numberOfCars2,    // name and surname
                studentName1, studentName2);

            using (StreamWriter wr = new StreamWriter(fileToWrite, true))
            {
                wr.WriteLine(name);
            }

            Car[] CarsCopy = new Car[arraySize];   // Object collection
                                                   // to store data
                                                   // about the cars with
                                                   // number of
                                                   // passengers lower than p

            int numCarsCopy = 0;  // Number of cars
                                  // in new object collection
            CarsCopy = NumberPassengersLessP(Car1, Car2,
                numberOfCars1, numberOfCars2, ref numCarsCopy);

            PrintToFile(fileToWrite, "Cars with number of passengers lower P "
                        , CarsCopy, CarsCopy.Length);
        }

        /// <summary>
        /// Reads data from intial data textfile to object collection
        /// </summary>
        /// <param name="filename">Filename for input data text file</param>
        /// <param name="Car">Object collection to store 
        /// data about the cars</param>
        /// <param name="numberOfCars">Number of cars in object collection</param>
        /// <param name="studentName">Student name</param>
        static void ReadFile(string filename, Car[] Car,
            out int numberOfCars, out string studentName)
        {
            string model;            // Car model
            int numberOfPassengers;  // Car number of passengers
            double fuelConsumption;  // Car fuel consumption
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;  // Line in text file
                line = reader.ReadLine();
                studentName = line;  // Student name
                string[] parts;
                line = reader.ReadLine();
                for (numberOfCars = 0; line != null; numberOfCars++)
                {
                    parts = line.Split(';');
                    model = parts[0];
                    numberOfPassengers = int.Parse(parts[1]);
                    fuelConsumption = double.Parse(parts[2]);
                    Car[numberOfCars] = new Car(model, numberOfPassengers,
                        fuelConsumption);
                    line = reader.ReadLine();
                }
            }
        }

        /// <summary>
        /// Prints car object collection data to a file using table format
        /// </summary>
        /// <param name="fileToWrite">Result filename</param>
        /// <param name="heading">Label above table</param>
        /// <param name="Car">Object collection to store data about cars</param>
        /// <param name="numberOfCars">Number of cars in 
        /// object collection</param>
        /// <param name="studentName">Student name</param>
        static void PrintToFile(string fileToWrite, string heading, Car[] Car,
            int numberOfCars, string studentName = null)
        {
            using (StreamWriter wr = new StreamWriter(fileToWrite, true))
            {
                wr.WriteLine(heading);
                if (studentName != null)
                    wr.WriteLine(studentName);
                wr.WriteLine("--------------------------------------------");
                wr.WriteLine("|   Model   | Passengers | Fuel Consumption |");
                wr.WriteLine("--------------------------------------------");
                for (int i = 0; i < numberOfCars - 1 && Car[i] != null; i++)
                {
                    wr.WriteLine("| {0,9} | {1,10:d} | {2,15:f2} |",
                        Car[i].GetModel(),
                        Car[i].GetNumberOfPassengers(),
                        Car[i].GetFuelConsumption());
                }
                wr.WriteLine("--------------------------------------------");
                wr.WriteLine();
            }
        }

        /// <summary>
        /// Prints car object collection data to a file using table format
        /// </summary>
        /// <param name="fileToWrite">Result filename</param>
        /// <param name="heading">Label above table</param>
        /// <param name="Car">Object collection to store data about cars</param>
        /// <param name="dataAboutCar">Index of car with specific data</param>
        static void PrintOneCarToFile(string fileToWrite,
            string heading, Car[] Car, int dataAboutCar)
        {
            using (StreamWriter wr = new StreamWriter(fileToWrite, true))
            {
                wr.WriteLine(heading, "\n");
                wr.WriteLine("--------------------------------------------");
                wr.WriteLine("|   Model   | Passengers | Fuel Consumption |");
                wr.WriteLine("--------------------------------------------");

                wr.WriteLine("| {0,9} | {1,10:d} | {2,15:f2} |",
                    Car[dataAboutCar].GetModel(),
                    Car[dataAboutCar].GetNumberOfPassengers(),
                    Car[dataAboutCar].GetFuelConsumption());

                wr.WriteLine("--------------------------------------------");
                wr.WriteLine();
            }
        }

        /// <summary>
        /// Finds the car with the least fuel consumption in object collection
        /// </summary>
        /// <param name="Car">Object collection to store data about cars</param>
        /// <param name="numberOfCars">Number of cars in object collection</param>
        /// <returns> Index of car with least fuel consumption</returns>
        static int LeastFuelConsumption(Car[] Car, int numberOfCars)
        {
            int indCarLeastFuelConsumption = 1;  // Index of car with
                                                 // the least fuel consumption
            double leastFuelConsumption = Car[0].GetFuelConsumption();
            for (int i = 1; i < numberOfCars; i++)
            {
                if (leastFuelConsumption > Car[i].GetFuelConsumption())
                {
                    leastFuelConsumption = Car[i].GetFuelConsumption();
                    indCarLeastFuelConsumption = i;
                }
            }
            return indCarLeastFuelConsumption;
        }

        /// <summary>
        /// Counts maximum number of passengers in object collection
        /// </summary>
        /// <param name="Car">Object collection to store data about cars</param>
        /// <param name="numberOfCars">Number of cars in object collection</param>
        /// <param name="indMaxNumPassengers">Index of car with 
        /// maximum number of passengers</param>
        /// <returns> Index of car with maximum number of passengers</returns>
        static int MaxNumPassengers(Car[] Car, int numberOfCars,
            int indMaxNumPassengers = 0)
        {
            double maxNumPassengers = Car[0].GetNumberOfPassengers();
            for (int i = 1; i < numberOfCars; i++)
            {
                if (maxNumPassengers <= Car[i].GetNumberOfPassengers())
                {
                    maxNumPassengers = Car[i].GetNumberOfPassengers();
                    indMaxNumPassengers = i;
                }
            }
            return indMaxNumPassengers;
        }

        /// <summary>
        /// Decreases fuel consumption in cars that can carry 
        /// number of passengers less than y
        /// </summary>
        /// <param name="Car">Object collection to store data about cars</param>
        /// <param name="yNumOfPass">Number of passengers</param>
        /// <param name="numberOfCars">Number of cars in object collection</param>
        static void DecreasingFuelConsumption(Car[] Car, int yNumOfPass,
            int numberOfCars)
        {
            for (int i = 0; i < numberOfCars; i++)
            {
                if (Car[i].GetNumberOfPassengers() < yNumOfPass)
                {
                    Car[i].SetFuelConsumption(Car[i].GetFuelConsumption()
                        * 93 / 100);
                }
            }
        }

        /// <summary>
        /// Finds which student found a car that can carry 
        /// the largest number of passengers
        /// </summary>
        /// <param name="filename1">Filename for input data text file</param>
        /// <param name="filename2">Filename for input data text file</param>
        /// <param name="Car1">Object collection to store data about cars</param>
        /// <param name="Car2">Object collection to store data about cars</param>
        /// <param name="numberOfCars1">Number of cars in 
        /// object collection</param>
        /// <param name="numberOfCars2">Number of cars in 
        /// object collection</param>
        /// <returns> Students, who have the largest 
        /// number of passengers in their cars</returns>
        static string TheLargestNumOfPass(string filename1, string filename2,
            Car[] Car1, Car[] Car2, int numberOfCars1, int numberOfCars2,
            string studentName1, string studentName2)
        {
            if (Car1[MaxNumPassengers
                (Car1, numberOfCars1)].GetNumberOfPassengers() >
                Car2[MaxNumPassengers
                (Car2, numberOfCars2)].GetNumberOfPassengers())
            {
                return "This student " + studentName1 +
                    " has the largest number of passengers in his car(s)\n";
            }
            else if (Car1[MaxNumPassengers
                (Car1, numberOfCars1)].GetNumberOfPassengers() <
                Car2[MaxNumPassengers
                (Car2, numberOfCars2)].GetNumberOfPassengers())
            {
                return "This student " + studentName2 +
                    " has the largest number of passengers in his car(s)\n";
            }
            else
            {
                return "These students " + studentName1 + ", " + studentName2 +
                    " have the largest number of passengers in their cars\n";
            }
        }

        /// <summary>
        /// Creates new collection of cars from both lists, 
        /// which have number of passengers lower than P
        /// </summary>
        /// <param name="Car1">Object collection to store data about cars</param>
        /// <param name="Car2">Object collection to store data about cars</param>
        /// <param name="pInt">Number of passengers 
        /// <param name="numberOfCars1">Number of cars in 
        /// object collection</param>
        /// <param name="numberOfCars2">Number of cars in 
        /// object collection</param>
        /// <param name="numCarsCopy">Number of cars in 
        /// new object collection</param>
        /// (entered from keyboard)</param>
        /// <returns>New collection of cars from both lists, 
        /// which have number of passengers 
        /// lower than P</returns>
        static Car[] NumberPassengersLessP(Car[] Car1, Car[] Car2, 
            int numberOfCars1, int numberOfCars2, ref int numCarsCopy)
        {
            Car[] CarsCopy = new Car[arraySize];
            Console.WriteLine("Write number of passengers p: ");
            int pInt = int.Parse(Console.ReadLine());  // Number of passengers
                                                       // (from keyboard)

            for (int i = 0; i < CarsCopy.Length &&
                (numberOfCars1 > i || numberOfCars2 > i); i++)
            {
                if (numberOfCars1 > i && Car1[i].GetNumberOfPassengers() < pInt)
                {
                    CarsCopy[numCarsCopy] = new Car(Car1[i].GetModel(),
                        Car1[i].GetNumberOfPassengers(),
                        Car1[i].GetFuelConsumption());
                    numCarsCopy++;
                }
                if (numberOfCars2 > i && Car2[i].GetNumberOfPassengers() < pInt)
                {
                    CarsCopy[numCarsCopy] = new Car(Car2[i].GetModel(),
                        Car2[i].GetNumberOfPassengers(),
                        Car2[i].GetFuelConsumption());
                    numCarsCopy++;
                }
            }
            return CarsCopy;
        }
    }
    class Car
    {
        private string model;  //Car's model
        private int numberOfPassengers;  //Car's number of passengers
        private double fuelConsumption;  //Car's fuel consumption

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="model">Car model</param>
        /// <param name="numberOfPassengers">Car number of passengers</param>
        /// <param name="fuelConsumption">Car fuel consumption</param>
        public Car(string model, 
            int numberOfPassengers, 
            double fuelConsumption)
        {
            this.model = model;
            this.numberOfPassengers = numberOfPassengers;
            this.fuelConsumption = fuelConsumption;
        }
        /// <summary>
        /// Constructor without parameters (default)
        /// </summary>
        public Car()
        {
            model = "";
            numberOfPassengers = 0;
            fuelConsumption = 0.0;
        }
        /// <summary>
        /// Returns car model
        /// </summary>
        /// <returns>Car model</returns>
        public string GetModel()
        {
            return model;
        }
        /// <summary>
        /// Returns car number of passengers
        /// </summary>
        /// <returns>Car number of passengers</returns>
        public int GetNumberOfPassengers()
        {
            return numberOfPassengers;
        }
        /// <summary>
        /// Returns car fuel consumption
        /// </summary>
        /// <returns>Car fuel consumption</returns>
        public double GetFuelConsumption()
        {
            return fuelConsumption;
        }
        /// <summary>
        /// Sets car model
        /// </summary>
        /// <param name="model">Car model</param>
        public void SetModel(string model)
        {
            this.model = model;
        }
        /// <summary>
        /// Sets car number of passengers
        /// </summary>
        /// <param name="numberOfPassengers">Number of passengers</param>
        public void SetNumberOfPassengers(int numberOfPassengers)
        {
            this.numberOfPassengers = numberOfPassengers;
        }
        /// <summary>
        /// Sets car fuel consumption
        /// </summary>
        /// <param name="fuelConsumption">Fuel consumption</param>
        public void SetFuelConsumption(double fuelConsumption)
        {
            this.fuelConsumption = fuelConsumption;
        }
    }
}