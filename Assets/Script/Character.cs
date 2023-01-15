using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Character : MonoBehaviour
{
    public AudioClip WalkAudio;
    public AudioClip AttackAuido;

    [HideInInspector]
    public int CharacterUID;

    public bool IsEnemy = false;
    public float Speed = 5;
    public float MaxHealth = 100;
    public float Attack = 10;
    public float Deffend = 2;
    public float Weight = 5;
    [HideInInspector]
    public int WeaponId = 0;

    // 0 静止 1 行走
    [HideInInspector]
    public int WalkState = 0;
    [HideInInspector]
    public Animator animator;

    //队伍中的站位
    public int postionInCamp = -1;

    private float pos = 0;
    public float Pos
    {
        get
        {
            return pos;
        }
        set
        {
            pos = value;

            Vector3 vet3 = gameObject.transform.position;
            vet3.x = pos;
            gameObject.transform.position = vet3;
        }
    }

    private float depth = 0;
    public float Depth
    {
        get
        {
            return depth;
        }
        set
        {
            depth = value;

            Vector3 vet3 = gameObject.transform.position;
            vet3.y = depth;
            gameObject.transform.position = vet3;
        }
    }

    public bool IsInFight = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (IsEnemy)
        {
            gameObject.transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject)
        {
        }
    }
}
