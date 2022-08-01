namespace Investment.Domain.Constants
{
  public static class ErrorMessage
  {
    public static readonly string VALUE_LESS_ZERO = "Valor tem que ser maior que zero";
    public static readonly string ACCOUNT_NOT_FOUND = "Conta não encontrada com Id Informado";
    public static readonly string INVALID_BALANCE = "Valor a ser retirado não pode ser maior do que da carteira";
    public static readonly string ACCOUNT_NOT_CREATED = "Houve algum problema ao criar conta";
    public static readonly string ASSET_NOT_FOUND = "Ativo não encontrado com ID informado";
    public static readonly string OUT_OF_BALANCE = "Saldo insuficiente";
    public static readonly string ASSET_UNAVAILABLE = "Ativo não disponível";
    public static readonly string MARKET_CLOSED = "Mercado fechado. Disponível de Segunda a Sexta entre 10:00 e 17:55 UTC-3";
    public static readonly string INVALID_LOGIN = "Dados Inválidos ou inexistentes";
    public static readonly string TOKEN_INVALID = "Token invalido para essa operação";
    public static readonly string OUT_OF_ASSET = "Cliente não possui quantidade de ativos suficiente";
  }
}
