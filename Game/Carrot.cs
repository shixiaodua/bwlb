using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Carrot : MonoBehaviour
{
    public Sprite[] carrotStageSprite;
    private SpriteRenderer spriteRenderer;
    private GameController gameController;
    private Animator animator;
    private Text text;
    private float timeVal;
#if Game
    private void Start()
    {
        gameController = GameController.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        text = transform.Find("HPCanvas/Text (Legacy)").GetComponent<Text>();
        carrotStageSprite = new Sprite[11];
        timeVal = 2;
        for(int i = 0; i <= 6; i++)
        {
            carrotStageSprite[i] = gameController.GetSprite("Pictures/NormalMordel/Game/Carrot/" + i.ToString());
        }
    }
    private void Update()
    {
        if (timeVal <= 0)
        {
            animator.Play("Idle");
            timeVal = 2;
        }
        else
        {
            timeVal -= Time.deltaTime;
        }
    }
    private void OnMouseDown()
    {
        if (gameController.carrotHP == 10)
        {
            animator.Play("Carrot", 0, 0);
            int randomNum = Random.Range(1, 4);
            GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Carrot/" + randomNum.ToString());
        }
    }
    public void TakeDamage()
    {
        animator.enabled = false;
        GameController.Instance.PlayEffectMusic("AudioClips/NormalMordel/Carrot/Crash");
        if (gameController.carrotHP < 10&&gameController.carrotHP>5)
        {
            
            spriteRenderer.sprite = carrotStageSprite[6];
        }
        else
        {
            spriteRenderer.sprite = carrotStageSprite[gameController.carrotHP];
            if (gameController.carrotHP == 0)//”Œœ∑ ß∞‹
            {
                gameController.GameOver();
            }
        }
        text.text = gameController.carrotHP.ToString();
    }
#endif
}
