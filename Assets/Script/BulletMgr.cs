using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script
{
    public static class BulletMgr
    {
        public static List<GameObject> prefabs = GameMode.GetGameMode().BulletPrefab;

        public static List<BulletRunningTask> tasks = new List<BulletRunningTask>();
        public class BulletRunningTask
        {
            public Character spwaner;
            public Character target;
            public GameObject bullet;
            public float totalTime;
            public float currentTime = 0;
            public void Update()
            {
                if(spwaner == null || target == null)
                {
                    return;
                }
                currentTime += Time.deltaTime;
                bullet.transform.position = (spwaner.transform.position * (totalTime - currentTime) + target.transform.position * currentTime) / totalTime;
            }
            public bool Finished()
            {
                if (target == null)
                {
                    return true;
                }
                return Vector3.Distance(bullet.transform.position, target.transform.position) < 0.1f;
            }
        }

        public static void SpawnBullet(Character spawner, Character target, GameObject bullet, float time)
        {
            var pos1 = spawner.transform.position;
            var pos2 = target.transform.position;
            var task = new BulletRunningTask();
            task.spwaner = spawner;
            task.target = target;
            task.totalTime = time;
            task.bullet = GameObject.Instantiate(bullet);
            tasks.Add(task);
        }

        public static void Update()
        {
            for(int i = tasks.Count - 1; i >= 0 ; i--)
            {
                tasks[i].Update();
                if (tasks[i].Finished())
                {
                    GameObject.DestroyImmediate(tasks[i].bullet);
                    tasks.RemoveAt(i);
                }
            }
        }

    }
}
