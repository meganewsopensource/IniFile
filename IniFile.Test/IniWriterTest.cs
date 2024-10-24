namespace IniFile.Test;


public class IniWriterTest
{ 
    [Fact]
    public void TestIniWriter()
    {
        IIniWriter iniWriter = new IniWriter();

        var listIndexFormat = "000";

        var emitSection = iniWriter.CreateSection("emit");

        emitSection?.AddItem("pedido_valor_1","valor");
        
        var descSection = iniWriter.CreateListItemSection("DESC", listIndexFormat, null);
        descSection?.AddItem("item_valor_1","valor");
        
        
        var infoCteSection = iniWriter.CreateListItemSection("infoCte", listIndexFormat, descSection);
        infoCteSection?.AddItem("valor_info_cte","valor");

        var infoCteSection2 = iniWriter.CreateListItemSection("infoCte", listIndexFormat, descSection);
        infoCteSection2?.AddItem("valor_info_cte2", "valor");

        var infoCteSection3 = iniWriter.CreateListItemSection("infoCte", listIndexFormat, descSection);
        infoCteSection3?.AddItem("valor_info_cte3", "valor");


        var descSection2 = iniWriter.CreateListItemSection("DESC", listIndexFormat);
        descSection2?.AddItem("item_valor_1", "valor");


        var infoCteSectionaqui = iniWriter.CreateListItemSection("infoCte", listIndexFormat, descSection2);
        infoCteSectionaqui?.AddItem("valor_info_cte", "valor");

        var infoCteMaisUma = iniWriter.CreateListItemSection("infoCte", listIndexFormat, descSection2);
        infoCteMaisUma?.AddItem("valor_info_cte_mais_uma", "valor");

        var infoCteMaisOutra = iniWriter.CreateListItemSection("infoCte", listIndexFormat, descSection2);
        infoCteMaisOutra?.AddItem("valor_info_cte_mais_outra", "valor");


        var infoCteMaisOutraDentro = iniWriter.CreateListItemSection("peri", listIndexFormat, infoCteMaisOutra);
        infoCteMaisOutraDentro?.AddItem("valor_info_cte_mais_outra_dentro", "valor");

        var infoCteMaisUmPeri = iniWriter.CreateListItemSection("peri", listIndexFormat, infoCteMaisOutra);
        infoCteMaisUmPeri?.AddItem("valor_info_cte_mais_outra_dentro", "valor");

        var periInternoSection = iniWriter.CreateListItemSection("periInterno", listIndexFormat, infoCteMaisUmPeri);
        periInternoSection?.AddItem("valorPriInterno", "Valor");


        var iniContent = iniWriter.ToString();


        var expectedIniContent = @"[emit]
pedido_valor_1=valor

[DESC001]
item_valor_1=valor

[infoCte001001]
valor_info_cte=valor

[infoCte001002]
valor_info_cte2=valor

[infoCte001003]
valor_info_cte3=valor

[DESC002]
item_valor_1=valor

[infoCte002001]
valor_info_cte=valor

[infoCte002002]
valor_info_cte_mais_uma=valor

[infoCte002003]
valor_info_cte_mais_outra=valor

[peri002003001]
valor_info_cte_mais_outra_dentro=valor

[peri002003002]
valor_info_cte_mais_outra_dentro=valor

[periInterno002003002001]
valorPriInterno=Valor";

        Assert.Equal(expectedIniContent, iniContent);

    }


    [Fact]
    public void TestIniWriterWithParentSectionComment()
    {
        IIniWriter iniWriter = new IniWriter();
        iniWriter.WriteParentSectionsInComment(true);

        var listIndexFormat = "000";

        var item = iniWriter.CreateListItemSection("Item",listIndexFormat);
        item?.AddItem("property1","value1");
        
          var subItem = iniWriter.CreateListItemSection("SubItem", listIndexFormat, item);
          subItem?.AddItem("property1","value1");
        
            var subSubItem = iniWriter.CreateListItemSection("SubSubItem", listIndexFormat, subItem);
            subSubItem?.AddItem("property1","value1");
            var subSubItem2 = iniWriter.CreateListItemSection("SubSubItem", listIndexFormat, subItem);
            subSubItem2?.AddItem("property1","value1");
            
          var subItem2 = iniWriter.CreateListItemSection("SubItem", listIndexFormat, item);
          subItem2?.AddItem("property1","value1");
        
        var item2 = iniWriter.CreateListItemSection("Item",listIndexFormat);
        item2?.AddItem("property1","value1");
        
      
        var iniContent = iniWriter.ToString();
        var expectedIniContent = @"[Item001]
property1=value1

[SubItem001001] #item 1 de Item001
property1=value1

[SubSubItem001001001] #item 1 de SubItem001001
property1=value1

[SubSubItem001001002] #item 2 de SubItem001001
property1=value1

[SubItem001002] #item 2 de Item001
property1=value1

[Item002]
property1=value1";
        
        Assert.Equal(expectedIniContent, iniContent);
       
    }


    [Fact]
    public void IniWriterShowEmptySectionsTrueTest()
    {
        IIniWriter iniWriter = new IniWriter();
        iniWriter.ShowEmptySections(true);

        iniWriter.CreateSection("Section1");
        var iniContent = iniWriter.ToString();
        var expectedIniContent = "[Section1]";
       
        Assert.Equal(iniContent, expectedIniContent);
        
        
        File.WriteAllText("C:\\v2\\IniWriterShowEmptySectionsTest.ini",iniWriter.ToString());

    }
    
    [Fact]
    public void IniWriterShowEmptySectionsFalseTest()
    {
        IIniWriter iniWriter = new IniWriter();
        iniWriter.ShowEmptySections(false);

        iniWriter.CreateSection("Section1");
        var iniContent = iniWriter.ToString();
        var expectedIniContent = "";
       
        Assert.Equal(iniContent, expectedIniContent);

    }
}