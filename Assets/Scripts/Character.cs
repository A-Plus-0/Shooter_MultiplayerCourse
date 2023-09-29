using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [field: SerializeField] public float speed { get; protected set; } = 2f;
    [field: SerializeField] public float speedUP { get; protected set; } = 3f;
    public Vector3 velocity { get; protected set; }

    public bool isCrouch { get; protected set; }

}
