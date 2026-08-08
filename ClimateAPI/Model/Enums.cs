namespace CCDbApi.Model
{
    public class Enums
    {
    }
    public enum Status
    {
        Published = 1,
        Draft = 2,
        Trash = 3,
        Shedule = 4,
    }
    public enum CommentStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }
    public enum PaymentMethod
    {
        Cash = 1,
        BankTransfer = 2,
        CreditCard = 3,
        DebitCard = 4,
        Cheque = 5,
        Online = 6
    }
    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        PartiallyPaid = 3,
        Failed = 4,
        Refunded = 5
    }
  
    public enum OrderStatus
    {
        Pending = 1,
        Confirmed = 2,
        Fullfilled = 3,
        Cancelled = 4
    }
}
