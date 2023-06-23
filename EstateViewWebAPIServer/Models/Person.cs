namespace EstateViewWebAPIServer.Models
{
    public class Person
    {
        public Person()
        {
            this.LifeInsurance = new LifeInsurancePolicy();
            this.ExistingLifeInsurance = new LifeInsurancePolicy();
            this.FirstName = "Bob";
            this.LastName = "Sample";
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Sex Sex { get; set; }
        public int Age { get; set; }
        public bool IsSmoker { get; set; }

        public LifeInsurancePolicy LifeInsurance { get; set; }
        public LifeInsurancePolicy ExistingLifeInsurance { get; set; }

        public int ProjectedYearOfDeath { get; set; }
        public decimal LifetimeGiftExclusionAmountUsed { get; set; }

        public string Name
        {
            get { return string.Format("{0} {1}", this.FirstName, this.LastName); }
        }

    }
}
