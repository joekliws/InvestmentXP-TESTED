# Teste Técnico XP - API de compra e venda de ações

API construída em .NET 6, simulando criação de conta em uma corretora, compra venda de ações e deposito e saque


## Table of Contents

* [Prerequisites](#prerequisites)
* [General info](#general-info)
* [Installation](#installation)
* [Usage](#usage)
*[Tests](#tests)


## Prerequisites
Para implementação da API é necessário:

SDK [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

[Microsoft SQL Server Express](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)

OS:
#### Windows: 10/11 Server 2012 e 
#### Linux: Ubuntu 20.04

Para instalação do MS SQL server e da SDK acesse os Links abaixo.

[Instalando SQL Server no Ububtu](https://docs.microsoft.com/pt-br/sql/linux/quickstart-install-connect-ubuntu?view=sql-server-ver16)

[Instalando o .NET 6 no Uubuntu](https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu)

VS Code ou MS Visual Studio

## General Info
Foram criados Endpoints para criação e leituras de dados como: criação de conta, consultas de saldo, depósitos, saques, compras e vendas de ativos


## Installation
Após a instalação do banco e sdk, é possível rodar a aplicação tanto pela IDE do Visual Studio, como pelo terminal.

No terminal, navegue até o diretorio do projeto `InvestingXP`, abra o diretório `Investment.API`. Abra o terminal ou Powershell no diretorio informado anteriormente e rode o commando:

```bash
dotnet run
```
Dai poderar executar a API localmente pelo endereço `https://localhost:7130/swagger` pelo browser ou utilizar o postman.


## Usage

A API contém as Rotas:


### `api/auth`
- POST `api/auth/login` :
A pessoa usuária faz o login e recebe um token;

A Requisição recebe os parâmetros no formato:

```json
{
   "UserLogin": "string",
   "Password": "string"
}
```
- O parâmetro `UserLogin` pode ser o **_cpf_** ou **_Número da Conta_** da pessoa usuária;
- Com os parametros corretos, são gerados: a lista de claims (informações da pessoa a logar), a chave, as credenciais e no fim o token;
- O token é retornado junto o `status 200`;
- O token expira em 5 minutos depois de gerado;

### `ativo`
Onde são feitas as consultas de Ativos ou Ativos adquiridos. Para retornar é necessário ter o token. O token não pode está expirado ou ter outra credencial.
- GET`ativo/cliente/{cod-cliente}` :
Retorna a lista de ativos da pessoa logada.

A resposta da requisição é no seguinte formato:

```json
[
  {
    "CodAtivo": 0, // integer
    "QtdeAtivo": 0, // integer
    "Valor": 0, // decimal
    "CodCliente": 0 // integer
  }
]
```
- GET`ativo/{cod-ativo}`: 
Retorna o ativo especificado pelo _id_ na requisição; 

```json
  {
    "CodAtivo": 0, // integer
    "QtdeAtivo": 0, // integer
    "Valor": 0, // decimal
  }

```
- **codCliente**  código de identificação única da pessoa cliente
- **CodAtivo** código de identificação única do ativo
- **QtdeAtivo** quantidade de ações que a pessoa cliente possui
- **Valor** Valor da ação
#
### `conta`
Requisição para depósitos e saques e consulta de saldo. Para essa rota é necessário ter o token.  

- POST `conta/deposito` : Para depósito, Quantidade a ser depositada não poderá ser negativa ou igual a zero.
  - Retorna a mensagem `Compra realizada com sucesso`; 
- POST `conta/saque`: Para a requisição de saque é necessário que a conta da pessoa logada tenha saldo maior que a quantidade solicitada e a quantidade deve ser maior que 0. 
  - Retorna a mensagem `Venda realizada com sucesso`; 
  - O corpo da requisição tem os parâmetros: 
```json
  {
    "CodCliente": 0, // integer
    "Saldo": 0, // decimal
  }
```
- **CodCliente** ID da pessoa cliente possui
- **Valor** Valor da ação


- GET `conta/{cod-cliente}`: Retorna o Id e o saldo do usuário.
```json
  {
    "CodCliente": 0, // integer
    "Saldo": 0, // decimal
  }
```
#

### `investimentos`
O endpoint recebe como entradas o código do ativo,

 a quantidade de ações compradas, número da conta compradora.

- POST `investimentos/comprar` :
O endpoint recebe como
entradas o código do ativo, a
quantidade de ações compradas,
número da conta compradora.
No ato da compra, a Quantidade a ser comprada é subtraída, do volume da corretora.
 
  - Quantidade de ativo a ser comprada não pode ser maior que a quantidade disponível na corretora;
  - Compra de Ativo de ativo não pode ser feita fora dos horários 1:00pm-8:55pm(UTC) 10:00 as 17:55(UTC-3);
  - Compra de Ativo de ativo não pode ser feita no sábado nem domingo;

- POST `investimentos/vender` :
O endpoint recebe como
entradas o id do ativo, a
quantidade de ações vendidas,
número da conta vendedora.
  - A Quantidade de ativo a ser vendida não pode ser maior que a quantidade disponível na carteira
  - Venda de Ativo de ativo não pode ser feita fora dos horarios 1:00pm-8:55pm(UTC) 10:00 as 17:55(UTC-3)
  - Venda de Ativo de ativo não pode ser feita no sábado nem domingo
  - Adicionar da conta do cliente

O corpo da requisição para qualquer das duas rotas deve seguir o formato:
```json
  {
    "CodCliente": 0, // integer
    "CodAtivo": 0,  // integer
    "QtdeAtivo": 0, // integer
  }
```
Com os parametros corretos a requisição retorna o `staus 200` e mensagem `Operação realizada com sucesso`.
