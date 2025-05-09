namespace ProjectVersion2.Utilities
{
    public enum Role
    {
        User,
        Admin,
        Guest
    }

    public enum ExpenseStatus
    {
        Pending,
        Approved,
        Rejected,
        Edited,
        Requested,
        Completed,
        InProgress
    }

    public enum SalaryType
    {
        Fixed,
        Variable,
        Commission,
        Bonus,
        Overtime
    }

    public enum PaymentMethod
    {
        Cash,
        CreditCard,
        DebitCard,
        BankTransfer,
        JazzCash,
        EasyPaisa,
        Other
    }

    public enum ExpenseCategories
    {
        Food,
        Transport,
        Entertainment,
        Utilities,
        Health,
        Education,
        Travel,
        Other
    }
}