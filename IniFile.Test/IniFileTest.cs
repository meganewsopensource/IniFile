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

       var iniFileContent = mock.ToIniFile();
       mock.SaveToIniFile(@"C:\v2\iniContent.ini");
      
       //
       // Assert.NotNull(s);
       //
       // Assert.Equal(ExpectS,s);
    }
}