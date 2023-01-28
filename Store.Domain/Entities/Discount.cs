namespace Store.Domain.Entities
{
    public class Discount : BaseEntity
    {
        public Discount(decimal amount, DateTime expireDate)
        {
            Amount = amount;
            ExpireDate = expireDate;
        }

        public decimal Amount { get; private set; }
        public DateTime ExpireDate { get; private set; }

        public bool IsExpireDateValid()
        {
            return DateTime.Compare(DateTime.Now, ExpireDate) < 0;
        }

        public decimal Value()
        {
            if (IsExpireDateValid())
                return Amount;
            else
                return 0;
        }
    }
}