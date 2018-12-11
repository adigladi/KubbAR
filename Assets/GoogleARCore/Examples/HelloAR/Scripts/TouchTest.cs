#if UNITY_EDITOR
     // NOTE:
     // - InstantPreviewInput does not support `deltaPosition`.
     // - InstantPreviewInput does not support input from
     //   multiple simultaneous screen touches.
     // - InstantPreviewInput might miss frames. A steady stream
     //   of touch events across frames while holding your finger
     //   on the screen is not guaranteed.
     // - InstantPreviewInput does not generate Unity UI event system
     //   events from device touches. Use mouse/keyboard in the editor
     //   instead.
     using Input = GoogleARCore.InstantPreviewInput;
#endif

//Attach this script to an empty GameObject
//Create some UI Text by going to Create>UI>Text.
//Drag this GameObject into the Text field of your GameObject’s Inspector window.

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.Examples.Common;



public class TouchTest : MonoBehaviour
{
	
    public Vector2 startPos;
    public Vector2 direction;

    void Update()
    {
        //Update the Text on the screen depending on current TouchPhase, and the current direction vector

        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position.
                    startPos = touch.position;
                    Debug.Log("Begun", gameObject);
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    direction = touch.position - startPos;
                    Debug.Log("Moving", gameObject);
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    Debug.Log("Ending", gameObject);
                    break;
            }
        }
    }
}