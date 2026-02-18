using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

//stisamo creando un custom editor per poter scegliere che tipo di arma sia uno ScriptableObject
[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor : Editor
{
    WeaponData weaponData;
    string[] weaponSubtypes;
    int selectedWeaponSubtypes;

    void OnEnable(){

        //prendi i dati dell'arma
        weaponData = (WeaponData)target;

        //recupera tutti i sotto tipi di armi
        System.Type baseType = typeof(Weapons);
        List<System.Type> subTypes = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => baseType.IsAssignableFrom(p) && p != baseType)
            .ToList();
        
        //aggiungi l'opzione None davanti a tutti
        List<string> subTypesStrings = subTypes.Select(t => t.Name).ToList();
        subTypesStrings.Insert(0, "None");
        weaponSubtypes = subTypesStrings.ToArray();

        //Assicurati che stiamo usando il corretto sotto tipo
        selectedWeaponSubtypes = Mathf.Max(0, Array.IndexOf(weaponSubtypes, weaponData.behaviour));
    }

    public override void OnInspectorGUI(){
        //Crea il carosello delle scelte
        selectedWeaponSubtypes = EditorGUILayout.Popup("Behaviour", Mathf.Max(0, selectedWeaponSubtypes), weaponSubtypes);

        if(selectedWeaponSubtypes > 0){
            weaponData.behaviour = weaponSubtypes[selectedWeaponSubtypes].ToString();
            EditorUtility.SetDirty(weaponData);//Marca l'oggetto come salvato
            DrawDefaultInspector();//crea l'inspector di default dell'oggetto
        }
    }
}
