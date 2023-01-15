using System.Collections;
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
                if (target.Pos == Pos + characterOffset[pos] && target.Depth == characterDepths[pos])
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
            if(members.Count <= 0 || dis <= 0)
                return null;
            var list = new List<Character>();
            for(int i = 0; i < dis; i++)
            {
                Character target = GetFightingChara(i);
                if (target != null)
                {
                    list.Add(target);
                }
            }
            if(list.Count <= 0)
            {
                return null;
            }
            int pos = Random.Range(0, list.Count);
            return list[pos];
        }

    }
    public class OurCamp : Camp
    {
        public override void ReCalculPos()
        {
            Character target;
            if(members.Count <= 0)
            {
                return;
            }
            while(!members.TryGetValue(0,out target))
            {
                var temp = new Dictionary<int, Character>(members);
                members.Clear();
                foreach (var pair in temp)
                {
                    members[pair.Key - 1] = pair.Value;
                }
            }

            foreach (var pair in members)
            {
                Character member = pair.Value;
                if (member == null)
                {
                    continue;
                }
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

                if (member.Depth < characterDepths[index] - member.Speed * Time.deltaTime)
                {
                    member.Depth += member.Speed * Time.deltaTime * 0.2f;
                }
                else if (member.Depth > characterDepths[index] + member.Speed * Time.deltaTime)
                {
                    member.Depth -= member.Speed * Time.deltaTime * 0.2f;
                }
                else
                {
                    member.Depth = characterDepths[index];
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