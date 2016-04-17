using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerGhost : MonoBehaviour {

    //public enum PLAYER_TYPE
    //{
    //    NONE,
    //    ONE,
    //    TWO,
    //}

   // static uint playerIndex = 3;

    //public PLAYER_TYPE m_playerType = PLAYER_TYPE.NONE;
	
    public TriggerPoint m_triggerPoint = null;
	public SteamVR_TrackedObject m_trackedObject = null;

    public bool m_withinInteractRange = false;

    float distanceToTrigger = 0;

    // max distance from which one can activate the trigger points
    [Tooltip("max distance from which one can activate the trigger points")]
    [Range (0.0f, 2.0f)]
    public float distanceThreshold;

    [Range(0.0f, 5.0f)]
    public float speedScaler = 3.0f;

    [Range(0.0f, 0.5f)]
    public float visionScaler = 0.4f;

    [Range(0.0f, 1.0f)]
    public float detectionScaler = 0.2f;

    [Range(-0.5f, 0.0f)]
    public float minimumTransparency = -0.5f;

    // want to keep this between 0 (for not moving) and 0.4f (for fast moving)
    float m_speedVisibility;

    // want to keep this between 0 (for far away) and 0.4f (for close to center of vision)
    float m_visionVisibility;

    // want to keep this between 0 (for far from trigger point) and 0.2f (for close to it)
    float m_detectingVisibility;

    // optimally should float between 0 (minimum visibility) and 1.0f (maximum visibility)
    float m_totalVisibility;

    // scale this total visibility down for transparency
    // add a minumum area of effect for 'force field' effect

    // between (-30, 30)
    float m_transparencyDelta;

    [Range(-1.0f, 0.0f)]
    public float defaultTransparencyDelta = -0.3f; // 30% default 

    [Range(0.1f, 0.6f)]
    public float vibeDelayModifier = 0.2f;

    [Range(500, 3000)]
    public ushort vibeLengthModifier = 3000;

    float m_vibeTimer = 0.0f;

    // Will go between 0 and 1
    float m_transparencyValue;

    public bool dead = false;

    public Renderer ghostRenderer;

    Transform parentTransform;
	Color baseColor;

    //TrailRenderer m_ghostTrail;

    public Transform ghostTrailPrefab;
    Transform ghostTrail;

    Collider m_collider;

    public bool m_readyToPlay = false;

    public PlayerHuman PlayerHead;

	void Awake()
	{
		m_trackedObject = GetComponentInParent<SteamVR_TrackedObject>();
		baseColor = ghostRenderer.sharedMaterial.GetColor("_TintColor");
	}

	// Use this for initialization
	void Start ()
    {
        SetTriggerPoint();
        ghostTrail = Instantiate(ghostTrailPrefab);

        m_collider = GetComponent<Collider>();

        m_collider.enabled = false;
	}

    //void Awake()
    //{
    //    parentTransform = GetComponentsInParent<Transform>()[1];
    //}

    float GetDistanceFromViewCenter()
    {
        Ray playerVision = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        float inFrontOfPlayer = Vector3.Dot(playerVision.direction, transform.position - playerVision.origin);

        if (inFrontOfPlayer < 0)
        {
            return -1;
        }

        return Vector3.Cross(playerVision.direction, transform.position - playerVision.origin).magnitude;
    }

    void UpdateVisibilityValues()
    {
		StateManager sm = StateManager.instance;
		if (sm.state != State.Playing)
		{
			m_totalVisibility = 0;
			m_transparencyValue = 0;
			return;
		}

		var device = SteamVR_Controller.Input((int)m_trackedObject.index);
		m_speedVisibility = speedScaler * device.velocity.magnitude;

        //Debug.Log("Speed vis value : " + m_speedVisibility);

        if (m_speedVisibility > 0.65f)
        {
            ghostTrail.position = transform.position;
        }

        float viewCenterDistance = GetDistanceFromViewCenter();

        if (viewCenterDistance == -1) // if it's -1 then it's behind
        {
            m_totalVisibility = 0; // so you can't see it at all, noob
            return;
        }

        m_visionVisibility = visionScaler / (viewCenterDistance == 0 ? 1 : viewCenterDistance);
        if (device.GetHairTrigger()) // if detecting
        {
            if (distanceToTrigger != 0)
                m_detectingVisibility = detectionScaler / distanceToTrigger;
        }
        else
        {
            m_detectingVisibility = 0.0f;
        }

        m_totalVisibility = m_speedVisibility + m_visionVisibility + m_detectingVisibility;
    }

    void UpdateGhostTransparency()
    {
        m_transparencyDelta = m_totalVisibility + defaultTransparencyDelta;

        m_transparencyValue += m_transparencyDelta * Time.deltaTime;

        m_transparencyValue = Mathf.Clamp(m_transparencyValue, minimumTransparency, 1.0f);

        // hardcoded value necessary for telling if ghost is within visibility of player
        if (m_visionVisibility < 0.35f)
        {
            m_transparencyValue = minimumTransparency;
        }

		ghostRenderer.material.SetColor("_TintColor", new Color(baseColor.r, baseColor.g, baseColor.b, m_transparencyValue));

        if (m_transparencyValue > 0.95f)
        {
            Die();
        }
    }
	
    void Vibrate()
    {
        m_vibeTimer += Time.deltaTime;

		//Set to 2D.
		Vector3 triggerPointLocation = m_triggerPoint.transform.position;
        Vector3 transformLocation = transform.position;
        distanceToTrigger = (triggerPointLocation - transformLocation).magnitude;

        float vibeDelay = vibeDelayModifier * distanceToTrigger;
        float vibeLength = vibeLengthModifier / (distanceToTrigger == 0 ? 1 : distanceToTrigger);

		var device = SteamVR_Controller.Input((int)m_trackedObject.index);

		if (m_vibeTimer > vibeDelay)
        {
            m_vibeTimer = 0;
            ushort clampedLength = (ushort)Mathf.Clamp(vibeLength, 500, 2000);

            if (distanceToTrigger < distanceThreshold)
            {
                device.TriggerHapticPulse((ushort)3200);
            } else
            {
				device.TriggerHapticPulse(clampedLength);
            }
        }
    }

	// Update is called once per frame
	void Update ()
	{
		//if (dead)
  //      {
  //          if (Input.GetKey(KeyCode.Space))
  //          {
  //              Reset();
  //          }

  //          return;
  //      }

		var device = SteamVR_Controller.Input((int)m_trackedObject.index);
		bool appMenuDown = device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
		bool touchPadDown = device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad);
		bool hairTriggerDown = device.GetHairTriggerDown();


		StateManager sm = StateManager.instance;
		if (!dead && sm.state == State.Playing && m_readyToPlay)
        {
            UpdateVisibilityValues();
            UpdateGhostTransparency();

            Vibrate();

            if (touchPadDown || hairTriggerDown)
            {
                if (distanceToTrigger < distanceThreshold)
                {
                    ReachTrigger();
                }
            }
        }

        if (appMenuDown)
        {
            if (!m_readyToPlay && sm.state == State.PreGame && sm.Idle)
            {
                m_readyToPlay = true;
                device.TriggerHapticPulse((ushort)3000);
				m_collider.enabled = true;
                TriggerManager.s_manager.m_activePlayers++;
            }
			
            if (sm.state == State.PostGame && sm.Idle)
            {
				TriggerManager.ReloadGame();
				return;
            }
        }


        // change the 2 if we wanted arbitrary number of players
		if (sm.state == State.PreGame)
		{
			if (TriggerManager.s_manager.m_playing == false)
			{
				if (TriggerManager.s_manager.m_activePlayers == 2)
				{
					m_collider.enabled = true;
					TriggerManager.s_manager.m_playing = true;
					StateManager.instance.state = State.Playing;
				}
				else
				{
					m_collider.enabled = false;
					ghostRenderer.material.SetColor("_TintColor", new Color(baseColor.r, baseColor.g, baseColor.b, 0));
				}
			}
			else
			{
				m_collider.enabled = true;
			}
		} else if (sm.state == State.PostGame)
		{
			ghostRenderer.material.SetColor("_TintColor", new Color(81f / 255f, 0f, 0f, 1-sm.alpha));
		}
	}

    public void Reset()
    {
        dead = false;
		transform.parent = null;
        m_transparencyValue = 0.0f;
    }

    void Die()
    {
        dead = true;
		ghostRenderer.material.SetColor("_TintColor", new Color(81f/255f, 0f, 0f, 1f));
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHuman>().m_deadPlayers++;
        transform.SetParent(null, true);
		m_collider.enabled = false;
    }

    public void SetTriggerPoint()
    {
        List<TriggerPoint> list = TriggerManager.s_manager.m_triggerList;
        m_triggerPoint = list[Random.Range(0, list.Count-1)];
        list.Remove(m_triggerPoint);
    }

    public void ReachTrigger()
    {
        m_triggerPoint.ActivateTrigger();
        if (TriggerManager.s_manager.m_triggerList.Count == 0)
        {
            Debug.Log("Ghosts ween");
            return;
        }
        SetTriggerPoint();
    }
}
