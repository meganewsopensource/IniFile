namespace IniFile.Test.Models;

[IniSection("Teste")]
internal class DataTypeMock : IniFile.IniFileSerializer<DataTypeMock>
{
    [IniProperty("valString")]
    private string valorString;

    [IniProperty("valInteiro")]
    private int valorInteiro;

    [IniProperty("valDouble_duas_casas")]
    [FormatNumeric(2)]
    private double valorDouble;

    [IniProperty("valDouble_tres_casas")]
    [FormatNumeric(3)]
    private double valorDouble3Casas;


    [IniProperty("valDataSemAtributoDeFormatacao")]
    private DateTime somenteDataSemAtributoFormatacao;

    [IniProperty("valSomenteDataComAtributo")]
    [FormatDate]
    private DateTime somenteDataComAtributoFormatacao;

    [IniProperty("valDataHoraComAtributo")]
    [FormatDateAndTime]
    private DateTime dataEHora;

    [IniProperty("valSomenteHoraComAtributo")]
    [FormatTime]
    private DateTime somenteHoraComAtributoFormatacao;

    [IniProperty("valEnum")]
    private StatusPedido statusPedido;

    public DataTypeMock(string valorString, int valorInteiro, double valorDouble, double valorDouble3Casas, DateTime somenteDataSemAtributoFormatacao, DateTime somenteDataComAtributoFormatacao, DateTime dataEHora, DateTime somenteHoraComAtributoFormatacao, StatusPedido statusPedido)
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