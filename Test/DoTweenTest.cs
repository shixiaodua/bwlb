using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DoTweenTest : MonoBehaviour
{
    private Image maskImage;
    private Tween w;
    // Start is called before the first frame update
    void Start()
    {
        maskImage = GetComponent<Image>();
        //dotween.to(() => maskimage.color, tocolor => maskimage.color = tocolor, new color(0, 0, 0, 0),2f);
        w= transform.DOLocalMoveX(300f,0.5f);//LocalÏà¶Ô×ø±ê
        w.Pause();
        w.SetAutoKill(false);
        w.OnComplete(hd);
        w.SetEase(Ease.InOutBounce);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // w.Play();
            w.PlayForward();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            w.PlayBackwards();
        }
    }
    private void hd()
    {
        DOTween.To(() => maskImage.color, tocolor => maskImage.color = tocolor, new Color(0, 0, 0, 0), 2f);
    }
}
