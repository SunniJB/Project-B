using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform targetMarker;

    public float movementSpeed, rotationSpeed, stopDist, minDistToAvoid, force;

    public GameManager gameManager;

    public GameObject projectile;
    public float launchVelocity = 700f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Shoot();
        }
        // Stop once the player is close enough to the target marker
        if (Vector3.Distance(transform.position, targetMarker.position) < stopDist)
        {
            return;
        }

        // Calculate how to get from the player's current position to the target
        Vector3 targetPosition = targetMarker.position;
        targetPosition.y = transform.position.y;
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        // Avoid hitting any obstacles
        AvoidObstacles(ref directionToTarget);

        // Turn around to look at the target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        //Go there
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.Translate(movementSpeed * Time.deltaTime * Vector3.forward);


    }


    private void AvoidObstacles(ref Vector3 dir) //Find where the walls are, go around them
    {
        RaycastHit hitInfo;

        // Only detect obstacles on the obstacle layer
        int layerMask = 1 << 6;

        // Check if there are any obstacles close to the front of this object
        if (Physics.Raycast(transform.position + new Vector3(0f, 1f, 0.5f), transform.forward, out hitInfo, minDistToAvoid, layerMask))
        {
            // Get the normal vector of the raycast's hitpoint 
            Vector3 hitNormal = hitInfo.normal;
            hitNormal.y = 0f;

            // create new direction for object to move in, by adding "force" to the gameobject's direction
            dir = transform.forward + hitNormal * force;
        }
    }

    public void Shoot()
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound("PlayerShoot");
        float spawnDistance = 1f;
        GameObject ball = Instantiate(projectile, transform.localPosition + spawnDistance * Vector3.forward, transform.rotation);
        ball.GetComponent<Projectile>().gameManager = gameManager;
        ball.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * launchVelocity);
    }

}
