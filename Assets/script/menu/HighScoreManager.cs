using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighScoreManager {

    private static HighScoreManager _instance;

    private List<int> highscores;
    private List<string> scoreNames;

    private const string HIGHSCORE_PREF = "highscore_";
    private const string NAME_PREF = "highscorer_";

    private const string DEFAULT_NAME = "NO1";
    private const int DEFAULT_SCORE = 0;

    private const int NUM_HIGHSCORES = 10;

    protected HighScoreManager() {
        highscores = loadHighscores();
        scoreNames = loadScoreNames();

        // highscores.Clear();
        // scoreNames.Clear();

        while (highscores.Count < NUM_HIGHSCORES) {
            highscores.Add(DEFAULT_SCORE);
        }
        while (scoreNames.Count < NUM_HIGHSCORES) {
            scoreNames.Add(DEFAULT_NAME);
        }
    }

    public static HighScoreManager instance() {
        if (_instance == null) {
            _instance = new HighScoreManager();
        }
        return _instance;
    }

    public bool tryInsertHighscore(int score, string name) {
        int ranking = getRanking(score);
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

            saveHighscores();
            saveScoreNames();
            return true;
        }
        return false;
    }

    public int getRanking(int score) {
        for (int i = 0; i < highscores.Count; i++) {
            if (highscores[i] < score) {
                return i;
            }
        }
        return -1;
    }

    public List<int> getHighscores() {
        return highscores;
    }

    public List<string> getScoreNames() {
        return scoreNames;
    }

    private void saveHighscores() {
        for (int i = 0; i < NUM_HIGHSCORES; i++) {
            PlayerPrefs.SetFloat(HIGHSCORE_PREF + i, highscores[i]);
            //Debug.Log("saving score #" + i + " " + highscores[i]);
        }
    }

    private void saveScoreNames() {
        for (int i = 0; i < NUM_HIGHSCORES; i++) {
            PlayerPrefs.SetString(NAME_PREF + i, scoreNames[i]);
            //Debug.Log("saving score #" + i + " " + scoreNames[i]);
        }
    }

    private List<int> loadHighscores() {
        List<int> scores = new List<int>();
        for (int i = 0; i < NUM_HIGHSCORES; i++) {
            scores.Insert(i, PlayerPrefs.GetInt(HIGHSCORE_PREF + i, DEFAULT_SCORE));
        }
        return scores;
    }

    private List<string> loadScoreNames() {
        List<string> names = new List<string>();
        for (int i = 0; i < NUM_HIGHSCORES; i++) {
            names.Insert(i, PlayerPrefs.GetString(NAME_PREF + i, DEFAULT_NAME));
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
