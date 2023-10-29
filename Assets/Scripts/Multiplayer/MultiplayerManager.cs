using Colyseus;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [field: SerializeField] public LossCounter _lossCounter { get; private set; }
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private EnemyController _enemy;

    private ColyseusRoom<State> _room;
    private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();

    protected override void Awake()
    {
        base.Awake();

        Instance.InitializeClient();
        Connect();
    }

    private async void Connect()
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"speed", _player.speed },
            {"hp", _player.maxHealth}
        };

        _room = await Instance.client.JoinOrCreate<State>("state_handler", data);

        _room.OnStateChange += OnStateChange;
        _room.OnMessage<string>("Shoot", ApplyShoot);
    }

    private void ApplyShoot(string jsomShootInfo)
    {
        ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(jsomShootInfo);
        if (_enemies.ContainsKey(shootInfo.key) == false)
        {
            Debug.LogError("Enemy not exit, but he try shoot");
            return;
        }
        _enemies[shootInfo.key].Shoot(shootInfo);
    }

    private void OnStateChange(State state, bool isFirstState)
    {
        if (!isFirstState) { return; }

        state.players.ForEach((key, player) =>
        {
            if (key == _room.SessionId) CreatePlayer(/*key,*/ player);
            else CreateEnemy(key, player);
        });

        _room.State.players.OnAdd += CreateEnemy;
        _room.State.players.OnRemove += RemoveEnemy;
    }

    private void CreatePlayer(/*string key, */Player player)
    {
        var position = new Vector3(player.pX, player.pY, player.pZ);

        var playerCharacter = Instantiate(_player, position, Quaternion.identity);
        player.OnChange += playerCharacter.OnChange;

        _room.OnMessage<string>("Restart", playerCharacter.GetComponent<PlayerController>().Restart);
    }

    private void CreateEnemy(string key, Player player)
    {
        var position = new Vector3(player.pX, player.pY, player.pZ);

        var enemy = Instantiate(_enemy, position, Quaternion.identity);
        enemy.Init(key, player);

        _enemies.Add(key, enemy);
    }

    private void RemoveEnemy(string key, Player player)
    {
        if (_enemies.ContainsKey(key) == false) return;

        var enemy = _enemies[key];
        _enemy.Destroy();

        _enemies.Remove(key);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_room != null) _room.Leave();
    }


    public string GetSessionID() => _room.SessionId;

    public void SendMessage(string key, Dictionary<string, object> data)
    {
        //Debug.Log(data);

        _room.Send(key, data);
    }

    public void SendMessage(string key, string data)
    {
        _room.Send(key, data);
    }
}

