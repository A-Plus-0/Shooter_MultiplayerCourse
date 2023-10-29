using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int _max;
    private int _current;
    [SerializeField] private HealthUI _healthUI;

    public void SetMax(int max)
    {
        _max = max;
        UpdateHP();
    }

    public void SetCurrent(int current)
    {
        _current = current;
        UpdateHP();
    }

    public void ApplyDamage(int damage)
    {
        _current -= damage;
        UpdateHP();
    }


    private void UpdateHP()
    {
        _healthUI.UpdateHealth(_max, _current);
    }
}
