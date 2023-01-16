using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script
{
    public class GameLevelMode : MonoBehaviour
    {

        public GameMode gameMode {
            get
            {
                return GameMode.GetGameMode();
            }
        }
        //关卡数据
        public PlayerData playerData;
        public GameObject selectPrefab;
        //配置数据
        public PlayerData configPlayerData;
        public SelectUI selectUI = new SelectUI();
        public bool isSelect = false;
        public int CurSelectCharaId = -1;
        public LevelData curLevelData;
        public int CurEnemyLevel = 0;

        public SpriteRenderer backGround1;
        public SpriteRenderer backGround2;

        //关卡状态 0 - 未开始 1 -战斗中
        public int state = 0;
        public void StarGame(int level,LevelData data)
        {
            ResetLevel();
            InitSelectLevel(level, data);
        }
        public void OnEnemyCharaDeath(Character target)
        {
            playerData.enemyNum--;
            playerData.money += target.enemyDeathPoint;

        }
        public void OnOurCharaDeath(Character target)
        {
            playerData.ourNum--;
            playerData.PosData[target.postionInCamp] = false;
           
        }
        //初始化关卡
        public void InitSelectLevel(int level, LevelData data)
        {
            playerData = configPlayerData.Copy();
            curLevelData = data;
            CurEnemyLevel = 0;
            state = 1;
            //生成默认角色
            gameMode.SpawnNewChara(data.playerId, 0);
            playerData.PosData[0] = true;
            playerData.ourNum++;
            playerData.money = data.firstMoney;
            selectPrefab.transform.gameObject.SetActive(false);
        }
        //使用道具
        public void UseItem(int ItemId)
        {
            QuitSelect();
            if (playerData.useItem(ItemId) == false )
            {
                //todo tips
                return;
            }
            //todo 使用逻辑
        }
        //使用技能
        public void UseSkill(int ItemId)
        {
            QuitSelect();
            if (playerData.useSkill(ItemId) == false)
            {
                //todo tips
                return;
            }
            //todo 使用逻辑
        }
        //创建人物
        public void UseChara(int ItemId)
        {
            QuitSelect();
            if (playerData.CheckCharaCost(ItemId) == false)
            {
                //todo tips
                return;
            }
            CurSelectCharaId = ItemId;
            //进入选位置
            EnterSelect();
        }
        //选位置结束
        public void OnCharaSelect(int posId)
        {
            QuitSelect();
            if (CurSelectCharaId == -1 || playerData.useChara(CurSelectCharaId,posId) == false)
            {
                //todo tips
                return;
            }
            //生成人物，标记数据
            gameMode.SpawnNewChara(CurSelectCharaId, posId);
            playerData.PosData[posId] = true;
            playerData.ourNum++;
            selectUI.OnSelect(posId);
        }
        public void EnterSelect()
        {
            if (isSelect == true)
            {
                return;
            }
            isSelect = true;
            bool[] list = new bool[100];
            foreach (var pair in gameMode.ourCamp.members)
            {
                list[pair.Value.postionInCamp] = true;
            }
            selectUI.EnterSelect(list,gameMode.ourCamp.MaxCount);
        }
        public void QuitSelect()
        {
            if(isSelect == false)
            {
                return;
            }
            isSelect = false;
            selectUI.QuitSelect();
        }
        //检测是否生成新一波怪
        public bool checkCanSpawnEnemy()
        {
            if(curLevelData == null)
            {
                return false;
            }
            if(CurEnemyLevel == 0 )
            {
                return true;
            }
            if (CurEnemyLevel == curLevelData.levelNum)
            {
                return false;
            }
            return playerData.enemyNum == 0;
        }
        //检测胜利
        public bool checkVictory()
        {
            if (CurEnemyLevel < curLevelData.levelNum)
            {
                return false;
            }
            return playerData.enemyNum == 0;
        }
        public bool checkLoss()
        {
            return playerData.ourNum == 0;
        }
        //生成新一波怪
        public void SpawnEnemyLevel()
        {
            CurEnemyLevel = CurEnemyLevel + 1;
            int length = curLevelData.enemyList[CurEnemyLevel - 1].Count();
            for (int i = 0; i < length; i++)
            {
                gameMode.SpawnNewChara(curLevelData.enemyList[CurEnemyLevel - 1][i].Item1, i);
                playerData.enemyNum ++;
                playerData.OtherPosData[i] = true;
            }
        }
        //收集UI上的配置，
        public void setPlayerDataConfig(int type,int id,int cost)
        {
            if(type == 1)
            {
                configPlayerData.SetCharaCost(id,cost);
            }
            else if(type == 2)
            {
                configPlayerData.SetItemCost(id, cost);
            }
            else
            {
                configPlayerData.SetSkillNum(id, cost);
            }
        }
        void Awake()
        {
            configPlayerData = new PlayerData();
        }

        public GameObject battleResult;
        public GameObject victory;
        public GameObject fail;

        void Update()
        {
            if(state == 0)
            {
                return;
            }
            if (checkVictory())
            {
                state = 0;
                ResetLevel();
                battleResult.SetActive(true);
                victory.SetActive(true);
                //胜利事件
                return;
            }
            if (checkLoss())
            {
                state = 0;
                ResetLevel();
                battleResult.SetActive(true);
                fail.SetActive(true);
                //失败事件
                return;
            }
            if (checkCanSpawnEnemy())
            {
                SpawnEnemyLevel();
            }
        }
        public void ResetLevel()
        {
            List<int> desObj = new List<int>();
            foreach (var pair in gameMode.ourCamp.members)
            {
                desObj.Add(pair.Value.CharacterUID);
            }
            foreach (var pair in gameMode.enemyCamp.members)
            {
                desObj.Add(pair.Value.CharacterUID);
            }
            for(int i = 0;i<desObj.Count;i++)
            {
                gameMode.DestroyCharacter(desObj[i]);
            }
        }
    }
    //关卡数据，目前只有怪波次
    public class LevelData
    {
        public List<List<Tuple<int,float>>> enemyList;
        public int levelNum 
        {
            get
            {
                return enemyList.Count();
            }
        }
        public int levelId;

        public int playerId;

        public int firstMoney;
    }
    //玩家数据，包含技能可用数据，每个位置上是否已经创建角色/怪，道具和角色花费
    public class PlayerData
    {
        public int money = 0;
        const int maxSize = 1000;
        public int[] Skill = new int[maxSize];
        public int[] ItemCost = new int[maxSize];
        public int[] charaCost = new int[maxSize];
        public bool[] PosData = new bool[maxSize];
        public bool[] OtherPosData = new bool[maxSize];
        public int enemyNum = 0;
        public int ourNum = 0;
        public bool CheckCharaCost(int charaId)
        {
            return money >= charaCost[charaId];
        }
        public bool CheckItemCost(int ItemId)
        {
            return money >= ItemCost[ItemId];
        }
        public bool CheckSkillCost(int skillId)
        {
            return Skill[skillId] > 0;
        }
        public void SetCharaCost(int charaId,int cost)
        {
            charaCost[charaId] = cost;
        }
        public void SetItemCost(int itemId, int cost)
        {
            ItemCost[itemId] = cost;
        }
        public void SetSkillNum(int skillId, int num)
        {
            Skill[skillId] = num;
        }
        public bool useItem(int itemId)
        {
            if(CheckItemCost(itemId) == true)
            {
                money -= ItemCost[itemId];
                return true;
            }
            return false;
        }
        public bool useSkill(int skillId)
        {
            if (CheckSkillCost(skillId) == true)
            {
                Skill[skillId] --;
                return true;
            }
            return false;
        }
        public bool useChara(int charaId,int posId)
        {
            if (CheckCharaCost(charaId) == true)
            {
                money -= charaCost[charaId];
                PosData[posId] = true;
                return true;
            }
            return false;
        }
        public PlayerData Copy()
        {
            PlayerData copy = new PlayerData();
            for(int i = 0; i < maxSize; i ++)
            {
                copy.charaCost[i] = charaCost[i];
                copy.ItemCost[i] = ItemCost[i];
                copy.Skill[i] = Skill[i];
                copy.PosData[i] = false;
                copy.OtherPosData[i] = false;
            }
            return copy;
        }
    }
    //选位置UI
    public class SelectUI
    {
        bool[] tempList;
        int maxSize = 0;
        public GameMode gameMode
        {
            get
            {
                return GameMode.GetGameMode();
            }
        }
        public void GetSelectUI()
        {

        }
        public void EnterSelect(bool[] list,int size)
        {

            tempList = list;
            maxSize = size;
            for(int i = 0;i < maxSize; i ++)
            {
                if(list[i] == false)
                {
                    Debug.Log("select true:" + i);
                    gameMode.SetPositionEffect(i, true);
                }
               
            }
        }
        public void QuitSelect()
        {
            for (int i = 0; i < maxSize; i++)
            {
                if (tempList[i] == false)
                {
                    Debug.Log("select false:" + i);
                    gameMode.SetPositionEffect(i, false);
                }

            }
        }
        public void OnSelect(int pos)
        {

        }
    }
}
