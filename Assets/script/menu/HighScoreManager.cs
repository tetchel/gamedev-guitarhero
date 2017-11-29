using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighScoreManager {

    private static HighScoreManager _instance;

    private const string HIGHSCORE_PREF = "highscore_";
    private const string NAME_PREF = "highscorer_";

    private const string DEFAULT_NAME = "NO1";
    private const int DEFAULT_SCORE = 0;

    private const int NUM_HIGHSCORES = 10;

    public static HighScoreManager instance() {
        if (_instance == null) {
            _instance = new HighScoreManager();
        }
        return _instance;
    }

    public bool tryInsertHighscore(string songRef, int score, string name) {
        List<int> highscores = loadHighscores(songRef);
        List<string> scoreNames = loadScoreNames(songRef);

        int ranking = getRanking(songRef, score);
        if(ranking != -1) {
            int prev = highscores[ranking];
            string prevName = scoreNames[ranking];

            highscores[ranking] = score;
            scoreNames[ranking] = name;

            // Shift lower scores down
            for (int j = ranking; j < highscores.Count - 1; j++) {
                int tmp = highscores[j + 1];
                highscores[j + 1] = prev;
                prev = tmp;

                // shift name too
                string tmpStr = scoreNames[j + 1];
                scoreNames[j + 1] = prevName;
                prevName = tmpStr;
            }

            saveHighscores(songRef);
            saveScoreNames(songRef);
            return true;
        }
        return false;
    }

    public int getRanking(string songRef, int score) {
        List<int> highscores = loadHighscores(songRef);
        for (int i = 0; i < highscores.Count; i++) {
            if (highscores[i] < score) {
                return i;
            }
        }
        return -1;
    }

    private void saveHighscores(string songRef) {
        List<int> highscores = loadHighscores(songRef);
        for (int i = 0; i < NUM_HIGHSCORES; i++) {
            PlayerPrefs.SetFloat(HIGHSCORE_PREF + songRef + i, highscores[i]);
            //Debug.Log("saving score #" + i + " " + highscores[i]);
        }
    }

    private void saveScoreNames(string songRef) {
        List<string> scoreNames = loadScoreNames(songRef);
        for (int i = 0; i < NUM_HIGHSCORES; i++) {
            PlayerPrefs.SetString(NAME_PREF + songRef + i, scoreNames[i]);
            //Debug.Log("saving score #" + i + " " + scoreNames[i]);
        }
    }

    public List<int> loadHighscores(string songRef) {
        List<int> scores = new List<int>();
        for (int i = 0; i < NUM_HIGHSCORES; i++) {
            scores.Insert(i, PlayerPrefs.GetInt(HIGHSCORE_PREF + songRef + i, DEFAULT_SCORE));
        }
        return scores;
    }

    public List<string> loadScoreNames(string songRef) {
        List<string> names = new List<string>();
        for (int i = 0; i < NUM_HIGHSCORES; i++) {
            names.Insert(i, PlayerPrefs.GetString(NAME_PREF + songRef + i, DEFAULT_NAME));
        }
        return names;
    }

    public static string ordinal(int i) {
        // only handle 1-10
        if (i % 10 == 1) {
            return i + "st";
        }
        else if (i % 10 == 2) {
            return i + "nd";
        }
        else if (i % 10 == 3) {
            return i + "rd";
        }
        else {
            return i + "th";
        }
    }
}
