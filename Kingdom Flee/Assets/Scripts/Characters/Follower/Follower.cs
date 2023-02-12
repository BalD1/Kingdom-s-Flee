using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private SO_FollowerColors colors;

    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private SpriteRenderer bodyRenderer;

    private void Awake()
    {
        headRenderer.color = colors.GetRandomHeadColor();
        bodyRenderer.color = colors.GetRandomBodyColor();
    }
}
