using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    //public SpriteRenderer green, red, yellow, blue;
    public SpriteMask greenMask, redMask, yellowMask, blueMask;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.J)) {
            // Green
            greenMask.enabled = false;
        }
        else {
            greenMask.enabled = true;
        }

        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.K)) {
            redMask.enabled = false;
        }
        else {
            redMask.enabled = true;
        }

        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.L)) {
            yellowMask.enabled = false;
        }
        else {
            yellowMask.enabled = true;
        }

        if(Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.Semicolon)) {
            blueMask.enabled = false;
        }
        else {
            blueMask.enabled = true;
        }
	}
}
