using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public Vector2 centreCoords;
    public Texture2D bumpTexture, colorTexture, originalBumpTexture;
    public float scale = .1f;
    public float strength = .00f;
    public Renderer debugQuadRenderer;

    private int playerNumber;
    private int initialWidth, initialHeight;

    public void Initialize(Vector2 _centreCoords, Texture2D _bumpTexture, int _pNo, float _str)
    {

        centreCoords = _centreCoords;
        originalBumpTexture = _bumpTexture;
        playerNumber = _pNo;
        strength += _str;

        //cache size for scaling in the Update
        initialWidth = originalBumpTexture.width;
        initialHeight = originalBumpTexture.height;

        transform.position = new Vector3(_centreCoords.x, 0f, _centreCoords.y) * .1f;

        Expand();
    }

    public void Expand()
    {
        scale += Time.deltaTime * (.35f + strength);
        strength *= 1f - (Time.deltaTime * 1.5f);

        transform.localScale = Vector3.one * scale * 5f; //will expand the collider

        //bump
        bumpTexture = WaveManager.Instance.CreateTextureCopy(originalBumpTexture);
        TextureScale.Point(bumpTexture, Mathf.RoundToInt(initialWidth * scale), Mathf.RoundToInt(initialHeight * scale));
    }

    public void LateUpdate()
    {
        if (scale > 3f)
        {
            Clear();
            WaveManager.Instance.WaveDead(this);
        }
    }

    public void Clear()
    {
        bumpTexture = null;
        colorTexture = null;
        originalBumpTexture = null;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            int pNo = coll.gameObject.GetComponent<Character>().playerNumber;
            
            if (pNo != this.playerNumber)
            {
                //Debug.Log("push dude " + pNo + " with wave no: " + this.playerNumber);
                //if he's jumping
                if (coll.transform.position.y < .5f + strength)
                {
                    Vector3 forceToAdd = coll.transform.position - transform.position;
                    forceToAdd.y = 0f;
                    coll.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 3f * strength, ForceMode.VelocityChange);
                    coll.gameObject.GetComponent<Rigidbody>().AddForce(forceToAdd.normalized * 2f * strength, ForceMode.Impulse);
                }
            }
        }

        if (coll.gameObject.tag == "ThrowableItem")
        {

            //Debug.Log("push dude " + pNo + " with wave no: " + this.playerNumber);
            //if he's jumping
            if (coll.transform.position.y < .5f + strength)
            {
                Vector3 forceToAdd = coll.transform.position - transform.position;
                forceToAdd.y = 0f;
                coll.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 3f * strength, ForceMode.VelocityChange);
                coll.gameObject.GetComponent<Rigidbody>()
                    .AddForce(forceToAdd.normalized * 2f * strength, ForceMode.Impulse);
            }
        }
    }
}