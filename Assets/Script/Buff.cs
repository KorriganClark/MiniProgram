using System.Collections;
using System.Collections.Generic;
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
        public List<Character> targets;
        public BuffType type;
        //初始攻击力
        public float attackOriDamge;
        public float skillRate;

        //增加血量
        public float addHpValue;
    }

    public static class BuffMgr
    {
        public static GameMode gameMode = GameMode.GetGameMode();

        public static float Rate(Nat a,Nat b)
        {
            if(a == Nat.Gray || b == Nat.Gray)
            {
                return 1f;
            }
            if((a==Nat.Bule && b == Nat.Red)||
                (a == Nat.Green && b == Nat.Bule)||
                (a == Nat.Red && b == Nat.Green))
            {
                return 0.8f;
            }
            return 1.2f;
        }
        public static void DealAttack(Buff buff)
        {
            if (!buff.target)
            {
                return;
            }
            var damage = buff.attackOriDamge * buff.skillRate * Rate(buff.owner.nat, buff.target.nat);
            var target = buff.target;
            damage -= target.currentDeffend;
            target.currentHP -= damage;
        }
        public static void DealAddHealth(Buff buff)
        {
            if (buff.target)
            {
                buff.target.currentHP += buff.addHpValue;
                if(buff.target.currentHP > buff.target.MaxHealth)
                {
                    buff.target.currentHP = buff.target.MaxHealth;
                }
            }
            else if(buff.targets != null)
            {
                for(int i = 0; i < buff.targets.Count; i++)
                {
                    buff.targets[i].currentHP += buff.addHpValue;
                    if (buff.targets[i].currentHP > buff.targets[i].MaxHealth)
                    {
                        buff.targets[i].currentHP = buff.targets[i].MaxHealth;
                    }
                }
            }
        }
    }
}