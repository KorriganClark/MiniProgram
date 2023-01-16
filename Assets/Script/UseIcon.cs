using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class UseIcon : MonoBehaviour
    {

        public int type;
        public int id;
        public int cost;
        public GameLevelMode gameLevel
        {
            get
            {
                return GameMode.GetGameMode().levelMode;
            }
        }
        public void Click()
        {
            if(type == 1)
            {
                gameLevel.UseChara(id);
            }
            else if(type == 2)
            {
                gameLevel.UseItem(id);
            }
            else
            {
                gameLevel.UseSkill(id);
            }
        }
        void Awake()
        {
            gameLevel.setPlayerDataConfig(type, id, cost);
        }
    }
}