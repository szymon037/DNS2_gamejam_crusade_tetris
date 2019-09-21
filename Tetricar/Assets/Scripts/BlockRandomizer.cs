using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRandomizer : MonoBehaviour
{
    public GameObject[] BlockPrefabs;

    public GameObject GetRandomBlock() {
        return BlockPrefabs[Random.Range(0, BlockPrefabs.Length)];
    }
}
