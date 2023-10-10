using SpeedCalculationFromWatts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class DownhillFormViewModel : SegmentViewModelBase
    {
        private double distanceForm;
        private double targetedAverageSpeedForm;

        public DownhillFormViewModel(WeightFormViewModel weightFormViewModel) : base(new Segment(true), weightFormViewModel)
        {
        }

        public double? DistanceForm
        {
            get => distanceForm == 0 ? null : distanceForm;
            set
            {
                distanceForm = value ?? 0;
                base.segment.DistanceKm = distanceForm;
                RaisePropertyChanged();
                RunCalculations();
            }
        }

        public double? TargetedAverageSpeedForm
        {
            get => targetedAverageSpeedForm == 0 ? null : targetedAverageSpeedForm;
            set
            {
                targetedAverageSpeedForm = value ?? 0;
                base.segment.TargetedAverageSpeedKmh = targetedAverageSpeedForm;
                RaisePropertyChanged();
                RunCalculations();
            }
        }
    }
}
