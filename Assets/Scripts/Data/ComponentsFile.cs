using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUCC;
using System.IO;
using System.Linq;

public class ComponentsFile
{
    public readonly string name;

    private readonly string filePath;

    public Dictionary<string, ComponentData> components;

    public ComponentsFile(string filePath)
    {
        this.filePath = filePath;

        name = Path.GetFileNameWithoutExtension(filePath);

        if(!File.Exists(filePath))
        {
            //if no file, move on
            components = new Dictionary<string, ComponentData>();
            return;
        }

        //read the file
        ReadOnlyDataFile file = new ReadOnlyDataFile(filePath);

        components = file.GetAsDictionary<string, ComponentData>();

        //set all their names quick, for the lists and UI display
        //also reassign their indexes to match what was read from the file
        foreach (var pair in components)
        {
            pair.Value.name = pair.Key;

            pair.Value.Load();
        }
    }

    public override string ToString()
    {
        return name;
    }

    private void RebuildDictionary()
    {
        Dictionary<string, ComponentData> renamedComponents = new Dictionary<string, ComponentData>(components.Count);
        foreach (var pair in components)
        {
            renamedComponents.Add(pair.Value.name, pair.Value);
        }
        components = renamedComponents;
    }

    public void CreateNewComponent(string name)
    {
        //find a name that is not repeated
        name = FindUniqueName(name);

        //add it
        ComponentData cd = new ComponentData();
        cd.name = name;

        components.Add(name, cd);
    }

    private string FindUniqueName(string original)
    {
        string newName = original;
        int number = 2;

        while (components.ContainsKey(newName))
        {
            newName = original + number;

            number++;
        }

        return newName;
    }

    public void DeleteFile()
    {
        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    public void Save()
    {
        //make sure the file exists
        //create the file
        //or clear it if it exists
        if (File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
        }
        else
        {
            File.Create(filePath).Close();
        }

        //before save, make sure the prefabs are ready to go
        foreach (var pair in components)
        {
            pair.Value.Save();
        }

        //rebuild the dictionary with the proper names
        RebuildDictionary();

        //now save this file
        DataFile file = new DataFile(filePath);

        file.SaveAsDictionary(components);

        // after saving, remove all lines that end in null
        File.WriteAllLines(filePath, File.ReadAllLines(filePath).Where(s =>
        {
            string t = s.TrimEnd();

            return !t.EndsWith("null") && !t.EndsWith("\"null\"");
        }));
    }
}
