using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private Transform t;
    private Rigidbody rb;

    public float baseSpeed;
    public float speed;
    public float rotationDuration;
    private bool isRotating = false;
    public AnimationCurve rotationCurve;
    private float angleOfCar;
    private Vector3 scale = Vector3.zero;
    private GameManager gm;

    public bool gameOver = false;
    public Camera carCamera;

    private bool hasStarted = false;

    private float timer = 0f;
    void Start()
    {
        t = transform;
        rb = GetComponent<Rigidbody>();
        baseSpeed = 10f;
        rotationDuration = 0.2f;
        scale = t.localScale;
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

    }

    void Update()
    {
        if (Time.timeScale == 1) {
            t.Translate(Vector3.forward * speed * Time.deltaTime);

            if (!gameOver && Input.GetKeyDown(KeyCode.A) && !isRotating && angleOfCar > -90f)
            {
                StartCoroutine(Rotate(Vector3.up, -90f));
            }
            if (!gameOver && Input.GetKeyDown(KeyCode.D) && !isRotating && angleOfCar < 90f)
            {
                StartCoroutine(Rotate(Vector3.up, 90f) );
            }

            var hit = !Physics.Raycast(t.position, Vector3.down, 5f);

            if (!gameOver && hit) 
            {
                Debug.Log("Game over!");
                gameOver = true;
                GameOver();
            }

            if (Input.GetKey(KeyCode.Q))
            {
                carCamera.GetComponent<CameraShaker>().StartShaking();
            }

            hasStarted = (Input.GetKey(KeyCode.Space) && !hasStarted);
            
            if (hasStarted && speed < 150f){
                speed = baseSpeed + Mathf.Sqrt(timer * 100);
                timer += Time.deltaTime;
            }
        }
    }

    private IEnumerator Rotate(Vector3 rotationVector, float rotationAngle)     //rotate Right Left
    {
        angleOfCar += rotationAngle;
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

    private void GameOver()
    {
        carCamera.transform.parent = null;
        rb.constraints = 0;
        rb.useGravity = true;
        GameManager.instance.CurrentState = GameManager.GameState.END;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            gm.AddPoint();
            Destroy(other.gameObject);
        }
    }
}
