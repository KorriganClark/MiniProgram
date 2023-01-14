using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class AddNewChara : MonoBehaviour
    {

        public int CharaID;

        public void Click()
        {
            GameMode.GetGameMode().SpawnNewChara(CharaID);
        }
    }
}