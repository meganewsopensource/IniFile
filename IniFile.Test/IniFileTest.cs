using System.Collections.Immutable;
using IniFile.Test.Models;
using static IniFile.IniFile;

namespace IniFile.Test;

public class IniFileTest
{
  
    [Fact]
    public void NestedSectionTest()
    {
        var mock = new MDFeMock();
        var iniContent = mock.ToIniFile();
        var expectedIniContent = @"[ide]
cUF=35
tpAmb=1
tpEmit=1
mod=58
serie=1

[emit]
CNPJCPF=12345678
IE=123
xNome=Nome
xFant=nome fantasia
xLgr=Rua das flores
nro=12
xCpl=

[perc001]
UFPer=BA

[perc002]
UFPer=MG

[DESC001]
cMunDescarga=3518701
xMunDescarga=GURARUJA

[infCTe001001]
chCTe=98374949404949
SegCodBarra=errer
indReentrega=45655

[peri001001001]
nONU=kuy
xNomeAE=test
xClaRisco=ab00e

[peri001001002]
nONU=yml
xNomeAE=test2
xClaRisco=bibiou

[infCTe001002]
chCTe=036848746484847
SegCodBarra=irurr
indReentrega=9789

[peri001002001]
nONU=kuy
xNomeAE=test
xClaRisco=ab00e";     

        Assert.Equal(iniContent, expectedIniContent);
    }


    [Fact]
    public void DataTypeTest()
    {
        var dateMock = new DateTime(2024, 12, 31, 01, 59, 59);

        var dataTypeMock = new DataTypeMock(
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

        var  iniContent = dataTypeMock.ToIniFile();

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
   
         Assert.Equal(expextedContent, iniContent);
    }

    [Fact]
    public void RequiredFieldsMockTest()
    {

        Assert.Throws<ArgumentNullException>(() => {
            var stringRequired = new RequiredFieldsMock(null);

            var result = stringRequired.ToString();
        });

        var stringRequired = new RequiredFieldsMock("value");
        var result = stringRequired.ToString();
        Assert.Equal($"[RequiredTests]{Environment.NewLine}NullableString=value", result);

    }

    [Fact]
    public void DefaultValuesTest()
    {
        var defaultValues = new DefaultValueMock();

        var iniContent = defaultValues.ToString();

        var expectedIniContent = @"[DefaultValueTests]
StringValue=string
IntValue=2147483647
LongValue=9223372036854775807
DoubleValue=12.34
FloatValue=12.34
DecimalValue=12.34
BooleanValue=False
EnumValue=1";
        
        Assert.Equal(expectedIniContent, iniContent);
    }



}