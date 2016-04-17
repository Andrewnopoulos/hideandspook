using UnityEngine;
using System;
using System.Collections;

public enum State
{
	PreGame,
	Playing,
	PostGame
}

public class StateManager : MonoBehaviour
{
	private State mState;
	private Mesh mQuad;
	private Material mMaterial;
	private MeshRenderer mRenderer;
	private MeshFilter mFilter;

	private float mAlpha;
	private float mTarget;

	private Action mFadeCallback;

	public State state
	{
		get { return mState; }
		set { mState = value; }
	}

	public float alpha
	{
		get { return mAlpha; }
	}

	public bool Idle
	{
		get
		{
			return Mathf.Abs(mTarget - mAlpha) < Mathf.Epsilon;
		}
	}

	private void Awake()
	{
		mQuad = new Mesh();
		Vector3[] vertices = new Vector3[4];
		int[] indices = new int[] { 2, 1, 0, 3, 1, 2 };//{ 0, 1, 2, 2, 1, 3 };
		vertices[0] = new Vector3(-1f, 1f, 0.0f);
		vertices[1] = new Vector3(1f, 1f, 0.0f);
		vertices[2] = new Vector3(-1f, -1f, 0.0f);
		vertices[3] = new Vector3(1f, -1f, 0.0f);
		mQuad.vertices = vertices;
		mQuad.SetIndices(indices, MeshTopology.Triangles, 0);
		mQuad.UploadMeshData(true);
		mQuad.bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(100f, 100f, 100f));

		mMaterial = new Material(Shader.Find("Unlit/FullScreenUnlit"));
		mMaterial.color = Color.black;

		mFilter = gameObject.AddComponent<MeshFilter>();
		mFilter.mesh = mQuad;

		mRenderer = gameObject.AddComponent<MeshRenderer>();
		mRenderer.sharedMaterial = mMaterial;

		SetTransparency(1f);
		FadeOut(State.PreGame, null);
	}

	private void Update()
	{
		if (Mathf.Abs(mTarget - mAlpha) > Mathf.Epsilon)
		{
			float speed = Time.deltaTime / 0.5f;
			if (mTarget > mAlpha)
			{
				mAlpha += speed;
				if (mAlpha > mTarget)
				{
					mAlpha = mTarget;
				}
			}
			else
			{
				mAlpha -= speed;
				if (mAlpha < mTarget)
				{
					mAlpha = mTarget;
				}
			}

			mMaterial.color = new Color(0, 0, 0, mAlpha);
		}
		else if (mFadeCallback != null)
		{
			mFadeCallback();
			mFadeCallback = null;
		}
	}

	public void FadeIn(State newState, Action callback)
	{
		mState = newState;
		mTarget = 1f;
		mFadeCallback = callback;
	}

	public void FadeOut(State newState, Action callback)
	{
		mState = newState;
		mTarget = 0f;
		mFadeCallback = callback;
	}

	public void SetTransparency(float transparency)
	{
		mAlpha = transparency;
		mTarget = mAlpha;
		mMaterial.color = new Color(0, 0, mAlpha);
	}

	private static StateManager _instance;
	public static StateManager instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject obj = new GameObject("StateManager");
				DontDestroyOnLoad(obj);
				_instance = obj.AddComponent<StateManager>();
			}

			return _instance;
		}
	}
}
