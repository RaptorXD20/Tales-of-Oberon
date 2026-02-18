using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomTipUI : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] TMP_Text displayText;
    [SerializeField] TMP_Text displayTextShadow;
    [SerializeField] TMP_Text displayDescription;
    [SerializeField] Image icon;

    [Header("Reference")]
    public List<WeaponData> weapons = new List<WeaponData>();
    public List<PassiveData> passives = new List<PassiveData>();

    void Awake(){

        int aux = Random.Range(0,2);
        if(aux == 1){
            WeaponData data = weapons[Random.Range(0, weapons.Count)];

            displayText.text = data.name;
            displayTextShadow.text = displayText.text;
            displayDescription.text = data.baseStats.description;
            icon.sprite = data.icon;

        }else if(aux == 0){
            PassiveData data = passives[Random.Range(0, passives.Count)];

            displayText.text = data.baseStats.name;
            displayTextShadow.text = displayText.text;
            displayDescription.text = data.baseStats.description;
            icon.sprite = data.icon;
        }
    }

}
