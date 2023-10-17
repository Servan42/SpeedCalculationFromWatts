using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class FtpZonesCalculator
    {
        private List<double> zonesPercentage = new() { 0.55, 0.75, 0.90, 1.05, 1.2, 1.5 };

        public List<double> GetLowerBoundWattZones(double ftp)
        {
            List<double> result = new();
            result.Add(0.0);
            foreach (var per in zonesPercentage)
            {
                result.Add(ftp * (per + 0.1));
            }
            return result;
        }

        public List<double> GetUpperBoundWattZones(double ftp)
        {
            List<double> result = new();
            foreach (var per in zonesPercentage)
            {
                result.Add(ftp * per);
            }
            return result;
        }

        public List<double> GetMiddleBoundWattZones(double ftp)
        {
            List<double> result = new();
            result.Add(ftp * ((0 + zonesPercentage[0]) / 2.0));
            for (int i = 0; i < zonesPercentage.Count - 1; i++)
            {
                result.Add(ftp * ((zonesPercentage[i] + zonesPercentage[i + 1]) / 2.0));
            }
            return result;
        }
    }
}
