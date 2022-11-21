using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy", menuName ="Enemies/Enemy", order =1)]
public class EnemyScriptableObject : ScriptableObject
{
    public int Health;
    public int MoveSpeed;
    public int Damage;
    //public int AttackDelay;
}
