using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CsvDeSerializer
{
    
    //TODO: Separate the storingPath and the FileName combination!!! 
    public void WriteCSVFile(List<string[]> csvInput, string storePath, string fileName)
    {
        Debug.Log("Start: " + DateTime.Now);
        List<string> combinedCsvLines = new List<string>();
        Debug.Log("CSVCount: " + csvInput.Count);


        foreach (string[] lineGroup in csvInput)
        {
            string[] replacedStrings = new string[lineGroup.Length];
            for (int i = 0; i < lineGroup.Length; i++)
            {
                replacedStrings[i] = lineGroup[i].Replace(',', '.');
            }

            combinedCsvLines.Add(String.Join(",", replacedStrings));
        }

        try
        {
            if (!Directory.Exists(storePath))
            {
                Directory.CreateDirectory(storePath);
            }
            //TODO: The Filename should not be managed inside of this class
            using (FileStream fileStream =
                new FileStream(storePath + Path.DirectorySeparatorChar + ".csv",
                    FileMode.OpenOrCreate))
            {
                using (StreamWriter csvWriter =
                    new StreamWriter(fileStream))
                {
                    foreach (string csvLine in combinedCsvLines)
                    {
                        csvWriter.WriteLine(csvLine);
                    }

                    csvWriter.Close();
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }

        Debug.Log("End: " + DateTime.Now);

    }


    public void ReadCSVFile(string filePath, ref List<string[]> csvReadedFile)
    {
        try
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (StreamReader csvReader = new StreamReader(fileStream))
                {
                    while (!csvReader.EndOfStream)
                    {
                        string readLine = csvReader.ReadLine();
                        //TODO: Parametraize the separator
                        string[] strings = readLine.Split(',');
                        csvReadedFile.Add(strings);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
    }
}