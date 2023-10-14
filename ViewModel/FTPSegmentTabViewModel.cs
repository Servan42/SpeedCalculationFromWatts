using SpeedCalculationFromWatts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Extensions;

namespace ViewModel
{
    public class FTPSegmentTabViewModel : ViewModelBase
    {
        private readonly WeightFormViewModel weightFormViewModel;
        private double distanceForm;
        private double elevationForm;
        private double ftpForm;

        public FTPSegmentTabViewModel(WeightFormViewModel weightFormViewModel)
        {
            this.weightFormViewModel = weightFormViewModel;
            this.WattTextBlocks = new() { "", "", "", "", "", "" };
            this.WattPerKgTextBlocks = new() { "", "", "", "", "", "" };
            this.EstimatedTimeTextBlocks = new() { "", "", "", "", "", "" };
        }

        public double? DistanceForm
        {
            get => distanceForm == 0 ? null : distanceForm;
            set
            {
                distanceForm = value ?? 0;
                RaisePropertyChanged();
                RunCalculations();
            }
        }

        public double? ElevationForm
        {
            get => elevationForm == 0 ? null : elevationForm;
            set
            {
                elevationForm = value ?? 0;
                RaisePropertyChanged();
                RunCalculations();
            }
        }

        public double? FtpForm
        {
            get => ftpForm == 0 ? null : ftpForm;
            set
            {
                ftpForm = value ?? 0;
                RaisePropertyChanged();
                RunCalculations();
            }
        }

        public ObservableCollection<string> WattTextBlocks { get; set; }
        public ObservableCollection<string> WattPerKgTextBlocks { get; set; }
        public ObservableCollection<string> EstimatedTimeTextBlocks { get; set; }

        private void RunCalculations()
        {
            var segment = new Segment(false);
            var ftpZonesCalculator = new FtpZonesCalculator();
            int totalWeight = this.weightFormViewModel.GetTotalWeight();
            segment.TotalWeightKg = totalWeight;
            segment.ElevationM = this.elevationForm;
            segment.DistanceKm = this.distanceForm;

            var lowerBoundFtpZones = ftpZonesCalculator.GetLowerBoundWattZones(this.ftpForm);
            var upperBoundFtpZones = ftpZonesCalculator.GetUpperBoundWattZones(this.ftpForm);

            for(int i = 0; i < this.WattTextBlocks.Count; i++)
            {
                this.WattTextBlocks[i] = $"{Math.Round(lowerBoundFtpZones[i], 0)} - {Math.Round(upperBoundFtpZones[i], 0)} W";
            }

            if (totalWeight <= 0)
                return;

            for (int i = 0; i < this.WattPerKgTextBlocks.Count; i++)
            {
                this.WattPerKgTextBlocks[i] = $"{Math.Round(lowerBoundFtpZones[i] / totalWeight, 1)} - {Math.Round(upperBoundFtpZones[i] / totalWeight, 1)} W/kg";
            }

            var middleBoundFtpZones = ftpZonesCalculator.GetMiddleBoundWattZones(this.ftpForm);
            for(int i = 0; i < this.EstimatedTimeTextBlocks.Count; i++)
            {
                segment.TargetedAveragePowerW = middleBoundFtpZones[i];
                segment.ComputeEstimatedData();
                this.EstimatedTimeTextBlocks[i] = segment.EstimatedTime.GetHourMinSecString();
            }
        }
    }
}
