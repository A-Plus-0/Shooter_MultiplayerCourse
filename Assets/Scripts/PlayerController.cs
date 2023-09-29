using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private PlayerGun _gun;
    [SerializeField] private float _mouseSensetivite = 2f;
    private MultiplayerManager _multiplayerManager;

    // [SerializeField] ControlsSetting _cs;
    [SerializeField] KeyCode _jumpKey;
    [SerializeField] KeyCode _CrouchKey;
    [SerializeField] KeyCode _speedUpKey;

    private void Start()
    {
        _multiplayerManager = MultiplayerManager.Instance;
    }
    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        bool isShoot = Input.GetMouseButton(0);

        bool space = Input.GetKeyDown(_jumpKey);

        bool isCrouch = Input.GetKey(_CrouchKey);

        bool isSpeedUp = Input.GetKey(_speedUpKey);


        _player.SetInput(h, v, mouseX * _mouseSensetivite, isCrouch, isSpeedUp);

        _player.RotateX(-mouseY * _mouseSensetivite);

        if (space) _player.Jump();

        if (isShoot && _gun.TryShoot(out ShootInfo shootInfo)) SendShoot(ref shootInfo);

        SendMove();
    }

    private void SendShoot(ref ShootInfo shootInfo)
    {
        shootInfo.key = _multiplayerManager.GetSessionID();
        string json = JsonUtility.ToJson(shootInfo);

        _multiplayerManager.SendMessage("shoot", json);
    }

    private void SendMove()
    {
        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity, out Vector2 rotate, out bool isCrouch);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX", position.x},
            {"pY", position.y},
            {"pZ", position.z},

            {"vX", velocity.x},
            {"vY", velocity.y},
            {"vZ", velocity.z},

            {"rX", rotate.x},
            {"rY", rotate.y},

            {"iC", isCrouch}
        };
        _multiplayerManager.SendMessage("move", data);
    }
}

[System.Serializable]
public struct ShootInfo
{
    public string key;

    public float pX;
    public float pY;
    public float pZ;

    public float dX;
    public float dY;
    public float dZ;
}
