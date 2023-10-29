using UnityEngine;
using UnityEngine.UI;

public class LossCounter : MonoBehaviour
{
    [SerializeField] private Text _text;
    private int _PlayerLoss = 0;
    private int _EnemyLoss = 0;


    public void SetEnemyLoss(int value)
    {
        _EnemyLoss = value;
        UpdateText();
    }
    public void SetPlayerLoss(int value)
    {
        _PlayerLoss = value;
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = $"{_PlayerLoss} : {_EnemyLoss}";
    }
}
