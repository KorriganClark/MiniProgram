using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public enum BuffType
    {
        Damage,
        AddHealth,
        AddAttack,
        
    }

    public struct Buff
    {
        public Character owner;
        public Character target;
        public BuffType type;
        //初始攻击力
        public float attackOriDamge;
        public float skillRate;
        
    }

    public static class BuffMgr
    {
        public static GameMode gameMode = GameMode.GetGameMode();
        public static void DealAttack(Buff buff)
        {
            if (!buff.target)
            {
                return;
            }
            var damage = buff.attackOriDamge * buff.skillRate;
            var target = buff.target;
            damage -= target.currentDeffend;
            target.currentHP -= damage;
        }
    }
}