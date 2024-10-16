namespace IniFile.Test.Models;

[IniFile.IniSection("ide")]
public class MDFeMock
{
    [IniFile.IniProperty("cUF")]
    public int UF { get; set; }
    [IniFile.IniProperty("tpAmb")]
    public int Ambiente { get; set; }
    [IniFile.IniProperty("tpEmit")]
    public int TipoEmitente { get; set; }
    [IniFile.IniProperty("mod")]
    public string Modelo { get; set; }
    [IniFile.IniProperty("serie")]
    public string Serie { get; set; }

    [IniFile.IniProperty("perc")]
    public IEnumerable<Percurso> Percursos { get; set; }

    
    [IniFile.IniProperty("emit")]
    public Emitente Emitente { get; set; }
    
    public MDFeMock()
    {
        UF = 35;
        Ambiente = 1;
        TipoEmitente = 1;
        Modelo = "58";
        Serie = "1";

        Percursos = new List<Percurso>()
        {
            new()
            {
                UF = "BA",
            },
            new()
            {
                UF = "MG",
            }
        };

        Emitente = new Emitente()
        {
            Complemento = "Casa",
            Fantasia = "Nome Fantasia",
            Logradouro = "Rua das flores",
            Nome = "Caps",
            Numero = "12345678",
            IE = "123",
            CNPJCPF = "12345678"
        };
    }

    public override string ToString()
    {
       return IniFile.ObjectToIni(this);
    }
}



public class Percurso
{
    [IniFile.IniProperty("UFPer")]
    public string UF { get; set; }
}

public class Emitente
{
    [IniFile.IniProperty("CNPJCPF")]
    public string CNPJCPF { get; set; }
    [IniFile.IniProperty("IE")]
    public string IE { get; set; }
    [IniFile.IniProperty("xNome")]
    public string Nome { get; set; }
    [IniFile.IniProperty("xFant")]
    public string Fantasia { get; set; }
    
    [IniFile.IniProperty("xLgr")]
    public string Logradouro { get; set; }
    
    [IniFile.IniProperty("nro")]
    public string Numero { get; set; }
    
    [IniFile.IniProperty("xCpl")]
    public string Complemento { get; set; }
  
    

    
    
}