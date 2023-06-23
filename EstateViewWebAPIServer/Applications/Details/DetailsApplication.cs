using EstateViewWebAPIServer.Models.Scenarios;
using EstateViewWebAPIServer.Models;
using Newtonsoft.Json;

namespace EstateViewWebAPIServer.Applications.Details
{
    public class DetailsApplication : BaseApplication
    {
        public string Name
        {
            get
            {
                return this.Scenario.Name;
            }
            set
            {
                this.Scenario.Name = value;
            }
        }

        [JsonIgnore]
        public EstateProjectionOptions Options
        {
            get { return this.GetValue(() => this.Options); }
            private set { this.SetValue(() => this.Options, value); }
        }

        [JsonIgnore]
        public EstatePlanningScenario Scenario
        {
            get { return this.GetValue(() => this.Scenario); }
            set { this.SetValue(() => this.Scenario, value); }
        }

        public PersonApplication Spouse1
        {
            get { return new PersonApplication(this.Options.Spouse1); }
        }

        public PersonApplication Spouse2
        {
            get { return new PersonApplication(this.Options.Spouse2); }
        }

        public IEnumerable<EstateProjection> FilteredProjections
        {
            get { return this.GetValue(() => this.FilteredProjections); }
            private set { this.SetValue(() => this.FilteredProjections, value); }
        }

        public DetailsApplication(EstatePlanningScenario scenario, EstateProjectionOptions options)
        {
            this.Bind(scenario, options);
        }

        public void Bind(EstatePlanningScenario scenario, EstateProjectionOptions options)
        {
            this.Scenario = scenario;
            this.Options = options;
            this.Scenario.UpdateProjections();

            this.FilteredProjections = 
                scenario.Projections
                .Where(projection =>
                    projection.YearNumber % 5 == 0 ||
                    projection.Year == this.Options.Spouse1.ProjectedYearOfDeath ||
                    projection.Year == this.Options.Spouse2.ProjectedYearOfDeath);
        }
    }
}
