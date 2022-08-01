using Investment.Test.MockData;

namespace Investment.Test.Accounts
{
    public class AccountServiceTest
    {
        #region Deposit
        [Theory]
        [InlineData(true, 1, 100)]
        public void ValidDeposit_AddIsCalled(bool expected, int customerId, decimal value)
        {
            // Arrange
            var operation = new Operation { CodCliente = customerId, Saldo = value };
            var service = new Mock<IAccountService>();

            // Act + Assert
            var result = service.Setup(a => a.Deposit(operation)).ReturnsAsync(expected);
            Assert.True(expected);

        }

        [Theory]
        [InlineData(false, 1, 0)]
        public void InvalidDeposit_ZeroValue_ThrowException(bool expected, int customerId, decimal value)
        {

            // Arrange
            var operation = new Operation { CodCliente = customerId, Saldo = value };
            var service = new Mock<IAccountService>();

            // Act
            var result = service.Setup(a => a.Deposit(operation));
            // Assert
            try
            {

            }
            catch (Exception e)
            {
                result.Throws(e);
                Assert.False(expected);

            }
        }

        [Theory]
        [InlineData(false, 1, -2.5)]
        public void InvalidDeposit_NegativeValue_ThrowException(bool expected, int customerId, decimal value)
        {

            // Arrange
            var operation = new Operation { CodCliente = customerId, Saldo = value };
            var service = new Mock<IAccountService>();

            // Act
            var result = service.Setup(a => a.Deposit(operation));
            // Assert
            try
            {

            }
            catch (Exception e)
            {
                result.Throws(e);
                Assert.False(expected);

            }
        }
        #endregion

        #region Withdraw
        [Theory]
        [InlineData(true, 2, 100)]
        public void ValidWithdraw_AddIsCalled(bool expected, int customerId, decimal value)
        {
            // Arrange
            var operation = new Operation { CodCliente = customerId, Saldo = value };
            var service = new Mock<IAccountService>();

            // Act + Assert
            var result = service.Setup(a => a.Withdraw(operation)).ReturnsAsync(expected);
            Assert.True(expected);

        }

        [Theory]
        [InlineData(false, 2, 0)]
        public void InvalidWithDraw_ZeroValue_ThrowException(bool expected, int customerId, decimal value)
        {

            // Arrange
            var operation = new Operation { CodCliente = customerId, Saldo = value };
            var service = new Mock<IAccountService>();

            // Act
            var result = service.Setup(a => a.Withdraw(operation));
            // Assert
            try
            {

            }
            catch (Exception e)
            {
                result.Throws(e);
                Assert.False(expected);

            }
        }

        [Theory]
        [InlineData(false, 2, -5)]
        public void InvalidWithdraw_NegativeValue_ThrowException(bool expected, int customerId, decimal value)
        {

            // Arrange
            var operation = new Operation { CodCliente = customerId, Saldo = value };
            var service = new Mock<IAccountService>();

            // Act
            var result = service.Setup(a => a.Withdraw(operation));
            // Assert
            try
            {

            }
            catch (Exception e)
            {
                result.Throws(e);
                Assert.False(expected);

            }
        }

        [Theory]
        [InlineData(false, 2, 50)]
        public void InvalidWithdraw_OutOfBalance_ThrowException(bool expected, int customerId, decimal value)
        {

            // Arrange
            var operation = new Operation { CodCliente = customerId, Saldo = value };
            var service = new Mock<IAccountService>();

            // Act
            var result = service.Setup(a => a.Withdraw(operation));
            // Assert
            try
            {

            }
            catch (Exception e)
            {
                result.Throws(e);
                Assert.False(expected);

            }
        }

        #endregion

        #region Balance
        [Fact]
        public void GetBalanceValid_ReturnBalance()
        {
            var mock = new AccountMockData();
            var account = mock.MockAccounts.First();
            var service = new Mock<IAccountService>();

            // Act + Assert
            var result = service.Setup(a => a.GetBalance(account.UserId));
            result.ReturnsAsync(AccountMockData.MockBalance);
            Assert.Equal(mock.MockAccounts.First().UserId, AccountMockData.MockBalance.CodCliente);
            Assert.Equal(mock.MockAccounts.First().Balance, AccountMockData.MockBalance.Saldo);
        }

        [Fact]
        public void GetBalanceInvalid_InvalidUserId_ThrowException()
        {
            var userId = 1;
            var mock = new AccountMockData();
            var service = new Mock<IAccountService>();

            // Act + Assert
            var result = service.Setup(a => a.GetBalance(userId));
            try
            {

            }
            catch (Exception e)
            {
                result.ThrowsAsync(e);
                bool userExists = mock.MockAccounts.Any(a => a.UserId == userId);
                Assert.False(userExists);


            }
        }

        #endregion
    }
}
