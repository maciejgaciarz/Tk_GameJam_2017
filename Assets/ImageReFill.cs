using UnityEngine;
using UnityEngine.UI;

public class ImageReFill : MonoBehaviour
{

    public bool isRefilled = true;

    float currentFillAmount = 0;
    
    Image image;
    
    float reFillSpeed = 0.1f;
    Character character;

    private void Start()
    {
        
       image = GetComponent<Image>();
    }
    void Update ()
    {
        /*	   
        if(character.GetJumpTimer() < 0)
        {
            isRefilled = false;
            image.fillAmount += reFillSpeed * Time.deltaTime;            
        }
        else
        {
            isRefilled = true;
        }*/
	}
}

