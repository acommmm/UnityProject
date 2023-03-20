using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public List<GameObject> Buttons = new List<GameObject>();
    public List<GameObject> Images = new List<GameObject>();
    public List<Image> ButtonImages = new List<Image>();
    private float coolDown;
    private int choice;

    private void Start()
    {
        GameObject SkillsObj = GameObject.Find("Skills");

        for(int i = 0; i < SkillsObj.transform.childCount; ++i)
        {
            Buttons.Add(SkillsObj.transform.GetChild(i).gameObject); // 스킬 안의 버튼들 넣음        
        }

        for (int i = 0; i < Buttons.Count; ++i)
        {
            Images.Add(Buttons[i].transform.GetChild(0).gameObject); // 버튼안 이미지오브젝트 넣음
        }

        for (int i = 0; i < Images.Count; ++i)
        {
            ButtonImages.Add(Images[i].GetComponent<Image>()); // 이미지 오브젝트의 이미지
        }

        coolDown = 0.0f;
        choice = 0;

    }

    public void PushButton()
    {
        ButtonImages[choice].fillAmount = 1;
        Buttons[choice].GetComponent<Button>().enabled = false;

        StartCoroutine(PushButton_Coroutine());
    }

    IEnumerator PushButton_Coroutine()
    {
        int c = choice;
        float cool = coolDown;
        while (ButtonImages[c].fillAmount != 0)
        {
            ButtonImages[c].fillAmount -= Time.deltaTime * cool;
            yield return null;
        }
        Buttons[c].GetComponent<Button>().enabled = true;
    }

    public void TestCase1()
    {
        choice = 0;
        ControllerManager.GetInstance().BulletSpeed += 1.0f;
        coolDown = 1.0f;
    }
    public void TestCase2()
    {
        choice = 1;
        coolDown = 1.0f;
    }
    public void TestCase3()
    {
        choice = 2;
        coolDown = 0.5f;
    }
    public void TestCase4()
    {
        choice = 3;
        coolDown = 2.0f;
    }
    public void TestCase5()
    {
        choice = 4;
       coolDown = 1.2f;
    }


}
