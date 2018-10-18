using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{

    private static int spawningAnimationHash = Animator.StringToHash("Spawning");
    private float scaleMultipler = 1;
    public float scaleMax = 1.15f;
    public float scaleSpeed = 0.01f;

    [Serializable]
    internal struct SpawnerData
    {
        [SerializeField] internal GameObject spawnObject;
        [SerializeField] internal int maxObjects;
        [SerializeField] internal float spawnTime;
        [Range(0,1)]
        [Tooltip("0 = from the ground, 1 = from above")]
        [SerializeField] internal int typeOfSpawn;
    }

    [SerializeField] private SpawnerData[] objectsToSpawn;
    [SerializeField] private float radiusOfArea;
    [SerializeField] private float heightOfResp;
    [SerializeField] private float respLevel;
    [SerializeField] private float spawnAreaRadius = 3;


    private GameObject[] objectsInScene;
    private float[] timeToRespawn; //-10f if active
    private float[] spawnTime;
    private int[] typeOfSpawn;
    private bool isActive = true;

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    public void Reload()
    {

        
        scaleMultipler = 1;
        //Debug.Log("New scale multipler is " + scaleMultipler);
        for (int i = 0; i < objectsToSpawn.Length; i++)
        {
            timeToRespawn[i] = -10f;
            ActivateInRandomPosition(objectsInScene[i].gameObject, i);
        }
        isActive = true;
        scaleMultipler = 1; //do it again
    }

    private void Start ()
    {
        //calculate number of obj
        int _numberOfObjects = 0;
        for (int i = 0; i < objectsToSpawn.Length; i++)
        {
            _numberOfObjects += objectsToSpawn[i].maxObjects;
        }

        objectsInScene = new GameObject[_numberOfObjects];
        timeToRespawn = new float[_numberOfObjects];
        spawnTime = new float[_numberOfObjects];
        typeOfSpawn = new int[_numberOfObjects];

        int _k = 0;
        //loop SpawnerData 
        for (int j = 0; j < objectsToSpawn.Length; j++)
        {
            //loop objects
            for (int i = 0; i < objectsToSpawn[j].maxObjects; i++)
            {
                Vector2 _vector2 =new Vector2(transform.position.x,transform.position.z) + Random.insideUnitCircle * spawnAreaRadius;
                objectsInScene[_k] = Instantiate(objectsToSpawn[j].spawnObject, new Vector3(_vector2.x, respLevel + transform.position.y,_vector2.y), Quaternion.identity);
                spawnTime[_k] = objectsToSpawn[j].spawnTime;
                typeOfSpawn[_k] =objectsToSpawn[j].typeOfSpawn;
                timeToRespawn[_k] = -10f;
                _k++;
            }
        }

    }
	
	private void Update ()
	{
	    if (!isActive) return; 
	    //Debug.Log("scalemultipler spawn: " + scaleMultipler);
        //look for inactive objects
        for (int i = 0; i < timeToRespawn.Length; i++)
        {
            //check objects if they are not active
            if (objectsInScene[i].activeSelf && timeToRespawn[i] != 10f)
            {
                timeToRespawn[i] = spawnTime[i];
            }
            //spawn
            else if (timeToRespawn[i] < 0 && timeToRespawn[i] != 10f)
            {
                timeToRespawn[i] = -10f;
                ActivateInRandomPosition(objectsInScene[i].gameObject, i);
            }

            else if (timeToRespawn[i] > 0)
            {
                timeToRespawn[i] -= Time.deltaTime;
            }
        }

    }


    private void ActivateInRandomPosition(GameObject spawningObject, int index)
    {
        Vector2 _vector2 = new Vector2(transform.position.x, transform.position.z) + Random.insideUnitCircle * spawnAreaRadius;      
        spawningObject.transform.position = typeOfSpawn[index] == 0 ? new Vector3(_vector2.x, respLevel, _vector2.y) : new Vector3(_vector2.x, heightOfResp + respLevel, _vector2.y);
        scaleMultipler = Mathf.Min(scaleMax, scaleMultipler + 0.01f);
        //Debug.Log(scaleMultipler);
        spawningObject.transform.localScale *= scaleMultipler;

        //run animation
        Animator anim = spawningObject.GetComponent<Animator>();
        //Debug.Log(anim);
        if (anim != null && anim.isInitialized)
        {
            anim.SetTrigger(spawningAnimationHash);
        }

        //Debug.Log(spawningObject + " type of spawn: " + typeOfSpawn[index] + " position: " + spawningObject.transform.position);

        spawningObject.SetActive(true);
    }

}
