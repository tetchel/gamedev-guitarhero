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

        float yPadding = 5;
        float y = yPadding;

        for(int i = 0; i < songFilePaths.Length; i++) {
            string songRef = getSongRef(songFilePaths[i]);
    
            // Instantiate the new row into the list view
            GameObject newRow = Instantiate(rowFab, listContent.transform);
            // Position below existing rows
            RectTransform rt = newRow.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -y);

            y += rowRect.rect.height + yPadding;
            newRow.GetComponentInChildren<Text>().text = getNiceSongName(songRef);

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
        string songRef = getSongRef(songPath);
        NoteManager.setSongData(/*getSongMp3Path(songRef),*/ songPath);
        // Prepare the Post Song menu by notifying it what song it will display
        PostSongMenu.setSongref(songRef);

        SceneManager.LoadScene("scene");
    }

    public void onViewScores(string songRef) {
        Scoreboard.setSongref(songRef);
        SceneManager.LoadScene("scoreboard");
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

    // format the song name to be displayed in the menu
    // Assumes that song words are split by '_'
    public static string getNiceSongName(string songRef) {
        string niceSongName = "";
        foreach(string word in songRef.Replace('_', ' ').Split(' ')) {
            niceSongName += char.ToUpper(word[0]);
            niceSongName += word.Substring(1, word.Length - 1);
            niceSongName += ' ';
        }
        return niceSongName;
    }

    private static string[] getSongDataFiles() {
        string path = Application.streamingAssetsPath + "/songdata/";
        return Directory.GetFiles(path, "*.csv");
    }
    
    /*
    private static string getSongMp3Path(string songRef) {
        string ext;
        if(Application.platform == RuntimePlatform.WindowsEditor || 
            Application.platform == RuntimePlatform.WindowsPlayer) {
            ext = "ogg";
        }
        else {
            ext = "mp3";    // What is it on mac/linux?
        }

        return Application.dataPath + "/sound/music/" + songRef + "." + ext;
    }
    */

    public void onHowToPlay() {
        SceneManager.LoadScene("howtoplay");
    }

    public void onQuitPressed() {
        Application.Quit();
    }
}
