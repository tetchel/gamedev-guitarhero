using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Scoreboard : MonoBehaviour {

    public GameObject namesParent;
    public GameObject scoresParent;

    private Text[] namesTexts;
    private Text[] scoreTexts;

    // Use this for initialization
    void Start() {
        namesTexts = namesParent.GetComponentsInChildren<Text>();
        scoreTexts = scoresParent.GetComponentsInChildren<Text>();
        updateScores();
    }

    private void updateScores() {
        List<string> scoreNames = HighScoreManager.instance().getScoreNames();
        List<int>    highscores = HighScoreManager.instance().getHighscores();

        for(int i = 0; i < scoreTexts.Length; i++) {
            namesTexts[i].text = scoreNames[i];
            scoreTexts[i].text = highscores[i].ToString() + "s";
        }
    }
}
