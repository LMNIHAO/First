using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : ReactiveObject
{
    //侧边提示UI
    public TipPanel tipPanel;
    //记录生成角色的数量
    private int characterCount=0;
    //角色槽位
    private List<GameObject> slots;
    //默认一排生成6个槽位
    public int count = 6;
    //生成位置与物体的距离
    public float distance = 10f;

    public override void Show(string showContexts)
    {
        base.Show(showContexts);
    }

    // Use this for initialization
    void Start () {
        //初始化槽位
        slots = new List<GameObject>();

        for (int i = 0; i < count; ++i)
        {

            slots.Add(null);

        }
        //开始的时候 关闭特效
        if (effectShow.isPlaying)
        {
            effectShow.Stop();
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnMouseEnter()
    {
        if (effectShow.isStopped||effectShow.isPaused)
        {
            effectShow.Play();
        }
    }
    private void OnMouseExit()
    {

        if (effectShow.isPlaying)
        {
            effectShow.Stop();
        }

        //tipPanel.Hide();
    }
    private void OnMouseDown()
    {
        //Debug.Log("鼠标点击");

        tipPanel.Show();

        //UI to do
        Show(@"基地
击毁敌方基地即可获取胜利
同时点击基地可以造兵");

    }

    /// <summary>
    /// 点击UI 根据传入的ID 生成角色
    /// </summary>
    /// <param name="characterId">传入的角色ID</param>
    public void GenerateCharacter(int characterId=0)
    {
        //Debug.Log(characterId);

        GameObject temp = null;

        switch (characterId)
        {
            case 0:

                temp = ResourceTool.GetResFromLocal("Character/SF_Character_Elf_01");

                break;

            case 1:

                temp = ResourceTool.GetResFromLocal("Character/SF_Character_Goblin_01");

                break;

            case 2:

                temp = ResourceTool.GetResFromLocal("Character/SF_Character_Human_Bard");

                break;

            case 3:

                temp = ResourceTool.GetResFromLocal("Character/SF_Character_Undead_Footman_01");

                break;

            default:

                temp = ResourceTool.GetResFromLocal("Character/SF_Character_Elf_01");

                break;
        }

        characterCount++;

        //Debug.Log("CharacterCount:" + characterCount);

        //Debug.Log(" tempName" + temp.name);

        Instantiate(temp, GetPosition(characterCount), Quaternion.identity);

    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < count; ++i)
        {

            if (slots == null || slots.Count <= i || slots[i] == null)
            {

                Gizmos.color = Color.black;

            }

            else
            {

                Gizmos.color = Color.red;

            }
            Gizmos.DrawWireSphere(GetPosition(i), 0.5f);
        }
    }

    /// <summary>
    /// 获取出生的位置  (待完善=》TODO)
    /// </summary>
    /// <param name="count"> 索引</param>
    /// <returns>位置</returns>
    private Vector3 GetPosition(int count)
    {
        if (count<=6)
        {
            float degreePerIndex = 180f / 6;

            var pos = transform.position;

            var offset = new Vector3(0, 0, distance);

            return pos + (Quaternion.Euler(new Vector3(0f, degreePerIndex * count, 0f)) * offset);
        }

        else

        {
            count++;

            distance++;

            float degreePerIndex = 180f / (characterCount-count);

            var pos = transform.position;

            var offset = new Vector3(0, 0, distance);

            return pos + (Quaternion.Euler(new Vector3(0f, degreePerIndex * count, 0f)) * offset);

        }
    }
}
