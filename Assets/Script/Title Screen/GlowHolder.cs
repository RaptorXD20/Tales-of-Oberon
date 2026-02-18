using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlowHolder : MonoBehaviour
{
    public void ActiveGlow(GameObject glow){
        glow.SetActive(true);
    }

    public void DisableGlow(GameObject glow){
        glow.SetActive(false);
    }

    public void ActiveButtonGlow(GameObject button){
        Color effect = button.GetComponent<Image>().color;
        effect.a = 0.5f;
        button.GetComponent<Image>().color = effect;
    }

    public void DisableButtonGlow(GameObject button){
        Color effect = button.GetComponent<Image>().color;
        effect.a = 0;
        button.GetComponent<Image>().color = effect;
    }

    public void ActiveInstructionScreen(GameObject instructionScreen){
        instructionScreen.SetActive(true);
    }

    public void DisableInstructionScreen(GameObject instructionScreen){
        instructionScreen.SetActive(false);
    }
}
