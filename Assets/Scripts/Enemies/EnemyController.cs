using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //public float Speed = 4;
    public EntitiesScriptableObject EnemyBaseValues;

    private GameObject _player;
    private Rigidbody2D _rb;

    private float _timeForMove;
    private float _timerForMove;
    private Vector3 TargetPos;

    enum State
    {
        Waiting,
        Moving
    }

    State _state = State.Waiting;

    public int NumberRushMax = 3;
    private int _numberRush = 0;
    private int _health;
    private int _healthMax = 100;

    private SpriteRenderer _spriteRenderer;

    private bool _canLoot = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _health = EnemyBaseValues.Health;
        _timeForMove = EnemyBaseValues.TimeForMove;
        _spriteRenderer.sprite = EnemyBaseValues.Visuel;
    }

    public void Initialize(GameObject player)
    {
        _player = player;

    }

    // Update is called once per frame
    void Update()
    {
        if (_state == State.Waiting)
        {
            _timerForMove += Time.deltaTime;

            if (_timerForMove >= _timeForMove)
            {
                TargetPos = _player.transform.position;
                TargetPos.z = 0;
                _timerForMove -= _timeForMove;
                _state = State.Moving;
            }
        }
        if (_state == State.Moving)
        {
            ILikeTrainAgain();
        }

        if (!EnemyBaseValues.ILikeTrain)
        {
            MoveToPlayer();
        }

        if (_health <= 0)
        {
            MainGameplay.Instance.WinXP(EnemyBaseValues.BonusExp);
            MainGameplay.Instance.WinScore(EnemyBaseValues.BonusScore);
            if (EnemyBaseValues.ILikeTrain && _canLoot)
            {
                _canLoot = false;
                Instantiate(MainGameplay.Instance.Loot, this.transform.position, Quaternion.identity, MainGameplay.Instance.LootParent);
            }
            MainGameplay.Instance.Enemies.Remove(this);
            GameObject.Destroy(gameObject, 0);
        }
    }

    private void MoveToPlayer()
    {
        Vector3 direction = _player.transform.position - transform.position;
        direction.z = 0;

        if (direction.sqrMagnitude > 0)
        {
            direction.Normalize();
            _rb.velocity = direction * EnemyBaseValues.MoveSpeed;

        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }
    public void BackOf(Vector3 direction, int force)
    {
        transform.position += direction * force;
    }

    public void Damage(int damage)
    {
        _health -= damage;
    }


    private void ILikeTrainAgain()
    {
        Vector3 direction = TargetPos - transform.position;
        direction.z = 0;


        if (direction.magnitude > 0.2f)
        {
            direction.Normalize();
            _rb.velocity = direction * EnemyBaseValues.MoveSpeed;
        }
        else
        {
            _rb.velocity = Vector2.zero;
            _state = State.Waiting;
            _numberRush++;
            if (_numberRush >= NumberRushMax)
            {
                MainGameplay.Instance.Enemies.Remove(this);
                GameObject.Destroy(gameObject, 0);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var bullet = collision.gameObject.GetComponent<Bullet>();
        if (collision.gameObject.tag == "Bullet")
        {
            if (bullet.Principal)
                _health -= bullet.DamagePrincipal;
            else
                _health -= bullet.DamageSecond;

            GameObject.Destroy(collision.gameObject, 0);
        }
    }
}
