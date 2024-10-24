namespace IniFile;

public interface IIniWriter
{
    
    
    void WriteParentSectionsInComment(bool write);
    void ShowEmptySections(bool show);
    
    Section? CreateSection(string sectionName);
    Section? CreateListItemSection(string singleName, string listIndexFormat, Section? parentSection = null);

    void AddItem(Section section, string key, string value);
    
    string ToString();
    
    void SaveToFile(string filePath);
}