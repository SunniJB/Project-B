using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 5f;

    private Vector3 targetPosition;
    private float minX, minZ, maxX, maxZ;
    private float distanceToStop = 3f;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        minX = minZ = -17f;
        maxX = maxZ = -minZ;

        // find the first waypoint to move towards
        GetNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we're near the current waypoint
        if (Vector3.Distance(targetPosition, transform.position) <= distanceToStop)
        {
            GetNextWaypoint();
        }

        // Rotate towards it
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);

        // Update rotation and position
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.Translate(new Vector3(0f, 0f, movementSpeed * Time.deltaTime));

    }

    private void GetNextWaypoint()
    {
        targetPosition = new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound("LosePoint");
            gameManager.currentHealth -= 10;
        }
    }

}