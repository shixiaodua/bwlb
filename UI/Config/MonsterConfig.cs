using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterConfig : MonoBehaviour
{
    public struct Monster
    {
        public int[] Cookie { get; set; }
        public int[] Milk { get; set; }

        public Monster(int[] cookie, int[] milk) : this()
        {
            Cookie = cookie;
            Milk = milk;
        }
    }
    public static Monster mconfig = new Monster(new[] { 10, 20 }, new[] { 10, 20 });
    public static int addCookie = 10;
    public static int addMilk = 10;
    public static string[] tipsString = new string[]
    {
        "我饿啦，我要吃大块的！",
        "疯狂星期四...",
        "天上有几颗星星呢..."
    };
    public static string[] initAi = new string[]
    {
        "你是一个傲娇美少女，请记住这个设定，后面的提问都要以这个设定来进行符合设定的回复",
        "你是一个可爱小萝莉，请记住这个设定，后面的提问都要以这个设定来进行符合设定的回复",
        "你是一个高冷御姐，请记住这个设定，后面的提问都要以这个设定来进行符合设定的回复"
    };
}