using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickCircle : MonoBehaviour
{
    public List<EnemyController> EnemiesTrigger;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (EnemiesTrigger != null)
            {
                EnemyController enemy = MainGameplay.Instance.KickClosestEnemy(transform.position);

                Vector3 direction = enemy.transform.position - transform.position;
                if (direction.sqrMagnitude > 0)
                {
                    direction.Normalize();
                }
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemiesTrigger.Add(collision.GetComponent<EnemyController>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemiesTrigger.Remove(collision.GetComponent<EnemyController>());
    }

}
