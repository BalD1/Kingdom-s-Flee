using System.Collections;
using System.Collections.Generic;
using System.Text;
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

    public void Setup(Sprite _image, int _minAmount, int _maxAmount)
    {
        displayImage.sprite = _image;

        StringBuilder sb = new StringBuilder("x ");
        sb.Append(_minAmount.ToString());
        sb.Append(" ~ ");
        sb.Append(_maxAmount.ToString());

        count.text = sb.ToString();
    }
}
