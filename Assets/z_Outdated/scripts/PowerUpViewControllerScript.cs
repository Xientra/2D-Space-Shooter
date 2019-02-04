using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpViewControllerScript : MonoBehaviour {

    public GameObject PowerUpExampleElement;

    private GameObject[] PowerUpElements = new GameObject[9];



    void Start () {
        
        
        CreatePowerUpUIObject(1f);
        CreatePowerUpUIObject(2f);
        CreatePowerUpUIObject(3f);
        CreatePowerUpUIObject(4f);
        CreatePowerUpUIObject(5f);
        CreatePowerUpUIObject(6f);
        StartCoroutine(Wait(3.5f));
        
    }
	
	void Update () {
		
	}

    public void CreatePowerUpUIObject(float _duration) { //PickUpBehaviourScript.PickUpTypes _pickUpType, float _duration) {
        if (PowerUpExampleElement != null) {
            GameObject temp = Instantiate(PowerUpExampleElement, this.transform);
            int index = FillInNewElement(temp);
            if (index == -1) {
                Destroy(temp);
                Debug.LogError("there couldn't be found a empty space in the PowerUpElements Array");
            }
            else {
                temp.SetActive(true);

                //temp.GetComponent<RectTransform>().anchoredPosition = new Vector2((PowerUpExampleElement.GetComponent<RectTransform>().sizeDelta.x + 10) * index, 0); //goes right
                temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (PowerUpExampleElement.GetComponent<RectTransform>().sizeDelta.y + 10) * -index); //goes down

                temp.GetComponent<PowerUpUIElementBehaviourScript>().StartCoroutine(temp.GetComponent<PowerUpUIElementBehaviourScript>().DestroyAfterTime(_duration));
             }
        }
        else {
            Debug.LogError("The PowerUp View Script has no Example Element Assing to it");
        }
    }

    public IEnumerator Wait(float _time) {
        yield return new WaitForSecondsRealtime(_time);
        CreatePowerUpUIObject(4f);
    }

    private int FillInNewElement(GameObject _element) {

        for (int i = 0; i < PowerUpElements.Length; i++) {
            if (PowerUpElements[i] == null) {
                PowerUpElements[i] = _element;
                return i;
            }
        }
        //else
        return -1;
    }
}