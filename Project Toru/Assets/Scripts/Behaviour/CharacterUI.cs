using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public GameObject prefab;
    public GameObject CharacterSelectionBox;

    void Start()
    {
        int loop = 0;

		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if(players.Length == 1)
        {
            CharacterSelectionBox.SetActive(false);
		} 
        else
        {
            foreach (GameObject obj in players)
            {
                GameObject spawn = Instantiate(prefab);
                spawn.transform.SetParent(transform.GetChild(0));

                Sprite s = obj.GetComponent<SpriteRenderer>().sprite;
                transform.GetChild(0).GetChild(loop).GetComponent<CharacterSlot>().AddCharacter(obj.GetComponent<Character>());
                transform.GetChild(0).GetChild(loop).GetComponent<CharacterSlot>().setSprite(s);
				Tint.Transparent(transform.GetChild(0).GetChild(loop).GetComponent<CharacterSlot>().GetComponent<Image>());

				loop++;

				obj.transform.Find("SelectedTriangle").gameObject.GetComponent<SpriteRenderer>().enabled = false;
			}
        }
		transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100 , players.Length * 115);
    }
}
