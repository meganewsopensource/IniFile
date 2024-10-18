using System.Collections.Immutable;
using IniFile.Test.Models;

namespace IniFile.Test;

public class IniFileTest
{
    
    string ExpectS = $"[MyClass]{Environment.NewLine}Property=FristProperty{Environment.NewLine}";
    
    [IniSection("MyClass")]
    internal class MyClass
    {
        [IniProperty("Property")]
        private string _property;

        [IniProperty("PropertyNoRequired","10",false)]
        private uint _propertyNoRequired;
        
        [IniProperty("nestedClass")]
        private MyNestedClass _nestedClass;
        
        [IniProperty("nestedListClass")]
        private IImmutableList<MyNestedClass> _nestedListClass;
        

        public MyClass(string property, uint propertyNoRequired, MyNestedClass nestedClass, IImmutableList<MyNestedClass> list)
        {
            _property = property;
            _propertyNoRequired = propertyNoRequired;
            _nestedClass = nestedClass;
            _nestedListClass = list;
        }

        public override string ToString()
        {
            return IniFile.ObjectToIni(this);
        }
    }
    
    internal class MyNestedClass
    {
        [IniProperty("NestedProperty")]
        private string _nestedProperty;
        
        [IniProperty("DoubleNestedProperty","10.45",false)]
        private double _nestedDoubleProperty;

        public MyNestedClass(string nestedProperty, double nestedDoubleProperty)
        {
            _nestedProperty = nestedProperty;
            _nestedDoubleProperty = nestedDoubleProperty;
        }
    }


    [Fact]
    public void TestIniWriter()
    {
        var iniWriter = new IniWriter();

        var emitSection = iniWriter.WriteSection("emit", false, null);
        
        iniWriter.Write(emitSection,"pedido_valor_1","valor");


        var descName = "DESC";
        
        var descSection = iniWriter.WriteSection("DESC", true, null);
        iniWriter.Write(descSection,"item_valor_1","valor");
        
        
        var infoCteSection = iniWriter.WriteSection("infoCte", true, descSection?.Name);
        infoCteSection?.AddItem("valor_info_cte","valor");
        
        var infoCteSection2 = iniWriter.WriteSection("infoCte", true, descSection?.Name);
        infoCteSection2?.AddItem("valor_info_cte2","valor");
        
        var infoCteSection3 = iniWriter.WriteSection("infoCte", true, descSection?.Name);
        infoCteSection3?.AddItem("valor_info_cte3","valor");
        
        
        var descSection2 = iniWriter.WriteSection("DESC", true, null);
        iniWriter.Write(descSection2,"item_valor_1","valor");
        
        
        var infoCteSectionaqui = iniWriter.WriteSection("infoCte", true, descSection2?.Name);
        infoCteSectionaqui?.AddItem("valor_info_cte","valor");
        
        // var descSection2 = iniWriter.WriteSection("DESC", true, null);
        // iniWriter.Write(descSection2,"item_valor_1","valor");
        //
        // var descSection3 = iniWriter.WriteSection("DESC", true, null);
        // iniWriter.Write(descSection3,"item_valor_1","valor");
        // iniWriter.Write(descSection3,"desc_valor2","valor");
        
       // var infCteSection = iniWriter.WriteSection("infCte", true, descSection.Name);

        

        
        File.WriteAllText("C:\\v2\\testando.ini",iniWriter.ToString());
    }

    [Fact]
    public void TestTiposDeDados()
    {
        var dateMock = new DateTime(2024, 12, 31, 01, 59, 59);

        var mockTiposDados = new SingleMock(
            "string",
            10,
            1234.5678,
            1234.5678,
            dateMock,
            dateMock,
            dateMock,
            dateMock,
            StatusPedido.Concluido
            );

        var  iniContent = mockTiposDados.ToIniFile();


        var expextedContent = @"[Teste]
valString=string
valInteiro=10
valDouble_duas_casas=1234.57
valDouble_tres_casas=1234.568
valDataSemAtributoDeFormatacao=31/12/2024 01:59:59
valSomenteDataComAtributo=31/12/2024
valDataHoraComAtributo=31/12/2024 01:59:59
valSomenteHoraComAtributo=01:59:59
valEnum=500";

         Assert.NotNull(mockTiposDados);        
         Assert.Equal(expextedContent, iniContent);
    }

    [Fact]
    public void Test()
    {
        var myClass = new MyClass("FristProperty",10, 
            new MyNestedClass("testing", 90.05f),
            new List<MyNestedClass>()
            {
                new("testing item 1", 10.10f),
                new("testing item 2", 20.20f),
            }.ToImmutableList()
            
            );

      //  var mock = new MDFeMock();

      //  var a = mock.ToString();
        
       //  var s = myClass.ToString();

       var mock = new PedidoMock();
      
      
       //
       // Assert.NotNull(s);
       //
       // Assert.Equal(ExpectS,s);
    }
}