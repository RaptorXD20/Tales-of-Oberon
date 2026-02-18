using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamerOverStats : MonoBehaviour
{
    PlayerStats player;

    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text moveSpeedText;
    [SerializeField] TMP_Text armorText;
    [SerializeField] TMP_Text recoveryText;
    [SerializeField] TMP_Text speedText;
    [SerializeField] TMP_Text mightText;
    [SerializeField] TMP_Text magnetText;
    [SerializeField] TMP_Text luckText;
    

    public void SetStatsDisplay(){
        player = FindObjectOfType<PlayerStats>();

        hpText.text = player.CurrentMaxHP.ToString();
        moveSpeedText.text = player.CurrentSpeed.ToString();
        speedText.text = player.CurrentProjectileSpeed.ToString();
        armorText.text = player.CurrentArmor.ToString();
        recoveryText.text = player.CurrentRecovery.ToString();
        mightText.text = player.CurrentMight.ToString();
        magnetText.text = player.CurrentMagnet.ToString();
        luckText.text = player.CurrentLuck.ToString();
    }

}
