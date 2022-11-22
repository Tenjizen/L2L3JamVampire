using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //public float Speed = 4;
    public EntitiesScriptableObject EnemyBaseValues;

    private GameObject _player;
    private Rigidbody2D _rb;

    public float TimeForMove; //remonte
    private float _timeForMove; //remonte
    private Vector3 TargetPos;
    
    enum State
    {
        Waiting,
        Moving
    }

    State _state = State.Waiting;


    private int _health;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _health = EnemyBaseValues.Health;
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
            _timeForMove += Time.deltaTime;

            if (_timeForMove >= TimeForMove)
            {
                TargetPos = _player.transform.position;
                TargetPos.z = 0;
                _timeForMove -= TimeForMove;
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
    public void BackOf(Vector3 direction)
    {
        //_direction = direction;
    }


    private void ILikeTrainAgain()
    {
        Vector3 direction = TargetPos - transform.position;
        direction.z = 0;


        print(TargetPos.magnitude);
        if (direction.magnitude > 0.2f)
        {
            direction.Normalize();
            _rb.velocity = direction * EnemyBaseValues.MoveSpeed;
        }
        else
        {
            _rb.velocity = Vector2.zero;
            _state = State.Waiting;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "bullet")
    //        _health -= quelquechose;
    //}
}
