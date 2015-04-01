using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAD.Core.Data;

namespace SAD.Core.IO
{
    public abstract class FileReader
    {
        public abstract void ExtractTargetData();
    }

    public enum FRType
    {
        INIReader,
        JSONReader,
        XMLReader
    }

    public class INIReader : FileReader
    {
        private string filePath;
        private TargetManager targetManager = TargetManager.GetInstance();
        private string inputString;
        private string[] names = { "0", "0", "0" };
        private string[] xValue = { "0", "0", "0" };
        private string[] yValue = { "0", "0", "0" };
        private string[] zValue = { "0", "0", "0" };
        private string[] friendValue = { "0", "0", "0" };
        private string[] pointValue = { "0", "0", "0" };
        private string[] flashValue = { "0", "0", "0" };
        private string[] spawnValue = { "0", "0", "0" };
        private string[] swapValue = { "0", "0", "0" };

        public INIReader(string filepass)
        {
            filePath = filepass.ToLower();
            inputString = string.Empty;
            ExtractTargetData();
        }
        public override void ExtractTargetData()
        {
            targetManager.TargetList = new List<Targets>();
            string[] lines = System.IO.File.ReadAllLines(filePath);
            char[] delimiterChar = { '=', '#' };
            foreach (string line in lines)
            {
                try
                {
                    if (line.Contains("[") && line != "[Target]") //check to make sure target file is valid
                    {
                        throw new System.ArgumentException();
                    }
                }
                catch
                {
                    Console.WriteLine("Error! Invalid format tags! Please exit the program and fix the file!");
                    Environment.Exit(1);
                }
                if (!line.Contains("[Target]")) //When the line does not contain target
                {                               //they will be read and put into the list
                    if (line.StartsWith("Name="))
                    { //when line stats with "Name=", delimit, and add the Name to a list
                        inputString = line;
                        names = inputString.Split(delimiterChar); //split the string by =.
                        if (names[1] == "") // if no name is specified thats an error
                        {
                            Console.WriteLine("Error! Target name not found! Please exit the program and fix the file!");
                            Environment.Exit(1);
                        }
                        else
                        {
                            //check to make sure there are no spaces
                            try
                            {
                                string nameCheck = names[1];
                                bool fHasSpace = nameCheck.Contains(" ");
                                if (fHasSpace == true) //if the name has a space throw an exception
                                {
                                    throw new System.ArgumentException();
                                }
                                else
                                {
                                    //Console.WriteLine("Target Name successfully extracted.");
                                }
                            }
                            catch (ArgumentException)
                            {
                                Console.WriteLine("Error! Target names cannot contain spaces.");
                                return;
                            }

                        }

                    }
                    else if (line.StartsWith("X="))
                    { //when line stats with "X=", and delimit it
                        inputString = line;
                        xValue = inputString.Split(delimiterChar);
                        //Console.WriteLine("X coordinate successfully extracted.");
                    }
                    else if (line.StartsWith("Y="))
                    { //when line stats with "Y=", and delimit it
                        inputString = line;
                        yValue = inputString.Split(delimiterChar);
                        //Console.WriteLine("Y coordinate successfully extracted.");
                    }
                    else if (line.StartsWith("Z="))
                    { //when line stats with "Z=", and delimit it
                        inputString = line;
                        zValue = inputString.Split(delimiterChar);
                        //Console.WriteLine("Z coordinate successfully extracted.");
                    }
                    else if (line.StartsWith("Friend="))
                    { //when line stats with "Friend=", and delimit it
                        inputString = line;
                        friendValue = inputString.Split(delimiterChar);
                        //Console.WriteLine("Friend value successfully extracted.");
                    }
                    else if (line.StartsWith("Points="))
                    { //when line stats with "Points=", and delimit it
                        inputString = line;
                        pointValue = inputString.Split(delimiterChar);
                        //Console.WriteLine("Point value successfully extracted.");
                    }
                    else if (line.StartsWith("FlashRate="))
                    { //when line stats with "Points=", and delimit it
                        inputString = line;
                        flashValue = inputString.Split(delimiterChar);
                        //Console.WriteLine("Flash Rate successfully extracted.");
                    }
                    else if (line.StartsWith("SpawnRate="))
                    { //when line stats with "Points=", and delimit it
                        inputString = line;
                        spawnValue = inputString.Split(delimiterChar);
                        //Console.WriteLine("Spawn Rate successfully extracted.");
                    }
                    else if (line.StartsWith("CanSwapSidesWhenHit="))
                    { //when line stats with "CanSwapSidesWhenHit=", and delimit it
                        inputString = line;
                        swapValue = inputString.Split(delimiterChar);
                        //Console.WriteLine("Swap value successfully extracted.");
                    }
                    else if (line.StartsWith("#"))
                    { //when line starts with "#"
                        //it is a comment, ignore
                    }
                    else //when line is a tag, put all the data for the previous target into a Targets object.
                    {
                        targetManager.TargetList.Add(new Targets() // add all the extracted data to the list
                        {
                            TargetName = names[1],
                            X = double.Parse(xValue[1]), //have to do this all at the same time
                            Y = double.Parse(yValue[1]), //so it is in the same Targets object
                            Z = double.Parse(zValue[1]),
                            IsFriend = Convert.ToBoolean(friendValue[1]),
                            Points = int.Parse(pointValue[1]),
                            FlashRate = int.Parse(flashValue[1]),
                            SpawnRate = int.Parse(spawnValue[1]),
                            CanSwapSidesWhenHit = Convert.ToBoolean(swapValue[1]),
                            Status = "Still At Large"
                        });
                    }
                }
            }
            //when done reading file we still have to add the last target
            targetManager.TargetList.Add(new Targets()
            {
                TargetName = names[1],
                X = double.Parse(xValue[1]),
                Y = double.Parse(yValue[1]),
                Z = double.Parse(zValue[1]),
                IsFriend = Convert.ToBoolean(friendValue[1]),
                Points = int.Parse(pointValue[1]),
                FlashRate = int.Parse(flashValue[1]),
                SpawnRate = int.Parse(spawnValue[1]),
                CanSwapSidesWhenHit = Convert.ToBoolean(swapValue[1]),
                Status = "Still At Large"
            });

        }
        public void Print()
        {
            foreach (var targets in targetManager.TargetList) //iterate through the list
            {
                Console.WriteLine("Name={0}",targets.TargetName);
                Console.WriteLine("X={0}", targets.X);
                Console.WriteLine("Y={0}", targets.Y);
                Console.WriteLine("Z={0}", targets.Z);
                Console.WriteLine("Friend={0}", targets.IsFriend);
                Console.WriteLine("Points={0}", targets.Points);
                Console.WriteLine("FlashRate={0}", targets.FlashRate);
                Console.WriteLine("SpawnRate={0}", targets.SpawnRate);
                Console.WriteLine("CanSwapSidesWhenHit={0}", targets.CanSwapSidesWhenHit);
                Console.WriteLine("Status: {0}\n", targets.Status);
            }
        }
    }
}
