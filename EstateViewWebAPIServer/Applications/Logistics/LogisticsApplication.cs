using EstateViewWebAPIServer.Models.Scenarios;
using EstateViewWebAPIServer.Models;

namespace EstateViewWebAPIServer.Applications.Logistics
{
    public class LogisticsApplication : BaseApplication
    {
        public LogisticsApplication(EstatePlanningScenario scenario)
        {
            this.Bind(scenario);
        }

        public EstatePlanningScenario CurrentScenario
        {
            get { return this.GetValue(() => this.CurrentScenario); }
            set { this.SetValue(() => this.CurrentScenario, value); }
        }

        public YearTaxApplication Today
        {
            get { return this.GetValue(() => this.Today); }
            set { this.SetValue(() => this.Today, value); }
        }

        public YearTaxApplication FirstDeath
        {
            get { return this.GetValue(() => this.FirstDeath); }
            set { this.SetValue(() => this.FirstDeath, value); }
        }

        public YearTaxApplication SecondDeath
        {
            get { return this.GetValue(() => this.SecondDeath); }
            set { this.SetValue(() => this.SecondDeath, value); }
        }

        public decimal TotalPassedToBeneficiaries
        {
            get { return this.GetValue(() => this.TotalPassedToBeneficiaries); }
            set { this.SetValue(() => this.TotalPassedToBeneficiaries, value); }
        }

        public void Bind(EstatePlanningScenario scenario)
        {
            this.CurrentScenario = scenario;

            EstateProjection currentScenarioProjection = this.CurrentScenario.Projections.SingleOrDefault(p => p.Year == this.CurrentScenario.Options.FirstDyingSpouse.ProjectedYearOfDeath);

            if (currentScenarioProjection != null)
            {
                this.Today = this.GenerateYearBoxes(
                    this.CurrentScenario.Projections.First(),
                    string.Format("{0} & {1} {2}", this.CurrentScenario.Options.Spouse1.FirstName, this.CurrentScenario.Options.Spouse2.FirstName, this.CurrentScenario.Options.Spouse1.LastName),
                    isLastYear: false);

                this.FirstDeath = this.GenerateYearBoxes(
                    currentScenarioProjection,
                    string.Format("{0} {1}", this.CurrentScenario.Options.SecondDyingSpouse.FirstName, this.CurrentScenario.Options.SecondDyingSpouse.LastName),
                    isLastYear: false);

                this.SecondDeath = this.GenerateYearBoxes(
                    this.CurrentScenario.Projections.Last(),
                    string.Format("{0}'s Estate", this.CurrentScenario.Options.SecondDyingSpouse.FirstName),
                    isLastYear: true);

                this.TotalPassedToBeneficiaries = this.CurrentScenario.Projections.Last().TotalAmountPassedToFamily;
            }
        }

