using UnityEngine;

public class Sense : MonoBehaviour
{
    public Aspect.aspect aspectName = Aspect.aspect.Enemy;
    public float detectionRate = 1f;
    protected float elapsedTime = 0f;
    protected virtual void Initialize() { }
    protected virtual void UpdateSense() { }

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        elapsedTime = 0f;
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSense();
    }
}
