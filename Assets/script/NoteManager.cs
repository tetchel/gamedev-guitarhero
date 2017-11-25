using UnityEngine;
using System.Collections.Generic;

public class NoteManager : MonoBehaviour {

    public GameObject notePrefab;
    public float noteSpawnYPos;

    private double timeMs;

    private List<Note> notes;       // MUST BE SORTED by their spawn time 
    private IEnumerator<Note> notesEnum;

    // Use this for initialization
    void Start() {
        timeMs = 0;

        // Load notes somehow
        notes = loadNotes();

        notesEnum = notes.GetEnumerator();
        notesEnum.MoveNext();
    }

    // Update is called once per frame
    void Update() {
        timeMs += Time.deltaTime * 1000;

        Note current = notesEnum.Current;
        if(current == null) {
            Debug.Log("No more notes!");
        }
        else {
            while(current.getSpawnTime() <= timeMs) {
                spawn(current);
                notesEnum.MoveNext();
                current = notesEnum.Current;
            }
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
            float timestamp = float.Parse(contents[i, 0]);
            int timestampMs = Mathf.RoundToInt(timestamp * 1000);

            List<int> notesAtThisTime = new List<int>();
            for(int j = 1; j < contents.GetLength(1); j++) {
                if(!string.IsNullOrEmpty(contents[i, j])) {
                    Note.NoteColor color = Note.getNoteColor(j - 1);
                    workingNotes.Add(new Note(color, timestampMs));
                } 
            }
        }
        return workingNotes;
    }

    void spawn(Note note) {
        GameObject spawnNote = Instantiate(notePrefab, 
            new Vector3(note.getNoteXPos(), noteSpawnYPos, 0), 
            Quaternion.identity);

        spawnNote.GetComponent<SpriteRenderer>().color = note.getRenderColor();
    }
}
