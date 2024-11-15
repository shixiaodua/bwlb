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
    private Sprite canClickSprite;//���Ե��ͼƬ
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
            button.interactable = false;//���ܵ��
        }
    }
    private void BuildTower()
    {
        //�ɽ�����ȥ��������
        gameController.towerBuilder.m_TowerID = towerID;
        gameController.towerBuilder.m_TowerLevel = 1;
        GameObject gameObject=gameController.towerBuilder.GetProduct();
        gameController.selectGrid.tower = gameObject.GetComponent<Tower>();
        gameObject.transform.SetParent(gameController.selectGrid.transform);
        gameObject.transform.position = gameController.selectGrid.transform.position+new Vector3(0,0,1);

        //������Ч
        GameObject effect = gameController.GetGameObject("BuildEffect");
        effect.transform.position = gameObject.transform.position;
        effect.transform.SetParent(gameController.selectGrid.transform);
        //������Ч
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Tower/TowerBulid");
        //���ؽ����б��빥����Χ��ʾ
        gameController.selectGrid.HideGrid();
        gameController.selectGrid.attackRangeGo=gameObject.transform.Find("attackRange").gameObject;
        gameController.selectGrid.hasTower = true;
        gameController.selectGrid.HideGrid();
        //��Ҽ���
        gameController.ChangeCoin(-price);
        //��������
        gameController.selectGrid.AfterBuild();
        //����һЩ����
        gameController.selectGrid = null;
    }
#endif
}
