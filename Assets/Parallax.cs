using System.Numerics;
using UnityEngine;

public class Parallax : MonoBehaviour

            Material mat;
            public float speed=0.2f;

            [Range((0f,0.5f))] 
            public float speed= 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    { 
        distance += Time,deltaTime* speed;
        mat.SetTextureOffest("_Main text",Vector.right * distance)
    }
}
