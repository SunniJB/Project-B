using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameManager gameManager;
    public float timer, despawnTime = 2;
    [SerializeField] private SoundManager soundManager;

    private void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > despawnTime)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("Breakable"))
        {
            soundManager.PlaySound("Explosion");
            hit.gameObject.GetComponent<Animator>().SetTrigger("Break");
            StartCoroutine(LetAnimationFinish(hit));
            gameManager.points += 5;
        }
        
        if (hit.CompareTag("Enemy"))
        {
            soundManager.PlaySound("GetPoint");
            Destroy(hit.gameObject);
            gameManager.points += 10;
        }
    }

    IEnumerator LetAnimationFinish(Collider other)
    {
        Debug.Log("Waiting Started");
        yield return new WaitForSeconds(1);
        Debug.Log("Waiting ended");
        gameManager.RemoveWall(other.gameObject.transform);

    }
}
