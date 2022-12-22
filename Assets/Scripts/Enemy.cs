using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //AI, �׺���̼� �ý��� ���� �ڵ� ��������
using UnityEngine.UI; //Slider �����ϱ� ���ؼ� �ʿ�

public class Enemy : LivingEntity
{

    public LayerMask whatIsTarget; //������� ���̾�

    private LivingEntity targetEntity;//�������
    private NavMeshAgent pathFinder; //��� ��� AI ������Ʈ

    /*public ParticleSystem hitEffect; //�ǰ� ����Ʈ
    public AudioClip deathSound;//��� ����
    public AudioClip hitSound; //�ǰ� ����
    */

    private Animator enemyAnimator;
    //private AudioSource enemyAudioPlayer; //����� �ҽ� ������Ʈ

    public float damage = 20f; //���ݷ�
    public float attackDelay = 1f; //���� ������
    private float lastAttackTime; //������ ���� ����
    private float dist; //���������� �Ÿ�

    public Transform tr;

    private float attackRange = 2.3f;
    //HpBarUI �߰��� ����
    public GameObject hpBarPrefab; //Instantiate �޼���� ������ �������� ���� ����
    public Vector3 hpBarOffset = new Vector3(-0.5f, 2.4f, 0);

    public Canvas enemyHpBarCanvas;
    public Slider enemyHpBarSlider; //Slider�� �ʱ� ����, Hp ���ſ� ����� Slider�� ���� ����
    //���� ����� �����ϴ��� �˷��ִ� ������Ƽ
    private bool hasTarget
    {
        get
        {
            //������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            //�׷��� �ʴٸ� false
            return false;
        }
    }

    private bool canMove;
    private bool canAttack;

    private void Awake()
    {
        //���� ������Ʈ���� ����� ������Ʈ ��������
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        //enemyAudioPlayer = GetComponent<AudioSource>();
    }

    //�� AI�� �ʱ� ������ �����ϴ� �¾� �޼���
    public void Setup(float newHealth, float newDamage, float newSpeed)
    {
        //ü�� ����
        startingHealth = newHealth;
        health = newHealth;
        //���ݷ� ����
        damage = newDamage;
        //�׺�޽� ������Ʈ�� �̵� �ӵ� ����
        pathFinder.speed = newSpeed;
    }


    void Start()
    {
        SetHpBar();
        //���� ������Ʈ Ȱ��ȭ�� ���ÿ� AI�� Ž�� ��ƾ ����
        StartCoroutine(UpdatePath());
        tr = GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        if (hasTarget)
        {
            //���� ����� ������ ��� �Ÿ� ����� �ǽð����� �ؾ��ϴ� Update()
            dist = Vector3.Distance(tr.position, targetEntity.transform.position);
        }
    }

    //������ ����� ��ġ�� �ֱ������� ã�� ��� ����
    private IEnumerator UpdatePath()
    {
        //��� �ִ� ���� ���� ����
        while (!dead)
        {
            if (hasTarget)
            {
                Attack();
            }
            else
            {
                //���� ����� ���� ���, AI �̵� ����
                pathFinder.isStopped = true;
                canAttack = false;
                canMove = false;

                //������ 20f�� �ݶ��̴��� whatIsTarget ���̾ ���� �ݶ��̴� �����ϱ�
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);

                //��� �ݶ��̴��� ��ȸ�ϸ鼭 ��� �ִ� LivingEntity ã��
                for (int i = 0; i < colliders.Length; i++)
                {
                    //�ݶ��̴��κ��� LivingEntity ������Ʈ ��������
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    //LivingEntity ������Ʈ�� �����ϸ�, �ش� LivingEntity�� ��� �ִٸ�
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        //���� ����� �ش� LivingEntity�� ����
                        targetEntity = livingEntity;

                        //for�� ���� ��� ����
                        break;
                    }
                }
            }

            //0.25�� �ֱ�� ó�� �ݺ�
            yield return new WaitForSeconds(0.25f);
        }
    }

    //���� ������ �Ÿ��� ���� ���� ����
    public virtual void Attack()
    {

        //�ڽ��� ���X, ���� ������ �Ÿ��� ���� ��Ÿ� �ȿ� �ִٸ�
        if (!dead && dist < attackRange)
        {
            //���� �ݰ� �ȿ� ������ �������� �����.
            canMove = false;

            //���� ��� �ٶ󺸱�
            this.transform.LookAt(targetEntity.transform);

            //�ֱ� ���� �������� attackDelay �̻� �ð��� ������ ���� ����
            if (lastAttackTime + attackDelay <= Time.time)
            {
                canAttack = true;
            }

            //���� �ݰ� �ȿ� ������, �����̰� �������� ���
            else
            {
                canAttack = false;
            }
        }

        //���� �ݰ� �ۿ� ���� ��� �����ϱ�
        else
        {
            canMove = true;
            canAttack = false;
            //��� ����
            pathFinder.isStopped = false; //��� �̵�
            pathFinder.SetDestination(targetEntity.transform.position);
        }
    }

    //����Ƽ �ִϸ��̼� �̺�Ʈ�� �ֵθ� �� ������ �����Ű��
    public void OnDamageEvent()
    {

        //���� ����� ������ ���� ����� LivingEntity ������Ʈ ��������
        LivingEntity attackTarget = targetEntity.GetComponent<LivingEntity>();

        //���� ó��
        attackTarget.OnDamage(damage);

        //�ֱ� ���� �ð� ����
        lastAttackTime = Time.time;

     

    
    }
    void SetHpBar()
    {
        enemyHpBarCanvas = GameObject.Find("Enemy HpBar Canvas").GetComponent<Canvas>();
        GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, enemyHpBarCanvas.transform);

        var _hpbar = hpBar.GetComponent<EnemyHpBar>();
        _hpbar.enemyTr = this.gameObject.transform;
        _hpbar.offset = hpBarOffset;
    }

    //�������� �Ծ��� �� ������ ó��
    public override void OnDamage(float damage)
    {
        //LivingEntity�� OnDamage()�� �����Ͽ� ������ ����
        base.OnDamage(damage);

        //ü�� ����
        enemyHpBarSlider.value = health;
    }
    protected override void OnEnable()
    {

        //LivingEntity�� OnEnable() ����(�����ʱ�ȭ)
        base.OnEnable();

        //ü�� �����̴� Ȱ��ȭ
        enemyHpBarSlider.gameObject.SetActive(true);
        //ü�� �����̴��� �ִ��� �⺻ ü�°����� ����
        enemyHpBarSlider.maxValue = startingHealth;
        //ü�� �����̴��� ���� ���� ü�°����� ����
        enemyHpBarSlider.value = health;
    }
    //��� ó��
    public override void Die()
    {
        //LivingEntity�� DIe()�� �����Ͽ� �⺻ ��� ó�� ����
        base.Die();

        //�ٸ� AI�� �������� �ʵ��� �ڽ��� ��� �ݶ��̴��� ��Ȱ��ȭ
        Collider[] enemyColliders = GetComponents<Collider>();
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        //AI������ �����ϰ� �׺�޽� ������Ʈ�� ��Ȱ��ȭ
        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        //��� �ִϸ��̼� ���
        enemyAnimator.SetTrigger("Die");
        /*//��� ȿ���� ���
        enemyAudioPlayer.PlayOnShot(deathSound);
        */
    }
}