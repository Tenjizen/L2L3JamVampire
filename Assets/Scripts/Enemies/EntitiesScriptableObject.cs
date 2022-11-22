using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy", menuName ="Enemies/Enemy", order =1)]
public class EntitiesScriptableObject : ScriptableObject
{
    public string Name;
    public float Health;
    public int MoveSpeed;
    public int Damage;
    public float BonusScore;
    public float BonusExp;
    public Sprite Visuel;
    public GameObject BonusLoot;
    public bool ILikeTrain = false;
    public float TimeForMove;
    public List<AnimatorControllerParameter> Animations; 
}
