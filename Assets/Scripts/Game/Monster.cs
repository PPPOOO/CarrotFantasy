using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Monster : MonoBehaviour
{

    public int monsterID;
    public int HP;
    public int currentHP;
    public float moveSpeed;
    public float initMoveSpeed;
    public int prize;

    private Animator animator;
    private Slider slider;
    public GameObject TshitGo;
    private GameController gameController;
    private List<Vector3> monsterPathPosList;
    

    private int roadPointIndex = 1;
    private bool reachCarrot;
    private bool hasDecreasSpeed;

    private float decreaseSpeedTimer;
    private float decreaseTime;

    public AudioClip dieAudioClip;
    private RuntimeAnimatorController runtimeAnimatorController;

    private void Awake()
    {
        gameController = GameController.Instance;
        monsterPathPosList = GameController.Instance.mapMaker.monsterPathPos;
        animator = GetComponent<Animator>();
        slider = transform.Find("MonsterCanvas").Find("HPSlider").GetComponent<Slider>();
        slider.gameObject.SetActive(false);
        TshitGo = transform.Find("TShit").gameObject;
    }

    private void OnEnable()
    {
        gameController = GameController.Instance;
        monsterPathPosList = GameController.Instance.mapMaker.monsterPathPos;
        if (roadPointIndex + 1 < monsterPathPosList.Count)
        {
            float xOffset = monsterPathPosList[0].x - monsterPathPosList[1].x;
            if (xOffset < 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (xOffset > 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
    }

    private void Update()
    {
        if (gameController.isPause||gameController.gameOver)
        {
            return;
        }
        if (!reachCarrot)
        {
            transform.position = Vector3.Lerp(transform.position, monsterPathPosList[roadPointIndex], 1/Vector3.Distance(transform.position, monsterPathPosList[roadPointIndex])*moveSpeed*Time.deltaTime* gameController.gameSpeed);
            if(Vector3.Distance(transform.position, monsterPathPosList[roadPointIndex]) <= 0.01f)
            {
                if (roadPointIndex + 1 < monsterPathPosList.Count)
                {
                    float xOffset = monsterPathPosList[roadPointIndex].x - monsterPathPosList[roadPointIndex + 1].x;
                    if (xOffset < 0)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                    else if (xOffset > 0)
                    {
                        transform.eulerAngles = new Vector3(0, 180, 0);
                    }
                }
                slider.gameObject.transform.eulerAngles = Vector3.zero;
                roadPointIndex++;
                if(roadPointIndex>= monsterPathPosList.Count)
                {
                    reachCarrot = true;
                }
            }
        }
        else
        {
            DestroyMonster();
            gameController.DecreaseHP();
        }

        if (hasDecreasSpeed)
        {
            decreaseTime += Time.deltaTime;
        }
        if (decreaseSpeedTimer >= decreaseTime / gameController.gameSpeed)
        {
            CancelDecreaseDebuff();
            decreaseSpeedTimer = 0;
        }
    }





    private void InitMonsterGo()
    {
        monsterID = 0;
        HP = 0;
        currentHP = 0;
        moveSpeed = 0;
        roadPointIndex = 1;
        dieAudioClip = null;
        reachCarrot = false;
        slider.value = 1;
        slider.gameObject.SetActive(false);
        prize = 0;
        transform.eulerAngles = Vector3.zero;
        decreaseSpeedTimer = 0;
        decreaseTime = 0;
        monsterPathPosList = null;
        CancelDecreaseDebuff();
    }

    private void TakeDamage(int attackValue)
    {
        slider.gameObject.SetActive(true);
        currentHP -= attackValue;
        if (currentHP <= 0)
        {
            GameController.Instance.PlayEffectMusic("NormalModel/Monster/"+gameController.currentStage.mBigLevelID+"/"+monsterID);
            DestroyMonster();
            return;
        }
        slider.value = (float)currentHP / HP;
    }

    public void DecreaseSpeed(BullectProperty bullect)
    {
        if (!hasDecreasSpeed)
        {
            moveSpeed = moveSpeed - bullect.debuffValue;
            TshitGo.SetActive(true);
        }
        decreaseSpeedTimer = 0;
        hasDecreasSpeed = true;
        decreaseTime = bullect.debuffTime;
    }

    private void CancelDecreaseDebuff()
    {
        hasDecreasSpeed = false;
        moveSpeed = initMoveSpeed;
        TshitGo.SetActive(false);
    }

    private void DestroyMonster()
    {
        if (gameController.targetSignal == transform)
        {
            gameController.HideSignal();
        }

        if (!reachCarrot)
        {
            GameObject coinGo = gameController.GetGameObjectResource("CoinCanvas");
            coinGo.transform.Find("Emp_Coin").GetComponent<CoinMove>().prize = prize;
            coinGo.transform.SetParent(gameController.transform);
            coinGo.transform.position = transform.position;
            gameController.ChangeCoin(prize);
            int randomNum = Random.Range(0, 100);
            if (randomNum < 5)
            {
                GameObject prizeGo = gameController.GetGameObjectResource("Prize");
                prizeGo.transform.position = transform.position+ new Vector3(0,0,-6);
                GameController.Instance.PlayEffectMusic("NormalModel/GiftCreate");
            }
        }
        if (gameController.targetTrans == transform)
        {
            gameController.HideSignal();
        }

        GameObject effectGo = gameController.GetGameObjectResource("DestoryEffect");
        effectGo.transform.SetParent(gameController.transform);
        effectGo.transform.position = transform.position;

        gameController.killMonsterNum++;
        gameController.killMonsterTotal++;
        InitMonsterGo();
        gameController.PushGameObjectToFactory("MonsterPrefab", gameObject);
    }

    public void GetMonsterProperty()
    {
        
        runtimeAnimatorController = gameController.contorllers[monsterID - 1];
        animator.runtimeAnimatorController = runtimeAnimatorController;
    }

#if Game
    private void OnMouseDown()
    {
        if (gameController.targetTrans == null)
        {
            gameController.targetTrans = transform;
            gameController.ShowSignal();
        }
        else if (gameController.targetTrans != transform)
        {
            gameController.HideSignal();
            gameController.targetTrans = transform;
            gameController.ShowSignal();
        }
        else if(gameController.targetTrans == transform)
        {
            gameController.HideSignal();
        }
    }
#endif
}
