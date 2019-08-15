using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxCoverageCalc
{
    class Program
    {
        static string inputPath = @"../../../files/";
        static int numberOfLifeguards;
        static char timeUnitSeparator = ' ';

        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(inputPath, "*.in");
            List<Lifeguard> sortedLifeguards = new List<Lifeguard>();
            foreach (string file in files) {
                FileInfo fi = new FileInfo(file);
                Console.WriteLine("Processing file:" + fi.Name);
                sortedLifeguards = File2List(fi);
                Console.WriteLine("Lifeguards #:" + sortedLifeguards.Count);
                int max = GetMaxCoverage(sortedLifeguards);
                Console.WriteLine("Max Coverage :" + max);
                Save2File(fi, max);
                
            }
            Console.ReadKey();
        }

        static void Save2File(FileInfo inputFile, int maxCoverage)
        {
            string filename = Path.GetFileNameWithoutExtension(inputFile.Name) + ".out";
            string outputFile = inputFile.DirectoryName + Path.DirectorySeparatorChar + filename;

            using (StreamWriter file = new StreamWriter(outputFile))
            {
                file.WriteLine(maxCoverage);
            }

        }
        // This function will calculate the non overlapping hours for each
        // lifeguard. To get the maximum coverage, this function will substract
        // the minimum non-overlapping hours from the total of hours. 
        static int GetMaxCoverage(List<Lifeguard> lifeguards)
        {
            List<int> uniqueTimeUnits = new List<int>();

            int index = 0, shiftHours = 0;
            int previousEndTime = 0;
            int contiguousTimeUnits = 0;
            int nonOverlappingHours;

            foreach (Lifeguard lifeguard in lifeguards)
            {
                shiftHours = lifeguard.EndTimeUnit - lifeguard.StartTimeUnit;
                if (index == 0) //initialize values
                {
                    uniqueTimeUnits.Add(shiftHours);
                    contiguousTimeUnits = shiftHours;
                    previousEndTime = lifeguard.EndTimeUnit;
                }
                else
                {
                    if (lifeguard.StartTimeUnit < previousEndTime)
                    {
                        nonOverlappingHours = shiftHours - (previousEndTime - lifeguard.StartTimeUnit);
                        // Contiguous time units of the current lifeguard without overlap
                        uniqueTimeUnits.Add(nonOverlappingHours);
                        // Need to update the previous shift by substracting 
                        // the overlapping hours from it 
                        int previousHours = uniqueTimeUnits[index - 1] - (previousEndTime - lifeguard.StartTimeUnit);
                        uniqueTimeUnits[index - 1] = previousHours < 0 ? 0 : previousHours;
                        contiguousTimeUnits += nonOverlappingHours;
                        previousEndTime = lifeguard.EndTimeUnit;
                    }
                    else
                    {
                        uniqueTimeUnits.Add(shiftHours);
                        contiguousTimeUnits += shiftHours;
                    }
                }
                index++;
            }
     
            return contiguousTimeUnits - uniqueTimeUnits.Min();
        }

        static IEnumerable<string> Read(FileInfo file)
        {
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader reader =
                new System.IO.StreamReader(file.FullName);

            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }

            reader.Close();
        }

        static List<Lifeguard> File2List(FileInfo file)
        {
            int counter = 0;
            List<Lifeguard> lifeguards = new List<Lifeguard>();

            foreach (string line in Read(file))
            {
                if (counter == 0)
                {
                    numberOfLifeguards = int.Parse(line);
                }
                else
                {
                    string[] timeUnits = line.Split(timeUnitSeparator);

                    if (int.TryParse(timeUnits[0], out int start) &&
                        int.TryParse(timeUnits[1], out int end))
                    {
                        Lifeguard lifeguard = new Lifeguard(counter - 1);
                        lifeguard.StartTimeUnit = start;
                        lifeguard.EndTimeUnit = end;
                        lifeguards.Add(lifeguard);
                    }
                    else
                    {
                        throw new Exception("Cannot convert time units");
                    }
                    
                }
                counter++;
            }

            if (numberOfLifeguards != (counter - 1))
                throw new Exception("Number of lifeguards in file doesn't match");

            return lifeguards.OrderBy(x => x.StartTimeUnit).ToList();
        }
    }
}
