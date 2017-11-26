﻿using UnityEngine;
using System.Collections.Generic;

public class NoteManager : MonoBehaviour {

    public GameObject greenNotePrefab, redNotePrefab, yellowNotePrefab, blueNotePrefab;
    public float noteSpawnYPos;

    private double timeMs;

    private List<Note> notes;               // MUST BE SORTED by their spawn time 
    private IEnumerator<Note> notesEnum;
    private bool moreNotes = true;

    // Use this for initialization
    void Start() {
        timeMs = 0;

        notes = loadNotes();

        notesEnum = notes.GetEnumerator();
        moreNotes = notesEnum.MoveNext();
    }

    // Update is called once per frame
    void FixedUpdate() {
        timeMs += Time.deltaTime * 1000;
        //Debug.Log("total time " + timeMs + " deltatime " + Time.deltaTime * 1000);

        Note current = notesEnum.Current;
        while(moreNotes && current != null && current.getSpawnTime() <= timeMs) {
            spawn(current);
            moreNotes = notesEnum.MoveNext();
            current = notesEnum.Current;
        }
    }

    List<Note> loadNotes() {
        string dcPath = Application.dataPath + "/dani_california.csv";

        string[,] contents = CSVReader.parseCSV(dcPath);

        List<Note> workingNotes = new List<Note>();

        // For each row in the parsed CSV
        // Get the timestamp in column [0]
        // Then get the indices of any non-empty cells in that row
        // These marked cells are notes to be played at that timestamp
        for(int i = 1; i < contents.GetLength(0); i++) {
            if(string.IsNullOrEmpty(contents[i, 0])) {
                // Skip this row if the first cell is blank
                continue;
            }
            float timestamp = float.Parse(contents[i, 0]);
            int timestampMs = Mathf.RoundToInt(timestamp * 1000);

            for(int j = 1; j < contents.GetLength(1); j++) {
                if(!string.IsNullOrEmpty(contents[i, j])) {
                    NoteColor color = Note.getNoteColor(j - 1);
                    workingNotes.Add(new Note(color, timestampMs));
                } 
            }
        }
        return workingNotes;
    }

    void spawn(Note note) {
        GameObject fab = getFab(note.getColor());
        //Debug.Log("Making a fab " + fab.name);

        // GameObject spawnNote =
        Instantiate(fab);
    }

    GameObject getFab(NoteColor color) {
        GameObject fab = null;
        if (color == NoteColor.GREEN) {
            fab = greenNotePrefab;
        }
        if(color == NoteColor.RED) {
            fab = redNotePrefab;
        }
        if(color == NoteColor.YELLOW) {
            fab = yellowNotePrefab;
        }
        if(color == NoteColor.BLUE) {
            fab = blueNotePrefab;
        }

        return fab;
    }
}
