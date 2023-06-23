namespace EstateViewWebAPIServer.Applications.Logistics
{
    public class YearTaxApplication
    {
        public int DeltaYear { get; set; }
        public EstateApplication Estate { get; set; }
        public BypassTrustApplication BypassTrust { get; set; }
        public IList<GiftingTrustApplication> GiftingTrusts { get; set; }
        public IList<LifeInsuranceApplication> Ilits { get; set; }
        public IList<InstallmentSaleApplication> InstallmentSaleTrusts { get; set; }
    }
}
