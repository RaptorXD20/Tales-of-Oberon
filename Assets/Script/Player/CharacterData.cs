using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Charcter Data", menuName = "ScriptableObject/Character Data ")]

public class CharacterData : ScriptableObject
{
    [SerializeField]Sprite icon;
    public Sprite Icon{ get => icon; private set => icon = value;}

    [SerializeField] new string name;
    public string Name{ get => name; private set => name = value;}

    public RuntimeAnimatorController controller;

    [SerializeField] WeaponData startingWeapon;
    public WeaponData StartingWeapon{ get => startingWeapon; private set => startingWeapon = value;}

    [SerializeField] Sprite poltrait;
    public Sprite Poltrait{ get => poltrait; private set => poltrait = value;}

    [System.Serializable]
    public struct Stats{
        public float maxHP, recovery, moveSpeed, armor, healtTemp;
        public float might, speed, magnet, luck;

        public Stats(float maxHP = 10, float recovery = 0f, float moveSpeed = 1f,float armor = 1f, float healtTemp = 1f, float might = 1f, float speed = 1f, float magnet = 30f, float luck = 1f){
            this.maxHP = maxHP;
            this.recovery = recovery;
            this.moveSpeed = moveSpeed; 
            this.might = might;
            this.speed = speed;
            this.magnet = magnet;
            this.luck = luck;
            this.armor = armor;
            this.healtTemp = healtTemp;
        }

        public static Stats operator +(Stats s1, Stats s2){
            s1.maxHP += s2.maxHP;
            s1.recovery += s2.recovery;
            s1.moveSpeed += s2.moveSpeed;
            s1.might += s2.might;
            s1.speed += s2.speed;
            s1.magnet += s2.magnet;
            s1.luck += s2.luck;
            s1.armor += s2.armor;
            s1.healtTemp += s2.healtTemp;
            return s1; 
        }
    }

    public Stats stats = new Stats(10);
}
