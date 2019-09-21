using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHandling : MonoBehaviour
{
    public int CoinValue = 100;
    public GameObject Player;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Player)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().OnCoinPickUp(CoinValue);

            Destroy(gameObject);
        }
    }
}
