using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class objectPlacement : MonoBehaviour
{
    public List<Vector3> winConditionLocationList;
    public List<Vector3> weaponLocationList;
    public List<Vector3> safeLocationList;
    bool[] alreadyUsedLocation; 

    public GameObject bow, axe, usb, safe, monsterSafe;
    public Transform itemsArea;
    // Start is called before the first frame update
    public void OnStartGame()
    {
        alreadyUsedLocation = new bool[winConditionLocationList.Count];

        GameObject addedBow = PhotonNetwork.Instantiate($"OnGround/{bow.name}", weaponLocationList[Random.Range(0, weaponLocationList.Count)], Quaternion.identity);
        addedBow.transform.parent = itemsArea;
        addedBow.transform.localPosition = weaponLocationList[Random.Range(0, weaponLocationList.Count)];
        GameObject addedAxe = PhotonNetwork.Instantiate($"OnGround/{axe.name}", Vector3.zero, Quaternion.identity);
        addedAxe.transform.parent = itemsArea;
        addedAxe.transform.localPosition = weaponLocationList[Random.Range(0, weaponLocationList.Count)];
        for (int i = 0; i < 5; i++)
        {
            int rand = Random.Range(0, winConditionLocationList.Count);
            while (alreadyUsedLocation[rand])
                rand = Random.Range(0, winConditionLocationList.Count);

            GameObject addedUSB = PhotonNetwork.Instantiate($"OnGround/{usb.name}", Vector3.zero, Quaternion.identity);
            addedUSB.transform.parent = itemsArea;
            addedUSB.transform.localPosition = winConditionLocationList[rand];
            alreadyUsedLocation[rand] = true;
        }

        GameObject addedSafe = PhotonNetwork.Instantiate($"OnGround/{safe.name}", Vector3.zero, Quaternion.identity);
        addedSafe.transform.parent = itemsArea;
        addedSafe.transform.localPosition = safeLocationList[Random.Range(0, safeLocationList.Count)];

        GameObject addedMonsterSafe = PhotonNetwork.Instantiate($"OnGround/{monsterSafe.name}", Vector3.zero, Quaternion.identity);
        addedMonsterSafe.transform.parent = itemsArea;
        addedMonsterSafe.transform.localPosition = safeLocationList[Random.Range(0, safeLocationList.Count)];
        addedMonsterSafe.GetComponent<MonsterSafe>().Lower();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
