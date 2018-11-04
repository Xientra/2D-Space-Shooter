using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsViewControllerScript : MonoBehaviour {

    public List<GameObject> Elements = new List<GameObject>();

    public GameObject ExampleElement;
    public GameObject PaddingContent;

    private int row = 1;
    private int collum = 1;

	void Start () {
        Elements.Clear();   
        /*
        foreach (GameObject WepElement in ObjectHolder._PlayerWeapons) {
            GameObject goTemp = Instantiate(ExampleElement);
            goTemp.GetComponent<Image>().sprite = WepElement.GetComponent<WeaponBehaviourScript>().WeaponImage;
            goTemp.transform.position = new Vector2();

            Elements.Add(goTemp);
        }
        */
	}
	
	void Update () {
        PaddingContent.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(PaddingContent.transform.GetComponent<RectTransform>().sizeDelta.x, PaddingContent.transform.parent.GetComponent<RectTransform>().sizeDelta.y + 2*PaddingContent.transform.GetComponent<RectTransform>().offsetMax.y);
        //PaddingContent.transform.GetComponent<RectTransform>().sizeDelta
    }
}
