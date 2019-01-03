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
using System.Linq;


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
    public HitScript HitScript;
    public GameObject Baton;
    public GameObject Player1;
    public GameObject Player2;
    public GameObject P1Turn;
    public GameObject P2Turn;
    public GameObject Counter;
    GameObject P1Klossar;
    GameObject P2Klossar;
    GameObject Knug;
    bool throwActive = false;
    public bool P1Active = false;
    int numberThrows = 0;
    GameObject NewBaton;
    bool KnugHit = false;

    void Start()
    {
        rb = Baton.GetComponent<Rigidbody>();
    }

    IEnumerator WaitForTurnSwitch()
    {
        yield return new WaitForSeconds(3);
        KnugHit = CheckKloss(Knug);
        if (P1Active == true && KnugHit == false)
        {
            Player1.SetActive(false);
            P2Turn.SetActive(true);
        }
        else if (P1Active == false && KnugHit == false)
        {
            Player2.SetActive(false);
            P1Turn.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (throwActive)
        {

            // if you touch the screen
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
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
                numberThrows += 1;
                RemainingThrows.remainingNum = 6 - numberThrows;
                throwActive = false;
                if (numberThrows == 6)
                {
                    StartCoroutine(WaitForTurnSwitch());
                    Counter.SetActive(false);
                }
                else if (CheckKloss(Knug)) {
                    Counter.SetActive(false);
                }
                else if (numberThrows < 6)
                {
                    Invoke("SpawnBaton", 2);
                }
            }
        }
    }
    public void ChangePlayer()
    {
        if (P1Active)
        {
            P2Turn.SetActive(true);
            Player2.SetActive(true);
            P1Klossar.SetActive(false);
            P2Klossar.SetActive(true);
            P2Turn.SetActive(false);
        }
        else
        {
            P1Turn.SetActive(true);
            Player1.SetActive(true);
            P1Klossar.SetActive(true);
            P2Klossar.SetActive(false);
            P1Turn.SetActive(false);
        }
        numberThrows = 0;
        RemainingThrows.remainingNum = 6 - numberThrows;
        Counter.SetActive(true);
        Invoke("SpawnBaton", 0.3f);
        P1Active = !P1Active;
    }
    private void SpawnBaton()
    {
        NewBaton = Instantiate(Baton, new Vector3(0.0163f, -0.1988f, 0.5013f), Quaternion.identity);
        NewBaton.transform.parent = gameObject.transform;
        NewBaton.transform.localPosition = new Vector3(0.0163f, -0.1988f, 0.5013f);
        NewBaton.transform.localRotation = Quaternion.Euler(-52.953f, 0f, 0f);
        NewBaton.SetActive(true);
        rb = NewBaton.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Invoke("ActivateThrow", 0.2f);
    }
    public void StartGame()
    {
        P1Klossar = GameObject.FindGameObjectWithTag("P1Container");
        P2Klossar = GameObject.FindGameObjectWithTag("P2Container");
        Knug = GameObject.FindGameObjectWithTag("Knug");
        SpawnBaton();
        Player1.SetActive(Player1);
        P1Active = true;
        P2Klossar.SetActive(false);
        numberThrows = 0;
        RemainingThrows.remainingNum = 6 - numberThrows;
        Counter.SetActive(true);
    }
    public void ActivateThrow()
    {
        throwActive = true;
    }

    public bool CheckKloss(GameObject kloss)
    {
        if (kloss.transform.eulerAngles.x <= 320f && kloss.transform.eulerAngles.x >= 220f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
