using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameplay : MonoBehaviour
{
    public static MainGameplay Instance;

    public GameObject Player;
    public List<EnemyController> Enemies;

    public float TimerEnd; //en secondes pls, merci
    private float _timerEnd;

    private bool _playerAlive;

    public float Score = 0;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _timerEnd = TimerEnd;
        _playerAlive = Player.GetComponent<PlayerController>().isAlive;
        foreach (var enemy in Enemies)
        {
            enemy.Initialize(Player);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_timerEnd <= 0 && _playerAlive)
        {
            //print("Win");
        }
        else
            _timerEnd -= Time.deltaTime;
    }

    public EnemyController GetClosestEnemy(Vector3 position)
    {
        float bestDistance = float.MaxValue;
        EnemyController bestEnemy = null;

        foreach (var enemy in Enemies)
        {
            Vector3 direction = enemy.transform.position - position;

            float distance = direction.sqrMagnitude;

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestEnemy = enemy;
            }
        }

        return bestEnemy;
    }
    public bool PlayerIsAlive(bool die)
    {
        return _playerAlive = true;
    }
}
