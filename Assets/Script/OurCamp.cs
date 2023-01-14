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

        public List<Character> members = new List<Character>();

        public Camp (){}

        public float Speed
        {
            get
            {
                if(members.Count <= 0)
                {
                    return 0;
                }
                return GameMode.GetGameMode().charaMap[LeaderCharacterUID].Speed;
            }
        }
        public float Weight
        {
            get
            {
                if (members.Count <= 0)
                {
                    return 0;
                }
                return GameMode.GetGameMode().charaMap[LeaderCharacterUID].Weight;
            }
        }

        public virtual void AddChara(Character chara)
        {
            members.Add(chara);
        }

        public virtual void DeleteChara(int UID)
        {
            for(int i = 0; i < members.Count; i++)
            {
                if(members[i].CharacterUID == UID)
                {
                    members.RemoveAt(i);
                }
            }
        }
        public virtual void DeleteChara(Character chara)
        {
            members.Remove(chara);
        }

        public virtual void ReCalculPos()
        {
            for(int i = 0; i < members.Count; i++)
            {
                Character member = members[i];

                if (member.IsInFight)
                {
                    float pos = Pos + characterOffset[i];
                    member.Pos = pos;
                    float dep = characterDepths[i];
                    member.Depth = dep;
                }
            }
        }
        public virtual void CheckInFight()
        {

        }

    }
    public class OurCamp : Camp
    {
        public override void CheckInFight()
        {
            for (int i = 0; i < members.Count; i++)
            {
                Character member = members[i];

                if(member.Pos > Pos + InFightOffset)
                {
                    member.IsInFight = true;
                }
            }
        }

        public void UpdateNotInFightPos()
        {
            for (int i = 0; i < members.Count; i++)
            {
                Character member = members[i];

                if (!member.IsInFight)
                {
                    member.Pos += member.Speed * Time.deltaTime;
                }
            }
        }

        // Update is called once per frame
        public void Update()
        {
            CheckInFight();
            ReCalculPos();
            UpdateNotInFightPos();
        }
    }
}