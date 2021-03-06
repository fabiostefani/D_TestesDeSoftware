# Testes De Software

Dando continuidade a Formação de Arquiteto de Software da plataforma [Desenvolvedor.io](https://desenvolvedor.io/), nesse curso de ***Testes de Software*** iremos ver conceitos e práticas que devem ser aplicadas durante o ciclo de desenvolvimento do software. 

Alguns tópicos abordados, dentre outros:
* Porque testar
* Como testar
* Tipos de testes
* Frameworks (MSTeste, nUnit, xUnit)
* TDD
* Testes de Integração
* Etc

Como framework de testes, será utilizado o [XUnit.net](https://xunit.net/). É um framework free, open-source, baseado no nUnit e que também é utilizado pela própria Microsoft para testes do ASP.NET Core. 👌

## Stack utilizada

* [XUnit.net](https://xunit.net/): framework de testes
* [Bogus](https://github.com/bchavez/Bogus): gerador de dados humanizados.
* [Moq](https://github.com/moq/moq4): utilizado para fazer o moq (fake) de objetos/interfaces.
* [AutoMock](https://github.com/moq/Moq.AutoMocker): utilizado para automatizar a injeção de dependências nos objetos a serem construídos.
* [FluentAssertions](https://fluentassertions.com/): utilizado para escrever os asserts dos seus testes de forma mais fluida, gerando quase uma documentação deles.



## Comandos de CLI para o xUnit

O comando para criar um projeto de Testes de Unidade com xUnit via cli é

```
dotnet new xunit --name <NOME>
```
E para rodar os testes, você pode executar para a solution ou para um projeto em específico.
```
dotnet test <sln>
dotnet test <project de testes>
```


## Funcionalidades
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
* **ORDER:** utilizado para definir a ordem de execução dos testes. Mas convenhamos, se você tiver que definir ordem de execução em testes de unidade, provavelmente tem um problema de arquitetura com um alto acoplamento. Em alguns cenários até se faz necessário, por exemplo, em uma integração. Mas use com moderação.
Para utilização, você deve preparar um Attribute que irá identificar a ordem de execução. Você pode verificar a definição desse atributo na classe *PriorityOrderer* no projeto de testes.
E depois, você deve decorar o método de testes com a prioridade que você deseja executar
```
[Fact(DisplayName = "Teste 01"), TestPriority(2)]
[Trait("Categoria", "Ordenacao Testes")]
public void Teste01()
{
    // Arrange
        
    // Act
    
    // Assert 
}
```
E decorar a classe de testes que deseja ter a ordenação com o atributo *TestCaseOrderer*. Deve ser informado o namespace da classe que define a ordem de execução dos testes *(fabiostefani.io.ClienteTests.Order.PriorityOrderer)* e o namespace do projeto de testes *(fabiostefani.io.ClienteTests)*.
```
[TestCaseOrderer("fabiostefani.io.ClienteTests.Order.PriorityOrderer", "fabiostefani.io.ClienteTests")]
public class OrdemTestes
{
    
}
```


### **Bogus**

O Bogus é um gerador de dados humanizados. Muito poderoso. Ideal para a geração de dados aleatórios em seus testes. Isso fará você não ter testes viciados. No [Github](https://github.com/bchavez/Bogus) do projeto tem a documentação de como utilizar e suas funcionalidades.

Para instalar o bogus, no projeto de testes.
```
dotnet add package bogus
```

### **Moq**

O Moq é um instanciador Fake de objetos que você precisaria utilizar em construtores. Por exemplo, na instância da classe A, é injetado no construtor o service B. O Moq será responsável em gerar essa instância do service B. [Aqui](https://github.com/Moq/moq4/wiki/Quickstart) tem a documentação do projeto de como utilizar e suas funcionalidades.

Para instalar o moq, no projeto de testes.
```
dotnet add package moq
```

### **Automock**

O Automock é um automatizador da injeção de dependências do objeto que estou gerando fake. Ele cria as interfaces sem eu estar me preocupando com elas. Só um detalhe, você deve informar para ele que deseja construir a classe concreta. [Aqui](https://github.com/moq/Moq.AutoMocker) tem a documentação do projeto de como utilizar e suas funcionalidades.

Para instalar o AutoMock, no projeto de testes.
```
dotnet add package moq.automock
```

### **FluentAssertions**

O FluentAssertions é uma extensão utilizada para deixar de forma fluída os asserts. Você vai escrevendo os asserts de uma forma muito legível. Não é obrigatório utilizar. 
[Aqui](https://fluentassertions.com/introduction) tem a documentação do projeto e [aqui](https://github.com/fluentassertions/fluentassertions) o projeto no GitHub caso deseje conhecer um pouco mais o recurso.


Para instalar o FluentAssertions, no projeto de testes.
```
dotnet add package fluentassertions
```

### **Coverlet**

O [Coverlet](https://github.com/coverlet-coverage/coverlet) é utilizado para se calcular a cobertura de testes do seu projeto. A cobertura de testes indica o alcance da execução dos testes em um projeto. Se metade do código existente for executado durante o processamento, teremos uma cobertura de 50%. Testar absolutamente tudo e chegar a 100% nem sempre é possível. 
Quando se instala o xUnit, já é adicionado essa package ao projeto.
Para se executar a cobertura, deve ser executado o comando no projeto que desejo realizar os testes.

```
dotnet test --collect:"XPlat Code Coverage
```
Será gerado um arquivo XML na pasta TestResults com os dados da cobertura de testes. 
Com base nesse XML, é possível gerar um relatório em HTML através do [ReportGenerator](https://github.com/danielpalme/ReportGenerator) com a apresentação dos resultados. Para o funcionamento, devemos instalar a ferramenta
```
dotnet tool install --global dotnet-reportgenerator-globaltool
```
Após isso, deve ser acessado o diretório que foi gerado o XML com o resultado do Coverlet e executar o comando
```
reportgenerator "-reports:coverage.cobertura.xml" "-targetdir:coveragereport" -reporttypes:Html
```
Teremos a geração do diretório coveragereport contendo o site estático em HTML como resultado. Para visualizar, deve ser aberto o index.html.

### **Testes de Integração**

Serve para garantir que a aplicação está realizando a integração entre as camadas do sistema e garantir que as operações estão sendo realizadas com sucesso. Deve iniciar a aplicação e simular as operações como se fosse o usuário executando.