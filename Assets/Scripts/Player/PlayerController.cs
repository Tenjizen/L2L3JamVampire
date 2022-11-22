using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject PrefabBullet;

    public float CoolDown = 0.2f;
    private float _timerCoolDownFirstShoot;

    public Transform BulletParent;
    public EntitiesScriptableObject PlayerBaseValues;

    private int _health;
    private int _healthMax = 100;


    [HideInInspector] public int NumberMaxAmmo = 2;
    public int NumberCurrentAmmo = 2;
    private bool _canUseSecondShoot = true;
    private float _timerCoolDownSecondShoot;
    public float CoolDownSecondShoot = 2.0f;

    public bool isAlive = true;


    // Start is called before the first frame update
    void Start()
    {
        _health = PlayerBaseValues.Health;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (MainGameplay.Instance.Enemies.Count > 0)
            FirstShoot();
        SecondShoot();
        Die();
        UpdateLife();
    }

    private void FirstShoot()
    {
        _timerCoolDownFirstShoot += Time.deltaTime;

        if (_timerCoolDownFirstShoot < CoolDown)
            return;

        _timerCoolDownFirstShoot -= CoolDown;
        GameObject go = GameObject.Instantiate(PrefabBullet, transform.position, Quaternion.identity, BulletParent);
        go.GetComponent<Bullet>().Principal = true;

        EnemyController enemy = MainGameplay.Instance.GetClosestEnemy(transform.position);

        Vector3 direction = enemy.transform.position - transform.position;

        if (direction.sqrMagnitude > 0)
        {
            direction.Normalize();

            go.GetComponent<Bullet>().Initialize(direction);
        }
    }

    private void SecondShoot()
    {
        if (!_canUseSecondShoot)
        {
            _timerCoolDownSecondShoot += Time.deltaTime;
            if (_timerCoolDownSecondShoot > CoolDown)
            {
                _timerCoolDownSecondShoot -= CoolDown;
                _canUseSecondShoot = true;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && NumberCurrentAmmo > 0)
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

                if (hit.collider != null)
                {
                    GameObject go = GameObject.Instantiate(PrefabBullet, transform.position, Quaternion.identity, BulletParent);
                    go.GetComponent<Bullet>().Principal = false;

                    EnemyController enemy = hit.collider.gameObject.GetComponent<EnemyController>();

                    Vector3 direction = enemy.transform.position - transform.position;
                    if (direction.sqrMagnitude > 0)
                    {
                        direction.Normalize();
                        go.GetComponent<Bullet>().Initialize(direction);
                    }
                    _canUseSecondShoot = false;
                    NumberCurrentAmmo--;
                }
            }
        }
    }
    public void AddHealth(int life)
    {
        if (_health <= _healthMax)
            _health += life;
    }
    public void UpdateLife()
    {
        if (_health > _healthMax)
            _health = _healthMax;
    }
    private void Die()
    {
        if (_health <= 0)
        {
            print("t'es nul");
        }
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector2(horizontal, vertical);

        if (direction.sqrMagnitude > 0)
        {
            direction.Normalize();
            transform.position += direction * PlayerBaseValues.MoveSpeed * Time.deltaTime;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var enemy = collision.gameObject.GetComponent<EnemyController>();
            _health -= enemy.EnemyBaseValues.Damage;

            Vector3 direction = enemy.transform.position - transform.position;
            enemy.BackOf(direction, 1);
        }
    }



}
