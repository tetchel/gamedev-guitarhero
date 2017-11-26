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
}