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
  * A instancia da classe já conterá a geração do conteúdo ini pelo método .ToString() ou .ToIniFile()
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


#### Classes aninhadas

```csharp
public class MDFe : IniFileSerializer
{
    [IniProperty("ide")]
    private Ide Ide;

    [IniProperty("perc")]
    private IEnumerable<Percurso> Percursos;

    [IniProperty("DESC")]
    private IEnumerable<Descarga> Desc;

    public MDFe()
    {
        Ide = new Ide(35, 1, 1, "58", "1");

        Percursos =
        [
            new Percurso("BA"),
            new Percurso("MG"),
        ];

        Desc =
        [
            new Descarga(
                3518701,
                "GURARUJA", 
                [
                    new InformacaoCte("98374949404949","errer","45655", [
                        new Peri("kuy","test","ab00e"),
                        new Peri("yml","test2","bibiou"),
                    ]),
                    new InformacaoCte("036848746484847","irurr","9789", [
                        new Peri("kuy","test","ab00e"),
                    ]),
                     
                ]
            )
        ];
    }
}


public class Ide(int uF, int ambiente, int tipoEmitente, string modelo, string serie)
{
    [IniProperty("cUF")]
    private int UF = uF;
    [IniProperty("tpAmb")]
    private int Ambiente = ambiente;
    [IniProperty("tpEmit")]
    private int TipoEmitente = tipoEmitente;
    [IniProperty("mod")]
    private string Modelo = modelo;
    [IniProperty("serie")]
    private string Serie = serie;
}

public class Percurso(string uF)
{
    [IniProperty("UFPer")]
    private string UF = uF;
}

public class Descarga(int codigoMuniciopioDescarga, string municipioDescarga, IEnumerable<InformacaoCte> infoCte)
{
    [IniProperty("cMunDescarga")]
    private int CodigoMuniciopioDescarga = codigoMuniciopioDescarga;
    [IniProperty("xMunDescarga")]
    private string MunicipioDescarga = municipioDescarga;

    [IniProperty("infCTe")]
    [ListIndexFormat("000")]
    private IEnumerable<InformacaoCte> InformacoesCte = infoCte;
}

```

~~~ini
#conteúdo do arquivo ini:
[ide]
cUF=35
tpAmb=1
tpEmit=1
mod=58
serie=1

[perc001]
UFPer=BA

[perc002]
UFPer=MG

[DESC001]
cMunDescarga=3518701
xMunDescarga=GURARUJA

[infCTe001001] #item 1 de DESC001
chCTe=98374949404949
SegCodBarra=errer
indReentrega=45655

[peri001001001] #item 1 de infCTe001001
nONU=kuy
xNomeAE=test
xClaRisco=ab00e

[peri001001002] #item 2 de infCTe001001
nONU=yml
xNomeAE=test2
xClaRisco=bibiou

[infCTe001002] #item 2 de DESC001
chCTe=036848746484847
SegCodBarra=irurr
indReentrega=9789

[peri001002001] #item 1 de infCTe001002
nONU=kuy
xNomeAE=test
xClaRisco=ab00e
~~~


---

### Outras Opções

Há duas opções de customização no gerador de arquivo ini:
  - **ShowEmptySections**:<br>Exibe ou não as Seções sem valores. [valor padrão: false]
  - **WriteParentSectionsInComment**:<br>Escreve como comentário ao lado de uma seção sua respectiva seção pai. (Propriedades de classes aninhadas) [valor padrão: false]. Formato: 
      ```ini 
      #item {indice} de {nome-da-seção-pai}
      ```
   
  
  Para usar essas opções, você deverá passar uma instância de implementação da interface IIniWriter com os valores setados para as funções de serialização:

  * No método .ToIniFile() da classe de herança IniFileSerializer:
   ```csharp
     var writer = new IniWriter();
     writer.WriteParentSectionsInComment(true);
     writer.ShowEmptySections(true);
     var instancia = new MDFe();
     var iniContent = instancia.ToIniFile(writer);
   ```
  * No método estático: 
   ```csharp
     var writer = new IniWriter();
     writer.WriteParentSectionsInComment(true);
     writer.ShowEmptySections(true);
     var instancia = new MDFe();
     var iniContent = IniFile.ObjectToIni(instancia, writer);
   ```
 * Conteúdo do arquivo ini:
  ```ini
  [EmptySection] 

  
  [DESC001]
  ...

  [infCTe001001] #item 1 de DESC001
  ...
  
  ```

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

