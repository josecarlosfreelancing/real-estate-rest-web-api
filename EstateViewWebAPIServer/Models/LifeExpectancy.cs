namespace EstateViewWebAPIServer.Models
{
    public class LifeExpectancy
    {
        public int AttainedAge { get; private set; }
        public decimal MaleNonSmoker { get; private set; }
        public decimal MaleSmoker { get; private set; }
        public decimal FemaleNonSmoker { get; private set; }
        public decimal FemaleSmoker { get; private set; }

        public LifeExpectancy(int attainedAge, decimal maleNonSmoker, decimal maleSmoker, decimal femaleNonSmoker, decimal femaleSmoker)
        {
            this.AttainedAge = attainedAge;
            this.MaleNonSmoker = maleNonSmoker;
            this.MaleSmoker = maleSmoker;
            this.FemaleNonSmoker = femaleNonSmoker;
            this.FemaleSmoker = femaleSmoker;
        }
    }
}
