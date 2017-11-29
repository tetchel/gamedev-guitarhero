using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Scoreboard : MonoBehaviour {

    public GameObject namesParent;
    public GameObject scoresParent;

    public Text songNameDisplay;

    public static string songRef;

    private Text[] namesTexts;
    private Text[] scoreTexts;

    // Use this for initialization
    void Start() {
        if(songRef == null) {
            Debug.LogError("SONGREF NOT SET FOR SCOREBOARD");
        }
        songNameDisplay.text = MainMenu.getNiceSongName(songRef);

        namesTexts = namesParent.GetComponentsInChildren<Text>();
        scoreTexts = scoresParent.GetComponentsInChildren<Text>();
        updateScores();
    }

    public static void setSongref(string songRef) {
        Scoreboard.songRef = songRef;
    }

    private void updateScores() {
        List<string> scoreNames = HighScoreManager.instance().loadScoreNames(songRef);
        List<int>    highscores = HighScoreManager.instance().loadHighscores(songRef);

        for(int i = 0; i < scoreTexts.Length; i++) {
            namesTexts[i].text = scoreNames[i];
            scoreTexts[i].text = highscores[i].ToString();
        }
    }

    public void onBack() {
        SceneManager.LoadScene("mainmenu");
    }
}
