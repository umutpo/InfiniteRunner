using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
class SerializedSaveData
{
    public int highScore;
}

public class ScoreController : MonoBehaviour
{
    private Text score;
    public static int currentScore = 0;
    public static int highestScore = 0;

    void Start()
    {
        currentScore = 0;
        score = GetComponent<Text>();
        score.text = currentScore.ToString();
    }

    void Update()
    {
        score.text = currentScore.ToString();
    }

    public void updateHighScore(){

        if (PlayerPrefs.HasKey("HighScore"))
        {
            highestScore = PlayerPrefs.GetInt("HighScore");
        }
        if (File.Exists(Application.persistentDataPath 
                   + "/AppSaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = 
                    File.Open(Application.persistentDataPath 
                    + "/AppSaveData.dat", FileMode.Open);
            SerializedSaveData data = (SerializedSaveData) bf.Deserialize(file);
            file.Close();
            highestScore = data.highScore;
        }
        if (currentScore > highestScore){
            highestScore = currentScore;
            BinaryFormatter bf = new BinaryFormatter(); 
            FileStream file = File.Create(Application.persistentDataPath 
                        + "/AppSaveData.dat"); 
            SerializedSaveData data = new SerializedSaveData();
            data.highScore = currentScore;
            bf.Serialize(file, data);
            file.Close();
        }
    }
}
