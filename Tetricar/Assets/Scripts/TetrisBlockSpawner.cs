using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlockSpawner : MonoBehaviour
{
    Transform t;
    public BlockRandomizer blockRandomizer;
    private GameManager gm;
    private Transform car;
    private float offsetZ = 0f;
    [SerializeField]
    private Transform spawnPoint;

    void Awake()
    {
        t = transform;
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        car = GameObject.FindWithTag("Player").GetComponent<Transform>(); ;

        offsetZ = t.position.z - car.position.z;
    }

    void Update()
    {
        t.position = new Vector3(t.position.x, t.position.y, car.position.z + offsetZ);
    }

    public void SpawnTetris()
    {
        float sign = Mathf.Sign(Random.Range(-1f, 1f));
        float position = Mathf.Round(Random.Range(2f, 3f));
        Instantiate(blockRandomizer.GetRandomBlock(), spawnPoint.position + new Vector3(position * sign * gm.sizeOfBlock, 0f, 0f), Quaternion.Euler(new Vector3(0f, Mathf.Round(Random.Range(-3f, 3f)) * 90f, 0f)), t);
        //Debug.Log("spawnPoint.position: " + spawnPoint.position);
        //Instantiate(blockRandomizer.GetRandomBlock(), t.position + new Vector3(Mathf.Round(Random.Range(-3f, 3f)) * gm.sizeOfBlock, 0f, 0f), Quaternion.Euler(new Vector3(0f, Mathf.Round(Random.Range(-3f, 3f)) * 90f, 0f)), t);
    }
}
