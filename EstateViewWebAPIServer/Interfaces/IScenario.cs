using EstateViewWebAPIServer.Models;
using EstateViewWebAPIServer.Models.Scenarios;
using EstateViewWebAPIServer.Constants;
using EstateViewWebAPIServer.Applications.Logistics;
using EstateViewWebAPIServer.Applications.Details;
using EstateViewWebAPIServer.Applications.Timeline;

namespace EstateViewWebAPIServer.Interfaces
{
    public interface IScenario
    {
        public EstatePlanningScenario GetScenario(RequestFromClient request);
        public EstateProjectionAccountBook GetAccounts(RequestFromClient request);
        public TimelineApplication GetTimelines(RequestFromClient request);
        public LogisticsApplication GetLogistics(RequestFromClient request);
        public DetailsApplication GetDetails(RequestFromClient request);
    }

    public class CScenario : IScenario
    {
        public EstatePlanningScenario GetScenario(RequestFromClient request)
        {
            EstatePlanningScenario scenario = request.Name switch
            {
                Scenario.BothDieThisYearNoPlanning => new BothDieThisYearNoPlanningScenario(request.Options, request.Name),
                Scenario.NoPlanning => new NoPlanningScenario(request.Options, request.Name),
                Scenario.BypassTrust => new BypassTrustScenario(request.Options, request.Name),
                Scenario.AnnualGifting => new AnnualGiftingScenario(request.Options, request.Name),
                Scenario.DiscountedGifting => new DiscountedGiftingScenario(request.Options, request.Name),
                Scenario.LifeInsurancePostPlanning => new LifeInsuranceScenario(request.Options, request.Name),
                Scenario.YearGiftInstallmentSale => new InstallmentSaleScenario(request.Options, request.Name),
                _ => new EstatePlanningScenario(request.Options, request.Name),
            };
            return scenario;
        }

        public EstateProjectionAccountBook GetAccounts(RequestFromClient request)
        {
            EstatePlanningScenario scenario = GetScenario(request);
            EstateProjectionAccountBook accounts = scenario.Accounts;
            return accounts;
        }

        public TimelineApplication GetTimelines(RequestFromClient request)
        {
            EstatePlanningScenario scenario = GetScenario(request);
            TimelineApplication timelines = new TimelineApplication(scenario);

            return timelines;
        }

        public LogisticsApplication GetLogistics(RequestFromClient request)
        {
            EstatePlanningScenario scenario = GetScenario(request);
            LogisticsApplication logistics = new LogisticsApplication(scenario);

            return logistics;
        }

        public DetailsApplication GetDetails(RequestFromClient request)
        {
            EstatePlanningScenario scenario = GetScenario(request);
            DetailsApplication details = new DetailsApplication(scenario, request.Options);

            return details;
        }
    }
}
