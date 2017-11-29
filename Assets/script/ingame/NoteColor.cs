using UnityEngine;
using System.Collections;

 public enum NoteColor {
    GREEN, RED, YELLOW, BLUE
}

   
static class NoteColorMethods {
    public static int getXPosition(this NoteColor color) {
        if (color == NoteColor.GREEN)   return -3;
        if (color == NoteColor.RED)     return -1;
        if (color == NoteColor.YELLOW)  return 1;
        if (color == NoteColor.BLUE)    return 3;

        throw new System.Exception("Invalid NoteColor " + color);
    }
    
    // Convert an index to a NoteColor
    public static NoteColor getNoteColor(this int index) {
        if      (index == 0) return NoteColor.GREEN;
        else if (index == 1) return NoteColor.RED;
        else if (index == 2) return NoteColor.YELLOW;
        else if (index == 3) return NoteColor.BLUE;

        throw new System.Exception("Invalid note color index " + index);
    }


}