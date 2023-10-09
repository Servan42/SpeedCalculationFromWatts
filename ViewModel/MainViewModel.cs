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

        public MainViewModel()
        {
            this.weightFormViewModel = new();
        }

        public WeightFormViewModel WeightFormViewModel { get => weightFormViewModel; set => weightFormViewModel = value; }
    }
}
