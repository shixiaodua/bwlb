using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UPLevelTower : MonoBehaviour
{
    public int towerID;
    public int price;
    private Button button;
    private Text text;
    private Sprite canClickSprite;//可以点击图片
    private Sprite cantClickSprite;
    private Image image;
    private GameController gameController;
#if Game

    // Start is called before the first frame update
    private void Awake()
    {
        gameController = GameController.Instance;
        image = GetComponent<Image>();
        text = transform.Find("Text (Legacy)").GetComponent<Text>();
        button = GetComponent<Button>();
        canClickSprite = gameController.GetSprite("Pictures/NormalMordel/Game/Tower/Btn_CanUpLevel");
        cantClickSprite = gameController.GetSprite("Pictures/NormalMordel/Game/Tower/Btn_CantUpLevel");
        button.onClick.AddListener(UPLevel);
    }
    private void OnEnable()
    {
        price = gameController.selectGrid.tower.towerPersonalProperty.upLevelPrice;
        if (gameController.selectGrid.tower.towerPersonalProperty.towerLevel == 3)
        {
            text.text = "顶级";
        }
        else
        text.text = price.ToString();
    }
    private void Update()
    {
        if (gameController.coin >= price && gameController.selectGrid.tower.towerPersonalProperty.towerLevel < 3)
        {
            image.sprite = canClickSprite;
            button.interactable = true;
        }
        else
        {
            image.sprite = cantClickSprite;
            button.interactable = false;
        }
    }
    private void UPLevel()
    {
        gameController.towerBuilder.m_TowerID = gameController.selectGrid.tower.towerID;
        gameController.towerBuilder.m_TowerLevel = gameController.selectGrid.tower.towerPersonalProperty.towerLevel + 1;
        gameController.selectGrid.tower.towerPersonalProperty.UPLevelTower();
        //建造新塔
        GameObject gameObject=gameController.towerBuilder.GetProduct();
        gameController.selectGrid.tower = gameObject.GetComponent<Tower>();
        gameObject.transform.SetParent(gameController.selectGrid.transform);
        gameObject.transform.position = gameController.selectGrid.transform.position + new Vector3(0, 0, 1);
        //播放音效
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Tower/TowerUpdata");
        //隐藏建塔列表与攻击范围显示
        gameController.selectGrid.HideGrid();
        gameController.selectGrid.attackRangeGo = gameObject.transform.Find("attackRange").gameObject;
        gameController.selectGrid.hasTower = true;
        
        //后续处理
        gameController.selectGrid.AfterBuild();
        //更改一些属性
        gameController.selectGrid = null;
    }
#endif
}
