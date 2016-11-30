using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MeterController : MonoBehaviour
{
    public Image fillerImage;
    private float destiredValue;
    public float startValue = 0.5f;
	void Start ()
    {
        fillerImage.fillAmount = startValue;
        destiredValue = startValue;
    }

    public void SetToPosition(float value)
    {
        value = Mathf.Clamp(value, 0.0f, 1.0f);
        destiredValue = value;
    }
	
	// Update is called once per frame
	void Update ()
    {
        fillerImage.fillAmount = destiredValue;
    }
}
