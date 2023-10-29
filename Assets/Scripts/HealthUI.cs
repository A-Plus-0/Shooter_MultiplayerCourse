using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private RectTransform _filledImage;
    [SerializeField] private float _defoultWidth;

    private void OnValidate()
    {
        _defoultWidth = _filledImage.sizeDelta.x;
    }


    public void UpdateHealth(int max, int current)
    {
        float percent = (float)current / max;
        _filledImage.sizeDelta = new Vector2(_defoultWidth * percent, _filledImage.sizeDelta.y);

    }
}
