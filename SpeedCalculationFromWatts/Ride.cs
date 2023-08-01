using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedCalculationFromWatts
{
    internal class Ride
    {
        private List<Segment> segments;

        public Ride()
        {
            segments = new List<Segment>();
        }

        internal static Ride BuildFromString(string? inputString, double totalWeight)
        {
            if (string.IsNullOrEmpty(inputString))
                throw new ArgumentException("Input is empty", "inputString");

            string[] splittedString = inputString.Split(',');

            if (splittedString.Length != (inputString.Count(c => c == '+') * 4 + inputString.Count(c => c == '-') * 3))
                throw new ArgumentException("Input is incorrect. The segments are not well described, feilds are missing.", "inputString");

            Ride ride = new Ride();
            int i = 0;
            while (i < splittedString.Length)
            {
                // +,Distance,Elevation,ExpectedAveragePower
                // -,Distance,ExpectedAverageSpeed
                double distance = double.Parse(splittedString[i + 1]);
                if (splittedString[i] == "+")
                {
                    double elevation = double.Parse(splittedString[i + 2]);
                    double expectedAveragePower = double.Parse(splittedString[i + 3]);
                    ride.segments.Add(Segment.Build(distance, elevation, expectedAveragePower, totalWeight));
                    i += 4;
                }
                else if (splittedString[i] == "-")
                {
                    double expectedAverageSpeed = double.Parse(splittedString[i + 2]);
                    ride.segments.Add(Segment.Build(distance, expectedAverageSpeed));
                    i += 3;
                }
                else
                {
                    throw new ArgumentException("Could not find segement prefix delimitor (+ or -)", "inputString");
                }
            }
            return ride;
        }

        public void PrintRide()
        {
            Console.Clear();
            Console.WriteLine("RIDE:");
            TimeSpan totalTime = new TimeSpan();
            foreach (Segment segment in segments)
            {
                segment.PrintSegmentDetails();
                Console.WriteLine();
                totalTime = totalTime.Add(segment.EstimatedTime);
            }
            Console.WriteLine("TOTAL:");
            Console.WriteLine($"Expected total time: {$"{totalTime.Hours.ToString("00")}h{totalTime.Minutes.ToString("00")}m{totalTime.Seconds.ToString("00")}s"}");
        }
    }
}
