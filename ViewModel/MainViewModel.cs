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

        public MainViewModel()
        {
            this.weightFormViewModel = new();
            this.uphillFormViewModel = new(weightFormViewModel);
            this.downHillFormViewModel = new(weightFormViewModel);
            this.rideViewModel = new();
            AddUHSegementToRideCommand = new DelegateCommand(AddUHSegementToRide);
            AddDHSegementToRideCommand = new DelegateCommand(AddDHSegementToRide);
        }

        public WeightFormViewModel WeightFormViewModel { get => weightFormViewModel; set => weightFormViewModel = value; }
        public UphillFormViewModel UphillFormViewModel { get => uphillFormViewModel; set => uphillFormViewModel = value; }
        public DownhillFormViewModel DownhillFormViewModel { get => downHillFormViewModel; set => downHillFormViewModel = value; }
        public RideViewModel RideViewModel { get => rideViewModel; set => rideViewModel = value; }

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
