using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour {

    private float a = 0.001f;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        gameObject.GetComponent<Renderer>().material.mainTextureOffset += 
            new Vector2((a * PlayerController.GetSpeed()) / transform.position.z, 0);
	}
}
