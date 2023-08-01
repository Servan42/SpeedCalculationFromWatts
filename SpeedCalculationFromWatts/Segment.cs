using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedCalculationFromWatts
{
    internal class Segment
    {
        private double distanceKm;
        private double elevationM;
        private double gradientPer;
        private (double score, string cat) category;
        private double targetedAveragePowerW;
        private double totalWeightKg;
        private double expectedAverageSpeedKmh;
        private double calculatedAverageSpeedMs;
        private bool isDownhill;
        public TimeSpan EstimatedTime { get; private set; }

        public Segment(double distance, double expectedAverageSpeed)
        {
            this.distanceKm = distance;
            this.expectedAverageSpeedKmh = expectedAverageSpeed;
            isDownhill = true;
        }

        public Segment(double distance, double elevation, double expectedAveragePower, double totalWeight)
        {
            this.distanceKm = distance;
            this.elevationM = elevation;
            this.targetedAveragePowerW = expectedAveragePower;
            this.totalWeightKg = totalWeight;
            isDownhill = false;
        }

        public static Segment Build(double distance, double expectedAverageSpeed)
        {
            Segment segment = new Segment(distance, expectedAverageSpeed);
            segment.ComputeEstimatedData();
            return segment;
        }

        public static Segment Build(double distance, double elevation, double expectedAveragePower, double totalWeight)
        {
            Segment segment = new Segment(distance, elevation, expectedAveragePower, totalWeight);
            segment.ComputeEstimatedData();
            return segment;
        }

        public void ComputeEstimatedData()
        {
            if (this.isDownhill)
                ComputeEstimatedDataForDownhillSegment();
            else
                ComputeEstimatedDataForUphillOrFlatSegment();
        }

        private void ComputeEstimatedDataForUphillOrFlatSegment()
        {
            this.gradientPer = this.elevationM / (this.distanceKm * 1000);
            CalculateSegmentCategory();
            // http://bernard.mischler.free.fr/equacycle/exemple.htm
            double c1 = 0.086;
            double c2 = 0.0981;
            double c3 = 9.81;

            double a = c1;
            double b = c2 * this.totalWeightKg;
            double c = c3 * this.totalWeightKg * this.gradientPer;
            double d = this.targetedAveragePowerW;

            // Wolframe alpha magic
            double bigSquareNumber = Math.Cbrt((27 * a * a * d) + Math.Sqrt((729 * a * a * a * a * d * d) + (108 * a * a * a * Math.Pow(b + c, 3))));

            double member1 = (0.26457 * bigSquareNumber) / a;
            double member2 = (1.2599 * (b + c)) / bigSquareNumber;

            this.calculatedAverageSpeedMs = member1 - member2;
            this.EstimatedTime = new TimeSpan(0, 0, (int)((this.distanceKm * 1000) / this.calculatedAverageSpeedMs));
        }

        private void CalculateSegmentCategory()
        {
            this.category.score = gradientPer * 100 * gradientPer * 100 * distanceKm;
            this.category.score = Math.Round(this.category.score, 0);
            if (this.category.score < 35) this.category.cat = "";
            else if (this.category.score >= 35 && this.category.score < 80) this.category.cat = "4";
            else if (this.category.score >= 80 && this.category.score < 180) this.category.cat = "3";
            else if (this.category.score >= 180 && this.category.score < 250) this.category.cat = "2";
            else if (this.category.score >= 250 && this.category.score < 600) this.category.cat = "1";
            else if (this.category.score >= 600) this.category.cat = "HC";
        }

        private void ComputeEstimatedDataForDownhillSegment()
        {
            this.EstimatedTime = new TimeSpan(0, 0, (int)((this.distanceKm * 1000) / (this.expectedAverageSpeedKmh / 3.6)));
        }

        public void PrintSegmentDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Distance: ").Append(this.distanceKm).Append("km\t");
            if (isDownhill)
            {
                sb.AppendLine();
                sb.Append("Expected Average Speed: ").Append(this.expectedAverageSpeedKmh).Append("km/h    ");
                sb.Append("Expected Time: ").Append($"{this.EstimatedTime.Hours.ToString("00")}h{this.EstimatedTime.Minutes.ToString("00")}m{this.EstimatedTime.Seconds.ToString("00")}s    ");
            }
            else
            {
                sb.Append("Elevation: ").Append(this.elevationM).Append("m    ");
                sb.Append("Gradient: ").Append((this.gradientPer * 100).ToString("0.#")).Append("%    ");
                if (this.category.score >= 35) sb.Append("Category: ").Append(this.category.cat).Append($" ({this.category.score})");
                sb.AppendLine();
                sb.Append("Targeted Average Power: ").Append(this.targetedAveragePowerW).Append("W    ");
                sb.Append("Expected Average Speed: ").Append((this.calculatedAverageSpeedMs * 3.6).ToString("0.#")).Append("km/h    ");
                sb.Append("Expected Time: ").Append($"{this.EstimatedTime.Hours.ToString("00")}h{this.EstimatedTime.Minutes.ToString("00")}m{this.EstimatedTime.Seconds.ToString("00")}s");
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
