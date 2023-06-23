namespace EstateViewWebAPIServer.Applications.Logistics
{
    public class GiftingTrustApplication
    {
        public string Title { get; set; }
        public decimal Value { get; set; }
        public decimal AnnualGifts { get; set; }
        public float AnnualGrowthRate { get; set; }
        public float AnnualFeesRate { get; set; }
        public bool IsInTrust { get; set; }
    }
}
