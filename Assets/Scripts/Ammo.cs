using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private PlayerController player;
    public List<GameObject> SpawnObject;
    private float _timerSwitchPlace;
    public float TimerForSwitchPlace;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        SwitchPlace();
    }

    // Update is called once per frame
    void Update()
    {
        _timerSwitchPlace += Time.deltaTime;

        if (_timerSwitchPlace < TimerForSwitchPlace)
            return;
        _timerSwitchPlace = 0;
        SwitchPlace();
    }

    void SwitchPlace()
    {
        int place = Random.Range(0, SpawnObject.Count);
        transform.position = SpawnObject[place].transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player.NumberCurrentAmmo += player.NumberMaxAmmo - player.NumberCurrentAmmo;
        }
        
    }

}
