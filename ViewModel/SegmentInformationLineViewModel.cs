using SpeedCalculationFromWatts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Command;
using WpfApp.Extensions;

namespace ViewModel
{
    public class SegmentInformationLineViewModel
    {
        public Segment Segment { get; private set; }
        private readonly Action<Guid> requestForDeletionInParent;

        public Guid Id { get; private set; }

        public SegmentInformationLineViewModel(Segment segment, Action<Guid> requestForDeletionInParent)
        {
            this.Segment = segment;
            this.requestForDeletionInParent = requestForDeletionInParent;
            Id = Guid.NewGuid();
            RequestDeletionCommand = new DelegateCommand(RequestDeletion);
        }

        public DelegateCommand RequestDeletionCommand { get; }

        public string SlopeTextBlock => this.Segment.IsDownhill ? "Downhill" : "Uphill";
        public string DistanceTextBlock => $"{this.Segment.DistanceKm}km";
        public string ElevationTextBlock => this.Segment.IsDownhill ? "" : $"{this.Segment.ElevationM}m";
        public string GradientTextBlock => this.Segment.IsDownhill ? "" : $"{(this.Segment.GradientPer * 100).ToString("0.#")}%";
        public string CategoryTextBlock => this.Segment.IsDownhill ? "" : $"{this.Segment.Category}";
        public string PowerTextBlock => this.Segment.IsDownhill ? "" : $"{this.Segment.TargetedAveragePowerW}W";
        public string SpeedTextBlock  => this.Segment.IsDownhill ? $"{(this.Segment.TargetedAverageSpeedKmh).ToString("0.#")}km/h" : $"{(this.Segment.CalculatedAverageSpeedMs * 3.6).ToString("0.#")}km/h";
        public string TimeTextBlock => this.Segment.EstimatedTime.GetHourMinSecString();

        private void RequestDeletion()
        {
            requestForDeletionInParent(this.Id);
        }
    }
}
