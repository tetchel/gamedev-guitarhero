using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections;

public class NoteManager : MonoBehaviour {

    public GameObject greenNotePrefab, redNotePrefab, yellowNotePrefab, blueNotePrefab;
    public float noteSpawnYPos;
    public float startTime = 0;

    // One music source per song exists in the scene. This is a crap way of handling the multiple tracks. I tried
    // the proper way below (line ~55) but I could not get it to work reliably, so this is my workaround.
    public AudioSource[] musicSources;

    public Pauser pauser;

    //public float fadeOutTime;

    // Set by the menu before this scene is loaded.
    //public static string songMp3Path;
    public static string songCsvPath;

    private float timeMs;
    //private float initialMusicVolume;
    //private bool isFadingOut;

    private List<Note> notes;               // MUST BE SORTED by their spawn time 
    private IEnumerator<Note> notesEnum;
    private bool moreNotes = true;

    // Use this for initialization
    void Start() {
        //Debug.Log("Playing " + songMp3Path);
        //Debug.Log("and reading " + songCsvPath);

        if(songCsvPath == null) {
            // Disaster!
            Debug.LogError("SONG DATA WAS NOT SET!");
            //Debug.LogError("mp3Path: " + songMp3Path);
            Debug.LogError("csvPath: " + songCsvPath);
            SceneManager.LoadScene("mainmenu");

            // for testing only
            //songMp3Path = "E:/Tim/Programs/Unity/guitarhero/Assets/sound/music/twinkle_twinkle.ogg";
            songCsvPath = "E:/Tim/Programs/Unity/guitarhero/Assets/StreamingAssets/songdata/twinkle_twinkle.csv";
        }

        /*
        // Load track
        // NONE of this seems to work reliably. It never works on Dani and after one failure, all tracks will
        // fail until the game is restarted. No error is shown or anything, just the clip.length == 0 
        // and it will not play. Maybe due to my own failure, maybe due to this bug:
        // https://forum.unity.com/threads/streaming-audio-zero-length-and-no-isplaying.9409/
        // https://issuetracker.unity3d.com/issues/streaming-audio-is-broken
        WWW url = new WWW(songMp3Path);
        while(url.isDone) {
            // wait
        }
        AudioClip clip = url.GetAudioClip();
        //AudioClip clip = WWWAudioExtensions.GetAudioClip(url);
        clip.LoadAudioData();
        musicSource.clip = clip;
        if(clip == null) {
            Debug.LogError("NULL CLIP");
        }
        else if(clip.length == 0) {
            Debug.LogError("0-length clip. " + songMp3Path);
        }
        */

        // workaround - have a GO in the scene for each song, and load the one whose name matches the track we want
        AudioSource musicSource = null;
        foreach(AudioSource src in musicSources) {
            if(src.gameObject.name.Contains(MainMenu.getSongRef(songCsvPath))) {
                // This is the one we want to play
                musicSource = src;
            }
        }

        if(musicSource == null) {
            Debug.LogError("musicSource is NULL. No GameObject matched songref for " + songCsvPath);
        }

        pauser.setMusicSource(musicSource);

        musicSource.time = startTime;
        //musicSource.PlayScheduled(AudioSettings.dspTime);
        musicSource.Play();

        timeMs = musicSource.time * 1000;
        StartCoroutine(countdownMusicEnd(musicSource.clip.length));

        //initialMusicVolume = musicSource.volume;

        // Losing focus briefly can cause the game and audio to go out of sync
        Application.runInBackground = true;
        
        notes = loadNotes();

        notesEnum = notes.GetEnumerator();
        moreNotes = notesEnum.MoveNext();

        // skips over notes that have already been played, for when song is started in the middle
        /*
        bool after = false;
        while(!after) {
            notesEnum.MoveNext();
            after = notesEnum.Current.getSpawnTime() >= timeMs;
            //Debug.Log("Skip note at " + notesEnum.Current.getSpawnTime());
        }*/
    }

    // Update is called once per frame
    void FixedUpdate() {
        timeMs += Time.deltaTime * 1000;
        //Debug.Log("FixedUpdate Time : " + timeMs);
        //Debug.Log("total time " + timeMs + " deltatime " + Time.deltaTime * 1000);

        Note current = notesEnum.Current;
        //Debug.Log("it's " + timeMs + " and the next note is at " + current.getSpawnTime());
        while(moreNotes && current != null && current.getSpawnTime() <= timeMs) {
            spawn(current);
            moreNotes = notesEnum.MoveNext();
            current = notesEnum.Current;
        }

        /*
        // Fade out for 5 seconds
        if(isFadingOut) {
            if(musicSource.volume <= 0) {
                musicSource.volume = initialMusicVolume;
                SceneManager.LoadScene("postsong");
            }
            else {
                musicSource.volume -= (initialMusicVolume / 5) * Time.deltaTime;
            }
        }
        */
    }

    void OnDestroy() {
        StopAllCoroutines();
        //Debug.Log("Note Manager destroyed");
    }

    IEnumerator countdownMusicEnd(float length) {
        float endTime = length;
        /*
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
        */

        endTime -= startTime;
        if(endTime == 0) {
            // manifestation of failing to load the track properly
            Debug.LogError("EndTime was 0. StartTime was " + startTime);
        }

        yield return new WaitForSeconds(endTime);
        SceneManager.LoadScene("postsong");
        /*
        if(fadeOutTime > 0) {
            isFadingOut = true;
        }
        else {
            SceneManager.LoadScene("postsong");
        }
        */
    }

    public static void setSongData(/*string mp3Path,*/ string csvPath) {
        //songMp3Path = mp3Path;
        songCsvPath = csvPath;
        //Debug.Log("songCsvPath set to " + csvPath);
    }

    public double getTime() { return timeMs; }

    List<Note> loadNotes() {
        string[,] contents = CSVReader.parseCSV(songCsvPath);

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
