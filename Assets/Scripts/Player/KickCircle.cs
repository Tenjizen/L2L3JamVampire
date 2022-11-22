using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickCircle : MonoBehaviour
{
    public int Force;
    public int Damage;
    public float CoolDown = 4.0f;
    private float _coolDown = 0;

    enum State
    {
        canKick,
        cantKick
    }
    State _state = State.canKick;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_state == State.canKick)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (MainGameplay.Instance.EnemiesTriggerCircle.Count >= 1)
                {
                    EnemyController enemy = MainGameplay.Instance.KickClosestEnemy(transform.position);
                    Vector3 direction = enemy.transform.position - transform.position;
                    if (direction.sqrMagnitude > 0)
                    {
                        direction.Normalize();
                        enemy.BackOf(direction, Force);
                        enemy.Damage(Damage);
                        _state = State.cantKick;
                    }
                }

            }
        }
        if (_state == State.cantKick)
        {
            _coolDown += Time.deltaTime;

            if (_coolDown > CoolDown)
            {
                _coolDown -= CoolDown;
                _state = State.canKick;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            MainGameplay.Instance.EnemiesTriggerCircle.Add(collision.GetComponent<EnemyController>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        MainGameplay.Instance.EnemiesTriggerCircle.Remove(collision.GetComponent<EnemyController>());
    }

}
