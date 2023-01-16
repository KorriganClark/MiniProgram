using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class EnemyCamp : Camp
    {
        // Update is called once per frame
        public void Update()
        {
            ReCalculPos();
        }
    }
}