        private YearTaxApplication GenerateYearBoxes(EstateProjection projection, string title, bool isLastYear)
        {
            YearTaxApplication yearTax = new YearTaxApplication();
            yearTax.DeltaYear = projection.Year - DateTime.Today.Year;
            bool isFirstYear = yearTax.DeltaYear == 0;

            yearTax.Estate = new EstateApplication();
            yearTax.Estate.Title = title;
            yearTax.Estate.Residence = projection.ResidenceValue;
            yearTax.Estate.Investments = projection.InvestmentsValue;
            yearTax.Estate.EstateTax = projection.EstateTaxDue;

            if (isFirstYear)
            {
                yearTax.Estate.InvestmentsAnnualChange = projection.AnnualInvestmentsChangeBeforeFirstDeath;
            }
            else
            {
                yearTax.Estate.InvestmentsAnnualChange = projection.AnnualInvestmentsChangeAfterFirstDeath;
            }

            if (!isLastYear)
            {
                yearTax.Estate.ResidenceAnnualGrowthRate = (float)this.CurrentScenario.Options.HomeValueGrowthRate;
                yearTax.Estate.InvestmentsAnnualGrowthRate = (float)this.CurrentScenario.Options.InvestmentsGrowthRate;
                yearTax.Estate.InvestmentsAnnualFeesRate = (float)this.CurrentScenario.Options.InvestmentFeesRate;
            }
            else
            {
                yearTax.Estate.Portability = -projection.TotalExclusionAvailable;
                yearTax.Estate.NetTaxableEstate = projection.TaxableValueOfEstate;
            }

            if (projection.BypassTrustValue > 0)
            {
                yearTax.BypassTrust = new BypassTrustApplication();
                yearTax.BypassTrust.Value = projection.BypassTrustValue;
                if (!isLastYear)
                {
                    yearTax.BypassTrust.AnnualGrowthRate = (float)this.CurrentScenario.Options.InvestmentsGrowthRate;
                    yearTax.BypassTrust.AnnualFeesRate = (float)this.CurrentScenario.Options.InvestmentFeesRate;
                }
            }

            yearTax.GiftingTrusts = new List<GiftingTrustApplication>();
            if (this.CurrentScenario.Projections.Any(p => p.GiftingTrustValue > 0))
            {
                yearTax.GiftingTrusts.Add(this.GenerateGiftingTrustBox(projection, isFirstYear, isLastYear));
            }

            yearTax.InstallmentSaleTrusts = new List<InstallmentSaleApplication>();
            if (this.CurrentScenario.Projections.Any(p => p.InstallmentSaleTrustValue > 0))
            {
                yearTax.InstallmentSaleTrusts.Add(this.GenerateInstallmentSaleTrustBox(projection));
            }

            yearTax.Ilits = new List<LifeInsuranceApplication>();
            if (!this.CurrentScenario.Options.Spouse1.LifeInsurance.IsEmpty())
            {
                yearTax.Ilits.Add(
                    this.GenerateIlitBox(
                        this.CurrentScenario.Options.Spouse1.FirstName,
                        projection.LifeInsuranceOnFirstSpouseBenefit + projection.LifeInsuranceOnFirstSpouseBenefitInTrust > 0 ? projection.LifeInsuranceOnFirstSpouseBenefit + projection.LifeInsuranceOnFirstSpouseBenefitInTrust : this.CurrentScenario.Options.Spouse1.LifeInsurance.GetDeathBenefitForYear(projection.YearNumber),
                        isFirstYear || this.CurrentScenario.Options.Spouse1.Name == this.CurrentScenario.Options.SecondDyingSpouse.Name ? this.CurrentScenario.Options.Spouse1.LifeInsurance.GetPremiumForYear(projection.YearNumber) : 0,
                        isLastYear,
                        this.CurrentScenario.Options.Spouse1.LifeInsurance.IsInTrust));
            }

            if (!this.CurrentScenario.Options.Spouse2.LifeInsurance.IsEmpty())
            {
                yearTax.Ilits.Add(
                    this.GenerateIlitBox(
                        this.CurrentScenario.Options.Spouse2.FirstName,
                        projection.LifeInsuranceOnSecondSpouseBenefit + projection.LifeInsuranceOnSecondSpouseBenefitInTrust > 0 ? projection.LifeInsuranceOnSecondSpouseBenefit + projection.LifeInsuranceOnSecondSpouseBenefitInTrust : this.CurrentScenario.Options.Spouse2.LifeInsurance.GetDeathBenefitForYear(projection.YearNumber),
                        isFirstYear || this.CurrentScenario.Options.Spouse2.Name == this.CurrentScenario.Options.SecondDyingSpouse.Name ? this.CurrentScenario.Options.Spouse2.LifeInsurance.GetPremiumForYear(projection.YearNumber) : 0,
                        isLastYear,
                        this.CurrentScenario.Options.Spouse2.LifeInsurance.IsInTrust));
            }

            if (!this.CurrentScenario.Options.SecondToDieLifeInsurance.IsEmpty())
            {
                yearTax.Ilits.Add(
                    this.GenerateIlitBox(
                        "Survivorship",
                        this.CurrentScenario.Options.SecondToDieLifeInsurance.GetDeathBenefitForYear(projection.YearNumber),
                        this.CurrentScenario.Options.SecondToDieLifeInsurance.GetPremiumForYear(projection.YearNumber),
                        isLastYear,
                        this.CurrentScenario.Options.SecondToDieLifeInsurance.IsInTrust));
            }

            return yearTax;
        }

        private InstallmentSaleApplication GenerateInstallmentSaleTrustBox(EstateProjection projection)
        {
            InstallmentSaleApplication viewModel = new InstallmentSaleApplication();
            viewModel.Title = "Year 1 Gift / Installment Sale Trust";
            viewModel.GrossValue = projection.InstallmentSaleTrustValue;
            viewModel.NoteValue = projection.InstallmentSaleNoteValue;
            viewModel.Value = viewModel.GrossValue - viewModel.NoteValue;

            return viewModel;
        }

        private GiftingTrustApplication GenerateGiftingTrustBox(EstateProjection projection, bool isFirstYear, bool isLastYear)
        {
            GiftingTrustApplication giftingTrustViewModel = new GiftingTrustApplication();
            giftingTrustViewModel.Title = "Gifting Trust(s)";
            giftingTrustViewModel.Value = projection.GiftingTrustValue;

            if (!isLastYear)
            {
                int numberOfAliveSpouses = isFirstYear ? 2 : 1;
                giftingTrustViewModel.AnnualGifts = projection.AnnualGiftExclusionAmount * this.CurrentScenario.Options.NumberOfAnnualGiftsPerYear * numberOfAliveSpouses;
                giftingTrustViewModel.AnnualFeesRate = (float)this.CurrentScenario.Options.InvestmentFeesRate;
                giftingTrustViewModel.AnnualGrowthRate = (float)this.CurrentScenario.Options.InvestmentsGrowthRate;
            }

            return giftingTrustViewModel;
        }

        private LifeInsuranceApplication GenerateIlitBox(string name, decimal value, decimal annualPremium, bool isLastYear, bool isInTrust)
        {
            LifeInsuranceApplication lifeInsuranceViewModel = new LifeInsuranceApplication();
            lifeInsuranceViewModel.Title = string.Format("Life Ins. - {0}", name);
            lifeInsuranceViewModel.Value = value;

            if (!isLastYear)
            {
                if (annualPremium > 0)
                {
                    lifeInsuranceViewModel.AnnualPremium = annualPremium;
                }
                else
                {
                    lifeInsuranceViewModel.AnnualFeesRate = (float)this.CurrentScenario.Options.InvestmentFeesRate;
                    lifeInsuranceViewModel.AnnualGrowthRate = (float)this.CurrentScenario.Options.InvestmentsGrowthRate;
                }
            }

            lifeInsuranceViewModel.IsInTrust = isInTrust;

            return lifeInsuranceViewModel;
        }
    }
}
