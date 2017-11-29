using UnityEngine;
using System.Collections;

public class NoteBehaviour : MonoBehaviour {

    // Use this for initialization
    void Start() {
         
    }

    // Update is called once per frame
    void LateUpdate() {

    }

    void destroy() {
        // Called as an animation event after the note exits the trigger

        // If the Note makes it to this point without being destroyed by the player playing it, the player missed it :(

        ScoreKeeper.instance().noteMissed();
        Destroy(gameObject);
    }
}
