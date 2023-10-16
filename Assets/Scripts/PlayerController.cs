using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform targetMarker;

    public float movementSpeed, rotationSpeed, stopDist, minDistToAvoid, force;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        /*
        movementSpeed = 5f;
        rotationSpeed = 15f;
        stopDist = 2f;
        minDistToAvoid = 6f;
        force = 1f;
        */
    }

    // Update is called once per frame
    void Update()
    {
        // Stop once the player is close enough to the target marker
        if (Vector3.Distance(transform.position, targetMarker.position) < stopDist)
        {
            gameManager.currentlyWalking = false;
            return;
        }

        // Calculate the direction vector from the current position to the target marker
        Vector3 targetPosition = targetMarker.position;
        targetPosition.y = transform.position.y;
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        // Avoid hitting any obstacles
        AvoidObstacles(ref directionToTarget);

        // Build a quaternion for the new rotation towards the target marker
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Move and rotate with interpolation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.Translate(movementSpeed * Time.deltaTime * Vector3.forward);
    }


    /***********
     * AvoidObstacles
     * Using colliders without collisions
     * 
     */
    private void AvoidObstacles(ref Vector3 dir)
    {
        RaycastHit hitInfo;

        // Only detect obstacles - on the obstacle layer
        int layerMask = 1 << 6;

        // Check if there are any obstacles close to the front of this object
        if (Physics.Raycast(transform.position + new Vector3(0f, 1f, 0.5f), transform.forward, out hitInfo, minDistToAvoid, layerMask))
        {
            // Get the normal vector of the raycast's hitpoint 
            Vector3 hitNormal = hitInfo.normal;
            hitNormal.y = 0f;

            // create new direction for object to move in, by adding "force" to the gameobject's 
            // current forward direction - based on the hitpoint normal
            dir = transform.forward + hitNormal * force;
        }
    } 

}
