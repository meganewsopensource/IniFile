# Ini File Serializer


Ini file lightweight framework

Obs.: Usar somente classes com fields privadas

## Definindo a Seção do Ini
* Para definir a seção do ini especifique o atributo na classe
 ```csharp
  [IniSection("sectionName")] 
```

## Definindo as Propriedades do Ini
* Para definir a propridade que será gerada no arquivo use o atributo
 ```csharp
  [IniProperty("sectionName")] 
```
### Definindo Valor padrão
* Para especificar um valor padrão para a propriedade quando ela não for preenchida (nula) use o parâmetro defaultValue do atributo:
```csharp
  [IniProperty("sectionName",10)] 
  private int? valorInteiro;
```

### Definindo Propriedade Não Obrigatória
* Por padrão o IniProperty define uma propriedade como obrigatória, para definir como "Não obrigatória", use o terceiro parâmetro "required = false":
```csharp
  [IniProperty("sectionName",10,false)] 
  private int? valorInteiro;
```
---
  ### Exemplo Completo:
  ~~~csharp
  [IniSection("MySection")]
  public class MyClass
  {
      [IniProperty("ValorInteiro",10,false)] 
      private int? valorInteiro;
  }
  ~~~

  Conteudo do Arquivo Ini:
  ~~~ini
  [MySection]
  ValorInteiro=10
  ~~~
  ---


## Serializando a classe:

  ### Opção 1: Herança de IniFileSerializer
  * A instancia da classe já conterá a geração do conteúdo ini pelo método ToString()
    ~~~csharp
    [IniSection("MySection")]
    public class MyClass : IniFileSerializer
    {
        [IniProperty("ValorInteiro",10,false)] 
        private int? valorInteiro;
    }
    ~~~

    Serializar:
    ~~~csharp
      //Serialização
      var instance = new MyClass();
      var iniContent = instance.ToString();
    ~~~

### Opção 2: Classe Estática de Serialização
  * Chame o método estático ObjectToIni do Namespace IniFile:

    ~~~csharp
      var instance = new MyClass();
      var content = IniFile.ObjectToIni(instance);
    ~~~

---

## Usando Propriedades de Tipos Complexos

### Propriedades do tipo classe
* Deve ser usado o mesmo atributo iniProperty

  ~~~csharp
  Modelos de dados

  [IniSection("MySection")]
  public class MyClass : IniFileSerializer
  {
      [IniProperty("ValorInteiro",10,false)] 
      private int? valorInteiro;

      [IniProperty("MyNestedSection")] //O caption especificado no atributo será o nome da Seção do Ini
      private MyNestedClass NestedClass;
  }

  public class MyNestedClass //classe filha. Não é necessário especificar o atributo IniSection.
  {
      [IniProperty("NestedProperty1","Teste2",false)] 
      private string NestedProperty1;
      
      [IniProperty("NestedProperty2","Teste1",false)] 
      private string NestedProperty2;
  }

  ~~~

  ~~~ini
    #conteúdo do ini 
    [MySection]
    ValorInteiro=10

    [MyNestedSection]
    NestedProperty1=Teste2
    NestedProperty2=Teste1
  ~~~

### Propriedades do tipo Enumerable (Listas)

* Também deve ser usado o mesmo atributo iniProperty




  ~~~csharp
  [IniSection("MySection")]
  public class MyClass : IniFileSerializer
  {
      [IniProperty("ValorInteiro",10,false)] 
      private int? valorInteiro;

      [IniProperty("Item")] 
      [ListIndexFormat("0000")] //Atributo para especificar o formato de indice dos itens. No exemplo, formatando o índice para 4 dígitos. Por padrão são 3 dígitos se não for especificado.
      private List<ItemClass> Items = new List<ItemClass>()
      {
          new ItemClass(),
          new ItemClass(),
      };
  }

  public class ItemClass //classe filha. Não é necessário especificar o atributo IniSection.
  {
      [IniProperty("NestedProperty1","Teste2",false)] 
      private string NestedProperty1;
      
      [IniProperty("NestedProperty2","Teste1",false)] 
      private string NestedProperty2;

      [IniProperty("NestedSectionInItem")]
      private MyNestedClass NestedSection = new  MyNestedClass();
  }
  ~~~

  ~~~ini
  [MySection]
  ValorInteiro=10

  [Item001]
  NestedProperty1=Teste2
  NestedProperty2=Teste1

  [NestedSectionInItem]
  NestedProperty1=Teste2
  NestedProperty2=Teste1

  [Item002]
  NestedProperty1=Teste2
  NestedProperty2=Teste1
    ~~~

---

### Atributos de Formatação

~~~csharp
[IniSection("DataFormats")]
public class MyDataFormatsClass : IniFileSerializer
{
  [IniProperty("valDouble",10.5345)]
  [FormatNumeric(3)] //Formatando valores float para 3 casas decimais no padrão americano com "." (ponto) como separador de decimal. No segundo parâmetro pode ser especificado outra CultureInfo.
  private double? valorDouble;

  [IniProperty("valDate")]
  [FormatDate] //Exibe somente data no formato dd/MM/yyyy
  private DateTime valorDate;

  [IniProperty("valDateTime")]
  [FormatDateAndTime] //Exibe data e hora no formato dd/MM/yyyy HH:mm:ss
  private DateTime valorDataEHora;

  [IniProperty("valHora")]
  [FormatTime] //Exibe somente a hora no fomrato HH:mm:ss
  private DateTime valorHora;
}

~~~

~~~ini
#conteúdo do arquivo ini:
[DataFormats]
valDouble=10.534
valDate=23/10/2024
valDateTime=23/10/2024 12:14:17
valHora=12:14:17
~~~

