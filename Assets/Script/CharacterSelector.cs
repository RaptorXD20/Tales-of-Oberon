using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector instance; //la usiamo per salvare l'istanza di questa classe
    public CharacterData characterData;

    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Debug.LogWarning("Extra " + this + " Deleted");
            Destroy(gameObject);
        }
    }
    
    public static CharacterData GetData(){
        if(instance && instance.characterData){
            return instance.characterData;
        }else{
            //Sceglie randomicamente un character se noi facciamo partire il gioco dalla game scene
            //questo codice serve puramente per i test
            #if UNITY_EDITOR
            string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
            List<CharacterData> characters = new List<CharacterData>();
            foreach(string assetPath in allAssetPaths){
                if(assetPath.EndsWith(".asset")){
                    CharacterData characterData = AssetDatabase.LoadAssetAtPath<CharacterData>(assetPath);
                    if(characterData != null){
                        characters.Add(characterData);
                    }
                }
            }

            //scegli un personaggio casuale se ne abbiamo trovati alcuni
            if(characters.Count > 0){
                return characters[Random.Range(0, characters.Count)];
            }
            #endif

            /*//se non c'è alcun characterData assegnato 
            CharacterData[] characters = Resources.FindObjectsOfTypeAll<CharacterData>();
            if(characters.Length > 0){
                return characters[Random.Range(0, characters.Length)];
            }*/
        }
        return null;
    }

    public void SelectCharacter(CharacterData character){
        characterData = character;
    }

    public void DestroySigleton(){
        instance = null;
        Destroy(gameObject);
    }
}
