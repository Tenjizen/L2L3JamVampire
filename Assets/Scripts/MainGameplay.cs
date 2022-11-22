using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameplay : MonoBehaviour
{
    public static MainGameplay Instance;

    public GameObject Player;
    public GameObject Loot;
    public Transform LootParent;
    public List<EnemyController> Enemies;
    public List<EnemyController> EnemiesTriggerCircle;

    [SerializeField] List<int> _XPByLevel;

    public float TimerEnd; //en secondes pls, merci
    private float _timerEnd;

    private bool _playerAlive;

    public float Score = 0;

    private float _exp = 0;
    private int _level;
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

        UpdateLevel();
    }
    public void WinXP(EntitiesScriptableObject enemy)
    {
        _exp += enemy.BonusExp;
    }
    public void UpdateLevel()
    {
        for (int i = 0; i < _XPByLevel.Count; i++)
        {
            if (_exp >= _XPByLevel[i] && _exp < _XPByLevel[i + 1])
                _level = i + 1;
        }
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
    public EnemyController KickClosestEnemy(Vector3 position)
    {
        float bestDistance = float.MaxValue;
        EnemyController bestEnemy = null;

        foreach (var enemy in EnemiesTriggerCircle)
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
