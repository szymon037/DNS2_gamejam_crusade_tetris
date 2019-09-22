using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    private Transform t;
    private Rigidbody rb;

    // Rotation
    public float rotationDuration;
    private bool isRotating = false;
    public AnimationCurve rotationCurve;

    // Moving
    public float movingDuration;
    private bool isMoving;
    public AnimationCurve movingCurve;

    // Dropping
    public float dropDuration;
    public AnimationCurve dropCurve;
    public float dropDistance = 40f;

    // Spawn Ease In
    private bool spawnEaseIn = false;
    private float spawnEaseInDuration = 0.65f;
    public AnimationCurve easeInCurve;
    public float spawnDistance = 45f;

    public float forceValue = 0f;
    private GameManager gm;
    private Queue<Vector3> moveQueue;
    private int maxSizeMoveQueue = 3;
    public bool dropped = false;
    public List<SubBlock> subBlocks;

    private float shortestDistance = 9999f;
    private Vector3 nearestBlock;
    private float offsetZ = 60f;
    private Vector3 teleportPoint = Vector3.zero;
    private bool foundBlock = false;
    private Transform car;
    private float differenceCentre = 0f;
    private float timer = 0f;

    [SerializeField] private GameObject explosion_particle;

    void Start()
    {
        t = transform;
        rb = GetComponent<Rigidbody>();

        rotationDuration = 0.15f;
        movingDuration = 0.15f;
        dropDuration = 0.2f;
        forceValue = 100f;
        moveQueue = new Queue<Vector3>();
        car = GameObject.FindWithTag("Player").GetComponent<Transform>(); ;

        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        ChangeColors();

        //Debug.Log("t.position: " + t.position);
        StartCoroutine(SpawnEaseIn());
        
    }

    void Update()
    {
        if (!spawnEaseIn)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                StartCoroutine(Rotate(Vector3.up, 90f));
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //StartCoroutine(Move(Vector3.left));

                if (moveQueue.Count < maxSizeMoveQueue)
                    moveQueue.Enqueue(Vector3.left);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                //StartCoroutine(Move(Vector3.right));

                if (moveQueue.Count < maxSizeMoveQueue)
                    moveQueue.Enqueue(Vector3.right);

            }

            if (!isMoving && moveQueue.Count > 0)
            {
                StartCoroutine(Move(moveQueue.Dequeue()));
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isRotating && !isMoving)
            {
                Drop();
                dropped = true;
            }
            if (!dropped)
                t.position = new Vector3(t.position.x, Mathf.Sin(timer * 3f) * 4f, t.position.z);
            timer += Time.deltaTime;
        }

    }

    private IEnumerator Rotate(Vector3 rotationVector, float rotationAngle)     //rotate Right Left
    {
        isRotating = true;
        float rotationTimer = 0f;
        float rotatedAngle = 0f;
        float oldAngle = 0f;
        float newAngle = 0f;
        float angle = 0f;

        while (rotationTimer <= rotationDuration)
        {
            float rotationPercentage = rotationTimer / rotationDuration;
            newAngle = rotationCurve.Evaluate(rotationPercentage) * rotationAngle;
            angle = newAngle - oldAngle;

            t.Rotate(rotationVector * angle, Space.World);

            rotatedAngle += angle;

            rotationTimer += Time.fixedDeltaTime;
            oldAngle = newAngle;
            yield return null;
        }
        t.Rotate(rotationVector * (rotationAngle - rotatedAngle), Space.World);
        //yield return new WaitForSeconds(0.1f);
        isRotating = false;
    }

    private IEnumerator Move(Vector3 direction)
    {
        isMoving = true;
        float movingTimer = 0f;
        Vector3 startPosition = t.position;

        while (movingTimer < movingDuration)
        {
            float movingPercentage = movingTimer / movingDuration;
            //t.Translate(direction * movingCurve.Evaluate(movingPercentage) * sizeOfBlock);
            t.position = new Vector3(startPosition.x, startPosition.y, t.position.z) + (direction * movingCurve.Evaluate(movingPercentage) * gm.sizeOfBlock);
            movingTimer += Time.deltaTime;
            yield return null;
        }
        t.position = new Vector3(startPosition.x, startPosition.y, t.position.z) + direction * gm.sizeOfBlock;
        isMoving = false;
    }

    private IEnumerator DropCoroutine()
    {
        t.parent = null;
        float dropTimer = 0f;
        t.position = new Vector3(t.position.x, 0f, t.position.z);
        Vector3 startPosition = t.position;
        Vector3 endPosition = Vector3.zero;

        while (dropTimer < dropDuration)
        {
            float dropPercentage = dropTimer / dropDuration;
            t.position = startPosition + (Vector3.back * dropCurve.Evaluate(dropPercentage) * dropDistance);
            dropTimer += Time.deltaTime;
            yield return null;
        }
        endPosition = startPosition + Vector3.back * dropDistance;

        //t.position = teleportPoint;
        startPosition = teleportPoint;
        float secondDropDistance = 0f;

        if (foundBlock)
            secondDropDistance = offsetZ - differenceCentre - gm.sizeOfBlock;
        else
            secondDropDistance = 2f * offsetZ;

        dropTimer = 0f;

        while (dropTimer < dropDuration)
        {
            float dropPercentage = dropTimer / dropDuration;
            t.position = startPosition + (Vector3.back * dropCurve.Evaluate(dropPercentage) * secondDropDistance);
            dropTimer += Time.deltaTime;
            yield return null;
        }
        if (foundBlock)
            t.position = new Vector3(t.position.x, t.position.y, nearestBlock.z + differenceCentre + gm.sizeOfBlock);
        else
            t.position = new Vector3(t.position.x, t.position.y, nearestBlock.z + 20f);

        BlockAdded();
    }

    private IEnumerator SpawnEaseIn()
    {
        float movingTimer = 0f;
        Vector3 startPosition = t.position;
        spawnEaseIn = true;

        while (movingTimer < spawnEaseInDuration)
        {
            float movingPercentage = movingTimer / spawnEaseInDuration;
            t.position = new Vector3(startPosition.x, startPosition.y, t.parent.position.z + spawnDistance) + (Vector3.back * easeInCurve.Evaluate(movingPercentage) * spawnDistance);
            movingTimer += Time.deltaTime;
            yield return null;
        }
        t.position = new Vector3(startPosition.x, startPosition.y, t.parent.position.z + spawnDistance) + Vector3.back * spawnDistance;
        //Debug.Log(new Vector3(startPosition.x, startPosition.y, t.parent.position.z) + Vector3.back * spawnDistance);
        spawnEaseIn = false;
    }

    

    private void BlockAdded()
    {
        if (foundBlock)
        {
            SpawnCoins();
            gm.carCameraShaker.StartShaking();
            //Instant part
            GameObject particle = Instantiate(explosion_particle, new Vector3(nearestBlock.x, nearestBlock.y, nearestBlock.z + gm.sizeOfBlock * 0.5f), Quaternion.identity);
            Destroy(particle, 2.5f);
        }
        Destroy(rb, 1f);
        gm.blockAdded = true;
        if (!foundBlock)
            Destroy(gameObject);
        Destroy(this);
    }

    private void FindNearestBlock()
    {
        RaycastHit hit;

        foreach (SubBlock sb in subBlocks)
        {
            if (Physics.Raycast(sb.t.position, Vector3.back, out hit, 500f))
            {
                float distance = Vector3.Distance(sb.t.position, hit.transform.position);
                if (sb.t.parent != hit.transform.parent && distance < shortestDistance)
                {
                    nearestBlock = hit.transform.position;
                    shortestDistance = distance;
                    differenceCentre = t.position.z - sb.t.position.z;
                }
                if (nearestBlock != Vector3.zero)
                    foundBlock = true;
                //Debug.Log("nearestBlock: " + nearestBlock);
                //Debug.Log("sb.t.position: " + sb.t.position);
            }
        }
        
    }

    private void Drop()
    {
        FindNearestBlock();
        if (foundBlock)
        {
            teleportPoint = new Vector3(t.position.x, 0f, nearestBlock.z + offsetZ);
        }
        else
            teleportPoint = new Vector3(t.position.x, 0f, car.position.z + offsetZ);

        StartCoroutine(DropCoroutine());


        //rb.AddForce(Vector3.back * forceValue, ForceMode.Impulse);
    }

    private void SpawnCoins()
    {
        foreach (SubBlock sb in subBlocks)
        {
            sb.SpawnCoin();
        }
    }

    private void ChangeColors()
    {
        Material mat = gm.materials[Random.Range(0, gm.materials.Length)];
        foreach (SubBlock sb in subBlocks)
        {
            Debug.Log("KURWAAAAAAAAAAAAAAAAAAAAA");

            sb.ChangeColor(mat);
        }
    }

}
