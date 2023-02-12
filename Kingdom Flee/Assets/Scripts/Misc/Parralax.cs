using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralax : MonoBehaviour
{
    [SerializeField] private float parallaxEffect;

    private float length, startPos;

    private void Start()
    {
        startPos = this.transform.position.x;
        length = this.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate()
    {
        float tmp = (Camera.main.transform.position.x * (1 - parallaxEffect));
        float dist = (Camera.main.transform.position.x * parallaxEffect);

        this.transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (tmp > startPos + length) startPos += length;
        else if (tmp < startPos - length) startPos -= length;   
    }
}
