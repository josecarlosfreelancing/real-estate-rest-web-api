using EstateViewWebAPIServer.Models;
using EstateViewWebAPIServer.Models.Scenarios;
using System.Windows.Input;

namespace EstateViewWebAPIServer.Applications.Timeline
{
    public class TimelineApplication
    {
        public List<CustomAreaDataPoint> bypassTrustDataPoints { get; set; }
        public List<CustomAreaDataPoint> giftTrustDataPoints { get; set; }
        public List<CustomAreaDataPoint> personalResidenceDataPoints { get; set; }
        public List<CustomAreaDataPoint> investmentAssetsDataPoints { get; set; }
        public List<CustomAreaDataPoint> estateTaxDataPoints { get; set; }
        public List<CustomAreaDataPoint> lifeInsuranceFirstSpouseDataPoints { get; set; }
        public List<CustomAreaDataPoint> lifeInsuranceSecondSpouseDataPoints { get; set; }
        public List<CustomAreaDataPoint> lifeInsuranceSurvivorshipDataPoints { get; set; }
        public List<CustomAreaDataPoint> installmentSaleTrustDataPoints { get; set; }
        public EstatePlanningScenario Scenario { get; set; }

        public TimelineApplication (EstatePlanningScenario scenario)
        {
            this.Scenario = scenario;
            EstateProjection scenarioProjection = scenario.Projections.SingleOrDefault(p => p.Year == scenario.Options.SecondDyingSpouse.ProjectedYearOfDeath);

            if (scenarioProjection != null)
            {
                bypassTrustDataPoints = new List<CustomAreaDataPoint>();
                giftTrustDataPoints = new List<CustomAreaDataPoint>();
                personalResidenceDataPoints = new List<CustomAreaDataPoint>();
                investmentAssetsDataPoints = new List<CustomAreaDataPoint>();
                estateTaxDataPoints = new List<CustomAreaDataPoint>();
                lifeInsuranceFirstSpouseDataPoints = new List<CustomAreaDataPoint>();
                lifeInsuranceSecondSpouseDataPoints = new List<CustomAreaDataPoint>();
                lifeInsuranceSurvivorshipDataPoints = new List<CustomAreaDataPoint>();
                installmentSaleTrustDataPoints = new List<CustomAreaDataPoint>();

                foreach (EstateProjection projection in scenario.Projections)
                {
                    int year = projection.Year;
                    double lastValue = 0;
                    double currentValue = 0;


                    currentValue = (double)projection.InstallmentSaleTrustValue - (double)projection.InstallmentSaleNoteValue;
                    installmentSaleTrustDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    currentValue = (double)(projection.LifeInsuranceOnSecondToDieBenefit + projection.LifeInsuranceOnSecondToDieBenefitInTrust);
                    lifeInsuranceSurvivorshipDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    currentValue = (double)(projection.LifeInsuranceOnSecondSpouseBenefit + projection.LifeInsuranceOnSecondSpouseBenefitInTrust);
                    lifeInsuranceSecondSpouseDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    currentValue = (double)(projection.LifeInsuranceOnFirstSpouseBenefit + projection.LifeInsuranceOnFirstSpouseBenefitInTrust);
                    lifeInsuranceFirstSpouseDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    currentValue = (double)projection.GiftingTrustValue;
                    giftTrustDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    currentValue = (double)projection.BypassTrustValue;
                    bypassTrustDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    currentValue = (double)projection.ResidenceValue;
                    personalResidenceDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    currentValue = (double)projection.InvestmentsValue + (double)projection.InstallmentSaleNoteValue;
                    investmentAssetsDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    if (year <= scenario.Options.SecondDyingSpouse.ProjectedYearOfDeath)
                    {
                        currentValue = (double)(-projection.EstateTaxDue);
                        estateTaxDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    }
                }
            }
        }

        public class CustomAreaDataPoint 
        { 
            public int Year { get; set; }
            public double Value
            {
                get { return this.EndValue - this.StartValue; }
            }
            public double StartValue { get; private set; }
            public double EndValue { get; private set; }

            public CustomAreaDataPoint(int year, double startValue, double value)
            {
                this.Year = year;
                this.StartValue = startValue;
                this.EndValue = startValue + value;
            }
        }
    }
}
