namespace IniFile.Test.Models;

[IniSection("ide")]
public class MDFeMock
{
    [IniProperty("cUF")]
    public int UF { get; set; }
    [IniProperty("tpAmb")]
    public int Ambiente { get; set; }
    [IniProperty("tpEmit")]
    public int TipoEmitente { get; set; }
    [IniProperty("mod")]
    public string Modelo { get; set; }
    [IniProperty("serie")]
    public string Serie { get; set; }

    [IniProperty("perc")]
    public IEnumerable<Percurso> Percursos { get; set; }

    
    [IniProperty("emit")]
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
    [IniProperty("UFPer")]
    public string UF { get; set; }
}

public class Emitente
{
    [IniProperty("CNPJCPF")]
    public string CNPJCPF { get; set; }
    [IniProperty("IE")]
    public string IE { get; set; }
    [IniProperty("xNome")]
    public string Nome { get; set; }
    [IniProperty("xFant")]
    public string Fantasia { get; set; }
    
    [IniProperty("xLgr")]
    public string Logradouro { get; set; }
    
    [IniProperty("nro")]
    public string Numero { get; set; }
    
    [IniProperty("xCpl")]
    public string Complemento { get; set; }
  
    

    
    
}