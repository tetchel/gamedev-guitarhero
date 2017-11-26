using UnityEngine;
using System.Collections;

public class NoteBehaviour : MonoBehaviour {

    // "green", "red", "yellow", or "blue"
    public string color;

    // Use this for initialization
    void Start() {
       
    }

    // Update is called once per frame
    void LateUpdate() {

       

        // This note's key was pressed - check for this note being in position to be played

    }

    void destroy() {
        // Called as an animation event after the note exits the trigger

        // If the Note makes it to this point without being destroyed by the player playing it, the player missed it :(

        ScoreKeeper.instance().noteMissed();
        Destroy(gameObject);
    }
}
