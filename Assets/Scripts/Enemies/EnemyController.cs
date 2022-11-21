using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //public float Speed = 4;
    public EnemyScriptableObject EnemyBaseValues;

    private GameObject _player;
    private Rigidbody2D _rb;

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

    public void Initialize( GameObject player )
    {
        _player = player;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();

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

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "bullet")
    //        _health -= quelquechose;
    //}
}
