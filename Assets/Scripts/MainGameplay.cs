using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MainGameplay : MonoBehaviour
{
    public static MainGameplay Instance;

    public GameObject Player;
    public GameObject Loot;
    public GameObject CanvasLvlUp;
    public Transform LootParent;
    public List<EnemyController> Enemies;
    public List<EnemyController> EnemiesTriggerCircle;

    [SerializeField] List<int> _XPByLevel;

    public float TimerEnd; //en secondes pls, merci
    private float _timerEnd;

    private bool _playerAlive;

    public float Score = 0;

    private float _exp = 0;
    float _globalExp = 0;
    private int _level;
    private int _levelUpgrade;

    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI AmmoText;
    public TextMeshProUGUI LevelText;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
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

        print("lvl" + _level);
        print(_levelUpgrade);

        UpdateLevel();
        canvasLvlUp();
        UpdateText();
        FillAmoutXPBarre();
    }

    private void UpdateText()
    {
        TimerText.text = "" + Mathf.Round(_timerEnd * 100f) / 100f;
        ScoreText.text = "Score : " + Score;
        AmmoText.text = "" + Player.GetComponent<PlayerController>().NumberCurrentAmmo + " / " + Player.GetComponent<PlayerController>().NumberMaxAmmo;
        LevelText.text = "LEVEL " + _levelUpgrade;
    }
    public void WinXP(float exp)
    {
        _exp += exp;
        _globalExp += exp;
    }
    public void WinScore(float score)
    {
        Score += score;
    }


    public void UpdateLevel()
    {
        if (_exp >= _XPByLevel[_level] && _exp < _XPByLevel[_level + 1])
        {
            _levelUpgrade = _level + 1;
            _exp = 0;
            print("coucou");
            print(_XPByLevel[_level]);
            print(_XPByLevel[_level + 1]);
            print(_exp);
        }
    }
    public void canvasLvlUp()
    {
        if (_level != _levelUpgrade && _levelUpgrade > 1)
        {
            _exp = 0;
            _level = _levelUpgrade;
            Time.timeScale = 0f;
            CanvasLvlUp.SetActive(true);
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

    public Image image;
    public void FillAmoutXPBarre()
    {
        image.fillAmount = _exp / _XPByLevel[_levelUpgrade];
        //image.fillAmount = _globalExp / _XPByLevel[_levelUpgrade];
    }
}
