using System.Collections.ObjectModel;

namespace EstateViewWebAPIServer.Applications.Logistics
{
    public class TotalPassedToBeneficiariesApplication
    {
        public TotalPassedToBeneficiariesApplication()
        {
            this.Items = new ObservableCollection<TotalPassedToBeneficiariesItemApplication>();
        }

        public IList<TotalPassedToBeneficiariesItemApplication> Items { get; private set; }

        public decimal Total
        {
            get
            {
                return this.Items.Sum(item => item.Amount);
            }
        }
    }
}
