using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject rowFab;
    public GameObject listContent;

    // Use this for initialization
    void Start() {
        string[] songFilePaths = getSongDataFiles();

        RectTransform rowRect = rowFab.GetComponent<RectTransform>(); 
        // track y coordinate of next row
        float y = 0;

        for(int i = 0; i < songFilePaths.Length; i++) {
            string songRef = getSongRef(songFilePaths[i]);

            // format the song name to be displayed in the menu
            // Assumes that song words are split by '_'
            string niceSongName = "";
            foreach(string word in songRef.Replace('_', ' ').Split(' ')) {
                niceSongName += char.ToUpper(word[0]);
                niceSongName += word.Substring(1, word.Length - 1);
                niceSongName += ' ';
            }
            //Debug.Log("Nice song name " + niceSongName);
    
            // Instantiate the new row into the list view
            GameObject newRow = Instantiate(rowFab, listContent.transform);
            // Position below existing rows
            //newRow.GetComponent<RectTransform>().rect.y = y;
            y += rowRect.rect.height;
            newRow.GetComponentInChildren<Text>().text = niceSongName;

            Button[] buttons = newRow.GetComponentsInChildren<Button>();
            Button viewScore, playSong;
            if(buttons[0].name.ToLower().Contains("score")) {
                viewScore = buttons[0];
                playSong = buttons[1];
            }
            else {
                viewScore = buttons[1];
                playSong = buttons[0];
            }

            string thisSongPath = songFilePaths[i];

            viewScore.onClick.AddListener(delegate { onViewScores(songRef); });
            playSong.onClick.AddListener(delegate { onPlaySong(thisSongPath); });
        }
    }

    public void onPlaySong(string songPath) {
        NoteManager.setSongDataPath(songPath);
        SceneManager.LoadScene("scene");
    }

    public void onViewScores(string songName) {
        Debug.Log("We want to see the highscores for " + songName);
    }

    // Format song file path into reference for use by highscore manager
    // eg. /home/tim/game_data/dani_california.csv -> dani_california
    public static string getSongRef(string songDataFilePath) { 
        int lastSlash = songDataFilePath.LastIndexOf(Path.DirectorySeparatorChar);
        if(lastSlash == -1) {
            lastSlash = songDataFilePath.LastIndexOf(Path.AltDirectorySeparatorChar);
        }
        return songDataFilePath.Substring(lastSlash + 1, songDataFilePath.Length - lastSlash - 1)
            .Replace(".csv", "");
    }

    public static string[] getSongDataFiles() {
        string path = Application.streamingAssetsPath + "/songdata/";
        return Directory.GetFiles(path, "*.csv");
    }

    public void onQuitPressed() {
        Application.Quit();
    }
}
