using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class WeightFormViewModel : ViewModelBase
    {
        private int cyclistWeight;
        private int bikeWeight;

        public int? CyclistWeight
        {
            get => cyclistWeight == 0 ? null : cyclistWeight;
            set
            {
                cyclistWeight = value ?? 0;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(TotalWeight));
            }
        }

        public int? BikeWeight
        {
            get => bikeWeight == 0 ? null : bikeWeight;
            set
            {
                bikeWeight = value ?? 0;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(TotalWeight));
            }
        }

        public int GetTotalWeight()
        {
            return cyclistWeight + bikeWeight;
        }

        public string TotalWeight => $"Total: {GetTotalWeight()}kg";
    }
}
