using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 10;

    private Vector3 _direction;

    public void Initialize(Vector3 direction)
    {
        _direction = direction;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _direction * Speed * Time.deltaTime;
    }
}
