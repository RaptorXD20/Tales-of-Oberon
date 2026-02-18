using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObject/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    //Stats
    [SerializeField] float speed;
    public float Speed{ get => speed; set => speed = value;}
    [SerializeField] float maxHP;
    public float MaxHP{ get => maxHP; set => maxHP = value;}
    [SerializeField] float damage;
    public float Damage{ get => damage; set => damage = value;}
}
