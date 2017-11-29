﻿using UnityEngine;

public class Note {

    // How far the middle two notes are in x from y = 0. 
    // So, the green note will have x = -2*noteOffset, the red x = -noteOffset, 
    // yellow x = noteOffset, blue x = 2*noteOffset
    public float noteOffset = 1.5f;

    private int spawnTimeMs;
    private NoteColor color;

    private int id;
    private static int currentID = 0;

    // in seconds, how long it takes for the note to be "played" (be completely contained by the trigger) 
    // from its spawn time
    public const float spawnToNoteTime = 2.5f;

    public Note(NoteColor color, int spawnTimeMs) {
        this.color = color;
        this.spawnTimeMs = Mathf.RoundToInt(spawnTimeMs - spawnToNoteTime * 1000);

        id = currentID;
        currentID++;        
    }

    public int getSpawnTime()   { return spawnTimeMs; }

    public NoteColor getColor() { return color; }
}
