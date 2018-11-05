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

    private float widthOfAElement = 70f; //this should maybe be felxible

	void Start () {
        Elements.Clear();
        int elementsPerRow = Mathf.RoundToInt(PaddingContent.transform.GetComponent<RectTransform>().sizeDelta.x / widthOfAElement); //is 6 now, with the width of 445
        int rowPosCounter = -elementsPerRow / 2;
        
        foreach (GameObject WepElement in ObjectHolder._PlayerWeapons) {
            GameObject goTemp = Instantiate(ExampleElement, PaddingContent.transform);
            //goTemp.GetComponent<Image>().sprite = WepElement.GetComponent<WeaponBehaviourScript>().WeaponImage;
            Debug.Log(widthOfAElement * rowPosCounter + widthOfAElement / 2);
            goTemp.transform.position = new Vector2(widthOfAElement * rowPosCounter + widthOfAElement / 2, -20);

            goTemp.SetActive(true);
            Elements.Add(goTemp);
            rowPosCounter++;
            //if (rowPosCounter == 0) rowPosCounter++;
            if (rowPosCounter == elementsPerRow/2) rowPosCounter = -elementsPerRow/2;
            //if (rowPosCounter == elementsPerRow) rowPosCounter = 0;
        }
        
	}
	
	void Update () {
        //adjusts the size of the padding content to be the same as the normal contents - the padding
        PaddingContent.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(PaddingContent.transform.GetComponent<RectTransform>().sizeDelta.x, PaddingContent.transform.parent.GetComponent<RectTransform>().sizeDelta.y + 2*PaddingContent.transform.GetComponent<RectTransform>().offsetMax.y);
        //PaddingContent.transform.GetComponent<RectTransform>().sizeDelta
    }
}
