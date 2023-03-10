using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class GameMode : MonoBehaviour
    {
        public OurCamp ourCamp;
        public EnemyCamp enemyCamp;
        public GameLevelMode levelMode;

        public static GameMode GetGameMode()
        {
            return GameObject.Find("GameMode").GetComponent<GameMode>();
        }
        

        public int ourCampPos = 0;
        public int enemyCampPos = 100;


        public List<GameObject> CharacterPrefab;
        public List<GameObject> BulletPrefab;

        public int NowCharaIndex = 0;
        public Dictionary<int, Character> charaMap = new Dictionary<int, Character>();

        private List<BoxCollider2D> colliders = new List<BoxCollider2D>();
        // Use this for initialization
        void Awake()
        {
            ourCamp = GetComponent<OurCamp>();
            enemyCamp = GetComponent<EnemyCamp>();
            levelMode = GetComponent<GameLevelMode>();

            var list = ourCamp.PositionPoints;
            foreach(var go in list)
            {
                var collider = go.transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
                colliders.Add(collider);
            }
        }

        public void StarGame(int level, LevelData data)
        {
            levelMode.StarGame(level,data);
        }

        public void SetPositionEffect(int pos, bool isSeletect)
        {
            var go = ourCamp.PositionPoints[pos];
            //go.transform.GetChild(1).gameObject.SetActive(isSeletect);
            go.transform.GetChild(1).gameObject.SetActive(isSeletect);
        }

        //根据ID 找Prefab,并生成，阵营通过Prefab上的IsEnemy判定

        public void SpawnNewChara(int CharacterId, int Pos)
        {
            GameObject newTarget = Instantiate(CharacterPrefab[CharacterId]);
            Character chara = newTarget.GetComponent<Character>();
            charaMap[NowCharaIndex] = chara;
            chara.CharacterUID = NowCharaIndex ++;
            if (chara.IsEnemy)
            {
                if (enemyCamp.IsFull)
                {
                    GameObject.DestroyImmediate(chara.gameObject);
                    return;
                }
                chara.Pos = enemyCampPos;
                var res = enemyCamp.AddChara(chara, Pos);
                if (!res)
                {
                    DestroyImmediate(chara.gameObject);
                }
            }
            else
            {
                if (ourCamp.IsFull)
                {
                    GameObject.Destroy(chara.gameObject);
                    return;
                }
                chara.Pos = ourCampPos;
                var res = ourCamp.AddChara(chara, Pos);
                if (!res)
                {
                    DestroyImmediate(chara.gameObject);
                }
            }
        }

        public void DestroyCharacter(int UID)
        {
            var target = charaMap[UID];
            if (target)
            {
                if (target.IsEnemy)
                {
                    enemyCamp.members.Remove(target.postionInCamp);
                    levelMode.OnEnemyCharaDeath(target);
                }
                else
                {
                    ourCamp.members.Remove(target.postionInCamp);
                    levelMode.OnOurCharaDeath(target);
                }

                DestroyImmediate(target.gameObject);
                
            }
        }

        public void ApplyBuff(Buff buff)
        {
            switch (buff.type)
            {
                case BuffType.Damage:
                    BuffMgr.DealAttack(buff);
                    break;
                case BuffType.AddHealth:
                    BuffMgr.DealAddHealth(buff);
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            ourCamp.Update();
            enemyCamp.Update();
            BulletMgr.Update();
            CheckGetPos();
        }

        public void CheckGetPos()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                for (int i = 0;i< colliders.Count; i++)
                {
                    BoxCollider2D collider = colliders[i];
                    if (collider.OverlapPoint(pos))
                    {
                        if (levelMode.isSelect)
                        {
                            levelMode.OnCharaSelect(i);
                        }
                    }
                }

                
            }
        }
    }
}