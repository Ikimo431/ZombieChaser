using System;
using TMPro;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            uiText.text = "You Win!";
            uiText.enabled = true;
            // Access the TPCC script on the 'Player' object
            TPCC playerScript = other.GetComponent<TPCC>();
            playerScript.SetInvincible(true); 
        }
    }
}
