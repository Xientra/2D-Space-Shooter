using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpViewControllerScript : MonoBehaviour {

    public GameObject PowerUpExampleElement;

    public List<GameObject> PowerUpElements = new List<GameObject>();

    

    void Start () {
        CreatePowerUpUIObject(1f);
        CreatePowerUpUIObject(2f);
        CreatePowerUpUIObject(3f);
        CreatePowerUpUIObject(4f);
        CreatePowerUpUIObject(5f);
        CreatePowerUpUIObject(6f);
        CreatePowerUpUIObject(7f);
        StartCoroutine(Wait(2.2f));
    }
	
	void Update () {
		
	}

    public void CreatePowerUpUIObject(float _duration) { //PickUpBehaviourScript.PickUpTypes _pickUpType, 
        if (PowerUpExampleElement != null) {
            GameObject temp = Instantiate(PowerUpExampleElement, this.transform);

            /*
            bool b = false;
            for (int i = 0; i < PowerUpElements.Count; i++) {
                if (b == false && PowerUpElements.)
            }
            if (b == false) {
                PowerUpElements.Add(temp);
            }
            */
            //temp.GetComponent<Image>().sprite = ;

            temp.SetActive(true);

            temp.GetComponent<PowerUpUIElementBehaviourScript>().StartCoroutine(temp.GetComponent<PowerUpUIElementBehaviourScript>().DestroyAfterTime(_duration));

            temp.GetComponent<RectTransform>().anchoredPosition = new Vector2((PowerUpExampleElement.GetComponent<RectTransform>().sizeDelta.x + 10) * PowerUpElements.IndexOf(temp), 0);
        }
        else {
            Debug.LogError("The PowerUp View Script has no Example Element Assing to it");
        }
    }

    public IEnumerator Wait(float _time) {
        yield return new WaitForSecondsRealtime(_time);
        CreatePowerUpUIObject(4f);
    }
}