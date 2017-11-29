﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

    public Text scoreText;
    public Text streakText;
    public Text multiplierText;

    public NoteManager noteMan;

    public AudioSource missedNoteAudio;

    private static ScoreKeeper _instance;

    private int _streak = 0;
    private int _score = 0;
    private int _longestStreak = 0;

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

    void FixedUpdate() {
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
        if(_streak > _longestStreak) {
            _longestStreak = _streak;
        }
        _streak = 0;
        _notesMissed++;

        missedNoteAudio.Play();
        updateUI();
    }

    private void updateUI() {
        scoreText.text  = "" + _score;
        streakText.text = "Streak: " + _streak;
        int mult = getStreakMultiplier(_streak);

        multiplierText.color = getStreakColor(mult);
        multiplierText.text = "Multiplier: " + mult;
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

    private static Color getStreakColor(int mult) {
        if(mult == 2) {
            return Color.yellow;
        }
        else if(mult == 3) {
            return Color.cyan;
        }
        else if(mult == 4) {
            return Color.green;
        }
        else {
            return Color.white;
        }
    }

    public int getNotesHit() {
        return _notesHit;
    }

    public int getNotesMissed() {
        return _notesMissed;
    }

    public int getLongestStreak() {
        return _longestStreak;
    }

    public int getScore() {
        return _score;
    }

    void OnDestroy() {
        /*
        if(this == _instance) {
            _instance = null;
        }
        */
    }
}
