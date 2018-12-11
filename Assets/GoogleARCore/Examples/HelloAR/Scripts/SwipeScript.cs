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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeScript : MonoBehaviour
{

    Vector2 startPos, endPos, direction; // touch start position, touch end position, swipe direction
    float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to sontrol throw force in Z direction

    [SerializeField]
    float throwForceInX = 1f;

    [SerializeField]
    float throwForceInY = 1.5f;

    [SerializeField]
    float throwForceInZ = 25f; // to control throw force in Z direction

    Rigidbody rb;

    public GameObject Baton;

    bool throwActive = false;


    GameObject NewBaton;

    void Start()
    {
        rb = Baton.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (throwActive)
        {

            // if you touch the screen
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Debug.Log("Touch", gameObject);
                // getting touch position and marking time when you touch the screen
                touchTimeStart = Time.time;
                startPos = Input.GetTouch(0).position;
            }

            // if you release your finger
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {

                // marking time when you release it
                touchTimeFinish = Time.time;

                // calculate swipe time interval 
                timeInterval = touchTimeFinish - touchTimeStart;

                // getting release finger position
                endPos = Input.GetTouch(0).position;

                // calculating swipe direction in 2D space
                direction = startPos - endPos;

                // add force to baton rigidbody in 3D space depending on swipe time, direction and throw forces
                rb.isKinematic = false;
                rb.AddRelativeForce(-direction.x * throwForceInX, -direction.y * throwForceInY, throwForceInZ / timeInterval);
                NewBaton.transform.parent = null;

                Destroy(NewBaton, 3f);
                throwActive = false;
                Invoke("SpawnBaton", 2);
            }
        }
    }

    public void SpawnBaton()
    {
        NewBaton = Instantiate(Baton, new Vector3(0f, -0.1f, 0.555f), Quaternion.identity);
        NewBaton.transform.parent = gameObject.transform;
        NewBaton.transform.localPosition = new Vector3(0f, -0.1f, 0.555f);
        NewBaton.transform.localRotation = Quaternion.Euler(37.607f, 0f, 0f);
        NewBaton.SetActive(true);
        rb = NewBaton.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Invoke("ActivateThrow", 1);
    }

    public void ActivateThrow()
    {
        throwActive = true;
    }
}
