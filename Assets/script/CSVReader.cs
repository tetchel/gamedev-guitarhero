using System.IO;
using UnityEngine;

public class CSVReader {
    
    /**
     * Convert csv file to 2d array
     * Caveats: 
     * Don't put commas or newlines in the data
     * All rows must have same number of columns
     */
    public static string[,] parseCSV(string csvPath) {
        string[] contents = File.ReadAllLines(csvPath);

        int numCols = contents[0].Split(',').Length;

        string[,] result = new string[contents.Length, numCols];

        for(int i = 0; i < contents.Length; i++) {
            string[] row = contents[i].Split(',');
            if(row.Length == 0) {
                Debug.Log("skipping row " + i);
                continue;
            }
            for(int j = 0; j < row.Length; j++) {
                result[i, j] = row[j];
            }
        }

        return result;
    }
}