using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class CoinMove : MonoBehaviour
{
    private TextMeshProUGUI coinText;
    private Image coinImage;
    public Sprite[] coinSprites;
    [HideInInspector]
    public int prize;
#if Game
    private void Awake()
    {
        transform.position += new Vector3(0, 80, 0);
        coinText = transform.Find("Tex_Coin").GetComponent<TextMeshProUGUI>();
        coinImage = transform.Find("Img_Coin").GetComponent<Image>();
        coinSprites = new Sprite[2];
        coinSprites[0] = GameController.Instance.GetSprite("Pictures/NormalMordel/Game/Coin");
        coinSprites[1] = GameController.Instance.GetSprite("Pictures/NormalMordel/Game/ManyCoin");
    }
    private void OnEnable()
    {
        StartCoroutine(NextFrameCoroutine());
    }
    IEnumerator NextFrameCoroutine()
    {
        
        yield return null; // 等待一帧
        ShowCoin();
    }
    private void ShowCoin()
    {
        coinText.text = prize.ToString();
        
        if (prize >= 500)
        {
            coinImage.sprite = coinSprites[1];
        }
        else
        {
            coinImage.sprite = coinSprites[0];
        }
        transform.DOLocalMoveY(60, 0.5f);
        DOTween.To(() => coinImage.color, toColor => coinImage.color = toColor, new Color(1, 1, 1, 0), 0.5f);
        Tween tween=DOTween.To(() => coinText.color, toColor => coinText.color = toColor, new Color(1, 1, 1, 0), 0.5f);

        tween.OnComplete(DestroyCoin);
    }
    //销毁金币UI
    private void DestroyCoin()
    {
        transform.localPosition = Vector3.zero;
        coinImage.color = coinText.color = new Color(1, 1, 1, 1);
        GameController.Instance.PushGameObjectToFactory("CoinCanvas", transform.parent.gameObject);
    }
#endif
}
 