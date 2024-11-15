using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DestroyTower : MonoBehaviour
{
    public int towerID;
    public int price;
    private Button button;
    private Text text;
    private Sprite canClickSprite;//���Ե��ͼƬ
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
        canClickSprite = gameController.GetSprite("Pictures/NormalMordel/Game/Tower/Btn_SellTower");
        button.onClick.AddListener(SellLevel);
    }
    private void OnEnable()
    {
        price = gameController.selectGrid.tower.towerPersonalProperty.sellPrice;
        text.text = price.ToString();
    }
    private void SellLevel()
    {
        //������Ч
        GameObject effect = gameController.GetGameObject("DestoryEffect");
        effect.transform.position = gameController.selectGrid.transform.position;
        effect.transform.SetParent(gameController.selectGrid.transform);
        //����һЩ����
        gameController.selectGrid = null;
        gameController.ChangeCoin(gameController.selectGrid.tower.towerPersonalProperty.sellPrice);
        gameController.selectGrid.tower.DestroyTower();
    }
#endif
}
