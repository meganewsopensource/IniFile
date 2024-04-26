namespace IniFile.Test;

public class IniFileTest
{
    
    const string ExpectS = "[MyClass]\nProperty=FristProperty\n";
    
    [IniFile.IniSession("MyClass")]
    internal class MyClass
    {
        [IniFile.IniProperty("Property")]
        private string _property;

        [IniFile.IniProperty("PropertyNoRequired","10",false)]
        private uint _propertyNoRequired;
        
        public MyClass(string property, uint propertyNoRequired)
        {
            _property = property;
            _propertyNoRequired = propertyNoRequired;
        }

        public override string ToString()
        {
            return IniFile.ObjectToIni(this);
        }
    }
    
    [Fact]
    public void Test()
    {
        var myClass = new MyClass("FristProperty",10);

        var s = myClass.ToString();
        
        Assert.NotNull(s);
        
        Assert.Equal(ExpectS,s);
    }
}