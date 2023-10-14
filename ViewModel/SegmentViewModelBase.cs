using SpeedCalculationFromWatts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Extensions;

namespace ViewModel
{
    public class SegmentViewModelBase : ViewModelBase
    {
        public Segment Segment { get; protected set; }
        private readonly WeightFormViewModel weightFormViewModel;

        public SegmentViewModelBase(Segment segment, WeightFormViewModel weightFormViewModel)
        {
            this.Segment = segment;
            this.weightFormViewModel = weightFormViewModel;
        }

        protected void RunCalculations()
        {
            Segment.TotalWeightKg = weightFormViewModel.GetTotalWeight();
            Segment.ComputeEstimatedData();
            RaisePropertyChanged(nameof(EstimatedSpeedTextBlock));
            RaisePropertyChanged(nameof(EstimatedTimeTextBlock));
        }

        public string EstimatedSpeedTextBlock => $"Estimated Speed: {(Segment.CalculatedAverageSpeedMs * 3.6).ToString("0.#")} km/h";
        public string EstimatedTimeTextBlock => Segment.EstimatedTime.GetHourMinSecString();

    }
}
