using Investment.Domain.Enums;

namespace Investment.Domain.DTOs
{
    public class AccountReadDTO
    {
        public int AccountNumber { get; set; }

        public UserReadDTO User { get; set; } = new UserReadDTO();

        public decimal Balance { get; set; }
    }

    public class UserReadDTO
    {
        [Required, MinLength(3), MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MinLength(3), MaxLength(150)]
        public string LastName { get; set; } = string.Empty;

        public string? PreferedName { get; set; }

        [Required]
        public RiskTolerance InvestorStyle { get; set; }

        [Required, MinLength(11), MaxLength(11)]
        public string Cpf { get; set; } = string.Empty;
    }

    public class AccountCreateDTO : UserReadDTO
    {
        [Required, MaxLength(8), MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }

}
