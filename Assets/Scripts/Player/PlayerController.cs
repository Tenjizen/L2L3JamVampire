using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject PrefabBullet;
    //public float Speed = 5;

    public float CoolDown = 0.2f;
    private float _timerCoolDownFirstShoot;


    public Transform BulletParent;
    public EnemyScriptableObject PlayerBaseValues;

    private int _health;


    public int NumberMaxAmmo = 2;
    public int NumberCurrentAmmo = 2;
    private bool _canUseSecondShoot = true;
    private float _timerCoolDownSecondShoot;
    public float CoolDownSecondShoot = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        _health = PlayerBaseValues.Health;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        FirstShoot();
        SecondShoot();
    }

    private void FirstShoot()
    {
        _timerCoolDownFirstShoot += Time.deltaTime;

        if (_timerCoolDownFirstShoot < CoolDown)
            return;

        _timerCoolDownFirstShoot -= CoolDown;
        GameObject go = GameObject.Instantiate(PrefabBullet, transform.position, Quaternion.identity, BulletParent);

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
                    //Debug.Log(hit.collider.name);
                    GameObject go = GameObject.Instantiate(PrefabBullet, transform.position, Quaternion.identity, BulletParent);

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
}
