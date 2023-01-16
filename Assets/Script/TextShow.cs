using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script
{
    public class TextShow : MonoBehaviour
    {
        public int type;
        public Text text;
        public GameLevelMode gameLevel
        {
            get
            {
                return GameMode.GetGameMode().levelMode;
            }
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            int value = 0;
            if(type == 1)
            {
                if(gameLevel.playerData != null)
                value = gameLevel.playerData.money;
            }
            else
            {
                value = gameLevel.CurEnemyLevel;
            }
            text.text = value.ToString();
        }
    }
}