using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{


    public class Camp : MonoBehaviour
    {

        public List<float> characterDepths = new List<float>();
        public List<float> characterOffset = new List<float>();

        public List<GameObject> PositionPoints = new List<GameObject>();
        
        public int MaxCount = 5;

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

        public virtual void AddChara(Character chara, int index)
        {
            Character temp;
            if (members.TryGetValue(index, out temp))
            {
                return;
            }

            members.Add(index, chara);
            chara.postionInCamp = index;
            chara.transform.parent = PositionPoints[index].transform;
            var pos = chara.transform.localPosition;
            pos.z = -5;
            chara.transform.localPosition = pos;
            chara.Depth = 0;
        }

        public virtual void DeleteChara(int pos)
        {
            members.Remove(pos);
        }

        public virtual void ReCalculPos()
        {
            Character target;
            if (members.Count <= 0)
            {
                return;
            }
            while (!members.TryGetValue(0, out target))
            {
                var temp = new Dictionary<int, Character>(members);
                members.Clear();
                foreach (var pair in temp)
                {
                    members[pair.Key - 1] = pair.Value;
                    Character chara = pair.Value;
                    chara.postionInCamp = pair.Key - 1;
                    chara.transform.parent = PositionPoints[pair.Key - 1].transform;
                    var pos = chara.transform.localPosition;
                    pos.z = -5;
                    chara.transform.localPosition = pos;
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
                if (Mathf.Abs(member.Pos) >  member.Speed * Time.deltaTime)
                {
                    if (member.Pos > 0)
                    {
                        member.Pos -= member.Speed * Time.deltaTime;
                    }
                    else
                    {
                        member.Pos += member.Speed * Time.deltaTime;
                    }
                }
                else
                {
                    member.Pos = 0;
                }
                float deltaOffset = Time.deltaTime * member.Speed * Mathf.Abs(member.Depth) / Mathf.Abs(member.Pos);
                if (Mathf.Abs(member.Depth) > Mathf.Abs(deltaOffset))
                {
                    if (member.Depth > 0)
                    {
                        member.Depth -= Mathf.Abs(deltaOffset);
                    }
                    else
                    {
                        member.Depth += Mathf.Abs(deltaOffset);
                    }
                }
                else
                {
                    member.Depth = 0;
                }
            }
        }
        
        //是否已就位，就位的才能进行战斗
        public bool IsInFight(int pos)
        {
            Character target;
            if (members.TryGetValue(pos, out target))
            {
                if (target.Pos == 0 && target.Depth == 0)
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

        // Update is called once per frame
        public void Update()
        {
            ReCalculPos();
        }
    }
}