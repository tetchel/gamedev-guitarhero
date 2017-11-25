using UnityEngine;

public class Note {

    // This describes which 'track' the note spawns/exists on. See getRenderColor() for actual color.
    public enum NoteColor {
        GREEN, RED, YELLOW, BLUE
    };

    public enum NoteXPosition {
        GREEN   = -3,
        RED     = -1,
        YELLOW  = 1,
        BLUE    = 3
    };

    // How far the middle two notes are in x from y = 0. 
    // So, the green note will have x = -2*noteOffset, the red x = -noteOffset, 
    // yellow x = noteOffset, blue x = 2*noteOffset
    public float noteOffset = 1.5f;

    private int spawnTimeMs;
    private NoteColor color;

    private int id;
    private static int currentID = 0;

    // in seconds, how long it takes for the note to be "played" from its spawn time
    public const float spawnToNoteTime = 2.5f;

    public Note(NoteColor color, int spawnTimeMs) {
        this.color = color;
        this.spawnTimeMs = spawnTimeMs;

        id = currentID;
        currentID++;        
    }

    public int getSpawnTime()   { return spawnTimeMs; }

    public static NoteColor getNoteColor(int index) {
        if (index == 0) return NoteColor.GREEN;
        if (index == 1) return NoteColor.RED;
        if (index == 2) return NoteColor.YELLOW;
        if (index == 3) return NoteColor.BLUE;

        throw new System.Exception("Invalid note color index " + index);
    }

    public int getNoteXPos() {
        if (color == NoteColor.GREEN)   return (int)NoteXPosition.GREEN;
        if (color == NoteColor.RED)     return (int)NoteXPosition.RED;
        if (color == NoteColor.YELLOW)  return (int)NoteXPosition.YELLOW;
        if (color == NoteColor.BLUE)    return (int)NoteXPosition.BLUE;

        throw new System.Exception("Invalid note color " + color);
    }

    public Color getRenderColor() {
        if(color == NoteColor.GREEN) {
            return new Color(0x41, 0xB0, 0x54);
        }
        if(color == NoteColor.RED) {
            return new Color(0xEA, 0x24, 0x24);
        }
        if(color == NoteColor.YELLOW) {
            return new Color(0xEA, 0xFF, 0x27);
        }
        if(color == NoteColor.BLUE) {
            return new Color(0x19, 0x78, 0xD6);
        }

        throw new System.Exception("Invalid note color " + color);
    }
}
