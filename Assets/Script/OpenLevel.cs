using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class OpenLevel : MonoBehaviour
    {

        public List<int> enemyList;
        public List<float> birthOffsetList;
        public int level;
        public int defaultId = 0;
        public int firstMoney = 0;
        public Sprite bg1;
        public Sprite bg2;
        public void Click()
        {
            GameMode.GetGameMode().levelMode.backGround1.sprite = bg1;
            GameMode.GetGameMode().levelMode.backGround2.sprite = bg2;

            LevelData newData = new LevelData();
            newData.enemyList = new List<List<Tuple<int, float>>>();
            newData.levelId = level;
            newData.playerId = defaultId;
            newData.firstMoney = firstMoney;
            int index = 0;
            newData.enemyList.Add(new List<Tuple<int, float>>());
            for(int i = 0;i < enemyList.Count;i++)
            {
                if(enemyList[i] == -1 )
                {
                    index++;
                    newData.enemyList.Add(new List<Tuple<int, float>>());
                }
                else
                {
                    float time = 0;
                    if(birthOffsetList.Count > i)
                    {
                        time = birthOffsetList[i];
                    }
                    newData.enemyList[index].Add(new Tuple<int, float>(enemyList[i], time));
                }
            }
            GameMode.GetGameMode().StarGame(newData.levelId, newData);
        }
    }
}