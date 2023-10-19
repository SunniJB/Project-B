using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public bool currentlyWalking;

    public bool canHear, canSee, canTouch;

    private float wallDistance = 3f;
    public int startWalls = 6;

    [SerializeField] private List<Transform> wallPile = new List<Transform>();
    [SerializeField] private List<GameObject> wallPrefabs = new List<GameObject>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < startWalls; i++) //Chuck some walls in to start
        {
            NewWall(0);
        }

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

    public void NewWall(int start = 1)
    {
        int wallProb;

        if (start > 0)
        {
            wallProb = Random.Range(0, 100);
        }

        else
        {
            wallProb = 50;
        }


        int wallID = 0; //Normal wall

        if (wallProb < 10)
        {
            // Breakable wall
            wallID = 2;
        }
        else if (wallProb > 9 && wallProb < 25)
        {
            // Fake wall
            wallID = 1;
        }

        GameObject newWallObject = Instantiate(wallPrefabs[wallID], NewWallLocation(), Quaternion.identity);

        wallPile.Add(newWallObject.transform);

        if (wallID > 0)
        {
            NewWall();
        }

    }

    private Vector3 NewWallLocation()
    {
        Vector3 newLocation;
        do
        {
            newLocation = new Vector3(Random.Range(-24, 25), 0f, Random.Range(-24, 25));
        } while (WallTooClose(newLocation));
        return newLocation;
    }

    private bool WallTooClose(Vector3 location)
    {
        bool distanceTooClose = false;

        foreach (Transform wall in wallPile)
        {
            if (Vector3.Distance(wall.transform.position, location) < wallDistance)
            {
                distanceTooClose = true;
            }
        }
        return distanceTooClose;
    }
}
