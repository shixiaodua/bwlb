using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class StarBullect : MonoBehaviour
{
    public int attackValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.activeSelf == false)
        {
            return;
        }
        if (collision.gameObject.tag == "Monster" || collision.gameObject.tag == "Item")
        {
            collision.SendMessage("TakeDamage", attackValue);
        }
    }
}
