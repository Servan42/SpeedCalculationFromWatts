using SpeedCalculationFromWatts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class RideViewModel : ViewModelBase
    {
        public ObservableCollection<SegmentInformationLineViewModel> Segments { get; set; }

        public RideViewModel()
        {
            this.Segments = new();
        }

        public void AddSegment(Segment segment)
        {
            this.Segments.Add(new SegmentInformationLineViewModel(segment, DeleteSegment));
            RaisePropertyChanged(nameof(TotalDistanceTextBlock));
            RaisePropertyChanged(nameof(TotalElevationTextBlock));
            RaisePropertyChanged(nameof(AverageSpeedTextBlock));
            RaisePropertyChanged(nameof(TotalExpectedTimeTextBlock));
        }

        internal void DeleteSegment(Guid id)
        {
            var segementToRemove = this.Segments.First(s => s.Id == id);
            this.Segments.Remove(segementToRemove);
            RaisePropertyChanged(nameof(TotalDistanceTextBlock));
            RaisePropertyChanged(nameof(TotalElevationTextBlock));
            RaisePropertyChanged(nameof(AverageSpeedTextBlock));
            RaisePropertyChanged(nameof(TotalExpectedTimeTextBlock));
        }

        public string TotalDistanceTextBlock => $"{this.Segments.Sum(s => s.Segment.DistanceKm)}km";
        public string TotalElevationTextBlock => $"{this.Segments.Sum(s => s.Segment.ElevationM)}m";
        public string AverageSpeedTextBlock => GetAverageSpeed();
        public string TotalExpectedTimeTextBlock => GetTotalTimeString();

        private string GetAverageSpeed()
        {
            double totalSpeedKmh = 0;
            int nb = 0;
            foreach(var s in  this.Segments)
            {
                if(s.Segment.IsDownhill)
                {
                    totalSpeedKmh += s.Segment.TargetedAverageSpeedKmh;
                }
                else
                {
                    totalSpeedKmh += s.Segment.CalculatedAverageSpeedMs * 3.6;

                }
                nb++;
            }

            if (nb == 0) return "0km/h";
            else return $"{(totalSpeedKmh / nb).ToString("0.#")}km/h";
        }

        private string GetTotalTimeString()
        {
            TimeSpan totalTime = new();
            foreach(var s in Segments)
            {
                totalTime += s.Segment.EstimatedTime;
            }
            return $"{totalTime.Hours.ToString("00")}h{totalTime.Minutes.ToString("00")}m{totalTime.Seconds.ToString("00")}s";
        }
    }
}
