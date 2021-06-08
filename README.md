# Testes De Software

Dando continuidade a Formação de Arquiteto de Software da plataforma [Desenvolvedor.io](https://desenvolvedor.io/), nesse curso de ***Testes de Software*** iremos ver conceitos e práticas que devem ser aplicadas durante o ciclo de desenvolvimento do software. 

Alguns tópicos abordados, dentre outros:
* Porque testar
* Como testar
* Tipos de testes
* Frameworks (MSTeste, nUnit, xUnit)
* Etc

Como framework de testes, será utilizado o [XUnit.net](https://xunit.net/). É um framework free, open-source, baseado no nUnit e que também é utilizado pela própria Microsoft para testes do ASP.NET Core. 👌

## Stack utilizada

* [XUnit.net](https://xunit.net/)

### Comandos de CLI para o xUnit

Criação de projeto de Testes

```
dotnet new xunit --name <NOME>
```

### Funcionalidades
Possui algumas caracteristicas bem interessantes para o desenvolvimento do Teste de Unidade.
* **FACT:** é o mais simples teste de Unidade. Baste somente decorar o método com *[FACT]* que ele já entende que é um teste de Unidade.
```
[Fact]
public void Calculadora_Somar_RetornarValorSoma()
{
    // Arrange
    
    // Act
    
    // Assert
    
}
```
* **THEORY:** é uma teoria aonde você define uma série de valores que quer testar. Esses valores serão recebidos no método e utilizado nos casos de testes. Basta decorar o método com *[Theory]* e adicionar os valores que deseja estar testando com o decorator *[InlineData()]*.
```
[Theory]
[InlineData(1,1,2)]
[InlineData(2, 2, 4)]
[InlineData(4, 2, 6)]
public void Calculadora_Somar_RetornarValoresSomaCorretos(double v1, double v2, double total)
{
    // Arrange    

    // Act    

    // Assert    
}
```
* **TRAITS:** utilizado para melhorar a organização e legibilidade de seus testes no Test Explorer. Você define nomes de categorias e nome dos métodos de forma legível para quando precisar retornar nele, saber exatamente do que se trata.
```
[Fact(DisplayName = "Novo Cliente Válido")]
[Trait("Categoria","Cliente Trait Testes")]
public void Cliente_NovoCliente_DeveEstarValido()
{
    // Arrange
    
    // Act    

    // Assert     
}
```
Veja como fica a apresentação no Test Explorer do Visual Studio.

![Imgur](https://i.imgur.com/m0gi7aR.png)
* **FIXTURE:** serve para a geração de dados uma única vez durante a execução do testes. Por exemplo, para os testes de Clientes, você deve estar gerando Clientes em Situações Válidas e Situações Inválidas. Se você colocar isso no Construtor da Classe, será executado a cada teste. 
E se você ainda precisa desse mesmo cliente em outras classes de testes? O Fixture serve para resolver esse problema.
Você deve criar uma classe com os seus métodos de criação *(ClienteTestsFixture)* e definir a colection dessa classe de criação dos métodos *ClienteCollection*.
```
[CollectionDefinition(nameof(ClienteCollection))]
public class ClienteCollection : ICollectionFixture<ClienteTestsFixture>
{}

public class ClienteTestsFixture : IDisposable
{
    public Cliente GerarClienteValido()
    {
        return ....
    }

    public Cliente GerarClienteInValido()
    {
        return ....
    }

    public void Dispose()
    {
    }
}
```
Na classe que desejo utilizar essas collections, tenho que decorar a classe e informar qual a collection que desejo utilizar.
```
[Collection(nameof(ClienteCollection))]
public class ClienteTesteValido
{
    private readonly ClienteTestsFixture _clienteTestsFixture;

    public ClienteTesteValido(ClienteTestsFixture clienteTestsFixture)
    {
        _clienteTestsFixture = clienteTestsFixture;
    }
    
    [Fact(DisplayName = "Novo Cliente Válido")]
    [Trait("Categoria", "Cliente Fixture Testes")]
    public void Cliente_NovoCliente_DeveEstarValido()
    {
        // Arrange
        
        // Act
        
        // Assert 
    }
}
```

