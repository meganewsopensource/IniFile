using System.Collections.Immutable;
using static IniFile.IniFile;

namespace IniFile.Test.Models;


public class MDFeMock : IniFileSerializer<MDFeMock>
{
    [IniProperty("ide")]
    private Ide Ide;

    [IniProperty("emit")]
    private Emitente Emitente;

    [IniProperty("perc")]
    [ListIndexFormat("000")]
    private IEnumerable<Percurso> Percursos;

    [IniProperty("DESC")]
    [ListIndexFormat("000")]
    private IEnumerable<Descarga> Desc;

    public MDFeMock()
    {
        Ide = new Ide(35, 1, 1, "58", "1");

        Emitente = new Emitente("12345678", "123", "Nome", "nome fantasia", "Rua das flores", "12", "");

        Percursos =
        [
            new("BA"),
            new("MG"),
        ];

        Desc =
        [
            new(
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

    public override string ToString()
    {
       return IniFile.ObjectToIni(this);
    }
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

public class InformacaoCte(string chaveCte, string segCodBarra, string indReentrega, IEnumerable<Peri> peri)
{
    [IniProperty("chCTe")]
    private string ChaveCte = chaveCte;
    [IniProperty("SegCodBarra")]
    private string SegCodBarra = segCodBarra;

    [IniProperty("indReentrega")]
    private string IndReentrega = indReentrega;

    [IniProperty("peri")]
    [ListIndexFormat("000")]
    private IEnumerable<Peri> Peris = peri;
}
public class Peri(string oNU, string nome, string classificacaoRiscao)
{
    [IniProperty("nONU")]
    private string ONU = oNU;

    [IniProperty("xNomeAE")]
    private string Nome = nome;


    [IniProperty("xClaRisco")]
    private string ClassificacaoRiscao = classificacaoRiscao;
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

public class Emitente(string cNPJCPF, string iE, string nome, string fantasia, string logradouro, string numero, string complemento)
{
    [IniProperty("CNPJCPF")]
    private string CNPJCPF = cNPJCPF;
    [IniProperty("IE")]
    private string IE = iE;
    [IniProperty("xNome")]
    private string Nome = nome;
    [IniProperty("xFant")]
    private string Fantasia = fantasia;

    [IniProperty("xLgr")]
    private string Logradouro = logradouro;

    [IniProperty("nro")]
    private string Numero = numero;

    [IniProperty("xCpl")]
    private string Complemento = complemento;
}