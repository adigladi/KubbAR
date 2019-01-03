using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScript : MonoBehaviour
{

    public SwipeScript SwipeScript;
    AudioSource audioData;
    public AudioClip knugHit;
	public AudioClip winSound;
    public AudioClip failSound;
    GameObject P1Klossar;
    GameObject P2Klossar;
    public GameObject P1Win;
    public GameObject P2Win;
    public GameObject P1Loss;
    public GameObject P2Loss;
	public GameObject BG;
    public GameObject P1Active;
    public GameObject P2Active;
    private float velKonst = .2F;
    public bool HitActive = false;

    IEnumerator WaitFunction(GameObject kloss)
    {
        yield return new WaitForSeconds(1);
        if (CheckKloss(kloss))
        {
            if (kloss.tag == "Knug")
            {
                DidIWin();
            }
            else
            {
                Destroy(kloss);
                HitActive = false;
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        HitActive = true;
        float hitvol = col.relativeVelocity.magnitude * velKonst;
        if (col.gameObject.tag == "P1Kloss")
        {
            audioData = col.gameObject.GetComponent<AudioSource>();
            audioData.PlayOneShot(audioData.clip, hitvol);
            StartCoroutine(WaitFunction(col.gameObject));
        }

        else if (col.gameObject.tag == "P2Kloss")
        {
            audioData = col.gameObject.GetComponent<AudioSource>();
            audioData.PlayOneShot(audioData.clip, hitvol);
            StartCoroutine(WaitFunction(col.gameObject));
        }
        else if (col.gameObject.tag == "Knug")
        {
			audioData = col.gameObject.GetComponent<AudioSource>();
            audioData.PlayOneShot(knugHit, hitvol);
            StartCoroutine(WaitFunction(col.gameObject));
        }
        else if (col.gameObject.tag == "Floor")
        {
            audioData = col.gameObject.GetComponent<AudioSource>();
            audioData.PlayOneShot(audioData.clip, hitvol);
            HitActive = false;
        }
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
    private void DidIWin()
    {
        if (P1Active.activeSelf)
        {
            P1Active.SetActive(false);
            P1Klossar = GameObject.FindGameObjectWithTag("P1Container");
            P1Klossar.SetActive(true);
            if (P1Klossar.transform.childCount > 1)
            {
                P2Won();
				audioData.PlayOneShot(failSound, 1);
            }
            else
            {
                P1Won();
				audioData.PlayOneShot(winSound, 1);
            }
        }
        else
        {
            P2Active.SetActive(false);
            P2Klossar = GameObject.FindGameObjectWithTag("P2Container");
            P2Klossar.SetActive(true);
            if (P2Klossar.transform.childCount > 1)
            {
                P1Won();
				audioData.PlayOneShot(failSound, 1);
            }
            else
            {
                P2Won();
				audioData.PlayOneShot(winSound, 1);
            }
        }
    }
    private void P1Won()
    {
		BG.SetActive(true);
		P1Win.SetActive(true);
        P2Loss.SetActive(true);
    }

    private void P2Won()
    {
		BG.SetActive(true);
		P2Win.SetActive(true);
        P1Loss.SetActive(true);
    }

}
