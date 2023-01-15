using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class EnemyCamp : Camp
    {
        public override void ReCalculPos()
        {
            foreach(var pair in members)
            {
                Character member = pair.Value;
                int index = pair.Key;
                if (member.Pos > Pos + characterOffset[index])
                {
                    member.Pos =  -member.Speed * Time.deltaTime + member.Pos;
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