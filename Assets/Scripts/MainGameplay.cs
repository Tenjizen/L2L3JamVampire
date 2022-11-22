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
    private EnemiesGenerator _enemiesGenerator;

    [SerializeField] List<int> _XPByLevel;

    public float TimerEnd; //en secondes pls, merci
    private float _timerEnd;

    private bool _playerAlive;

    public float Score = 0;

    private float _exp = 0;
    public int Level = 1;

    public Image ImageFillAmout;
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
        _enemiesGenerator = FindObjectOfType<EnemiesGenerator>();
        foreach (var enemy in Enemies)
        {
            enemy.Initialize(Player);
        }

    }

    // Update is called once per frame
    void Update()
    {
        FillAmoutXPBarre();
        if (_timerEnd <= 0 && _playerAlive)
        {
            print("Win");
        }
        else
            _timerEnd -= Time.deltaTime;

        UpdateLevel();
        UpdateText();
    }

    private void UpdateText()
    {
        //TimerText.text = "" + Mathf.Round(_timerEnd * 100f) / 100f;
        TimerText.text = "" + (int)_timerEnd;
        ScoreText.text = "Score : " + Score;
        AmmoText.text = "" + Player.GetComponent<PlayerController>().NumberCurrentAmmo + " / " + Player.GetComponent<PlayerController>().NumberMaxAmmo;
        LevelText.text = "LEVEL " + Level;
    }
    public void WinXP(float exp)
    {
        _exp += exp;
    }
    public void WinScore(float score)
    {
        Score += score;
    }


    public void UpdateLevel()
    {
        if (_exp >= _XPByLevel[Level - 1])
        {
            Level++;
            _exp = 0;
            Time.timeScale = 0f;
            _enemiesGenerator.UpdateAllTimer();
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

    public void FillAmoutXPBarre()
    {
        if ((_exp / _XPByLevel[Level - 1]) > 1.0f)
            ImageFillAmout.fillAmount = 1;
        ImageFillAmout.fillAmount = (_exp / _XPByLevel[Level - 1]);
    }
}
