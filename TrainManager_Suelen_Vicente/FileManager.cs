using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TrainManager_Suelen_Vicente
{
    public class FileManager
    {
        private string currentFile;
        private List<string> allFiles;

        public FileManager()
        {
            allFiles = new List<string>();

            loadAllJsonFiles();

            if(existFilesToLoad())
                currentFile = allFiles[0];
        }

        public void setCurrentFile(string fileName)
        {
            currentFile = fileName;
        }

        public string getCurrentFile()
        {
            return currentFile;
        }

        public List<string> getAllFiles()
        {
            return allFiles;
        }

        public void loadAllJsonFiles()
        {
            allFiles.Clear();

            foreach (string file in Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.json"))
                allFiles.Add(Path.GetFileName(file));

        }

        public string saveFile(string json)
        {
            try
            {
                if (!currentFileIsEmpty())
                {
                    var fileDirectory = AppDomain.CurrentDomain.BaseDirectory + currentFile;

                    File.WriteAllText(fileDirectory, json);
                    return "seat plan saved on file " + currentFile + " successfully.";
                }

                return "File name must be filled.";
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Something went wrong. Try again!";
            }
            
        }

        public bool existFilesToLoad()
        {
            return allFiles.Count() > 0;
        }

        public bool currentFileIsEmpty()
        {
            return currentFile == null || currentFile == "";
        }

    }
}
