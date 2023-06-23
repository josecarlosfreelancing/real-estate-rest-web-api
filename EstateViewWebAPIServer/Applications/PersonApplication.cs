using EstateViewWebAPIServer.Models;

namespace EstateViewWebAPIServer.Applications
{
    public class PersonApplication : BaseApplication
    {
        private Person person;
        public PersonApplication(Person person)
        {
            this.Bind(person);
        }

        public string FirstName
        {
            get
            {
                return this.person.FirstName;
            }
            set
            {
                if (this.person.FirstName == value) return;
                this.person.FirstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return this.person.LastName;
            }
            set
            {
                if (this.person.LastName == value) return;
                this.person.LastName = value;
            }
        }

        public int Age
        {
            get
            {
                return this.person.Age;
            }
            set
            {
                if (this.person.Age == value) return;
                this.person.Age = value;
            }
        }

        public Sex Sex
        {
            get
            {
                return this.person.Sex;
            }
            set
            {
                this.person.Sex = value;
            }
        }

        public bool IsSmoker
        {
            get
            {
                return this.person.IsSmoker;
            }
            set
            {
                this.person.IsSmoker = value;
            }
        }

        public decimal LifetimeGiftExclusionAmountUsed
        {
            get
            {
                return this.person.LifetimeGiftExclusionAmountUsed;
            }
            set
            {
                if (this.person.LifetimeGiftExclusionAmountUsed == value) return;
                this.person.LifetimeGiftExclusionAmountUsed = value;
            }
        }

        public int ActurialYearOfDeath
        {
            get
            {
                return (int)(DateTime.Now.Year + this.LifeExpectancyNumber);
            }
        }

        public int ProjectedYearOfDeath
        {
            get
            {
                return this.person.ProjectedYearOfDeath;
            }
            set
            {
                this.person.ProjectedYearOfDeath = value;
            }
        }

        public string LifeExpectancy
        {
            get
            {
                decimal lifeExpectancy = this.LifeExpectancyNumber;

                if (lifeExpectancy == 0)
                {
                    return string.Format("No life expectancy data available for age {0}", this.Age);
                }

                decimal totalAge = this.Age + lifeExpectancy;
                return string.Format("An additional {0} years (total age {1})", lifeExpectancy, totalAge);
            }
        }

        public decimal LifeExpectancyNumber
        {
            get
            {
                return MortalityTable.GetLifeExpectancy(this.person.Age, this.person.Sex, this.person.IsSmoker);
            }
        }

        public void Bind(Person person)
        {
            this.person = person;
        }
    }
}
