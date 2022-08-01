namespace Investment.Test.MockData
{
    public class AccountMockData
    {
        public List<Account> MockAccounts = new List<Account>
        {
            new Account { AccountId = 1, AccountNumber = 762407, UserId = 3, Balance = 1000},
            new Account { AccountId = 2, AccountNumber = 13820, UserId = 6, Balance = 627.90m},
            new Account { AccountId = 3, AccountNumber = 55480, UserId = 8, Balance = 0m},
            new Account { AccountId = 5, AccountNumber = 504858, UserId = 10, Balance = 2000},
            new Account { AccountId = 8, AccountNumber = 10676, UserId = 26, Balance = 455.65m}
        };

        public static Operation MockBalance = new Operation { CodCliente = 3, Saldo = 1000 };
    }
}
