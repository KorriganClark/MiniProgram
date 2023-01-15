using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class EnemyCamp : Camp
    {
        public override void ReCalculPos()
        {
            for (int i = 0; i < members.Count; i++)
            {
                Character member = members[i];

                if (member.Pos > Pos + characterOffset[i])
                {
                    member.Pos =  -member.Speed * Time.deltaTime + member.Pos;
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