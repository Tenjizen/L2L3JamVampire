using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject PrefabBullet;
    public Slider SliderLife;

    public AnimatorScript Animator;

    public float CoolDown = 0.2f;
    private float _timerCoolDownFirstShoot;

    public Transform BulletParent;
    public EntitiesScriptableObject PlayerBaseValues;

    private float _health;
    private float _healthMax = 100;


    [HideInInspector] public int NumberMaxAmmo = 2;
    public int NumberCurrentAmmo = 2;
    private bool _canUseSecondShoot = true;
    private float _timerCoolDownSecondShoot;
    public float CoolDownSecondShoot = 2.0f;

    public bool isAlive = true;
    private bool _imunity = false;
    public float ImunityTime = 0.5f;
    private float _imunityTimer = 0;

    public float DamagePrincipal = 5;
    public float DamageSecondaire = 20;
    public float DamageKick = 15;
    Rigidbody2D _rb;
    public SpriteRenderer _spriteRenderer;

    public bool CanShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        //_spriteRenderer = GetComponent<SpriteRenderer>();
        //Animator = GetComponent<AnimatorScript>();
        _healthMax = PlayerBaseValues.Health;
        _health = _healthMax;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (CanShoot)
        {
            if (MainGameplay.Instance.Enemies.Count > 0)
                FirstShoot();
            SecondShoot();
        }
        Die();
        if (_imunity)
            Imunitytimer();
        UpdateLife();


        if (Input.GetKeyDown(KeyCode.K))
            BonusFireRat(0.10f);
    }

    private void Imunitytimer()
    {
        _imunityTimer += Time.deltaTime;
        _spriteRenderer.color = Color.gray;
        if (_imunityTimer < ImunityTime)
            return;

        _imunityTimer -= ImunityTime;
        _imunity = false;
        _spriteRenderer.color = Color.white;
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

                if (hit.collider != null && hit.collider.gameObject.tag == "Enemy")
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
        {
            _health += life;
            _spriteRenderer.color = Color.green;
            StartCoroutine(ColorWhite());
        }
    }
    IEnumerator ColorWhite()
    {
        yield return new WaitForSeconds(0.5f);
        _spriteRenderer.color = Color.white;
    }
    public void UpdateLife()
    {
        if (_health > _healthMax)
            _health = _healthMax;
        SliderLife.value = (_health / _healthMax);
    }
    private void Die()
    {
        if (_health <= 0)
        {
            print("t'es nul");
            UnityEngine.SceneManagement.SceneManager.LoadScene("DefeatScreen");
        }
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal > 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (horizontal < 0)
        {
            _spriteRenderer.flipX = false;
        }

        Vector3 direction = new Vector2(horizontal, vertical);

        if (direction.sqrMagnitude > 0)
        {
            direction.Normalize();
            _rb.velocity = direction * PlayerBaseValues.MoveSpeed;
            Animator.SetBool("Move", true);
        }
        else
        {
            _rb.velocity = Vector2.zero;
            Animator.SetBool("Move", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var enemy = collision.gameObject.GetComponent<EnemyController>();
            if (!_imunity)
            {
                _health -= enemy.EnemyBaseValues.Damage;
                _imunity = true;
                _spriteRenderer.color = Color.red;
                StartCoroutine(ColorWhite());
            }
            if (enemy.EnemyBaseValues.Name == "Enfant")
            {
                CanShoot = false;
                StartCoroutine(timerCanShoot(1.0f));
                GameObject.Destroy(enemy.gameObject);
                MainGameplay.Instance.Enemies.Remove(enemy);
            }

            if (enemy.EnemyBaseValues.ILikeTrain)
            {
                GameObject.Destroy(enemy.gameObject);
                MainGameplay.Instance.Enemies.Remove(enemy);
            }
            Vector3 direction = enemy.transform.position - transform.position;
            enemy.BackOf(direction, 1);
        }
    }

    IEnumerator timerCanShoot(float time)
    {
        yield return new WaitForSeconds(time);
        CanShoot = true;
    }

    public void LvlUp()
    {
        Time.timeScale = 1;
        MainGameplay.Instance.CanvasLvlUp.SetActive(false);
    }
    public void BonusFireRat(float time)
    {
        LvlUp();
        CoolDown -= CoolDown * time;
    }
    public void BonusAmmoSecondWeapon(int value)
    {
        LvlUp();
        NumberMaxAmmo += value;
    }
    public void BonusLife(int value)
    {
        LvlUp();
        //_healthMax += value;
        AddHealth(value);
    }
    public void BonusDegats(int value)
    {
        LvlUp();
        DamageKick += value;
        DamagePrincipal += value;
        DamageSecondaire += value;
    }



}
