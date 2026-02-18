using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveRun(float recordPlayer, float[] record, int[] kill){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerData.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(recordPlayer, record, kill);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void ResetSave(){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerData.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        float[] record = new float[4];
        for(int i=0; i < 4; i++){
            record[i] = 0f;
        }

        int[] kill = new int[4];
        for(int j=0; j < 4; j++){
            kill[j] = 0;
        }

        SaveData data = new SaveData(0f, record, kill);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static SaveData LoadData(){
        string path = Application.persistentDataPath + "/playerData.save";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }else{
            Debug.LogWarning("Save file not found in "+ path);
            return null;
        }
    }
}
