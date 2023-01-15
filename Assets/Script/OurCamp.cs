﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{


    public class Camp : MonoBehaviour
    {

        public List<float> characterDepths = new List<float>();
        public List<float> characterOffset = new List<float>();
        [InspectorName("进入前线的偏移量")]
        public float InFightOffset;
        public int MaxCount = 5;

        public float Pos;

        public int LeaderCharacterUID 
        {
            get
            {
                if(members.Count > 0)
                {
                    return members[0].CharacterUID;
                }
                return -1;
            }
        }

        public bool IsFull
        {
            get
            {
                return members.Count >= MaxCount;
            }
        }

        public Dictionary<int, Character> members = new Dictionary<int, Character>();

        public Camp (){}

        public virtual void AddChara(Character chara, int position)
        {
            members.Add(position, chara);
            chara.postionInCamp = position;
            chara.Depth = characterDepths[position];
        }

        public virtual void DeleteChara(int pos)
        {
            members.Remove(pos);
        }

        public virtual void ReCalculPos()
        {
            
        }
        
        //是否已就位，就位的才能进行战斗
        public bool IsInFight(int pos)
        {
            Character target;
            if (members.TryGetValue(pos, out target))
            {
                if (target.Pos == Pos + characterOffset[pos])
                {
                    return true;
                }
            }
            return false;
        }

        public Character GetFightingChara(int pos)
        {
            Character target;
            if (members.TryGetValue(pos, out target) && IsInFight(pos))
            {
                return target;
            }
            return null;
        }

        public Character GetTargetRamdom(int dis)
        {
            if(members.Count <= 0)
                return null;
            int front = -1;
            for(int i = 0; i< MaxCount; i++)
            {
                if(GetFightingChara(i) != null)
                {
                    front = i;break;
                }
            }
            if(front == -1)
            {
                return null;
            }
            //dis -= front;
            var list = new List<Character>();
            for(int i = front; i < front + dis; i++)
            {
                Character target = GetFightingChara(i);
                if (target != null)
                {
                    list.Add(target);
                }
            }
            int pos = Random.Range(0, list.Count);
            return list[pos];
        }

    }
    public class OurCamp : Camp
    {
        public override void ReCalculPos()
        {
            foreach (var pair in members)
            {
                Character member = pair.Value;
                int index = pair.Key;
                if (member.Pos < Pos + characterOffset[index])
                {
                    member.Pos = member.Speed * Time.deltaTime + member.Pos;
                }
                else
                {
                    float pos = Pos + characterOffset[index];
                    member.Pos = pos;
                }
            }
        }

        // Update is called once per frame
        public void Update()
        {
            ReCalculPos();
        }
    }
}