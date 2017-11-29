using System.IO;

public class CSVReader {

    private const int NUM_COLS = 6;
    
    /**
     * Convert csv file to 2d array
     * Caveats: 
     * Don't put commas or newlines in the data
     * Will only read NUM_COLS cols left-to-right, any further cols are ignored.
     * The first column is to be used to 'comment out' rows by putting anything in it.
     */
    public static string[,] parseCSV(string csvPath) {
        string[] contents = File.ReadAllLines(csvPath);

        string[,] result = new string[contents.Length, NUM_COLS];

        for(int i = 0; i < contents.Length; i++) {
            string[] row = contents[i].Split(',');
            if(row.Length == 0 || !string.IsNullOrEmpty(row[0])) {
                //Debug.Log("skipping row " + i);
                continue;
            }
            for(int j = 1; j < NUM_COLS; j++) {
                result[i, j-1] = row[j];
            }
        }

        return result;
    }
}