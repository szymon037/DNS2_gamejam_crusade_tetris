using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubBlock : MonoBehaviour
{
    public Transform t;
    private TetrisBlock tb;
    public GameObject coin_prefab;
    public float chanceOfCoinSpawn = 0.25f;
    public float coinHeight = 2f;
    private Material material;
    void Start()
    {
        t = transform;
        if (t.parent != null)
        {
            tb = t.parent.GetComponent<TetrisBlock>();
            tb.subBlocks.Add(this);
        }
        Debug.Log(GetComponent<MeshRenderer>().material.color);

        /*else
        {
            Debug.Log("NULL");
        }*/

        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        Debug.DrawRay(t.position, Vector3.back * 500f, Color.red);
        
    }

    public void SpawnCoin()
    {
        if (coin_prefab != null && Random.value <= chanceOfCoinSpawn)
        {
            Instantiate(coin_prefab, new Vector3(t.position.x, t.position.y + coinHeight, t.position.z), Quaternion.Euler(new Vector3(0f, 90f, 45f)));
        }
    }

    public void ChangeColor(Material mat)
    {
        Debug.Log(GetComponent<MeshRenderer>().material.color.r);
    }
}
