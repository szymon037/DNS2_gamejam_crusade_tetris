using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubBlock : MonoBehaviour
{
    private Transform t;
    private TetrisBlock tb;

    void Start()
    {
        t = transform;
        tb = t.parent.GetComponent<TetrisBlock>();
        tb.subBlocksTransforms.Add(t);
    }

    void Update()
    {
        Debug.DrawRay(t.position, Vector3.back * 500f, Color.red);
        
    }
}
