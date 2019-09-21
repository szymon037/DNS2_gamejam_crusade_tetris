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

    public float forceValue = 0f;
    private GameManager gm;
    private Queue<Vector3> moveQueue;
    private int maxSizeMoveQueue = 3;
    public bool dropped = false;
    public List<Transform> subBlocksTransforms;

    private float shortestDistance = 9999f;
    private Vector3 nearestBlock;
    private float offsetZ = 60f;
    private Vector3 teleportPoint = Vector3.zero;
    private bool foundBlock = false;
    private Transform car;
    private float differenceCentre = 0f;

    void Start()
    {
        t = transform;
        rb = GetComponent<Rigidbody>();

        rotationDuration = 0.15f;
        movingDuration = 0.2f;
        dropDuration = 0.2f;
        forceValue = 100f;
        moveQueue = new Queue<Vector3>();
        car = GameObject.FindWithTag("Player").GetComponent<Transform>(); ;


        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        
    }

    void Update()
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
            secondDropDistance = 1.5f * offsetZ;

        dropTimer = 0f;

        while (dropTimer < dropDuration)
        {
            float dropPercentage = dropTimer / dropDuration;
            t.position = startPosition + (Vector3.back * dropCurve.Evaluate(dropPercentage) * secondDropDistance);
            dropTimer += Time.deltaTime;
            yield return null;
        }
        t.position = new Vector3(t.position.x, t.position.y, nearestBlock.z + differenceCentre + gm.sizeOfBlock);
        BlockAdded();
    }

    

    

    private void BlockAdded()
    {
        Destroy(rb, 1f);
        Destroy(GetComponent<TetrisBlock>());
        gm.blockAdded = true;
    }

    private void FindNearestBlock()
    {
        RaycastHit hit;

        foreach (Transform s_t in subBlocksTransforms)
        {
            if (Physics.Raycast(s_t.position, Vector3.back, out hit, 500f))
            {
                float distance = Vector3.Distance(s_t.position, hit.transform.position);
                if (s_t.parent != hit.transform.parent && distance < shortestDistance)
                {
                    nearestBlock = hit.transform.position;
                    shortestDistance = distance;
                    differenceCentre = t.position.z - s_t.position.z;
                }
                foundBlock = true;
            }
        }
        
    }

    private void Drop()
    {
        FindNearestBlock();
        if (foundBlock)
        {
            teleportPoint = new Vector3(t.position.x, t.position.y, nearestBlock.z + offsetZ);
        }
        else
            teleportPoint = new Vector3(t.position.x, t.position.y, car.position.z + offsetZ);

        StartCoroutine(DropCoroutine());


        //rb.AddForce(Vector3.back * forceValue, ForceMode.Impulse);
    }

}
