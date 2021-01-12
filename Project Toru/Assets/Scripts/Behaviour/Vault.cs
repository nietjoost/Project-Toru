using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vault : Furniture
{
	public GameObject money = null;

    [SerializeField]
    public CardreaderColor keycardColor = CardreaderColor.Disabled;
    SpriteRenderer ColorIndicator = null;

    bool closed = true;

    void Start()
    {
        foreach(Component comp in GetComponentsInChildren<SpriteRenderer>())
        {
            if (comp.name == "Color Indicator")
            {
                ColorIndicator = (SpriteRenderer)comp;
            }
        }
        UpdateColor();
    }

    public bool Open()
    {
        closed = false;
        GetComponent<Animator>().SetBool("OpenVault", true);

		LevelManager.emit("vault_open");
        // door.Close();
		
		ColorIndicator.gameObject.SetActive(false);

        StartCoroutine(WaitForAnimationEndTimer());
        return true;
    }

    IEnumerator WaitForAnimationEndTimer()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public bool IsOpen()
    {
        return !closed;
    }

    public bool IsClosed()
    {
        return closed;
    }

    private void UpdateColor()
    {
        // Set color indicator
        switch (keycardColor)
        {
            case CardreaderColor.Blue:
               	ColorIndicator.color = ColorZughy.cyan;
                break;
            case CardreaderColor.Purple:
	            ColorIndicator.color = ColorZughy.purple;
                break;
            case CardreaderColor.Yellow:
	            ColorIndicator.color = ColorZughy.yellow;
                break;
            default:
        		ColorIndicator.color = ColorZughy.grey;
                break;
        }
    }
}