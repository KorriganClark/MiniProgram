using Assets.Script;
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
    public float Speed = 1;
    public float MaxHealth = 100;
    public float AttackDamage = 10;
    public float Deffend = 2;
    public float AttackInterval = 1;

    [InspectorName("¹¥»÷¾àÀë")]
    public int AttackDis = 1;

    // 0 ¾²Ö¹ 1 ÐÐ×ß 2 ¹¥»÷
    [HideInInspector]
    public int State = 0;
    [HideInInspector]
    public Animator animator;

    public List<AnimationClip> clips;

    //¶ÓÎéÖÐµÄÕ¾Î»
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

    public bool IsInFight
    {
        get
        {
            if (IsEnemy)
            {
                return GameMode.GetGameMode().enemyCamp.IsInFight(postionInCamp);
            }
            else
            {
                return GameMode.GetGameMode().ourCamp.IsInFight(postionInCamp);
            }
        }
    }


    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (IsEnemy)
        {
            gameObject.transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    float stateTime = 0;

    public void Attack()
    {
        animator.SetBool("attacking", true);
        Debug.Log("Attack!");

    }
    public void Idle()
    {
        //animator.Set
        animator.SetBool("walking", false);
        animator.SetBool("attacking", false);
   
        Debug.Log("Idle!");
    }
    public void Walk()
    {
        animator.SetBool("walking", true);
        Debug.Log("Walk!");

    }

    // Update is called once per frame
    void Update()
    {
        if(State == 0)//¾²Ö¹
        {
            if (!IsInFight)
            {
                State = 1;
                stateTime = 0;
                Walk();
            }
            else
            {
                stateTime = stateTime + Time.deltaTime;
                if (stateTime > AttackInterval)
                {
                    State = 2;
                    stateTime = 0;
                    Attack();
                }
            }
        }
        else if(State == 2)//¹¥»÷
        {
            stateTime = stateTime + Time.deltaTime;
            if (stateTime > 1)
            {
                State = 0;
                stateTime = 0;
                Idle();
            }
        }
        else if(State == 1)//×ßÂ·
        {
            if (IsInFight)
            {
                State = 0;
                stateTime = 0;
                Idle();
            }
        }
    }
}
