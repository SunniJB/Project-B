using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool canHear, canSee, canTouch;

    private float wallDistance = 3f;
    public int startWalls = 6;

    [SerializeField] private List<Transform> wallPile = new List<Transform>();
    [SerializeField] private List<GameObject> wallPrefabs = new List<GameObject>();

    public float currentHealth = 100, maxHealth, points;
    public GameObject healthBar, pointsText;

    [SerializeField] private float timeUntilEnemy, enemyTime;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private GameObject winCanvas, loseCanvas, pauseCanvas;
    private bool paused = false;


    void Start()
    {
        Time.timeScale = 1f;

        for (int i = 0; i < startWalls; i++) //Chuck some walls in to start
        {
            NewWall(0);
        }
        currentHealth = maxHealth;
        healthBar.GetComponent<Slider>().maxValue = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (paused == false) { Pause(); paused = !paused; return; }
            if (paused == true) { Unpause(); paused = !paused; return; }

        }

        healthBar.GetComponent<Slider>().value = currentHealth;
        pointsText.GetComponent<TextMeshProUGUI>().text = "Points: " + points + "/100";

        timeUntilEnemy += Time.deltaTime;
        if (timeUntilEnemy > enemyTime)
        {
            Instantiate(enemyPrefab, NewWallLocation(), Quaternion.identity);
            timeUntilEnemy = 0f;
        }

        if (points >= 100)
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound("Win");
            Win();
        }

        if (currentHealth <= 0)
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound("Death");
            Lose();
            Debug.Log("You lost");
        }
    }

    public void NewWall(int start = 1) //Procedurally generate walls
    {
        int wallProb;

        if (start > 0) // Make it way more likely that breakable walls spawn
        {
            wallProb = Random.Range(0, 100);
        }

        else
        {
            wallProb = 50;
        }


        int wallID = 0; //Breakable wall

        if (wallProb < 10)
        {
            // Normal wall
            wallID = 2;
        }
        else if (wallProb > 9 && wallProb < 25)
        {
            // Fake wall
            wallID = 1;
        }

        GameObject newWallObject = Instantiate(wallPrefabs[wallID], NewWallLocation(), Quaternion.identity);

        wallPile.Add(newWallObject.transform); //Update the list of walls currenly in the scene

        if (wallID > 0)
        {
            NewWall();
        }

    }

    private Vector3 NewWallLocation() //Make sure walls don't spawn on top of each other
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
    public void RemoveWall(Transform t)
    {
        wallPile.Remove(t);
        Destroy(t.gameObject);
        NewWall();
    }

    private void Win()
    {
        Time.timeScale = 0f;
        winCanvas.SetActive(true);
    }

    private void Lose()
    {
        Time.timeScale = 0f;
        loseCanvas.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        loseCanvas.SetActive(false);
        winCanvas.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
}
