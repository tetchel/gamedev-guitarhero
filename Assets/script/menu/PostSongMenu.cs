using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PostSongMenu : MonoBehaviour {

    public Text songNameText;
    public Text scoreText;
    public Text streakText;
    public Text notesHitText;

    public Text highscoreNotify;
    public InputField nameInput;
    public Button enterHighscoreButton;
    public GameObject endOfGameButtons;

    public GameObject newHighscoreDisplay;

    public static string songRef;

    private ScoreKeeper scoreKeeper;

    // Use this for initialization
    void Start() {
        newHighscoreDisplay.gameObject.SetActive(false);

        scoreKeeper = ScoreKeeper.instance();

        if(songRef == null) {
            Debug.LogError("SONGREF NOT SET FOR POST-SONG");
        }
        songNameText.text = MainMenu.getNiceSongName(songRef);

        int score = scoreKeeper.getScore();
        scoreText.text = "Score: " + score;
        streakText.text = "Best Streak: " + scoreKeeper.getLongestStreak();

        int totalNotes = scoreKeeper.getNotesHit() + scoreKeeper.getNotesMissed();
        float pct = Mathf.Round(((float)scoreKeeper.getNotesHit() / totalNotes) * 100);
        notesHitText.text = "Hit " + scoreKeeper.getNotesHit() + " / " + totalNotes + " - " + pct + "%";

        int ranking = HighScoreManager.instance().getRanking(songRef, score);

        if(ranking != -1) {
            // Hide the Play Again / Exit to Menu buttons to force them to enter a name
            endOfGameButtons.SetActive(false);
            // Show the menus for entering a name etc
            newHighscoreDisplay.SetActive(true);

            highscoreNotify.text = "Your score is the " + HighScoreManager.ordinal(ranking + 1) + " best!";
        }

    }

    public static void setSongref(string songRef) {
        PostSongMenu.songRef = songRef;
    }

    public void onEnterHighscore() {
        string name = nameInput.text;

        //Debug.Log("The name is " + name + " and the score is " + scoreDisplay.getScore());

        HighScoreManager.instance().tryInsertHighscore(songRef, scoreKeeper.getScore(), name);

        enterHighscoreButton.interactable = false;

        endOfGameButtons.SetActive(true);
    }

    public void onPlayAgain() {
        SceneManager.LoadScene("scene");
    }

    public void onViewScores() {
        Scoreboard.setSongref(songRef);
        SceneManager.LoadScene("scoreboard");
    }

    public void onExitToMenu() {
        SceneManager.LoadScene("mainmenu");
    }
}
