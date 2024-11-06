using System.Collections.Immutable;
using System.Text;

namespace IniFile;


public class Section
{
    public int Index;
    public readonly Section? ParentSection;
    public readonly string? IndexFormat;
    public string SingleName;

    private readonly Dictionary<string, string> _keyValues = new();
    private readonly bool IsItemList;


    public Section(string singleName)
    {
        Index = 1;
        IsItemList = false;
        ParentSection = null;
        IndexFormat = "";
        SingleName = singleName;
    }
    public Section(string singleName, bool isItemList, int listIndex, string? listIndexFormat, Section? parentSection)
    {
        SingleName = singleName;
        IsItemList = isItemList;
        ParentSection = parentSection;
        IndexFormat = listIndexFormat;
        SingleName = singleName;
        Index = listIndex;
    }
    
    public void AddItem(string key, string value)
    {
         _keyValues[key] = value;
    }

    public IImmutableList<KeyValuePair<string,string>> GetItems()
    {
        return _keyValues.ToImmutableList();
    }

    public string? GetItem(string key)
    {
        return _keyValues.GetValueOrDefault(key);
    }

    public string FullUniqueName {
        get  {

            if (IsItemList)
            {
                var uniqueName = $"{SingleName}{ParentIndex}" + Index.ToString(IndexFormat);

                return uniqueName;
            }
            else return SingleName;

        } }

    public string ParentIndex
    {
        get
        {
            if (ParentSection != null)
            {
                var name = RemoveSingleName(ParentSection.FullUniqueName,ParentSection.SingleName);
                return name;

            }
            else return "";
        }
    }
    private string RemoveSingleName(string fullName, string singleName)
    {
        int indice = fullName.IndexOf(singleName);

        if (indice >= 0)
        {
            return fullName.Remove(indice, singleName.Length);
        }

        return fullName;
    }

   
}





public class IniWriter : IIniWriter
{
    private List<Section> _sections = new();

    private bool _writeParentSectionsInComment = false;
    private bool _showEmptySections = false;


    public void WriteParentSectionsInComment(bool write)
    {
        _writeParentSectionsInComment = write;
    }

    public void ShowEmptySections(bool show)
    {
        _showEmptySections = show;
    }
    
    public Section? CreateListItemSection(string singleName,string listIndexFormat, Section? parentSection = null)
    {
        return CreateSection(singleName, true, parentSection, listIndexFormat);
    }

    public void AddItem(Section section, string key, string value)
    {
        if (_sections.Contains(section))
        {
            _sections.FirstOrDefault(e => e == section)?.AddItem(key,value);
        }
        else throw new KeyNotFoundException("Seção não encontrada");
    }

    public Section? CreateSection(string sectionName)
    {
        return CreateSection(sectionName, false, null,"");
    }

    private Section? CreateSection(string singleName, bool isListItem, Section? parentSection, string listIndexFormat)
    {
        Section? newSection = _sections.FirstOrDefault(e => e.FullUniqueName == singleName);
        
        if (newSection == null) 
        {
            var index = 1;

            if (parentSection != null)
                index = _sections.Count(e => e.SingleName == singleName && e.ParentSection == parentSection) + 1;
            else if (isListItem)
                index = _sections.Count(e => e.SingleName == singleName) + 1;

            newSection = new Section(singleName, isListItem, index, listIndexFormat, parentSection);
            _sections.Add(newSection);
        }
        
        return newSection;
    }

   
 

    public override string ToString()
    {

        var builder = new StringBuilder();

        foreach (var section in _sections)
        {
            var items = section.GetItems();
           
            if (items.Count > 0 || (items.Count == 0 && _showEmptySections))
            {
                var comment = "";
                if (section.ParentSection != null && _writeParentSectionsInComment)
                {
                    comment = $" #item {section.Index} de {section.ParentSection.FullUniqueName}";
                }

                builder.AppendLine($"[{section.FullUniqueName}]{comment}");
            }

            foreach (var item in items)
            {
                builder.AppendLine($"{item.Key}={item.Value}");
            }

            builder.AppendLine();
        }

        return builder.ToString().TrimEnd();
    }

    public void SaveToFile(string filePath)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);
        else
        {
            var directory = Path.GetDirectoryName(filePath);
            if (directory != null)
                Directory.CreateDirectory(directory);
        }

        File.WriteAllText(filePath, ToString());
    }
}