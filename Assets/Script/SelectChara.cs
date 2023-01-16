using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class SelectChara : MonoBehaviour
    {

        public GameLevelMode gameLevel
        {
            get
            {
                return GameMode.GetGameMode().levelMode;
            }
        }
        public int posId;
        public void Click()
        {
            if (gameLevel.isSelect)
            {
                gameLevel.OnCharaSelect(posId);
            }
        }
    }
}