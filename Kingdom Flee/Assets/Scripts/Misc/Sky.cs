using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    [SerializeField] private float blendDuration;

    [SerializeField] private Renderer rend;

    private void Update()
    {
        float lerp = Mathf.PingPong(Time.time, blendDuration) / blendDuration;
        rend.material.SetFloat("_Blend", lerp);
    }
}
