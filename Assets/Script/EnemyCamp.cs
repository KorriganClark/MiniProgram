using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class EnemyCamp : Camp
    {

        public override void CheckInFight()
        {
            for (int i = 0; i < members.Count; i++)
            {
                Character member = members[i];

                if (member.Pos < Pos + InFightOffset)
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
                    member.Pos -= member.Speed * Time.deltaTime;
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