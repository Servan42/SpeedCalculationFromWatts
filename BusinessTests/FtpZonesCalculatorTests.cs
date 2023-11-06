using Business;

namespace BusinessTests
{
    public class FtpZonesCalculatorTests
    {
        [Test]
        public void Should_get_lower_bound_watt_zones()
        {
            // GIVEN
            var sut = new FtpZonesCalculator();

            // WHEN
            var zones = sut.GetLowerBoundWattZones(100);

            // THEN
            Assert.That(Math.Round(zones[0],2), Is.EqualTo(0));
            Assert.That(Math.Round(zones[1],2), Is.EqualTo(56));
            Assert.That(Math.Round(zones[2],2), Is.EqualTo(76));
            Assert.That(Math.Round(zones[3],2), Is.EqualTo(91));
            Assert.That(Math.Round(zones[4],2), Is.EqualTo(106));
            Assert.That(Math.Round(zones[5],2), Is.EqualTo(121));
        }

        [Test]
        public void Should_get_middle_bound_watt_zones()
        {
            // GIVEN
            var sut = new FtpZonesCalculator();

            // WHEN
            var zones = sut.GetMiddleBoundWattZones(100);

            // THEN
            Assert.That(Math.Round(zones[0],2), Is.EqualTo(55 / 2.0));
            Assert.That(Math.Round(zones[1],2), Is.EqualTo((56 + 75) / 2.0));
            Assert.That(Math.Round(zones[2],2), Is.EqualTo((76 + 90) / 2.0));
            Assert.That(Math.Round(zones[3],2), Is.EqualTo((91 + 105) / 2.0));
            Assert.That(Math.Round(zones[4],2), Is.EqualTo((106 + 120) / 2.0));
            Assert.That(Math.Round(zones[5],2), Is.EqualTo((121 + 150) / 2.0));
        }

        [Test]
        public void Should_get_upper_bound_watt_zones()
        {
            // GIVEN
            var sut = new FtpZonesCalculator();

            // WHEN
            var zones = sut.GetUpperBoundWattZones(100);

            // THEN
            Assert.That(Math.Round(zones[0],2), Is.EqualTo(55));
            Assert.That(Math.Round(zones[1],2), Is.EqualTo(75));
            Assert.That(Math.Round(zones[2],2), Is.EqualTo(90));
            Assert.That(Math.Round(zones[3],2), Is.EqualTo(105));
            Assert.That(Math.Round(zones[4],2), Is.EqualTo(120));
            Assert.That(Math.Round(zones[5],2), Is.EqualTo(150));
        }
    }
}