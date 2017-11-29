using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strum : MonoBehaviour {

    //public SpriteRenderer green, red, yellow, blue;
    public GameObject greenTrigger, redTrigger, yellowTrigger, blueTrigger;

    private GameObject[] allTriggers;

	// Use this for initialization
	void Start () {
        allTriggers = new GameObject[] { greenTrigger, redTrigger, yellowTrigger, blueTrigger };	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if(Input.GetKeyDown(KeyCode.Space)) {
            foreach(GameObject trig in allTriggers) {
                trig.SendMessage("strum");
            }
        }
	}
}
