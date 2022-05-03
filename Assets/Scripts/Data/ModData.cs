using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUCC;
using System.IO;

public class ModData
{
    public readonly string name;

    private readonly string directoryPath;
    private readonly string componentsPath;

    public List<ComponentsFile> files = new List<ComponentsFile>();

    public ModData(string folderPath)
    {
        directoryPath = folderPath;

        name = Path.GetDirectoryName(folderPath);

        //get the files within the components folder
        componentsPath = Path.Combine(folderPath, "components");

        //make sure components exists, if not, load nothing
        if (!Directory.Exists(componentsPath))
        {
            //create it when saving
            return;
        }

        foreach (string path in Directory.GetFiles(componentsPath))
        {
            //only worry about it if it is a succ file
            if(Path.GetExtension(path).CompareTo(".succ") == 0)
            {
                files.Add(new ComponentsFile(path));
            }
        }
    }

    public void CreateNewFile(string name)
    {
        string path = Path.Combine(componentsPath, name + ".succ");

        //now create a new components file for it and add it
        files.Add(new ComponentsFile(path));
    }

    public void DeleteFile(int index)
    {
        files[index].DeleteFile();

        files.RemoveAt(index);
    }

    public void Save()
    {
        if(!Directory.Exists(componentsPath))
        {
            Directory.CreateDirectory(componentsPath);
        }

        //save all file objects
        foreach(ComponentsFile file in files)
        {
            file.Save();
        }
    }
}