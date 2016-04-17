using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ObjectMeta : MonoBehaviour {

    public int width, height;
    public bool _requireGrounded, _requireCenterY, _requireCeiling, _requireLeft, _requireRight;

#if UNITY_EDITOR
	public void OnDrawGizmosSelected()
	{
		float hOff = height - 1 * 0.5f;
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position + transform.localRotation * new Vector3(0, height * 0.5f, 0), transform.localRotation * new Vector3(width, height, 1));
		Gizmos.DrawLine(transform.position + transform.localRotation * new Vector3(width * -0.5f, 0, 0.5f), transform.position + transform.localRotation * new Vector3(width * 0.5f, height - hOff, 0.5f));
	}
#endif
}
