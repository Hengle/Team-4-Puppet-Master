using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPlacement : MonoBehaviour
{
    public List<Vector3> winConditionLocationList;
    public List<Vector3> weaponLocationList;
    public List<Vector3> safeLocationList;
    public GameObject bow, axe, usb, safe;
    public Transform itemsArea;
    // Start is called before the first frame update
    public void OnStartGame()
    {
        GameObject addedBow=Instantiate(bow, itemsArea);
        addedBow.transform.localPosition = weaponLocationList[Random.Range(0, weaponLocationList.Count)];
        GameObject addedAxe = Instantiate(axe, itemsArea);
        addedAxe.transform.localPosition = weaponLocationList[Random.Range(0, weaponLocationList.Count)];
        for (int i=0; i<5; i++)
        {
            GameObject addedUSB = Instantiate(usb, itemsArea);
            addedUSB.transform.localPosition = winConditionLocationList[Random.Range(0, winConditionLocationList.Count)];
        }
        GameObject addedSafe = Instantiate(safe, itemsArea);
        addedSafe.transform.localPosition = safeLocationList[Random.Range(0, safeLocationList.Count)];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
