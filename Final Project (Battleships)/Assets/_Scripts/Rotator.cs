using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    private GameObject obj;

    private void OnMouseDown()
    {
        obj = GameController.lastClicked;
        if(obj == null || obj.GetComponent<DragnDrop>().sz == 1)
        {
            return;
        }

        obj.transform.position = obj.GetComponent<DragnDrop>().init_pos;

        if(obj.transform.eulerAngles.z == 270.0f)
        {
            obj.GetComponent<DragnDrop>().rotated = true;
            obj.transform.eulerAngles += new Vector3(0, 0, 90);
        } else
        {
            obj.GetComponent<DragnDrop>().rotated = false;
            obj.transform.eulerAngles -= new Vector3(0, 0, 90);
        }

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
