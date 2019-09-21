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

    private float sizeOfBlock = 10f;
    public float forceValue = 0f;

    void Start()
    {
        t = transform;
        rb = GetComponent<Rigidbody>();

        rotationDuration = 0.15f;
        movingDuration = 0.2f;
        forceValue = 100f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(Rotate(Vector3.up, 90f));
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(Move(Vector3.left));
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(Move(Vector3.right));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Drop();
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
            t.position = startPosition + (direction * movingCurve.Evaluate(movingPercentage) * sizeOfBlock);
            movingTimer += Time.deltaTime;
            yield return null;
        }
        t.position = startPosition + direction * sizeOfBlock;
        isMoving = false;
    }

    private void Drop()
    {
        rb.AddForce(Vector3.back * forceValue, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Tetris"))
        {
            Destroy(rb, 1f);
            Destroy(GetComponent<TetrisBlock>());
        }
    }


}
