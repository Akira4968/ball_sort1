
using System;
using System.Collections;
using dotmob;
using UnityEngine;
using UnityEngine.UI;

public class Splash : MonoBehaviour
{
    public Image loading;
    private float time = 0;
    bool isLoadCompleted = false;
    private void Start()
    {
        loading.fillAmount= 0;
    }
    private void Update()
    {
        if(!isLoadCompleted)
        {
            time += Time.deltaTime;
            loading.fillAmount = time / 4;
            if (time >= 4f)
            {
                isLoadCompleted= true;
                LoadLevel();
            }
            
        }
    }
    public void LoadLevel()
    {
        var gameMode = GameMode.Easy;
        var levelNo = PrefManager.GetInt($"{GameMode.Easy}_Level_Complete");
        if (!ResourceManager.HasLevel(gameMode, levelNo + 1))
        {
            GameManager.LoadGame(new LoadGameData
            {
                Level = ResourceManager.GetLevel(gameMode, 1),
                GameMode = gameMode
            });
        }

        GameManager.LoadGame(new LoadGameData
        {
            Level = ResourceManager.GetLevel(gameMode, levelNo + 1),
            GameMode = gameMode
        });
    }
}