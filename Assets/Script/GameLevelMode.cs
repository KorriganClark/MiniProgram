using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        //配置数据
        public PlayerData configPlayerData;
        public SelectUI selectUI = new SelectUI();
        public bool isSelect = false;
        public int CurSelectCharaId = -1;
        public LevelData curLevelData;
        public int CurEnemyLevel = 0;
        //关卡状态 0 - 未开始 1 -战斗中
        public int state = 0;
        public void StarGame(int level,LevelData data)
        {
            InitSelectLevel(level, data);
        }
        public void OnEnemyCharaDeath(int posId)
        {
            playerData.OtherPosData[posId] = false;
        }
        public void OnOurCharaDeath(int posId)
        {
            playerData.PosData[posId] = false;
        }
        //初始化关卡
        public void InitSelectLevel(int level, LevelData data)
        {
            playerData = configPlayerData.Copy();
            curLevelData = data;
            CurEnemyLevel = 0;
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
            selectUI.QuitSelect();
            if (CurSelectCharaId == -1 || playerData.useChara(CurSelectCharaId,posId) == false)
            {
                //todo tips
                return;
            }
            //生成人物，标记数据
            gameMode.SpawnNewChara(CurSelectCharaId, posId);
            playerData.PosData[posId] = true;
            selectUI.OnSelect(posId);
        }
        public void EnterSelect()
        {
            if (isSelect == true)
            {
                return;
            }
            isSelect = true;
            selectUI.EnterSelect(playerData.PosData);
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
            int length = curLevelData.enemyList[CurEnemyLevel].Count();
            for(int i = 1; i < length; i++)
            {
                if(playerData.OtherPosData[i] == true)
                {
                    return false;
                }
            }
            return true;
        }
        //检测胜利
        public bool checkVictory()
        {
            if (CurEnemyLevel < curLevelData.levelNum)
            {
                return false;
            }
            int length = curLevelData.enemyList[CurEnemyLevel].Count();
            for (int i = 1; i < length; i++)
            {
                if (playerData.OtherPosData[i] == true)
                {
                    return false;
                }
            }
            return true;
        }
        public bool checkLoss()
        {
            return false;
        }
        //生成新一波怪
        public void SpawnEnemyLevel()
        {
            CurEnemyLevel = CurEnemyLevel + 1;
            int length = curLevelData.enemyList[CurEnemyLevel].Count();
            for (int i = 1; i < length; i++)
            {
                gameMode.SpawnNewChara(CurSelectCharaId, i);
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
        void Update()
        {
            if (checkVictory())
            {
                //胜利事件
                return;
            }
            if (checkLoss())
            {
                //失败事件
                return;
            }
            if (checkCanSpawnEnemy())
            {
                SpawnEnemyLevel();
            }
        }
    }
    //关卡数据，目前只有怪波次
    public class LevelData
    {
        public List<List<int>> enemyList;
        public int levelNum 
        {
            get
            {
                return enemyList.Count();
            }
        }
    }
    //玩家数据，包含技能可用数据，每个位置上是否已经创建角色/怪，道具和角色花费
    public class PlayerData
    {
        public int money = 0;
        const int maxSize = 10;
        public List<int> Skill = new List<int>(maxSize);
        public List<int> ItemCost = new List<int>(maxSize);
        public List<int> charaCost = new List<int>(maxSize);
        public List<bool> PosData = new List<bool>(maxSize);
        public List<bool> OtherPosData = new List<bool>(maxSize);
        public bool CheckCharaCost(int charaId)
        {
            return money > charaCost[charaId];
        }
        public bool CheckItemCost(int ItemId)
        {
            return money > ItemCost[ItemId];
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
            for(int i = 1; i < maxSize - 1; i ++)
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
        public void GetSelectUI()
        {

        }
        public void EnterSelect(List<bool> list)
        {

        }
        public void QuitSelect()
        {

        }
        public void OnSelect(int pos)
        {

        }
    }
}
