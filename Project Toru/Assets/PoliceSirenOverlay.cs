using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoliceSirenOverlay : MonoBehaviour
{
    public Image policeSiren;

    float r,g,b,a;

    bool flipBool = false;

    public bool startSiren = false;

    private void Start()
    {
        r = policeSiren.color.r;
        g = policeSiren.color.g;
        b = policeSiren.color.b;
        a = policeSiren.color.a;

        a = 0f;
        AdjustColor();
    }

    private void Update()
    {
        if (startSiren)
        {
            AutomateSiren();
        }
    }
	
	public void Activate() {
		gameObject.SetActive(true);
		startSiren = true;
	}
	
	public void Deactivate() {
		gameObject.SetActive(false);
		startSiren = false;
	}

    public void AutomateSiren()
    {
        if ((int)Time.timeSinceLevelLoad % 2 == 0)
        {
            a -= 0.1f;
            flipImage();
        }
        else
        {
            a += 0.1f;
        }
        a = Mathf.Clamp(a, 0, 1f);
        AdjustColor();
    }

    public void flipImage()
    {
        if(flipBool == false)
        {
            policeSiren.transform.localRotation = Quaternion.Euler(0, 0, 0);
            flipBool = true;
        } 
        else
        {
            policeSiren.transform.localRotation = Quaternion.Euler(0, 180, 0);
            flipBool = false;
        }

    }

    public void AdjustColor()
    {
        Color c = new Color(r, g, b, a);
        GetComponent<Image>().color = c;
    }
}
