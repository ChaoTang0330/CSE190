using System.Collections; using System.Collections.Generic;
using System.IO; using UnityEngine;

/// <summary>
/// Attached to left/right scene camera
/// </summary>

public class OffscreenRendering : MonoBehaviour {
	public Transform[] CAVETrans = new Transform[3];
	public RenderTexture[] CAVETex = new RenderTexture[3];

	private int ScreenshotsPerSecond = 60;
	private Camera OffscreenCamera;

	private Vector3[] p_a = new Vector3[3];
	private Vector3[] p_b = new Vector3[3];
	private Vector3[] p_c = new Vector3[3];
	private Vector3[] v_r = new Vector3[3];
	private Vector3[] v_u = new Vector3[3];
	private Vector3[] v_n = new Vector3[3];

	private float zNear = 0.1f;
	private float zFar = 100f;

	// Use this for initialization 	
	void Start () {

		for(int i = 0; i < 3; i++)
        {
			p_a[i] = CAVETrans[i].TransformPoint(new Vector3(5f, 0f, 5f));
			p_b[i] = CAVETrans[i].TransformPoint(new Vector3(-5f, 0f, 5f));
			p_c[i] = CAVETrans[i].TransformPoint(new Vector3(5f, 0f, -5f));

			v_r[i] = Vector3.Normalize(p_b[i] - p_a[i]);
			v_u[i] = Vector3.Normalize(p_c[i] - p_a[i]);
			v_n[i] = Vector3.Normalize(Vector3.Cross(v_r[i], v_u[i]));
		}

		OffscreenCamera = GetComponent<Camera>();
		StartCoroutine("CaptureAndSaveFrames"); 	
	}      
	
	
	IEnumerator CaptureAndSaveFrames() 
	{
		while (true)
		{
			yield return new WaitForEndOfFrame();

			OffscreenCamera.enabled = true;

			transform.rotation = Quaternion.identity;
			
			// Remember currently active render texture.
			RenderTexture currentRT = RenderTexture.active;
			RenderTexture targetRT = OffscreenCamera.targetTexture;

			//Texture2D offscreenTexture;

			for (int i = 0; i < 3; i++)
            {
				Vector3 v_a = p_a[i] - transform.position;
				Vector3 v_b = p_b[i] - transform.position;
				Vector3 v_c = p_c[i] - transform.position;

				float dis = -Vector3.Dot(v_n[i], v_a);

				float l = Vector3.Dot(v_r[i], v_a) * zNear / dis;
				float r = Vector3.Dot(v_r[i], v_b) * zNear / dis;
				float b = Vector3.Dot(v_u[i], v_a) * zNear / dis;
				float t = Vector3.Dot(v_u[i], v_c) * zNear / dis;

				Matrix4x4 P = Matrix4x4.Frustum(l, r, b, t, zNear, zFar);
				Matrix4x4 M_T = Matrix4x4.identity;
				M_T[0, 0] = -v_r[i].x; M_T[0, 1] = -v_r[i].y; M_T[0, 2] = v_r[i].z;
				M_T[1, 0] = -v_u[i].x; M_T[1, 1] = -v_u[i].y; M_T[1, 2] = v_u[i].z;
				M_T[2, 0] = -v_n[i].x; M_T[2, 1] = -v_n[i].y; M_T[2, 2] = v_n[i].z;

				Matrix4x4 T = Matrix4x4.identity;
				T[0, 3] = -transform.position.x;
				T[1, 3] = -transform.position.y;
				T[2, 3] = -transform.position.z;

				OffscreenCamera.projectionMatrix = P * M_T;// * T;

				// Set target texture as active render texture. 			
				RenderTexture.active = CAVETex[i];
				OffscreenCamera.targetTexture = CAVETex[i];
				// Render to texture 			
				OffscreenCamera.Render();

				// Read offscreen texture
				//CAVETex[i] = new Texture2D(OffscreenCamera.targetTexture.width, OffscreenCamera.targetTexture.height, TextureFormat.RGB24, false);
				//CAVETex[i].ReadPixels(new Rect(0, 0, OffscreenCamera.targetTexture.width, OffscreenCamera.targetTexture.height), 0, 0, false);
				//CAVETex[i].Apply();
			}

			// Reset previous render texture. 			
			RenderTexture.active = currentRT;
			OffscreenCamera.targetTexture = targetRT;

			// Delete textures. 			
			//UnityEngine.Object.Destroy(offscreenTexture);

			OffscreenCamera.enabled = false;

			yield return new WaitForSeconds(1.0f / ScreenshotsPerSecond);
		}
	}  	
	
	/// <summary>     
	/// Stop image capture.     
	/// </summary>     
	public void StopCapturing (){         
		StopCoroutine("CaptureAndSaveFrames");         
		//FrameCounter = 0;     
	} 	
	
	/// <summary> 	
	/// Resume image capture. 	
	/// </summary> 	
	public void ResumeCapturing () { 		
		StartCoroutine("CaptureAndSaveFrames"); 	
	} 
}