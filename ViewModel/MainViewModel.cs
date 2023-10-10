using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        WeightFormViewModel weightFormViewModel;
        UphillFormViewModel uphillFormViewModel;
        DownhillFormViewModel downHillFormViewModel;

        public MainViewModel()
        {
            this.weightFormViewModel = new();
            this.uphillFormViewModel = new(weightFormViewModel);
            this.downHillFormViewModel = new(weightFormViewModel);
        }

        public WeightFormViewModel WeightFormViewModel { get => weightFormViewModel; set => weightFormViewModel = value; }
        public UphillFormViewModel UphillFormViewModel { get => uphillFormViewModel; set => uphillFormViewModel = value; }
        public DownhillFormViewModel DownhillFormViewModel { get => downHillFormViewModel; set => downHillFormViewModel = value; }
    }
}
