using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackgroundController : MonoBehaviour
{
    private float startPos;
    public GameObject cam;
    public float parallaxEffect;

    void start()
    {
        startPos = transform.position.x;
    }

    void update()
    {
        float distance = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}
    