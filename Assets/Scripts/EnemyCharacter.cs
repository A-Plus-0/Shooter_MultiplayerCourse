using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemyCharacter : Character
{
    private string _sessionID;
    [SerializeField] private Health _health;
    [SerializeField] private Transform _head;
    public Vector3 targetPosition { get; private set; } = Vector3.zero;
    private float _velocityMagnitude = 0;

    public void Init(string sessionID)
    {
        _sessionID = sessionID;
    }

    public Vector2 targetRotate { get; private set; } = Vector2.zero;

    private float _crouchFase = 0;
    [SerializeField] private CapsuleCollider _capsuleCollider;



    private void Start()
    {
        targetPosition = transform.position;
        targetRotate = new Vector2(_head.localEulerAngles.x, transform.localEulerAngles.y);
    }


    private void Update()
    {
        if (_velocityMagnitude > .1f)
        {
            float maxDistance = _velocityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxDistance);
        }
        else
        {
            transform.position = targetPosition;
        }

        Vector3 targetHeadRotate = new Vector3(targetRotate.x, 0, 0);
        _head.localEulerAngles = Quaternion.Slerp(_head.localRotation, Quaternion.Euler(targetHeadRotate), Time.deltaTime * 10).eulerAngles;

        Vector3 targetBodyRotate = new Vector3(0, targetRotate.y, 0);
        transform.localEulerAngles = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(targetBodyRotate), Time.deltaTime * 10).eulerAngles;

        if (isCrouch)
        {
            _crouchFase += Time.deltaTime * 3;
        }
        else
        {
            _crouchFase -= Time.deltaTime * 3;
        }
        _crouchFase = Mathf.Clamp01(_crouchFase);

        _capsuleCollider.center = new Vector3(0, Mathf.Lerp(1f, .875f, Mathf.Clamp01(_crouchFase)), 0);
        _capsuleCollider.height = Mathf.Lerp(2f, 1.75f, Mathf.Clamp01(_crouchFase));
    }

    public void SetCrouch(bool value) => isCrouch = value;
    public void SetSpeed(float value) => speed = value;
    public void SetMaxHP(int value)
    {
        maxHealth = value;
        _health.SetMax(value);
        _health.SetCurrent(value);

    }

    public void RestoreHP(int newValue) {

        _health.SetCurrent(newValue);
    }
    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
    {
        targetPosition = position + (velocity * averageInterval);
        _velocityMagnitude = velocity.magnitude;

        this.velocity = velocity;
    }

    public void ApplyDamage(int damage)
    {
        _health.ApplyDamage(damage);

        Dictionary<string, object> data = new Dictionary<string, object>() {

            {"id", _sessionID },
            {"value", damage }
        };

        MultiplayerManager.Instance.SendMessage("damage", data);
    }
    public void SetRotateX(in float rotation)
    {
        targetRotate = new Vector2(rotation, targetRotate.y);
    }
    public void SetRotateY(in float rotation)
    {
        targetRotate = new Vector2(targetRotate.x, rotation);
    }
}
