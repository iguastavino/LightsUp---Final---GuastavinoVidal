using UnityEngine;

public class GulaMov : MonoBehaviour
{
    public float speed = 2f;
    public float enragedSpeed = 4f;

    public GameObject enragedLight;
    public GameObject normalLight;

    public bool enraged;
    public bool enragedAnimation;

    public GameObject pointA;
    public GameObject pointB;

    private Rigidbody2D Rigidbody;
    private Transform currentPoint;

    public static GulaMov instance;

    public float rollingTime = 6f;
    public float iddleTime = 3f;
    public float timeElapsed = 0f;

    public Animator animator;

    public GameObject batteryPrefab;

    public enum GulaState
    {
        Roll,
        Iddle,
    }

    public GulaState gulaState;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;

        Invoke("spawnBattery", 30f);
        Invoke("spawnBattery", 60f);
    }

    void Update()
    { 
        if (BossManager.instance.started)
        {
            timeElapsed += Time.deltaTime;

            if (enraged)
            {
                speed = enragedSpeed;

                if (!enragedAnimation)
                {
                    enragedAnimation = true;
                    if (gulaState != GulaState.Iddle) timeElapsed = 8f;
                    enragedLight.SetActive(true);
                    normalLight.SetActive(false);
                }
            }

            if (timeElapsed <= rollingTime)
            {
                gulaState = GulaState.Roll;
                animator.SetBool("startedRoll", true);
                animator.SetBool("stoppedRoll", false);
                Rigidbody.velocity = new Vector2(currentPoint == pointB.transform ? speed : -speed, Rigidbody.velocity.y);

                if (transform.position.x >= currentPoint.position.x && currentPoint == pointB.transform)
                {
                    flip();
                    currentPoint = pointA.transform;
                }
                else if (transform.position.x <= currentPoint.position.x && currentPoint == pointA.transform)
                {
                    flip();
                    currentPoint = pointB.transform;
                }
            }
            else
            {
                gulaState = GulaState.Iddle;
                animator.SetBool("startedRoll", false);
                animator.SetBool("stoppedRoll", true);
                Rigidbody.velocity = new Vector2(0, 0);
                if (timeElapsed > iddleTime + rollingTime) timeElapsed = 0;
            }
        }
       

    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void spawnBattery()
    {
        Instantiate(batteryPrefab, new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z), Quaternion.identity);
    }
}
