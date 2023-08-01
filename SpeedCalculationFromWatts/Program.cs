using System;
using System.Globalization;

namespace SpeedCalculationFromWatts
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MultiSegmentCalculation();
            Console.ReadLine();
        }

        public static void MultiSegmentCalculation()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            Console.Write("Enter Total weight (Kg): ");
            double weight = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter a sequence of segment details like the example: ");
            Console.WriteLine("\tEvery segment on the same line. Comma separated. \n\t\"+,Distance(km),Elevation(m),ExpectedAveragePower(W)\" for uphill/flat, \n\t\"-,Distance(km),ExpectedAverageSpeed(km/h)\" for downhill");
            string inputString = Console.ReadLine();

            Ride ride = Ride.BuildFromString(inputString, weight);
            ride.PrintRide();
        }

        public static void OneSegmentLoop()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            Console.WriteLine("SEGEMENT DETAILS: ");
            Console.Write("Distance (km): ");
            double distance = double.Parse(Console.ReadLine());
            Console.Write("Total weight (Kg): ");
            double weight = double.Parse(Console.ReadLine());
            Console.Write("Slope gradient (%) [Empty to provide elevation instead]: ");
            string gradientString = Console.ReadLine();
            double gradient;
            if (string.IsNullOrEmpty(gradientString))
            {
                Console.Write("Elevation: ");
                double elevation = double.Parse(Console.ReadLine());
                gradient = 100.0 * (elevation / (distance * 1000));
                Console.WriteLine($"(gradient: {gradient.ToString("#.#")}%)");
            }
            else
            {
                gradient = double.Parse(gradientString);
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"SEGEMENT DETAILS:\nDistance: {distance}km\nGradient: {gradient.ToString("#.#")}%\nTotal Weight: {weight}\n");
                Console.Write("Average Power (W): ");
                double power = double.Parse(Console.ReadLine());

                Console.Write("Estimated Time: ");
                PrintTimeSpan(CalculateTimeSpan(distance * 1000, CalculateSpeed(power, weight, gradient / 100)));
                Console.ReadKey();
            }
        }

        public static double CalculateSpeed(double power, double weight, double gradient)
        {
            // http://bernard.mischler.free.fr/equacycle/exemple.htm
            double c1 = 0.086;
            double c2 = 0.0981;
            double c3 = 9.81;

            double a = c1;
            double b = c2 * weight;
            double c = c3 * weight * gradient;
            double d = power;

            // Wolframe alpha magic
            double bigSquareNumber = Math.Cbrt((27 * a * a * d) + Math.Sqrt((729 * a * a * a * a * d * d) + (108 * a * a * a * Math.Pow(b + c, 3))));

            double member1 = (0.26457 * bigSquareNumber) / a;
            double member2 = (1.2599 * (b + c)) / bigSquareNumber;
            double speed = member1 - member2;

            return speed;
        }

        public static TimeSpan CalculateTimeSpan(double distance, double speed)
        {
            return new TimeSpan(0, 0, (int)(distance / speed));
        }

        public static void PrintTimeSpan(TimeSpan timeSpan)
        {
            Console.WriteLine($"{timeSpan.Hours}h{timeSpan.Minutes}m{timeSpan.Seconds}s");
        }
    }


}