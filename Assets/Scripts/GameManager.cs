using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public bool currentlyWalking;

    public bool canHear, canSee, canTouch;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        PlayFootsteps();
    }
    public void PlayFootsteps() //Play footsteps when moving. currentlyWalking is set in the player controller and target controller.
    {
        if (currentlyWalking == true && audioSource.isPlaying == false)
        {
            audioSource.Play();
        }
        else if (currentlyWalking == false && audioSource.isPlaying == true)
        {
            audioSource.Stop();
        }
    }
}
