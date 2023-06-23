using EstateViewWebAPIServer.Models;

namespace EstateViewWebAPIServer.Interfaces
{
    public interface IOption
    {
        public EstateProjectionOptions GetInitialOptions();
    }

    public class COption : IOption
    {
        public EstateProjectionOptions GetInitialOptions()
        {
            return EstateProjectionOptions.CreateEmptyOptions();
        }
    }
}
