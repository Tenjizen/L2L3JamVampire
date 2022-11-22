using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy", menuName ="Enemies/Enemy", order =1)]
public class EntitiesScriptableObject : ScriptableObject
{
    public string Name;
    public int Health;
    public int MoveSpeed;
    public int BaseDamage;
    public float BonusScore;
    public Sprite Visuel;
    public GameObject BonusLoot;
    public bool ILikeTrain = false;
}
