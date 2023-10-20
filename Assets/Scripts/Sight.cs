using UnityEngine;

public class Sight : Sense
{
    public float fieldOfView = 60f;
    public float fieldOfViewDistance = 10f;
    public LayerMask layerMask;

    private Transform playerTrans;
    private Vector3 directionToPlayer;
    private float halfFOV;

    public bool enemySpotted;

    protected override void Initialize()
    {
        // Find the player!
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

        halfFOV = fieldOfView * 0.5f;
    }


    protected override void UpdateSense()
    {
        halfFOV = fieldOfView * 0.5f;
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= detectionRate)
        {
            elapsedTime = 0f;
            DetectAspectWithOverlapSphere();
        }

    }

    private void DetectAspectWithOverlapSphere()
    {

        Collider[] objectsWithinSight = Physics.OverlapSphere(transform.position, fieldOfViewDistance, layerMask);    

        foreach (Collider seen in objectsWithinSight)
        {
            Vector3 directionToSeen = seen.transform.position - transform.position;

            // Check if the collider is within FOV
            if (Vector3.Angle(transform.forward, directionToSeen) <= halfFOV)
            {
                Aspect aspect = seen.GetComponentInParent<Aspect>();
                if (aspect != null)
                {
                    if (aspect.aspectName == aspectName)
                    {
                        enemySpotted = true;
                        Debug.Log("I am seeing " + aspect.gameObject.name);
                    }
                } else if (aspect == null)
                {
                    return;
                }
            }
        }   
    }

}
