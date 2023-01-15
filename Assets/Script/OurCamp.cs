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
            var chara = members[pos];
            if(chara.Pos == Pos + characterOffset[i])
            {
                return true;
            }
            return false;
        }

    }
    public class OurCamp : Camp
    {
        public override void ReCalculPos()
        {
            for (int i = 0; i < members.Count; i++)
            {
                Character member = members[i];

                if (member.Pos < Pos + characterOffset[i])
                {
                    member.Pos = member.Speed * Time.deltaTime + member.Pos;
                }
                else
                {
                    float pos = Pos + characterOffset[i];
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