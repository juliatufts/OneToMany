    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for both collider in hole, and rim.
/// </summary>
public class LotusHoleController : MonoBehaviour {

    public LotusController lotus;
    public AnimationCurve onTouchRampUpCurve;
    [Range(-2400, 2400)]
    public float pitch;
    public AK.Wwise.RTPC pitchRTPC;
    public AK.Wwise.Event onTouch;

	[Tooltip("Time since last touch must be above this in order for next touch to have OnTouch event fired.")]
	public float onTouchCoolDownThreshold = 1f;

    [Tooltip("Used only for initializing the mesh")]
    public float rimAngleThreshold = 80f;

    [HideInInspector]
    public float timeThisTouch;
    [HideInInspector]
    public float timeSinceLastTouch;
    [HideInInspector]
    public float totalTimeTouched;

    public delegate void OnStartTouch();
    public OnStartTouch onStartTouch;

    public int SubmeshIndex { get; private set; }
    public float TouchIntensity { get; private set; }

    private bool touching;
    private float maxTouchIntensity = 1f;
    private float touchValue;

    public void SetSubmeshIndex(int value)
    {
        SubmeshIndex = value;
    }

    void Awake() {
        var networkView = GetComponent<NetworkView>();
        networkView.observed = this;
    }
    void Start()
    {
        pitch = Random.Range(-2400, 2400);
        timeSinceLastTouch = onTouchCoolDownThreshold;
    }

    void Update()
    {
        if (touching)
        {
            touchValue += (Time.deltaTime * lotus.timeScale);
        }
        else
        {
            touchValue -= (Time.deltaTime * lotus.timeScale);
            timeSinceLastTouch += Time.deltaTime;
        }
        touchValue = Mathf.Clamp01(touchValue);

		TouchIntensity = onTouchRampUpCurve.Evaluate(touchValue);
    }

    void OnTriggerEnter(Collider other)
    {
		if (other.transform.CompareTag("GameController"))
		{
			if (timeSinceLastTouch > onTouchCoolDownThreshold)
			{
				if (onStartTouch != null)
				{
                    pitchRTPC.SetValue(gameObject, pitch);
                    onTouch.Post(gameObject);
                    onStartTouch();
				}
			}

            touching = true;
            timeSinceLastTouch = 0;
            timeThisTouch = 0;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (touching)
        {
            timeThisTouch += Time.deltaTime;
            totalTimeTouched += Time.deltaTime;
            TouchGazeManager.Instance.BankLotusTouchTime(Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("GameController"))
        {
            touching = false;
        }
    }

    void OnSerializeNetworkView(BitStream stream) {
        stream.Serialize(ref timeThisTouch);
        stream.Serialize(ref totalTimeTouched);
        stream.Serialize(ref timeSinceLastTouch);
        stream.Serialize(ref touchValue);
    }
}
