using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

    public Text scoreText;
    public Text streakText;
    public Text multiplierText;

    private static ScoreKeeper _instance;

    private int _streak = 0;
    private int _score = 0;

    private int _notesHit = 0;
    private int _notesMissed = 0;

    private const int NOTE_POINT_VALUE = 10;

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        }
        else {
            _instance = this;
        }

        updateUI();
    }

    public static ScoreKeeper instance() {
        return _instance;
    }

    public void noteHit() {
        _streak++;
        _notesHit++;

        _score += NOTE_POINT_VALUE * getStreakMultiplier(_streak);
        updateUI();
    }

    public void noteMissed() {
        _streak = 0;
        _notesMissed++;

        GetComponent<AudioSource>().Play();
        updateUI();
    }

    private void updateUI() {
        scoreText.text  = "" + _score;
        streakText.text = "Streak: " + _streak;
        multiplierText.text = "Multiplier: " + getStreakMultiplier(_streak);
    }

    private static int getStreakMultiplier(int streak) {
        if (streak >= 30) {
            return 4;
        }
        else if (streak >= 20) {
            return 3;
        }
        else if (streak >= 10) {
            return 2;
        }
        else {
            return 1;
        }
    }

    void OnDestroy() {
        /*
        if(this == _instance) {
            _instance = null;
        }
        */
    }
}
