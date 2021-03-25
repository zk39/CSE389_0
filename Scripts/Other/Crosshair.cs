using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {
    private AudioSource src;
	private Vector3 MouseCoords;
	public float MouseSensitivity = 1f;

    private void Start()
    {
        src = GetComponent<AudioSource>();
    }
    void Update () {
		MouseCoords = Input.mousePosition;
		MouseCoords = Camera.main.ScreenToWorldPoint (MouseCoords);
		transform.position = Vector2.Lerp (transform.position, MouseCoords, MouseSensitivity);

        if (Input.GetMouseButtonDown(0)==true)
        {
            src.Play();
        }
        
    } 
}
