using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SellTower : MonoBehaviour
{
#if Game
    public int towerID;
    public int price;
    private Button button;
    private Text text;
    private Sprite canClickSprite;//���Ե��ͼƬ
    private Image image;
    private GameController gameController;

    // Start is called before the first frame update
    private void Awake()
    {
        gameController = GameController.Instance;
        image = GetComponent<Image>();
        text = transform.Find("Text (Legacy)").GetComponent<Text>();
        button = GetComponent<Button>();
        canClickSprite = gameController.GetSprite("Pictures/NormalMordel/Game/Tower/Btn_SellTower");
        button.onClick.AddListener(SellTowerGo);
    }
    private void OnEnable()
    {
        price = gameController.selectGrid.tower.towerPersonalProperty.sellPrice;
        text.text = price.ToString();
    }
    private void SellTowerGo()
    {
        //������Ч
        GameObject effect = gameController.GetGameObject("DestoryEffect");
        effect.transform.position = gameController.selectGrid.transform.position;
        effect.transform.SetParent(gameController.selectGrid.transform);
        //������Ч
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Tower/TowerSell");
        //���ؽ����б��빥����Χ��ʾ
        gameController.selectGrid.HideGrid();
        gameController.selectGrid.hasTower = false;
        //����һЩ����
        gameController.ChangeCoin(gameController.selectGrid.tower.towerPersonalProperty.sellPrice);
        gameController.selectGrid.tower.DestroyTower();
        gameController.selectGrid.tower = null;
        gameController.selectGrid = null;
    }
#endif
}
