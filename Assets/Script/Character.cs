using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Nat
{
    Red,
    Bule,
    Green,
    Gray
}

public class Character : MonoBehaviour
{
    

    

    //用于编辑的属性
    public AudioClip WalkAudio;
    public AudioClip AttackAuido;
    public bool IsEnemy = false;
    public float Speed = 1;
    public float MaxHealth = 100;
    public float AttackDamage = 10;
    public float Deffend = 2;
    public float AttackInterval = 1;
    public AnimationClip idleClip;
    public AnimationClip walkClip;
    public AnimationClip attackClip;
    public AnimationClip deadClip;
    public List<AnimationClip> skillClips;
    [InspectorName("攻击距离")]
    public int AttackDis = 1;


    [InspectorName("远程攻击")]
    public bool IsEmitter = false;

    public GameObject bullet;

    
    public Nat nat = Nat.Gray;
    //运行时属性
    public float currentHP;
    public float currentAttackDamage;
    public float currentDeffend;


    public int CharacterUID;
    // 0 静止 1 行走 2 攻击 3 死亡
    [HideInInspector]
    public int State = 0;
    [HideInInspector]
    public Animator animator;


    //队伍中的站位
    public int postionInCamp = -1;

    public int enemyDeathPoint = 10;

    private float pos = 0;
    public float Pos
    {
        get
        {
            return gameObject.transform.localPosition.x;
        }
        set
        {
            pos = value;

            Vector3 vet3 = gameObject.transform.localPosition;
            vet3.x = pos;
            gameObject.transform.localPosition = vet3;
        }
    }

    private float depth = 0;
    public float Depth
    {
        get
        {
            return gameObject.transform.localPosition.y;
        }
        set
        {
            depth = value;

            Vector3 vet3 = gameObject.transform.localPosition;
            vet3.y = depth;
            gameObject.transform.localPosition = vet3;
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

    public Camp ownerCamp
    {
        get
        {
            if (IsEnemy)
            {
                return GameMode.GetGameMode().enemyCamp;
            }
            else
            {
                return GameMode.GetGameMode().ourCamp;
            }
        }
    }

    public Camp otherCamp
    {
        get
        {
            if (!IsEnemy)
            {
                return GameMode.GetGameMode().enemyCamp;
            }
            else
            {
                return GameMode.GetGameMode().ourCamp;
            }
        }
    }

    private GameMode gameMode;

    private float walkClipTime;
    private float idleClipTime;
    private float attackClipTime;
    private float deadClipTime;
    private void Awake()
    {
        gameMode = GameMode.GetGameMode();
        animator = GetComponent<Animator>();
        if (IsEnemy)
        {
            //gameObject.transform.Rotate(new Vector3(0, 180, 0));
        }
        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
        overrideController["Walk"] = walkClip;
        overrideController["PlayerIdle"] = idleClip;
        overrideController["Attacking"] = attackClip;
        overrideController["Dead"] = deadClip;
        walkClipTime = walkClip.length;
        idleClipTime = idleClip.length;
        attackClipTime = attackClip.length;
        deadClipTime = deadClip.length;
        animator.runtimeAnimatorController = overrideController;
    }
    private void Start()
    {
        currentHP = MaxHealth;
        currentAttackDamage = AttackDamage;
        currentDeffend = Deffend;
    }

    float stateTime = 0;

    public int CanAttackDis
    {
        get
        {
            return Mathf.Max(0, AttackDis - postionInCamp);
        }
    }

    public void Attack()
    {
        animator.SetBool("attacking", true);
        Buff buff = new Buff();
        buff.owner = this;
        if (IsEnemy)
        {
            buff.target = gameMode.ourCamp.GetTargetRamdom(CanAttackDis);
        }
        else
        {
            buff.target = gameMode.enemyCamp.GetTargetRamdom(CanAttackDis);
        }
        buff.attackOriDamge = currentAttackDamage;
        buff.skillRate = 1;
        gameMode.ApplyBuff(buff);
        if (IsEmitter)
        {
            BulletMgr.SpawnBullet(buff.owner, buff.target, bullet, 1);
        }
    }
    public void Idle()
    {
        animator.SetBool("walking", false);
        animator.SetBool("attacking", false);
    }
    public void Walk()
    {
        animator.SetBool("walking", true);

    }

    public void CheckDeath()
    {
        if(State != 3 && currentHP <= 0)
        {
            State = 3;
            stateTime = 0;
            animator.SetBool("died", true);
        }
    }

    public bool HasTarget()
    {
        return otherCamp.GetTargetRamdom(CanAttackDis);
    }

    public void DestroyCharacter()
    {
        Debug.Log("Dead:" + CharacterUID);
        gameMode.DestroyCharacter(CharacterUID);
    }

    // Update is called once per frame
    void Update()
    {
        CheckDeath();
        if(State == 0)//静止
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
                if (stateTime > AttackInterval && HasTarget())
                {
                    State = 2;
                    stateTime = 0;
                    Attack();
                }
            }
        }
        else if(State == 2)//攻击
        {
            stateTime = stateTime + Time.deltaTime;
            if (stateTime > attackClipTime)
            {
                State = 0;
                stateTime = 0;
                Idle();
            }
        }
        else if(State == 1)//走路
        {
            if (IsInFight)
            {
                State = 0;
                stateTime = 0;
                Idle();
            }
        }
        else if (State == 3)//死亡
        {
            stateTime = stateTime + Time.deltaTime;
            if(stateTime > deadClipTime)
            {
                DestroyCharacter();
            }
        }
    }
}
