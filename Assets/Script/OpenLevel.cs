using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class OpenLevel : MonoBehaviour
    {

        public List<int> enemyList;
        public int level;

        public void Click()
        {
            LevelData newData = new LevelData();
            newData.enemyList = new List<List<int>>();
            newData.levelId = level;
            int index = 0;
            newData.enemyList.Add(new List<int>());
            for(int i = 0;i < enemyList.Count;i++)
            {
                if(enemyList[i] == -1 )
                {
                    index++;
                    newData.enemyList.Add(new List<int>());
                }else
                {
                    newData.enemyList[index].Add(enemyList[i]);
                }
            }
            GameMode.GetGameMode().StarGame(newData.levelId, newData);
        }
    }
}