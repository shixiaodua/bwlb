using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonTower: MonoBehaviour
{
#if Game
    public int towerID;
    public int price;
    private Button button;
    private Sprite canClickSprite;//可以点击图片
    private Sprite cantClickSprite;
    private Image image;
    private GameController gameController;

    // Start is called before the first frame update
    private void Awake()
    {
        gameController = GameController.Instance;
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(BuildTower);
    }
    private void OnEnable()
    {
        Invoke("UpdateSprite", 0.1f);
    }
    private void UpdateSprite()
    {
        canClickSprite = gameController.GetSprite("Pictures/NormalMordel/Game/Tower/" + towerID.ToString() + "/CanClick1");
        cantClickSprite= gameController.GetSprite("Pictures/NormalMordel/Game/Tower/" + towerID.ToString() + "/CanClick0");
        price = gameController.towerPriceDict[towerID];
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.coin >= price)
        {
            image.sprite = canClickSprite;
            button.interactable = true;
        }
        else
        {
            image.sprite = cantClickSprite;
            button.interactable = false;//不能点击
        }
    }
    private void BuildTower()
    {
        //由建塔者去建造新塔
        gameController.towerBuilder.m_TowerID = towerID;
        gameController.towerBuilder.m_TowerLevel = 1;
        GameObject gameObject=gameController.towerBuilder.GetProduct();
        gameController.selectGrid.tower = gameObject.GetComponent<Tower>();
        gameObject.transform.SetParent(gameController.selectGrid.transform);
        gameObject.transform.position = gameController.selectGrid.transform.position+new Vector3(0,0,1);

        //产生特效
        GameObject effect = gameController.GetGameObject("BuildEffect");
        effect.transform.position = gameObject.transform.position;
        effect.transform.SetParent(gameController.selectGrid.transform);
        //播放音效
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Tower/TowerBulid");
        //隐藏建塔列表与攻击范围显示
        gameController.selectGrid.HideGrid();
        gameController.selectGrid.attackRangeGo=gameObject.transform.Find("attackRange").gameObject;
        gameController.selectGrid.hasTower = true;
        gameController.selectGrid.HideGrid();
        //金币减少
        gameController.ChangeCoin(-price);
        //后续处理
        gameController.selectGrid.AfterBuild();
        //更改一些属性
        gameController.selectGrid = null;
    }
#endif
}
