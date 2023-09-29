using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _character;
    [SerializeField] private EnemyGun _gun;
    [SerializeField] private List<float> _receiveTimeInterval = new List<float>() { 0, 0, 0, 0, 0 };
    private float AverageInterval
    {
        get
        {
            int receiveTimeIntervalCount = _receiveTimeInterval.Count;
            float summ = 0;
            for (int i = 0; i < receiveTimeIntervalCount; i++)
            {
                summ += _receiveTimeInterval[i];
            }
            return summ / receiveTimeIntervalCount;
        }
    }
    private float _lastReceiveTime = 0;
    private Player _player;

    public void Init(Player player)
    {
        _player = player;
        _character.SetSpeed(player.speed);
        player.OnChange += OnChange;
    }

    public void Shoot(in ShootInfo info)
    {
        Vector3 position = new Vector3(info.pX,
                                       info.pY,
                                       info.pZ);
        Vector3 velocity = new Vector3(info.dX,
                                       info.dY,
                                       info.dZ);
        _gun.Shoot(position, velocity);
    }

    public void Destroy()
    {
        if (_player != null) _player.OnChange -= OnChange;
        Destroy(gameObject);
    }

    private void SaveReceiveTime()
    {
        float interval = Time.time - _lastReceiveTime;
        _lastReceiveTime = Time.time;

        _receiveTimeInterval.Add(interval);
        _receiveTimeInterval.RemoveAt(0);

    }

    internal void OnChange(List<DataChange> changes)
    {
        SaveReceiveTime();

        Vector3 position = _character.targetPosition;
        Vector3 velocity = _character.velocity;

        Vector2 rotate = _character.targetRotate;

        foreach (DataChange dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "pX":
                    position.x = (float)dataChange.Value;
                    break;
                case "pY":
                    position.y = (float)dataChange.Value;
                    break;
                case "pZ":
                    position.z = (float)dataChange.Value;
                    break;

                case "vX":
                    velocity.x = (float)dataChange.Value;
                    break;
                case "vY":
                    velocity.y = (float)dataChange.Value;
                    break;
                case "vZ":
                    velocity.z = (float)dataChange.Value;
                    break;


                case "rX":
                    rotate.x = (float)dataChange.Value;
                    break;
                case "rY":
                    rotate.y = (float)dataChange.Value;
                    break;


                case "iC":
                    _character.SetCrouch((bool)dataChange.Value);
                    break;


                default:
                    Debug.LogWarning("Не обрабатывается изменение поля " + dataChange.Field);
                    break;
            }
        }
        _character.SetRotateY(rotate.y);
        _character.SetRotateX(rotate.x);



        _character.SetMovement(position, velocity, AverageInterval);
    }
}
