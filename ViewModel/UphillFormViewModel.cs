﻿using SpeedCalculationFromWatts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class UphillFormViewModel : SegmentViewModelBase
    {
        private double distanceForm;
        private double elevationForm;
        private double targetedAveragePowerForm;

        public UphillFormViewModel(WeightFormViewModel weightFormViewModel) : base(new Segment(false), weightFormViewModel)
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
                RaisePropertyChanged(nameof(ElevationTextBlock));
            }
        }

        public double? ElevationForm
        {
            get => elevationForm == 0 ? null : elevationForm;
            set
            {
                elevationForm = value ?? 0;
                base.segment.ElevationM = elevationForm;
                RaisePropertyChanged();
                RunCalculations();
                RaisePropertyChanged(nameof(ElevationTextBlock));
            }
        }

        public double? TargetedAveragePowerForm
        {
            get => targetedAveragePowerForm == 0 ? null : targetedAveragePowerForm;
            set
            {
                targetedAveragePowerForm = value ?? 0;
                base.segment.TargetedAveragePowerW = targetedAveragePowerForm;
                RaisePropertyChanged();
                RunCalculations();
            }
        }

        public string ElevationTextBlock => $"Gradient: {(segment.GradientPer * 100).ToString("0.#")}%  Category: {segment.Category}";
    }
}