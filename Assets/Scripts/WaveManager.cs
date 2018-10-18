using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    public Texture2D blackTexture, waterTexture, whiteRingBumpTex, shadowTexture;
    public GameObject wavePrefab;

    [HideInInspector]
    public int waterSize;

    public List<Wave> waves = new List<Wave>();
    private Texture2D finalBump, finalColor;
    private Material waterMat;

    private void Awake()
    {
        waterSize = blackTexture.width;
        waterMat = GetComponent<MeshRenderer>().material;
    }

    void Start()
    {
        finalBump = GetFillTexture(blackTexture.width, blackTexture.height, Color.black);
        finalColor = CreateTextureCopy(waterTexture);
        //finalColor.Apply();

        waterMat.SetTexture("_DispTex", (Texture)finalBump);
        waterMat.SetTexture("_MainTex", (Texture)finalColor);

//        CreateWave(new Vector2(3.2f, 3.2f) * 10f, 0, 0.5f);
//        CreateWave(new Vector2(3.2f, 3.2f), 0, 0.5f);
    }

    void LateUpdate()
    {
        //Bump
        FillTexture(finalBump, Color.black);
        //Colors
        CopyFullTexture(waterTexture, finalColor);

        //shadows
        Vector3 char0Pos = GameManager.Instance.characters[0].transform.position;
        
        if (char0Pos.y > 0f)
        {
            OverlayTexture(finalColor, shadowTexture, new Vector2(char0Pos.x, char0Pos.z) * 20f, 1f - (char0Pos.y * .3f));
        }
        Vector3 char1Pos = GameManager.Instance.characters[1].transform.position;
        if (char1Pos.y > 0f)
        {
            OverlayTexture(finalColor, shadowTexture, new Vector2(char1Pos.x, char1Pos.z) * 20f, 1f - (char1Pos.y * .3f));
        }

        Vector3 char2Pos = GameManager.Instance.characters[2].transform.position;
        if (char2Pos.y > 0f)
        {
            OverlayTexture(finalColor, shadowTexture, new Vector2(char2Pos.x, char2Pos.z) * 20f, 1f - (char2Pos.y * .3f));
        }

        for (int w = 0; w < waves.Count; w++)
        {
            waves[w].Expand();

            OverlayTexture(finalBump, waves[w].bumpTexture, waves[w].centreCoords, waves[w].strength);
            //float str = waves[w].strength * waves[w].strength * 6f; //reinforced strength at the beginning, then drops suddenly
            //OverlayTexture(finalColor, waves[w].colorTexture, waves[w].centreCoords * 2f, str);
        }

        finalColor.Apply();
        finalBump.Apply();
    }

    public void WaveDead(Wave who)
    {
        //Debug.Log("Removing " + who + " Total:" + waves.Count);
        Destroy(who.gameObject);
        waves.Remove(who);
    }

    public void CreateWave(Vector2 centre, int pNo, float strength)
    {
        GameObject newWave = GameObject.Instantiate<GameObject>(wavePrefab);
        Wave wScript = newWave.GetComponent<Wave>();
        wScript.Initialize(centre, whiteRingBumpTex, pNo, strength);

        waves.Add(wScript); //add it to the list of waves (see Update)
    }

    private void OverlayTexture(Texture2D baseTexture, Texture2D overlayedTexture, Vector2 overlayCentre, float multiplier)
    {
        int overlayX = 0, overlayY = 0;
        int startX = Mathf.RoundToInt(overlayCentre.x - overlayedTexture.width * .5f);
        int endX = startX + overlayedTexture.width;
        int startY = Mathf.RoundToInt(overlayCentre.y - overlayedTexture.height * .5f);
        int endY = startY + overlayedTexture.height;
        bool isOverlayPixel = false;

        int plainPixels = 0;

        for (int x = 0; x < baseTexture.width; x++)
        {
            for (int y = 0; y < baseTexture.height; y++)
            {
                Color baseTexelColor = baseTexture.GetPixel(x, y);
                Color finalColor;

                isOverlayPixel =
                    x != 0 && x != baseTexture.width - 1
                    && y != 0 && y != baseTexture.height - 1
                    && (x > startX && x < endX)
                    && (y > startY && y < endY);

                if (isOverlayPixel)
                {
                    //sample from the overlay texture
                    overlayX = x - startX;
                    overlayY = y - startY;
                    Color overlayTexelColor = overlayedTexture.GetPixel(overlayX, overlayY);
                    finalColor = Color.Lerp(baseTexelColor, overlayTexelColor, overlayTexelColor.a * multiplier); //blend
                                                                                                                  //Debug.Log("Overlaying " + overlayX + "," + overlayY + " to " + x + "," + y);
                }
                else
                {
                    //out of range, just plain colour
                    finalColor = baseTexelColor;
                    //Debug.Log("Plain colour " + x + "," + y);
                    plainPixels++;
                }

                baseTexture.SetPixel(x, y, finalColor);
                isOverlayPixel = false;
            }
        }
    }

    private Texture2D GetFillTexture(int width, int height, Color col)
    {
        Texture2D newTex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                newTex.SetPixel(x, y, col);
            }
        }

        return newTex;
    }

    public Texture2D CreateTextureCopy(Texture2D original)
    {
        Texture2D newTex = new Texture2D(original.width, original.height, TextureFormat.RGBA32, false);
        newTex.filterMode = FilterMode.Point;

        for (int x = 0; x < original.width; x++)
        {
            for (int y = 0; y < original.height; y++)
            {
                newTex.SetPixel(x, y, original.GetPixel(x, y));
            }
        }

        return newTex;
    }

    public void CopyFullTexture(Texture2D source, Texture2D destination)
    {

        for (int x = 0; x < source.width; x++)
        {
            for (int y = 0; y < source.height; y++)
            {
                destination.SetPixel(x, y, source.GetPixel(x, y));
            }
        }
    }

    private void FillTexture(Texture2D original, Color col)
    {
        for (int x = 0; x < original.width; x++)
        {
            for (int y = 0; y < original.height; y++)
            {
                original.SetPixel(x, y, col);
            }
        }
    }
}
