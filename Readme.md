# AttributeIniFile


Ini file lightweight framework

## Serialize Ini
~~~c#
[IniFile.IniSession("MyClass")]
internal class MyClass
{
     [IniFile.IniFile.IniProperty("Property")]
     private string _property;

     [IniFile.IniFile.IniProperty("PropertyNoRequired","10",false)]
     private uint _propertyNoRequired;
        
     public MyClass(string property, uint propertyNoRequired)
     {
          _property = property;
          _propertyNoRequired = propertyNoRequired;
     }
}
~~~

~~~ini

[MyClass]

Property=FristProperty

~~~



```c#
[IniSection("Teste")]
internal class SingleMock : IniFile.IniFileSerializer<SingleMock>
{
    [IniProperty("valString")]
    private string valorString;

    [IniProperty("valInteiro")]
    private int valorInteiro;

    [IniProperty("valDouble_duas_casas")]
    [FormatNumeric(2)] //formata o double com duas casas decimais
    private double valorDouble;

    [IniProperty("valDouble_tres_casas")] //formata o double com 3 casas decimais 
    [FormatNumeric(3)]
    private double valorDouble3Casas;

    [IniProperty("valDataSemAtributoDeFormatacao")]
    private DateTime somenteDataSemAtributoFormatacao;

    [IniProperty("valSomenteDataComAtributo")]
    [FormatDate] //formata DateTime para somente data
    private DateTime somenteDataComAtributoFormatacao;

    [IniProperty("valDataHoraComAtributo")]
    [FormatDateAndTime] //formata DateTime para data e hora
    private DateTime dataEHora;

    [IniProperty("valSomenteHoraComAtributo")]
    [FormatTime] //formata para Somente Horas
    private DateTime somenteHoraComAtributoFormatacao;

    [IniProperty("valEnum")]
    private StatusPedido statusPedido;

    public SingleMock(string valorString, int valorInteiro, double valorDouble, double valorDouble3Casas, DateTime somenteDataSemAtributoFormatacao, DateTime somenteDataComAtributoFormatacao, DateTime dataEHora, DateTime somenteHoraComAtributoFormatacao, StatusPedido statusPedido)
    {
        this.valorString = valorString;
        this.valorInteiro = valorInteiro;
        this.valorDouble = valorDouble;
        this.valorDouble3Casas = valorDouble3Casas;
        this.somenteDataSemAtributoFormatacao = somenteDataSemAtributoFormatacao;
        this.somenteDataComAtributoFormatacao = somenteDataComAtributoFormatacao;
        this.dataEHora = dataEHora;
        this.somenteHoraComAtributoFormatacao = somenteHoraComAtributoFormatacao;
        this.statusPedido = statusPedido;
    }
}

 var dateMock = new DateTime(2024, 12, 31, 01, 59, 59);
 
 var singleData = new SingleMock(
            "string",
            10,
            123.456,
            123.456,
            dateMock,
            dateMock,
            dateMock,
            dateMock,
            StatusPedido.Concluido
            );

  var iniFile = singleData.ToIniFile();




```


~~~ini
  #conteudo de iniFile
  
    [Teste]
    valString=string
    valInteiro=10
    valDouble_duas_casas=123.46
    valDouble_tres_casas=123.456
    valDataSemAtributoDeFormatacao=01/01/2024 01:59:59
    valSomenteDataComAtributo=01/01/2024
    valDataHoraComAtributo=01/01/2024 01:59:59
    valSomenteHoraComAtributo=01:59:59
    valEnum=500

~~~
