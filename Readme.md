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

