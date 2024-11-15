using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PrizePage : MonoBehaviour
{
    private Image img_Prize;//奖励图片
    private Image img_Instruction;//文字描述
    private Text tex_PrizeName;//奖励名称
    private Animator animator;
    private NormalModelPanel normalModelPanel;
    private Sprite[] prizeSprites;
    private Sprite[] InstructionSprites;
#if Game
    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        animator = GetComponent<Animator>();
        img_Prize = transform.Find("Img_Prize").GetComponent<Image>();
        img_Instruction = transform.Find("Img_Instruction").GetComponent<Image>();
        tex_PrizeName = transform.Find("Tex_PrizeName").GetComponent<Text>();
        prizeSprites = new Sprite[4];
        InstructionSprites = new Sprite[4];

        for(int i = 1; i <= 4; i++)
        {
            InstructionSprites[i-1] = GameController.Instance.GetSprite("Pictures/MonsterNest/Prize/Instruction" + i.ToString());
            prizeSprites[i - 1] = GameController.Instance.GetSprite("Pictures/MonsterNest/Prize/Prize" + i.ToString());
        }
    }
    private void OnEnable()
    {
        int randomNum = Random.Range(1, 5);//产生一个1到4的随机数
        string prizeName="";
        Debug.Log(GameManager.Instance.playerManager.monsterPetDatesList.Count+""+randomNum);
        if (randomNum == 4&&GameManager.Instance.playerManager.monsterPetDatesList.Count<3)//宠物蛋
        {
            int monsterPetNum=Random.Range(0,3);//随机怪物蛋
            //循环判断是否已经存在随机的怪物蛋
            while (HasThePet(monsterPetNum))
            {
                monsterPetNum = Random.Range(1, 4);
            }
            MonsterPetDate monsterPetDate = new MonsterPetDate
            {
                monsterID = monsterPetNum,
                monsterLevel = 0,
                remainCookies = 0,
                remainMilk = 0,
            };
            GameManager.Instance.playerManager.monsterPetDatesList.Add(monsterPetDate);
            prizeName = "宠物蛋";
            img_Prize.sprite = prizeSprites[3];
            img_Instruction.sprite = InstructionSprites[3];
        }
        else
        {
            if (randomNum == 4) randomNum = 2;//如果原本是蛋就改为牛奶

            switch (randomNum)
            {
                case 1:
                    prizeName = "饼干";
                    GameManager.Instance.playerManager.cookies += 20;
                    img_Prize.sprite = prizeSprites[1];
                    img_Instruction.sprite = InstructionSprites[1];
                    break;
                case 2:
                    prizeName = "牛奶";
                    GameManager.Instance.playerManager.milk += 20;
                    img_Prize.sprite = prizeSprites[0];
                    img_Instruction.sprite = InstructionSprites[0];
                    break;
                case 3:
                    prizeName = "怪物窝";
                    GameManager.Instance.playerManager.nest += 1;
                    img_Prize.sprite = prizeSprites[2];
                    img_Instruction.sprite = InstructionSprites[2];
                    break;
                default://其他情况
                    break;
            }
                
        }
        tex_PrizeName.text = prizeName;
    }
    private bool HasThePet(int monsterID)
    {
        for(int i = 0; i < GameManager.Instance.playerManager.monsterPetDatesList.Count; i++)
        {
            if (GameManager.Instance.playerManager.monsterPetDatesList[i].monsterID == monsterID)
                return true;
        }
        return false;
    }
    public void ClosePrizePage()
    {
        GameController.Instance.isPause = false;
        normalModelPanel.ClosePrizePage();
    }
#endif
}
