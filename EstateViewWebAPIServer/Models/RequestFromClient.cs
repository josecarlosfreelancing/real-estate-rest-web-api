namespace EstateViewWebAPIServer.Models
{
    public class RequestFromClient
    {
        public EstateProjectionOptions Options { get; set; }
        public string Name { get; set; }

        public RequestFromClient()
        {
            this.Options = new EstateProjectionOptions();
            this.Name = "BOTH DIE THIS YEAR - NO PLANNING";
        }
    }
}
