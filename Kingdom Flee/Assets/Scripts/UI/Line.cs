using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Line : MonoBehaviour
{
    [SerializeField] private Image displayImage;
    [SerializeField] private TextMeshProUGUI count;

    public void Setup(Sprite _image, int _count)
    {
        displayImage.sprite = _image;
        count.text = "x " + _count;
    }
}
