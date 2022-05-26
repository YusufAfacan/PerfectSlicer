using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class About : MonoBehaviour
{
    public Text aboutText;
    private bool aboutTextisActive;
    private bool CafeisActive;
    public GameObject cafe;
    public Text gameName;
    private bool gameNameTextisActive;

    // Start is called before the first frame update
    void Start()
    {
        aboutTextisActive = false;
        CafeisActive = true;
        gameNameTextisActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveAboutText()
    {
        cafe.SetActive(!CafeisActive);
        aboutText.gameObject.SetActive(!aboutTextisActive);
        CafeisActive = !CafeisActive;
        aboutTextisActive = !aboutTextisActive;
        gameName.gameObject.SetActive(!gameNameTextisActive);

    }
}
