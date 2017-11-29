using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections;

public class NoteManager : MonoBehaviour {

    public GameObject greenNotePrefab, redNotePrefab, yellowNotePrefab, blueNotePrefab;
    public float noteSpawnYPos;

    public AudioSource musicSource;
    public float startTime;

    public float fadeOutTime;

    // Set by the menu when this scene is loaded. Only one NoteManager exists at a time, but this is not enforced yet.
    public static string songDataPath;

    private float timeMs;
    private float initialMusicVolume;
    private bool isFadingOut = false;

    private List<Note> notes;               // MUST BE SORTED by their spawn time 
    private IEnumerator<Note> notesEnum;
    private bool moreNotes = true;

    // Use this for initialization
    void Start() {
        if(startTime != 0) {
            musicSource.time = startTime;
        }

        musicSource.Play();
        timeMs = musicSource.time * 1000;
        StartCoroutine(countdownMusicEnd());

        initialMusicVolume = musicSource.volume;

        // Losing focus briefly can cause the game and audio to go out of sync
        Application.runInBackground = true;
        
        // TODO Remove
        if(songDataPath == null) {
            songDataPath = "E:/Tim/Programs/Unity/guitarhero/Assets/StreamingAssets/songdata/dani_california.csv";
        }
        notes = loadNotes();

        notesEnum = notes.GetEnumerator();
        //moreNotes = notesEnum.MoveNext();

        // TODO remove this after testing is done - skips over notes that have already been played, for when
        // song is started in the middle
        bool after = false;
        while(!after) {
            notesEnum.MoveNext();
            after = notesEnum.Current.getSpawnTime() >= timeMs;
            //Debug.Log("Skip note at " + notesEnum.Current.getSpawnTime());
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        timeMs += Time.deltaTime * 1000;
        //Debug.Log("FixedUpdate Time : " + timeMs);
        //Debug.Log("total time " + timeMs + " deltatime " + Time.deltaTime * 1000);

        Note current = notesEnum.Current;
        if(current == null) {
            //Debug.Log("No more notes!");
        }

        while(moreNotes && current != null && current.getSpawnTime() <= timeMs) {
            spawn(current);
            moreNotes = notesEnum.MoveNext();
            current = notesEnum.Current;
        }

        // Fade out for 5 seconds
        if(isFadingOut) {
            if(musicSource.volume <= 0) {
                SceneManager.LoadScene("postsong");
            }
            else {
                musicSource.volume -= (initialMusicVolume / 5) * Time.deltaTime;
            }
        }
    }

    IEnumerator countdownMusicEnd() {
        float endTime;
        if (fadeOutTime > 0) {
            endTime = fadeOutTime;
        }
        else if (fadeOutTime > musicSource.clip.length) {
            Debug.LogError("FadeOutTime " + fadeOutTime + " is greater than clip length " + musicSource.clip.length);
            endTime = musicSource.clip.length;      // / audioSource.pitch * Time.timeScale;
        }
        else {
            endTime = musicSource.clip.length;      // / audioSource.pitch * Time.timeScale;
        }
        endTime -= startTime;

        yield return new WaitForSeconds(endTime);

        if(fadeOutTime > 0) {
            isFadingOut = true;
        }
        else {
            SceneManager.LoadScene("postsong");
        }
    }

    public static void setSongDataPath(string path) {
        songDataPath = path;
    }

    public double getTime() { return timeMs; }

    List<Note> loadNotes() {
        string[,] contents = CSVReader.parseCSV(songDataPath);

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
                    NoteColor color = NoteColorMethods.getNoteColor(j - 1);
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
