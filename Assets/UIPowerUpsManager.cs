using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerUpsManager : MonoBehaviour {

    public GameObject[] player1PowerUps;
    public GameObject[] player2PowerUps;
    public GameObject[] player3PowerUps;

    //

    private void Update()
    {

        //first player
        if (Input.GetButton("Jump_0"))
        {
            ReFillEnergy(player1PowerUps[0].GetComponent<Image>());
        }       

        if (Input.GetButton("Wave0_0"))
        {
            ReFillEnergy(player1PowerUps[1].GetComponent<Image>());
        }

        if (Input.GetButton("Wave1_0"))
        {
            ReFillEnergy(player1PowerUps[2].GetComponent<Image>());
        }

        //second player
        if (Input.GetButton("Jump_1"))
        {
            ReFillEnergy(player2PowerUps[0].GetComponent<Image>());
        }

        if (Input.GetButton("Wave0_1"))
        {
            ReFillEnergy(player2PowerUps[1].GetComponent<Image>());
        }

        if (Input.GetButton("Wave1_1"))
        {
            ReFillEnergy(player2PowerUps[2].GetComponent<Image>());
        }

        //3th player
        if (Input.GetButton("Jump_2"))
        {
            ReFillEnergy(player2PowerUps[0].GetComponent<Image>());
        }

        if (Input.GetButton("Wave0_2"))
        {
            ReFillEnergy(player2PowerUps[1].GetComponent<Image>());
        }

        if (Input.GetButton("Wave1_2"))
        {
            ReFillEnergy(player2PowerUps[2].GetComponent<Image>());
        }
    }

    void ReFillEnergy(Image image)
    {
        image.fillAmount = 0;
    }
}
