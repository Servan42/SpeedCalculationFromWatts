using SpeedCalculationFromWatts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Command;

namespace ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        WeightFormViewModel weightFormViewModel;
        UphillFormViewModel uphillFormViewModel;
        DownhillFormViewModel downHillFormViewModel;
        RideViewModel rideViewModel;
        FTPSegmentTabViewModel fTPSegmentTabViewModel;

        public MainViewModel()
        {
            this.weightFormViewModel = new();
            this.uphillFormViewModel = new(weightFormViewModel);
            this.downHillFormViewModel = new(weightFormViewModel);
            this.rideViewModel = new();
            this.fTPSegmentTabViewModel = new(weightFormViewModel);
            AddUHSegementToRideCommand = new DelegateCommand(AddUHSegementToRide);
            AddDHSegementToRideCommand = new DelegateCommand(AddDHSegementToRide);
        }

        public WeightFormViewModel WeightFormViewModel { get => weightFormViewModel; set => weightFormViewModel = value; }
        public UphillFormViewModel UphillFormViewModel { get => uphillFormViewModel; set => uphillFormViewModel = value; }
        public DownhillFormViewModel DownhillFormViewModel { get => downHillFormViewModel; set => downHillFormViewModel = value; }
        public RideViewModel RideViewModel { get => rideViewModel; set => rideViewModel = value; }
        public FTPSegmentTabViewModel FTPSegmentTabViewModel { get => fTPSegmentTabViewModel; set => fTPSegmentTabViewModel = value; }

        public DelegateCommand AddUHSegementToRideCommand { get; }
        public DelegateCommand AddDHSegementToRideCommand { get; }

        private void AddUHSegementToRide()
        {
            uphillFormViewModel.Segment.TotalWeightKg = weightFormViewModel.GetTotalWeight();
            rideViewModel.AddSegment(uphillFormViewModel.Segment.Clone());
        }

        private void AddDHSegementToRide()
        {
            downHillFormViewModel.Segment.TotalWeightKg = weightFormViewModel.GetTotalWeight();
            rideViewModel.AddSegment(downHillFormViewModel.Segment.Clone());
        }
    }
}
