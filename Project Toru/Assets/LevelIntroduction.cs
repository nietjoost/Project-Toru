using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIntroduction : MonoBehaviour
{

	public SpriteRenderer backgroundBlack;
	public SpriteRenderer background;

	public TextMesh textMesh;
	public TextMesh textMeshControls;

	public Transform cameraCenter;
	public int currentLevel;

	public List<string> text = new List<string>();

	public State state = State.Wait;

	float WaitTimer = 1;
	int currentLine = 0;

	public bool transistionToScene = false;

    // Start is called before the first frame update
    void Start()
    {
		Init();
    }

	public void Init() {
		// No text means no usage
		if (text.Count == 0 || LevelEndMessage.lastLevel == currentLevel) {
			LevelManager.emit("StartLevel");
			gameObject.SetActive(false);
			return;
		} else {
			gameObject.SetActive(true);
		}

		// Set background visable
		backgroundBlack.gameObject.SetActive(true);
		background.gameObject.SetActive(true);

		// Set UI to correct layer
		textMesh.GetComponent<Renderer>().sortingLayerName = "UI";
		textMeshControls.GetComponent<Renderer>().sortingLayerName = "UI";

		// Disable UI (will overlab otherwise)
		LevelManager.GetUI()?.SetActive(false);

		// Set Camera
		Camera.main.GetComponent<CameraBehaviour>().zoomDistance = 6;
		Camera.main.GetComponent<CameraBehaviour>().target = cameraCenter;
		Camera.main.GetComponent<CameraBehaviour>().smoothing = 1;
		Camera.main.GetComponent<CameraBehaviour>().movementDisabled = true;

		currentLine = 0;
	}

	public enum State {
		None,
		Start,
		BlackFadeIn,
		BackgroundFadeOut,
		FadeInText,
		SelectNextLine,
		ExtremeText,
		FadeOutText,
		Wait,
		FadeOutIntroduction,
		WaitForInput,
		FadeInControls
		
	};

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Equals))
        {
            state = State.FadeOutIntroduction;
			{
				Color color = textMesh.color;
				color.a = 0;
				textMesh.color = color;
			}

			{
				Color color = textMeshControls.color;
				color.a = 0;
				textMeshControls.color = color;
			}

			{
				Color color = backgroundBlack.color;
				color.a = 0;
				backgroundBlack.color = color;
			}
        }

        switch(state) {

			case State.Start:
				state = State.BlackFadeIn;
			break;

			case State.Wait:
				{
					WaitTimer -= Time.deltaTime;
					
					if (WaitTimer <= 0) {
						WaitTimer = 1;
						state = State.BackgroundFadeOut;
					}
				}
			break;

			case State.BlackFadeIn: 
			{
				Color color = backgroundBlack.color;
				color.a += 0.5f * Time.deltaTime;
				backgroundBlack.color = color;
					
				if (color.a >= 1) state = State.SelectNextLine;
			}
			break;

			case State.BackgroundFadeOut:
				{
					Color color = background.color;
					color.a -= 0.5f * Time.deltaTime;
					background.color = color;
					
					if (color.a <= 0) state = State.SelectNextLine;
				}
			break;

			case State.FadeInText:
				{
					Color color = textMesh.color;
					color.a += 1.5f * Time.deltaTime;
					textMesh.color = color;
					
					if (color.a >= 1) {
						if (currentLine == 1) {
							state = State.FadeInControls;
						}
						else {
							state = State.WaitForInput;
						}
					}
				}
			break;

			case State.FadeInControls:
				{
					Color color = textMeshControls.color;
					color.a += 1f * Time.deltaTime;
					textMeshControls.color = color;
					
					if (color.a >= 1) state = State.WaitForInput;
				}
			break;

			case State.SelectNextLine:
				{
					SelectNextLine();
				}
			break;

			case State.FadeOutText:
				{
					Color color = textMesh.color;
					color.a -= 1.5f * Time.deltaTime;
					textMesh.color = color;
					
					if (color.a <= 0) {
						if (textMesh.fontSize > 80) {
							textMesh.fontSize = 80;
							textMesh.transform.Rotate(new Vector3(0, 0, -10));
						}
						state = State.SelectNextLine;
					}
				}
			break;

			case State.WaitForInput:
				{
					if (Input.GetKeyDown(KeyCode.Space)) {
						if (currentLine < text.Count) {
							if (text[currentLine].StartsWith("!")) {
								state = State.ExtremeText;
								break;
							}
						}

						state = State.FadeOutText;
					}
				}
			break;

			case State.ExtremeText:
				SelectNextLine();
				textMesh.fontSize = 120;
				textMesh.transform.Rotate(new Vector3(0, 0, 10));
			break;

			case State.FadeOutIntroduction: {

				if (transistionToScene) {
					LevelManager.EndLevel(0);
				}

				{
					Color color = textMesh.color;
					color.a -= 0.5f * Time.deltaTime;
					textMesh.color = color;
				}

				{
					Color color = textMeshControls.color;
					color.a -= 1f * Time.deltaTime;
					textMeshControls.color = color;
				}

				{
					Color color = backgroundBlack.color;
					color.a -= 0.3f * Time.deltaTime;
					backgroundBlack.color = color;
					
					if (color.a <= 0) {
						LevelManager.GetUI()?.SetActive(true);
						Camera.main.GetComponent<CameraBehaviour>().movementDisabled = false;
						LevelManager.emit("StartLevel");
						gameObject.SetActive(false);
					}
				}
			}
			break;
			
			default:
				break;
		}
    }

	void SelectNextLine() {
		if (currentLine >= text.Count) {
			LevelManager.GetUI()?.SetActive(true);
			state = State.FadeOutIntroduction;
			return;
		}

		string textString = text[currentLine];
		if (textString.StartsWith("!")) {
			textString = textString.Substring(1);
		}

		textMesh.text = textString;
		currentLine++;

		state = State.FadeInText;
	}
}
