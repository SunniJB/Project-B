using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameManager gameManager;
    public float timer, despawnTime = 2;

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
            hit.gameObject.GetComponent<Animator>().SetTrigger("Break");
            StartCoroutine(LetAnimationFinish(hit)); 
        }
    }

    IEnumerator LetAnimationFinish(Collider other)
    {
        Debug.Log("Waiting started");
        yield return new WaitForSeconds(1);
        gameManager.RemoveWall(other.gameObject.transform);
        Debug.Log("Waiting finished");
    }
}
