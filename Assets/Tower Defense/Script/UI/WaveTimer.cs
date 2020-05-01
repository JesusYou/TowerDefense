using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class WaveTimer : MonoBehaviour
{
    public Image timeImage;
    public Text currentWaveNumberText;
    public Text maxWaveNumberText;
    public GameObject highLightFX;
    public float highLightTime = 0.5f;
    private WaveInfo waveInfo;
    private List<float> waves = new List<float>();
    private int currentWave;
    private float currentTimeout;
    private float counter;
    private bool finished;

    void Awake()
	{
        waveInfo = FindObjectOfType<WaveInfo>();
	}

    // Start is called before the first frame update
    void Start()
    {
        highLightFX.SetActive(false);
        waves = waveInfo.wavesTimeout;
        currentWave = 0;
        counter = 0f;
        finished = false;
        GetCurrentWaveCounter();
        maxWaveNumberText.text = waves.Count.ToString();
        currentWaveNumberText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
	{
        if (finished == false)
		{
            if (counter <= 0f)
			{
                EventManager.TriggerEvent("WaveStart", null, currentWave.ToString());
                currentWave++;
                currentWaveNumberText.text = currentWave.ToString();
                StartCoroutine("HighLightTime");
                if (GetCurrentWaveCounter() == false)
				{
                    finished = true;
                    EventManager.TriggerEvent("TimerEnd", null, null);
                    return;
				}
			}
            counter -= Time.fixedDeltaTime;
            if (currentTimeout > 0f)
			{
                timeImage.fillAmount = counter / currentTimeout;
			}
			else
			{
                timeImage.fillAmount = 0f;
			}
		}
	}

    void OnDisable()
	{
        StopAllCoroutines();
	}

    void OnDestroy()
	{
        StopAllCoroutines();
	}

    private bool GetCurrentWaveCounter()
	{
        bool res = false;
        if (waves.Count > currentWave)
		{
            counter = currentTimeout = waves[currentWave];
            res = true;
		}
        return res;
	}

    private IEnumerator HighLightTime()
	{
        highLightFX.SetActive(true);
        yield return new WaitForSeconds(highLightTime);
        highLightFX.SetActive(false);
	}
}
