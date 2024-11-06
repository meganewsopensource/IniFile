namespace IniFile.Test.Models;

[IniSection("teste")]
public class InformacaoCIOTMock : IniFileSerializer
{
    [IniProperty("CIOT")] 
    private string _CIOT;
    [IniProperty("CPF","0",false)]
    private string _CPF;
    [IniProperty("CNPJ","0",false)]
    private string _CNPJ;
    [IniProperty("enumTipo", 0, false)] 
    private ushort _Tipo;

    public InformacaoCIOTMock(string ciot, string documento, ushort tipo = 0)
    {
        _CIOT = ciot;
        _CPF = "0";
        _CNPJ = "0";

        if (documento.Length == 11)
        {
            _CPF = documento;
        }
        else
        {
            _CNPJ = documento;
        }

        _Tipo = tipo;
    }
}