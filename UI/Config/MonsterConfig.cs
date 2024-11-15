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
        "�Ҷ�������Ҫ�Դ��ģ�",
        "���������...",
        "�����м���������..."
    };
    public static string[] initAi = new string[]
    {
        "����һ����������Ů�����ס����趨����������ʶ�Ҫ������趨�����з����趨�Ļظ�",
        "����һ���ɰ�С�������ס����趨����������ʶ�Ҫ������趨�����з����趨�Ļظ�",
        "����һ���������㣬���ס����趨����������ʶ�Ҫ������趨�����з����趨�Ļظ�"
    };
}