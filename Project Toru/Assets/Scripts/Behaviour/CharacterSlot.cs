using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    public Sprite icon;
    public Character character;
	public bool selected;

	private void Awake()
	{
		selected = false;
	}

	private void Update()
	{
		checkSelected();
	}

	private void checkSelected()
	{
		if(character.Equals(Character.selectedCharacter) && !selected)
		{
			Tint.Reset(GetComponent<Image>());
			selected = true;
		}
		if (!character.Equals(Character.selectedCharacter) && selected)
		{
			Tint.Transparent(GetComponent<Image>());
			selected = false;
		}
	}

	public void AddCharacter(Character newCharacter)
    {
        character = newCharacter;
    }

    public void setSprite(Sprite s)
    {
        this.icon = s;
        transform.GetComponent<Image>().sprite = this.icon;
        transform.GetComponent<Mask>().showMaskGraphic = true;
        transform.GetChild(0).GetComponent<Button>().interactable = true;

    }

    public void SelectCharacter()
    {
		if(Character.selectedCharacter != null)
		{
			Character.selectedCharacter.transform.Find("SelectedTriangle").gameObject.GetComponent<SpriteRenderer>().enabled = false;
		}
		Character.selectedCharacter = character;
        Camera.main.GetComponent<CameraBehaviour>().target = Character.selectedCharacter.transform;
		Character.selectedCharacter.transform.Find("SelectedTriangle").gameObject.GetComponent<SpriteRenderer>().enabled = true;
		Character.selectedCharacter.inventory.UpdateUI();
	}
}