using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ScannerEffectDemo : MonoBehaviour
{
    public float cooldown;
    public float force = 10f;
	public Transform[] ScannerOrigins;
    public Image[] ScanImages;
    public Material EffectMaterial;
	public float ScanDistance;
    public Transform SelectedScannerOrigin;
    private Vector3 savedPosition;

	private Camera _camera;
    private bool isScanAvailible = true;

    // Demo Code
    bool _scanning;
	Scannable[] _scannables;

    //cooldowns
    private float cooldown0 = 0; //gracz1
    private float cooldown1 = 0; //gracz2
    private float cooldown2 = 0; //gracz3
    

	void Start()
	{
        
    }

	void Update()
	{
		if (_scanning)
		{
            //Debug.Log(_scannables.Length);
            ScanDistance += Time.deltaTime * 6;
			foreach (Scannable s in _scannables)
			{
                //Debug.Log(_scannables.Length);
			    if (Vector3.Distance(savedPosition, s.transform.position) <= ScanDistance)
			    {
                    //s.Ping();
			        if (!s.hitedRecently)
			        {
			            s.hitedRecently = true;
			            Vector3 forceToAdd = s.transform.position - savedPosition;
			            forceToAdd.y = 0f;
			            //s.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 5f * 1f, ForceMode.VelocityChange);
			            s.gameObject.GetComponent<Rigidbody>().AddForce(forceToAdd.normalized * 5f, ForceMode.Impulse);
			            //Debug.Log("Pushed Object " + s);
			        }
			    }
					
                    
			}
		}

		if (cooldown0 < 0 && Input.GetButtonDown("Wave0_0") && isScanAvailible)
		{
            _scannables = FindObjectsOfType<Scannable>();
            StartScan(0);
		    cooldown0 = cooldown;
		}

        if (cooldown1 < 0 && Input.GetButtonDown("Wave0_1") && isScanAvailible)
        {
            _scannables = FindObjectsOfType<Scannable>();
            StartScan(1);
            cooldown1 = cooldown;
        }

        if (cooldown2 < 0 && Input.GetButtonDown("Wave0_2") && isScanAvailible)
        {
            _scannables = FindObjectsOfType<Scannable>();
            StartScan(2);
            cooldown2 = cooldown;
        }

	    ReduceCooldowns();
	    GetScanCooldown();

//        if (Input.GetMouseButtonDown(0))
//		{
//			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
//			RaycastHit hit;
//
//			if (Physics.Raycast(ray, out hit))
//			{
//				_scanning = true;
//				ScanDistance = 0;
//				SelectedScannerOrigin.position = hit.point;
//			}
//		}
	}
	// End Demo Code

	void OnEnable()
	{
		_camera = GetComponent<Camera>();
		_camera.depthTextureMode = DepthTextureMode.Depth;
        SelectedScannerOrigin = ScannerOrigins[0];
	    savedPosition = SelectedScannerOrigin.position;
	}

    public void GetScanCooldown()
    {
        ScanImages[0].fillAmount = cooldown0 < 0 ? 0 : (cooldown0 / cooldown);
        ScanImages[1].fillAmount = cooldown1 < 0 ? 0 : (cooldown1 / cooldown);
        ScanImages[2].fillAmount = cooldown2 < 0 ? 0 : (cooldown2 / cooldown);

    }

    public void StartScan(int playerNumber)
    {
        
        _scanning = true;
        ScanDistance = 0;
        SelectedScannerOrigin = ScannerOrigins[playerNumber];
        savedPosition = SelectedScannerOrigin.position;
        isScanAvailible = false;
        StartCoroutine(EnableScan(playerNumber));

    }

    private void ReduceCooldowns()
    {
        cooldown0 -= Time.deltaTime;
        cooldown1 -= Time.deltaTime;
        cooldown2 -= Time.deltaTime;
    }

    private IEnumerator EnableScan(int playerNumber)
    {
        yield return new WaitForSeconds(1.0f);
        isScanAvailible = true;

        foreach (Scannable scannable in _scannables)
        {
            scannable.hitedRecently = false;
        }

        //reset cooldown
        switch (playerNumber)
        {
            case 0:
                cooldown0 = cooldown;
                break;
            case 1:
                cooldown1 = cooldown;
                break;
            case 2:
                cooldown2 = cooldown;
                break;
            default:
                Debug.Log("sth gone wrong");
                break;
        }
    }

	[ImageEffectOpaque]
	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		EffectMaterial.SetVector("_WorldSpaceScannerPos", savedPosition);
		EffectMaterial.SetFloat("_ScanDistance", ScanDistance);
		RaycastCornerBlit(src, dst, EffectMaterial);
	}

	void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
	{
		// Compute Frustum Corners
		float camFar = _camera.farClipPlane;
		float camFov = _camera.fieldOfView;
		float camAspect = _camera.aspect;

		float fovWHalf = camFov * 0.5f;

		Vector3 toRight = _camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
		Vector3 toTop = _camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

		Vector3 topLeft = (_camera.transform.forward - toRight + toTop);
		float camScale = topLeft.magnitude * camFar;

		topLeft.Normalize();
		topLeft *= camScale;

		Vector3 topRight = (_camera.transform.forward + toRight + toTop);
		topRight.Normalize();
		topRight *= camScale;

		Vector3 bottomRight = (_camera.transform.forward + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= camScale;

		Vector3 bottomLeft = (_camera.transform.forward - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= camScale;

		// Custom Blit, encoding Frustum Corners as additional Texture Coordinates
		RenderTexture.active = dest;

		mat.SetTexture("_MainTex", source);

		GL.PushMatrix();
		GL.LoadOrtho();

		mat.SetPass(0);

		GL.Begin(GL.QUADS);

		GL.MultiTexCoord2(0, 0.0f, 0.0f);
		GL.MultiTexCoord(1, bottomLeft);
		GL.Vertex3(0.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 0.0f);
		GL.MultiTexCoord(1, bottomRight);
		GL.Vertex3(1.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 1.0f);
		GL.MultiTexCoord(1, topRight);
		GL.Vertex3(1.0f, 1.0f, 0.0f);

		GL.MultiTexCoord2(0, 0.0f, 1.0f);
		GL.MultiTexCoord(1, topLeft);
		GL.Vertex3(0.0f, 1.0f, 0.0f);

		GL.End();
		GL.PopMatrix();
	}
}
