using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

public class CameraLogic : MonoBehaviour
{

	
	[SerializeField]private float smoothness=0.125f;
	
	[SerializeField]private float posY;
	[SerializeField]private float minX;
	[SerializeField]private float maxX;
	[SerializeField]private float minY;
	[SerializeField]private float maxY;
	[SerializeField] private float zoomSize;//어디까지 줌 할꺼냐?
	[SerializeField] private float zoomTime=0;//zoom 적용시간...
    [SerializeField] private float cameraSize;
    private Camera thisCam;
	
	private Transform target;//please put here Player! or Not
	
  
	private float curZoomtime;//current Zooming time	
	private bool zoomingNow;//zoomingNow?
	private bool zoomoutNow;
	private Vector3 veloc = Vector3.zero;
    public static CameraLogic _instance_camera;




	private void Awake()
	{
		thisCam    = gameObject.GetComponent<Camera>();
        _instance_camera = this;
	}

	private void Start()
	{

        thisCam.fieldOfView = cameraSize;
		curZoomtime= 0;
		zoomingNow = false;
		zoomoutNow = false;
		try
		{
			target = GameObject.FindGameObjectWithTag("Player").transform;
			
		}
		catch(Exception exception)
		{
			Debug.Log("Player not detective");
			target.transform.position = Vector3.zero;
		}
		
	}

	private void Update()
	{
		if (curZoomtime>0)
		{
			//줌 타임을 감소시키자.
			curZoomtime -= 1 * Time.deltaTime;
			if (curZoomtime <= 0)
			{
				curZoomtime = 0;
				zoomoutNow = true;

			}
		}

		if (zoomoutNow)
		{
			thisCam.fieldOfView +=30* Time.deltaTime;//보간으로 부드럽게 처리를 하면 더 좋은가?
			
			//줌 아웃 로직	
			if (thisCam.fieldOfView >= cameraSize)
			{
				thisCam.fieldOfView = cameraSize;
				zoomoutNow = false;
			}
		}

		if (zoomingNow)
		{
			thisCam.fieldOfView -= 30*Time.deltaTime;
            
			if (thisCam.fieldOfView <= zoomSize)
			{
				thisCam.fieldOfView = zoomSize;
				curZoomtime = zoomTime;
				zoomingNow = false;
			}
			//줌 인 로직
		}

		
		
		if (Input.GetKeyDown(KeyCode.W))
		{
			ShakeCamera(0.1f,1);
		}
		if (shakeTimer >= 0)
		{
			Vector2 ShakePos = UnityEngine.Random.insideUnitCircle * shakeAmount;
			transform.position = new Vector3(transform.position.x+ShakePos.x,transform.position.y+ShakePos.y,transform.position.z);
			shakeTimer -= Time.deltaTime;
		}

	
	}
	
	private void FixedUpdate()
	{
		Vector3 targetPos = target.TransformPoint(new Vector3(0, 1, -10));
		Vector3 fixedPos = Vector3.SmoothDamp(transform.position, targetPos, ref veloc, smoothness);
		transform.position = new Vector3(Mathf.Clamp(fixedPos.x,minX,maxX),
			Mathf.Clamp(fixedPos.y,minY,maxY),-10f);  
        
	}
    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(transform.rotation.x, 0.0f, transform.rotation.z);
    }


    public float shakeTimer;
	public float shakeAmount;

	public void ShakeCamera(float shakePwr, float shakeDur)
	{
		shakeAmount = shakePwr;
		shakeTimer = shakeDur;
	}
	

	public void zooming()
	{
		
	
		if (zoomoutNow)
		{
			zoomoutNow = false;
		}
		zoomingNow = true;

	}

}
