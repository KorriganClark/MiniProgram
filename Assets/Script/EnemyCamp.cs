using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class EnemyCamp : Camp
    {
        public override void ReCalculPos()
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
                    pair.Value.postionInCamp = pair.Key - 1;
                }
            }

            foreach (var pair in members)
            {
                Character member = pair.Value;
                if(member == null)
                {
                    continue;
                }
                int index = pair.Key;
                if (member.Pos > Pos + characterOffset[index] + member.Speed * Time.deltaTime)
                {
                    member.Pos = -member.Speed * Time.deltaTime + member.Pos;
                }
                else
                {
                    member.Pos = Pos + characterOffset[index];
                }
                float deltaOffset = Time.deltaTime * member.Speed * Mathf.Abs(member.Depth - characterDepths[index]) / (member.Pos - Pos - characterOffset[index]);
                if (member.Depth < characterDepths[index] - deltaOffset)
                {
                    member.Depth += deltaOffset;
                }
                else if (member.Depth > characterDepths[index] + deltaOffset)
                {
                    member.Depth -= deltaOffset;
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