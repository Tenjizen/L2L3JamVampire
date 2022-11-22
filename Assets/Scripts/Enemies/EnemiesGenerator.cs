using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour
{
    public List<GameObject> EnemiesPrefab;
    public Transform EnemiesParent;

    public float TimeSpawnNormal = 1;
    public float TimeSpawnGroup = 1;
    public float TimeSpawnReinhardt = 30;
    public int NumEnemies = 1;
    public int NumGroupEnemies = 10;
    public float RayCircle = 5.0f;

    private float _timer;
    private float _timerGroup;
    private float _timerReinhardt;

    void Start()
    {

    }

    void Update()
    {
        _timer += Time.deltaTime;
        _timerGroup += Time.deltaTime;

        timeSpawn();
        TimeReinhardt();

    }
    public void UpdateAllTimer()
    {
        TimeSpawnNormal -= TimeSpawnNormal* .05f;
        TimeSpawnGroup -= TimeSpawnGroup * .05f;
        TimeSpawnReinhardt -= TimeSpawnReinhardt * .05f;
    }
    public void timeSpawn()
    {
        if (_timer > TimeSpawnNormal)
        {
            _timer -= TimeSpawnNormal;
            SpawnerEnemyBase(NumEnemies);
        }
        else if (_timerGroup > TimeSpawnGroup)
        {
            _timerGroup -= TimeSpawnGroup;
            SpawnerEnemyBase(NumGroupEnemies);
        }
    }
    public void TimeReinhardt()
    {
        _timerReinhardt += Time.deltaTime;
        if (_timerReinhardt > TimeSpawnReinhardt)
        {
            _timerReinhardt -= TimeSpawnReinhardt;
            SpawnerEnemyReinhardt(NumEnemies);
        }
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }



    public void SpawnerEnemyBase(int numberEnemies)
    {
        float RandomRayCircle = Random.Range(RayCircle, RayCircle + 3);

        Vector3 center = transform.position;
        for (int i = 0; i < numberEnemies; i++)
        {
            Vector3 pos = RandomCircle(center, RandomRayCircle);
            GameObject spawned = Instantiate(EnemiesPrefab[0], pos, Quaternion.identity, EnemiesParent);
            spawned.GetComponent<EnemyController>().Initialize(MainGameplay.Instance.Player);
            MainGameplay.Instance.Enemies.Add(spawned.GetComponent<EnemyController>());
        }
    }
    public void SpawnerEnemyReinhardt(int numberEnemies)
    {
        float RandomRayCircle = Random.Range(RayCircle + 2, RayCircle + 5);

        Vector3 center = transform.position;
        for (int i = 0; i < numberEnemies; i++)
        {
            Vector3 pos = RandomCircle(center, RandomRayCircle);
            GameObject spawned = Instantiate(EnemiesPrefab[1], pos, Quaternion.identity, EnemiesParent);
            spawned.GetComponent<EnemyController>().Initialize(MainGameplay.Instance.Player);
            MainGameplay.Instance.Enemies.Add(spawned.GetComponent<EnemyController>());
        }
    }
}