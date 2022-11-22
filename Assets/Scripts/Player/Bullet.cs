using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    PlayerController _playerController;
    public float Speed = 10;
    public int DamagePrincipal = 10;
    public int DamageSecond = 10;
    public bool Principal = true;

    private Vector3 _direction;
    
    
    public void Initialize(Vector3 direction)
    {
        _direction = direction;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        DamagePrincipal = (int)_playerController.DamagePrincipal;
        DamageSecond = (int)_playerController.DamageSecondaire;
        GameObject.Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _direction * Speed * Time.deltaTime;
    }
}
