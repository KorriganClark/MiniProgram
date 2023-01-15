using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class GameMode : MonoBehaviour
    {
        public OurCamp ourCamp;
        public EnemyCamp enemyCamp;

        public static GameMode GetGameMode()
        {
            return GameObject.Find("GameMode").GetComponent<GameMode>();
        }

        public int ourCampPos = 0;
        public int enemyCampPos = 100;


        public List<GameObject> CharacterPrefab;
        public int NowCharaIndex = 0;
        public Dictionary<int, Character> charaMap = new Dictionary<int, Character>();

        // Use this for initialization
        void Awake()
        {
            ourCamp = GetComponent<OurCamp>();
            enemyCamp = GetComponent<EnemyCamp>();
        }

        public void StarGame(int level)
        {
            ourCamp.Pos = ourCampPos;
            enemyCamp.Pos = enemyCampPos;

        }

        //根据ID 找Prefab,并生成，阵营通过Prefab上的IsEnemy判定
        
        public void SpawnNewChara(int CharacterId, int Pos)
        {
            GameObject newTarget = Instantiate(CharacterPrefab[CharacterId]);
            Character chara = newTarget.GetComponent<Character>();
            charaMap[NowCharaIndex] = chara;
            if (chara.IsEnemy)
            {
                if (enemyCamp.IsFull)
                {
                    GameObject.Destroy(chara.gameObject);
                    return;
                }
                chara.Pos = enemyCampPos;
                enemyCamp.AddChara(chara, Pos);
            }
            else
            {
                if (ourCamp.IsFull)
                {
                    GameObject.Destroy(chara.gameObject);
                    return;
                }
                chara.Pos = ourCampPos;
                ourCamp.AddChara(chara, Pos);
            }
        }
       

        // Update is called once per frame
        void Update()
        {
            ourCamp.Update();
            enemyCamp.Update();
        }
    }
}