using SpeedCalculationFromWatts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class SegmentViewModelBase : ViewModelBase
    {
        protected Segment segment;
        private readonly WeightFormViewModel weightFormViewModel;

        public SegmentViewModelBase(Segment segment, WeightFormViewModel weightFormViewModel)
        {
            this.segment = segment;
            this.weightFormViewModel = weightFormViewModel;
        }

        protected void RunCalculations()
        {
            segment.TotalWeightKg = weightFormViewModel.GetTotalWeight();
            segment.ComputeEstimatedData();
            RaisePropertyChanged(nameof(EstimatedSpeedTextBlock));
            RaisePropertyChanged(nameof(EstimatedTimeTextBlock));
        }

        public string EstimatedSpeedTextBlock => $"Estimated Speed: {(segment.CalculatedAverageSpeedMs * 3.6).ToString("0.#")} km/h";
        public string EstimatedTimeTextBlock => $"Estimated Time: {segment.EstimatedTime.Hours.ToString("00")}h{segment.EstimatedTime.Minutes.ToString("00")}m{segment.EstimatedTime.Seconds.ToString("00")}s";

    }
}
