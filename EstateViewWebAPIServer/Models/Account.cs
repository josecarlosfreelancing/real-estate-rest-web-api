namespace EstateViewWebAPIServer.Models
{
    public class Account
    {
        private readonly List<Transaction> transactions;
        public string Name { get; private set; }

        public Account(string name)
        {
            this.Name = name;
            this.transactions = new List<Transaction>();
        }

        public IEnumerable<Transaction> Transactions
        {
            get { return this.transactions; }
        }

        public void Credit(int year, decimal amount, string description)
        {
            if (amount != 0)
            {
                this.transactions.Add(new Transaction(year, amount, this.GetBalance(year) + amount, description));
            }
        }

        public void Debit(int year, decimal amount, string description)
        {
            if (amount != 0)
            {
                this.transactions.Add(new Transaction(year, -amount, this.GetBalance(year) - amount, description));
            }
        }

        public void Grow(int year, decimal percent, string description)
        {
            if (percent != 0)
            {
                this.Credit(year, this.GetBalance(year) * percent, description);
            }
        }

        public decimal GetBalance(int year)
        {
            return this.transactions.Where(t => t.Year <= year).Sum(t => t.Amount);
        }
    }
}
