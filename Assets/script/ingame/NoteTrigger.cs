using UnityEngine;
using System.Collections.Generic;

public class NoteTrigger : MonoBehaviour {

    public GameObject triggerMask;

    private KeyCode[] keyCodes;

    private List<GameObject> collidingNotes = new List<GameObject>();

    private bool pressed;

    // Use this for initialization
    void Start() {
        string color = name.Replace("_trigger", "");

        // Map this trigger's color to the keys that can be used to activate this trigger
        if      (color == "green")   keyCodes = new KeyCode[] { KeyCode.A, KeyCode.J };
        else if (color == "red")     keyCodes = new KeyCode[] { KeyCode.S, KeyCode.K };
        else if (color == "yellow")  keyCodes = new KeyCode[] { KeyCode.D, KeyCode.L };
        else if (color == "blue")    keyCodes = new KeyCode[] { KeyCode.F, KeyCode.Semicolon };
        else {
            throw new System.Exception("Did not recognize color " + color + " from trigger name: " + name);
        }
    }

    // Update is called once per frame
    void LateUpdate() {
        // See if this Note's key was pressed
        pressed = false;
        foreach(KeyCode kc in keyCodes) {
            if(Input.GetKey(kc)) {
                //Debug.Log("You pressed the key for " + name);
                pressed = true;
                break;
            }
        }

        // Highlight or don't highlight the trigger
        if(pressed) {
            triggerMask.GetComponent<Renderer>().enabled = false;
        }
        else {
            triggerMask.GetComponent<Renderer>().enabled = true;
            // Nothing further to do if the key is not pressed.
            return;
        }

        //collidingNotes.Clear();
    }

    void strum() {
        if(pressed) {
            if(collidingNotes.Count > 0) {
                GameObject playedNote = collidingNotes[0];
                collidingNotes.RemoveAt(0);
                if(playedNote != null) {
                    Destroy(playedNote);
                    //Debug.Log("Played a note " + name);
                    ScoreKeeper.instance().noteHit();
                }
                else {
                    // Note already destroyed - Move on to the next one
                    strum();
                    //Debug.Log("Tried to play a null note");
                }
            }
            else {
                // No colliding notes at this time - a missed note
                //Debug.Log("Nothing to play! " + name);
                ScoreKeeper.instance().noteMissed();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject collidObj = collision.gameObject;

        if(collidObj == null) {
            // already destroyed
            return;
        }
        if (!collidingNotes.Contains(collidObj)) {
            collidingNotes.Add(collidObj);
        }
    }

    private string listToString(List<GameObject> list) {
        string result = "";
        foreach(GameObject obj in list) {
            result += obj.name + ' ';
        }
        return result;
    }
}
