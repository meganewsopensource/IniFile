namespace IniFile.Test.Models;

public class NestedClassesMock
{
    [IniSection("Section1")]
    public class ClassMock : IniFileSerializer
    {
        [IniProperty("Prop")]
        private string Property;

        [IniProperty("Section2")]
        private ClassMock2 ClassMockSingle;

        [IniProperty("DESC")]
        [ListIndexFormat("000")]
        private List<ClassItemMock> Desc;

        public ClassMock()
        {
            Property = "Valor1";
            ClassMockSingle = new ClassMock2();
            Desc = new List<ClassItemMock>()
            {
                new ClassItemMock("Valor 1", [
                    new NestedClassItemMock("innerValue 1 - 1")
                ]),
                new ClassItemMock("Valor 2", [
                    new NestedClassItemMock("innerValue 2 - 1")
                    
                ]),
            };
        }
    }

    public class ClassMock2
    {
        [IniProperty("Prop")]
        private string Property;

        public ClassMock2()
        {
            Property = "Valor2";
        }
    }

    public class ClassItemMock(string propertyValue, List<NestedClassItemMock> nestedList)
    {
        [IniProperty("PropItem")]
        [ListIndexFormat("000")]
        private string PropertyOfItem = propertyValue;

        [IniProperty("Nested")]
        [ListIndexFormat("000")]
        private List<NestedClassItemMock> NestedList = nestedList;
    }

    public class NestedClassItemMock(string propertyValue)
    {
        [IniProperty("PropItem")]
        private string PropertyOfItem = propertyValue;

    }
}