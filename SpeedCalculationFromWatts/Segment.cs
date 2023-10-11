using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedCalculationFromWatts
{
    public class Segment
    {
        public double DistanceKm { get; set; }
        public double ElevationM {get; set;}
        public double TargetedAveragePowerW {get; set;}
        public double TargetedAverageSpeedKmh { get; set; }
        public double TotalWeightKg {get; set;}
        public TimeSpan EstimatedTime { get; private set; }
        public double GradientPer { get; private set; }
        public string Category { get; private set; }
        public double CategoryScore { get; private set; }
        public double CalculatedAverageSpeedMs { get; private set; }
        public bool IsDownhill { get; private set; }

        public Segment(bool isDownhill)
        {
            this.IsDownhill = isDownhill;
        }

        public Segment(double distance, double targetedAverageSpeed)
        {
            this.DistanceKm = distance;
            this.TargetedAverageSpeedKmh = targetedAverageSpeed;
            IsDownhill = true;
        }

        public Segment(double distance, double elevation, double targetedAveragePower, double totalWeight)
        {
            this.DistanceKm = distance;
            this.ElevationM = elevation;
            this.TargetedAveragePowerW = targetedAveragePower;
            this.TotalWeightKg = totalWeight;
            IsDownhill = false;
        }

        public static Segment Build(double distance, double targetedAverageSpeed)
        {
            Segment segment = new Segment(distance, targetedAverageSpeed);
            segment.ComputeEstimatedData();
            return segment;
        }

        public static Segment Build(double distance, double elevation, double targetedAveragePower, double totalWeight)
        {
            Segment segment = new Segment(distance, elevation, targetedAveragePower, totalWeight);
            segment.ComputeEstimatedData();
            return segment;
        }

        public void ComputeEstimatedData()
        {
            if (this.IsDownhill)
                ComputeEstimatedDataForDownhillSegment();
            else
                ComputeEstimatedDataForUphillOrFlatSegment();
        }

        private void ComputeEstimatedDataForUphillOrFlatSegment()
        {
            if (this.DistanceKm == 0)
                return;

            this.GradientPer = this.ElevationM / (this.DistanceKm * 1000);
            CalculateSegmentCategory();

            if (TotalWeightKg == 0 || TargetedAveragePowerW == 0)
                return;

            // http://bernard.mischler.free.fr/equacycle/exemple.htm
            double c1 = 0.086;
            double c2 = 0.0981;
            double c3 = 9.81;

            double a = c1;
            double b = c2 * this.TotalWeightKg;
            double c = c3 * this.TotalWeightKg * this.GradientPer;
            double d = this.TargetedAveragePowerW;

            // Wolframe alpha magic
            double bigSquareNumber = Math.Cbrt((27 * a * a * d) + Math.Sqrt((729 * a * a * a * a * d * d) + (108 * a * a * a * Math.Pow(b + c, 3))));

            if (bigSquareNumber == 0)
                return;

            double member1 = (0.26457 * bigSquareNumber) / a;
            double member2 = (1.2599 * (b + c)) / bigSquareNumber;

            this.CalculatedAverageSpeedMs = member1 - member2;
            this.EstimatedTime = new TimeSpan(0, 0, (int)((this.DistanceKm * 1000) / this.CalculatedAverageSpeedMs));
        }

        private void CalculateSegmentCategory()
        {
            this.CategoryScore = GradientPer * 100 * GradientPer * 100 * DistanceKm;
            this.CategoryScore = Math.Round(this.CategoryScore, 0);
            if (this.CategoryScore < 35) this.Category = "";
            else if (this.CategoryScore >= 35 && this.CategoryScore < 80) this.Category = "4";
            else if (this.CategoryScore >= 80 && this.CategoryScore < 180) this.Category = "3";
            else if (this.CategoryScore >= 180 && this.CategoryScore < 250) this.Category = "2";
            else if (this.CategoryScore >= 250 && this.CategoryScore < 600) this.Category = "1";
            else if (this.CategoryScore >= 600) this.Category = "HC";
        }

        private void ComputeEstimatedDataForDownhillSegment()
        {
            if (TargetedAverageSpeedKmh == 0)
                return;

            this.EstimatedTime = new TimeSpan(0, 0, (int)((this.DistanceKm * 1000) / (this.TargetedAverageSpeedKmh / 3.6)));
        }

        public void PrintSegmentDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Distance: ").Append(this.DistanceKm).Append("km\t");
            if (IsDownhill)
            {
                sb.AppendLine();
                sb.Append("Expected Average Speed: ").Append(this.TargetedAverageSpeedKmh).Append("km/h    ");
                sb.Append("Expected Time: ").Append($"{this.EstimatedTime.Hours.ToString("00")}h{this.EstimatedTime.Minutes.ToString("00")}m{this.EstimatedTime.Seconds.ToString("00")}s    ");
            }
            else
            {
                sb.Append("Elevation: ").Append(this.ElevationM).Append("m    ");
                sb.Append("Gradient: ").Append((this.GradientPer * 100).ToString("0.#")).Append("%    ");
                if (this.CategoryScore >= 35) sb.Append("Category: ").Append(this.Category).Append($" ({this.CategoryScore})");
                sb.AppendLine();
                sb.Append("Targeted Average Power: ").Append(this.TargetedAveragePowerW).Append("W    ");
                sb.Append("Expected Average Speed: ").Append((this.CalculatedAverageSpeedMs * 3.6).ToString("0.#")).Append("km/h    ");
                sb.Append("Expected Time: ").Append($"{this.EstimatedTime.Hours.ToString("00")}h{this.EstimatedTime.Minutes.ToString("00")}m{this.EstimatedTime.Seconds.ToString("00")}s");
            }
            Console.WriteLine(sb.ToString());
        }

        public Segment Clone()
        {
            Segment result;
            if (this.IsDownhill)
            {
                result = Segment.Build(this.DistanceKm, this.TargetedAverageSpeedKmh);
            }
            else
            {
                result = Segment.Build(this.DistanceKm, this.ElevationM, this.TargetedAveragePowerW, this.TotalWeightKg);
            }
            result.ComputeEstimatedData();
            return result;
        }
    }
}
