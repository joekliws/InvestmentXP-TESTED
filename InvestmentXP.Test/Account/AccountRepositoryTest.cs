using Investment.Infra.Repository;
using Investment.Test.MockData;

namespace Investment.Test.Accounts
{
    public class AccountRepositoryTest
    {
        #region Deposit

        [Fact]
        public void UpdateBalance_Deposit_Succefull()
        {
            // Arrange
            var mock = new AccountMockData();
            var account = mock.MockAccounts.First();
            account.Balance += 500;
            var repo = new Mock<IAccountRepository>();

            // Act + Assert
            var result = repo.Setup(a => a.UpdateBalance(account)).ReturnsAsync(account.Balance == 1500);
        }

        [Fact]
        public void UpdateBalance_Deposit_ThrowException()
        {

            // Arrange
            var mock = new AccountMockData();
            var userId = 2;
            var account = mock.MockAccounts.FirstOrDefault(a=>a.UserId == userId);
            var repo = new Mock<IAccountRepository>();

            // Act + Assert
            var result = repo.Setup(a => a.UpdateBalance(account));
            try
            {

            }
            catch (Exception e)
            {
                result.Throws(e);
                var accountExists = mock.MockAccounts.Any(a => a.UserId == userId);
                Assert.False(accountExists);

            }
        }
        #endregion

        #region Withdraw
        [Fact]
        public void UpdateBalance_Withdraw_Succefull()
        {
            // Arrange
            var mock = new AccountMockData();
            var account = mock.MockAccounts.First(a=>a.UserId == 26);
            account.Balance -= 100;
            var repo = new Mock<IAccountRepository>();

            // Act + Assert
            var result = repo.Setup(a => a.UpdateBalance(account)).ReturnsAsync(account.Balance == 355.65m);
        }

        [Fact]
        public void UpdateBalance_Withdraw_OutOfBalance_ThrowException()
        {
            // Arrange
            var mock = new AccountMockData();
            var userId = 26;
            var account = mock.MockAccounts.First(a => a.UserId == userId);
            account.Balance -= 500;
            var repo = new Mock<IAccountRepository>();

            // Act + Assert
            var result = repo.Setup(a => a.UpdateBalance(account));
            try
            {

            }
            catch (Exception e)
            {
                result.Throws(e);
                var outOfBalance = account.Balance > 0;
                Assert.False(outOfBalance);

            }
        }

        #endregion

        #region Account
        [Fact]
        public void GetByCustomerId_Succefull()
        {

            // Arrange
            var mock = new AccountMockData();
            var userId = 8;
            var account = mock.MockAccounts.First(a => a.UserId == userId);

            var repo = new Mock<IAccountRepository>();

            // Act + Assert
            var result = repo.Setup(a => a.GetByCustomerId(userId)).ReturnsAsync(account);  
        }

        [Fact]
        public void GetByCustomerId_InvalidUser_ThrowException()
        {

            // Arrange
            var mock = new AccountMockData();
            var userId = 18;
            var repo = new Mock<IAccountRepository>();

            // Act + Assert
            var result = repo.Setup(a => a.GetByCustomerId(userId));
            // Assert
            try
            {

            }
            catch (Exception e)
            {
                result.Throws(e);
                var account = mock.MockAccounts.FirstOrDefault(a => a.UserId == userId);
                Assert.Null(account);

            }
        }
        #endregion

    }
}
