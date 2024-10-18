using System.Text;

namespace IniFile;


public class Section(string name, string singleName, int index,  string? parentSection = null, string? indexFormat = "000")
{
    public int Index = index;
    private readonly Dictionary<string,string> _keyValues = new();
    public string Name = name;
    public string? ParentSection = parentSection;
    public string? IndexFormat = indexFormat;
    public string SingleName = singleName;

    
    public void AddItem(string key, string value)
    {
         _keyValues[key] = value;
    }

    public string? GetItem(string key)
    {
        return _keyValues.GetValueOrDefault(key);
    }

    
    public override string ToString()
    {
          if (_keyValues.Count != 0)
          {
              var builder = new StringBuilder();
              builder.AppendLine($"[{Name}]");
              
              foreach (var item in _keyValues )
              {
                   builder.AppendLine($"{item.Key}={item.Value}");
              }
              return builder.ToString().TrimEnd();
          }
          return "";
    } 
}





public class IniWriter
{
    List<Section> _sections = new();


    public Section? WriteSection(string sectionName, bool isList, string? parentSectionName, string indexFormat = "000")
    {
        Section? newSection = _sections.FirstOrDefault(e => e.Name == sectionName);
        
        if (newSection == null) //Não existe
        {
            var newName = sectionName;

            var index = 1;
            if (parentSectionName != null)
            {
                var parentSection = _sections.FirstOrDefault(e => e.Name == parentSectionName);
                if (parentSection != null)
                {
                    //Procura todos 
                    var outros = _sections.Where(e => e.SingleName == sectionName && e.ParentSection == parentSection.Name);

                    var qtd = outros.Count();
                    index = qtd + 1;
                    newName = sectionName + parentSection.Index.ToString(indexFormat)  + index.ToString(indexFormat);
                    newSection = new Section(newName, sectionName,index, parentSectionName);
                }
                else throw new Exception($"Parent section {parentSection} not found");
            }
            else
            {
                if (isList)
                {
                    var qtd = _sections.Count(e => e.SingleName == sectionName);
                    index = qtd + 1;
                    newName = sectionName + index.ToString(indexFormat);
                }
                
                newSection = new Section(newName, sectionName,index, parentSectionName);
            }

          
            
            _sections.Add(newSection);
        }
        
        return newSection;
    }

   
   
    
    public void Write(Section? section, string key, string value)
    {
        if (section == null)
        {
            throw new Exception($"Section is null");
        }
        section?.AddItem(key, value);
    }


    public override string ToString()
    {
        var builder = new StringBuilder();

        foreach (var section in _sections)
        {
            builder.AppendLine(section.ToString());
        }

        return builder.ToString().TrimEnd();
    }


}