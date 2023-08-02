using Colyseus;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [SerializeField] private GameObject _player;
    [SerializeField] private EnemyController _enemy;
    private ColyseusRoom<State> _room;
    protected override void Awake()
    {
        base.Awake();

        Instance.InitializeClient();
        Connect();
    }


    private async void Connect()
    {
        _room = await Instance.client.JoinOrCreate<State>("state_handler");
        _room.OnStateChange += OnStateChange;
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
        var position = new Vector3(player.x, 0f, player.y);

        Instantiate(_player, position, Quaternion.identity);
    }
    private void CreateEnemy(string key, Player player)
    {
        var position = new Vector3(player.x, 0f, player.y);

        var enemy = Instantiate(_enemy, position, Quaternion.identity);
        player.OnChange += enemy.OnChange;
    }

    private void RemoveEnemy(string key, Player player)
    {

    }



    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_room != null) _room.Leave();
    }


    public void SendMessage(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }
}

