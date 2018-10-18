using UnityEngine;
using System.Collections;

public class Scannable : MonoBehaviour
{
	public Animator UIAnim;
    public bool hitedRecently = false;



	public void Ping()
	{
	    if (UIAnim != null && UIAnim.isInitialized)
	    {
	        UIAnim.SetTrigger("Ping");
	    }
    }
}
