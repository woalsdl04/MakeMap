using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapgenerator : MonoBehaviour
{
    
    

    public int size;
    public GameObject chunk;
    public float scale;
    public float depth;

    void Start()
    {
        for(int x = -size; x <= size; x++) 
        {                               
            for(int y = -size; y <= size; y++)
            {
                var c = Instantiate(chunk, new Vector3(x * 15, 0, y * 15), Quaternion.identity).GetComponent<Chunk>();

                c.CreateBlock(scale, depth);
            }
        }

    }

    void Update()
    {

    }
}