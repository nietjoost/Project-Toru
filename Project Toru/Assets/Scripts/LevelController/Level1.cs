using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : LevelScript
{	
	public Van van = null;
	public Room VaultRoom = null;
	public Character player = null;
	public Karen karen = null;
	public Employee employeeDownstairs = null;
	public Guard guardSecurityRoom = null;
	
	protected override void Awake() {
		
		base.Awake();
			
		{
			LevelCondition condition = new LevelCondition();
			condition.name = "DriveVan";
			
			condition.fullfillHandler = (LevelCondition c) => {
				if (c.failed) 
					return;
				
				van?.Drive();
			};
			
			LevelManager.AddCondition(condition);
		}
		
		{
			LevelCondition condition = new LevelCondition();
			condition.name = "PlayerFoundKey";
			
			condition.fullfillHandler = (LevelCondition c) => {
				{
					DialogueText text = new DialogueText();
					text.name = "You found a key";
					text.sentences.Add("I am sure this will open many doors");
					
					dialogueManager.QueueDialogue(text);
				}
				
				{	
					karen?.Say("The brutality!");
				}
			};
			
			LevelManager.AddCondition(condition);
		}
		
		{
			LevelCondition condition = new LevelCondition();
			condition.name = "GuardsAlerted";
			
			condition.fullfillHandler = (LevelCondition c) => {
				
				VaultRoom.door.Close();
				
				LevelManager.Delay(Random.Range(10, 20), () => {
					SpawnPoliceCar();
				});
			};
			
			LevelManager.AddCondition(condition);
		}
		
		{
			LevelCondition condition = new LevelCondition();
			condition.name = "CopsTriggered";
			condition.fullfillHandler = (LevelCondition c) => {
				PoliceSiren.startSiren = true;
				PoliceSiren.gameObject.SetActive(true);

				LevelManager.Delay(10, () => {
					SpawnPoliceCar();
				});
			};

			LevelManager.AddCondition(condition);
		}
		
		{
			LevelCondition condition = new LevelCondition();
			condition.name = "CameraDetectedPlayer";
			
			condition.fullfillHandler = (LevelCondition c) => {
				
				foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Room"))
				{
					if (obj.name.Equals("Security Room"))
					{
						obj.GetComponent<CameraRoom>()?.AlertGuard();
					}
				}
				
				if (!LevelManager.Condition("GuardsAlerted").fullfilled) {
					LevelManager.Condition("CameraWasDisabled").Fullfill();
				}
			};
			
			LevelManager.AddCondition(condition);
		}
		
		{
			LevelCondition condition = new LevelCondition();
			condition.name = "PlayerEntersCameraRoom";
			
			condition.fullfillHandler = (LevelCondition c) => {
				
				if (LevelManager.Condition("CamerasDisabled").fullfilled) {
					
					player.Say("That camera looks disabled");

				} else {
					player?.gameObject.GetComponent<ExecutePathFinding>()?.StopPathFinding();
					
					DialogueText text = new DialogueText();
					text.name = "Watch out!";
					text.sentences.Add("There is a camera in this room!");
					
					dialogueManager.QueueDialogue(text);
				}
				
			};
			
			LevelManager.AddCondition(condition);
		}
		
		{
			LevelCondition condition = new LevelCondition();
			condition.name = "PlayerTriedOpeningDoorButWasLocked";
			
			condition.fullfillHandler = (LevelCondition c) => {
				DialogueText text = new DialogueText();
				text.name = "Door is locked";
				text.sentences.Add("I don't have the correct key");
				text.sentences.Add("I got stuck here");
				
				text.callback = () => {
					LevelEndMessage.title = "You got stuck in the vault room";
					LevelEndMessage.message = "You didn't find all the necessaru pieces of the puzzle.";
					LevelEndMessage.nextLevel = "Level 1";
					LevelEndMessage.LevelSuccessfull = false;
					LevelManager.EndLevel(1);
				};
				
				dialogueManager.QueueDialogue(text);
			};
			
			LevelManager.AddCondition(condition);
		}
		
		{
			LevelCondition condition = new LevelCondition();
			condition.name = "SomeoneHeardShooting";
			
			condition.fullfillHandler = (LevelCondition c) => {
				LevelManager.Delay(Random.Range(10, 20), () => {
					SpawnPoliceCar();
				});
			};
			
			LevelManager.AddCondition(condition);
		}
		
		{
			LevelCondition condition = new LevelCondition();
			condition.name = "CharacterGotMoneyFromVault";
			LevelManager.AddCondition(condition);
		}
		
		{
			LevelCondition condition = new LevelCondition();
			condition.name = "LetKarenTalk";
			
			condition.fullfillHandler = (LevelCondition c) => {
				
				LevelManager.Delay(1, () => {
					karen?.Say("Welcome tot the Bank of Clyde");
				});
				
			};
			
			LevelManager.AddCondition(condition);
		}
		
		{
			LevelCondition condition = new LevelCondition();
			condition.name = "KarenSurrendered";
			
			condition.fullfillHandler = (LevelCondition c) => {				
				karen?.Say("Don't point that on me!!");
			};
			
			LevelManager.AddCondition(condition);
		}
		
		{
			LevelCondition condition = new LevelCondition();
			condition.name = "CamerasDisabled";
			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "PlayerInRoomWithEmployeeAndMoney";

			condition.fullfillHandler = (LevelCondition c) => {
				if (!LevelManager.Condition("EmployeeKilled").fullfilled) {
					employeeDownstairs?.Say("What are you doing?");
				}
			};

			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "PlayerInRoomWithKarenAndMoney";

			condition.fullfillHandler = (LevelCondition c) => {
				if (!LevelManager.Condition("KarenKilled").fullfilled) {
					karen?.Say("Now you have money, wanna date?");
				}
			};

			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "EmployeeKilled";

			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "KarenKilled";

			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "CharacterInRoomWithGuard";

			condition.fullfillHandler = (LevelCondition c) => {
				guardSecurityRoom.Say("What are you doing?");
				guardSecurityRoom.ShootAt(player);
			};

			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "ArchitectKilled";

			condition.fullfillHandler = (LevelCondition c) => {
				DialogueText text = new DialogueText();
				text.name = "O no";
				text.sentences.Add("You got killed");
				
				text.callback = () => {
					LevelEndMessage.title = "You got killed";
					LevelEndMessage.message = "Luck doesn't seem to be on your side.";
					LevelEndMessage.nextLevel = "Level 1";
					LevelEndMessage.LevelSuccessfull = false;
					LevelManager.EndLevel(1);
				};

				dialogueManager.QueueDialogue(text);
			};

			LevelManager.AddCondition(condition);
		}

		
		LevelManager.on("CameraDetectedPlayer", (GameObject gameObject) => {
			if (!LevelManager.Condition("CamerasDisabled").fullfilled)
				LevelManager.Condition("CameraDetectedPlayer").Fullfill();
		});

		LevelManager.on("CamerasDisabled", () => {
			LevelManager.Condition("CamerasDisabled").Fullfill();
		});
		
		LevelManager.on("GuardsAlerted", (GameObject room) => {
			LevelManager.Condition("GuardsAlerted").Fullfill();
			PoliceForce.getInstance().Alert(room.GetComponent<Room>());
		});
		
		LevelManager.on("PlayerFoundKey", () => {
			LevelManager.Condition("PlayerFoundKey").Fullfill();
		});
		
		LevelManager.on("CharacterIsInRoom", (GameObject gameObject) => {

			Character character = gameObject.GetComponent<Character>();
			string roomname = character.currentRoom.name;

			if (roomname == "L0 Room L") {
				LevelManager.Condition("LetKarenTalk").Fullfill();
				if (player.inventory.getMoney() != 0) {
					LevelManager.Condition("PlayerInRoomWithKarenAndMoney").Fullfill();
				}
			}
			else if (roomname == "L1 Room") {
				LevelManager.Condition("PlayerEntersCameraRoom").Fullfill();
			} else if (roomname == "L0 Room R"  && player.inventory.getMoney() != 0) {
				LevelManager.Condition("PlayerInRoomWithEmployeeAndMoney").Fullfill();
			} else if (roomname == "Security Room") {
				LevelManager.emit("CharacterInRoomWithGuard", character.currentRoom.gameObject);
			}
		});

		{}
		LevelManager.on("CharacterInRoomWithGuard", (GameObject room) => {
			LevelManager.Condition("CharacterInRoomWithGuard").Fullfill();
			PoliceForce.getInstance().Alert(room.GetComponent<Room>());
		});
		
		LevelManager.on("Surrendered", (GameObject gameObject) => {
			if (gameObject.name == "Karen")
				LevelManager.Condition("KarenSurrendered").Fullfill();
		});
		
		LevelManager.on("PlayerTriedOpeningDoorButWasLocked", (string roomname) => {
			if (roomname == "L1 Room") {
				LevelManager.Condition("PlayerTriedOpeningDoorButWasLocked").Fullfill();
			}
		});

		LevelManager.on("NPCKilled", (gObject) => {
			PoliceForce.getInstance().AlertKill();
			PoliceForce.getInstance().Alert(gObject.GetComponent<Room>());
		});

		LevelManager.on("CopsTriggered", () => {
			LevelManager.Condition("CopsTriggered").Fullfill();
		});

		LevelManager.on("EmployeeFleed", () => {
			LevelManager.Delay(Random.Range(10, 20), () => {
				SpawnPoliceCar();
				
				if (LevelManager.RandomChange(70)) {
					LevelManager.Delay(1, () => {
						DialogueText text = new DialogueText();
						text.name = "I hear the police";
						text.sentences.Add("That bloody employee called the cops, I am sure!");
						
						dialogueManager.QueueDialogue(text);
					});
				}
			});
		});
		
		LevelManager.on("PlayerHasUsedGun", (GameObject room) => {
			if (LevelManager.RandomChange(10)) {
				SpawnPoliceCar();
				PoliceForce.getInstance().Alert(room.GetComponent<Room>());
				//LevelManager.Condition("SomeoneHeardShooting").Fullfill();
			}
		});
		
		LevelManager.on("CharacterGotMoneyFromVault", () => {
			LevelManager.Condition("CharacterGotMoneyFromVault").Fullfill();
		});

		LevelManager.on("EmployeeKilled", (GameObject room) => {
			LevelManager.Condition("EmployeeKilled").Fullfill();
			PoliceForce.getInstance().AlertKill();
			PoliceForce.getInstance().Alert(room.GetComponent<Room>());
		});

		LevelManager.on("Killed", (GameObject gameobject) => {
			string name = gameobject.name;

			if (name == "Karen") {
				LevelManager.Condition("KarenKilled").Fullfill();
			} else if (name == "Employee 1") {
				LevelManager.Condition("EmployeeKilled").Fullfill();
			} else if (name == "Architect") {
				LevelManager.Condition("ArchitectKilled").Fullfill();
			}
		});


		LevelManager.on("CharacterEntersVan", (GameObject character) => {
			
			LevelManager.Condition("DriveVan").Fullfill();
			
			if (LevelManager.Condition("CharacterGotMoneyFromVault").fullfilled) {
				LevelEndMessage.title = "Good job!";
				LevelEndMessage.message = "You stole some money and didn't get caught!";
				LevelEndMessage.nextLevel = "Level 2";
				LevelEndMessage.LevelSuccessfull = true;
				LevelManager.EndLevel(2);
				return;
			}
			else if (LevelManager.Condition("CopsTriggered").fullfilled) {
				LevelEndMessage.title = "You got away!";
				LevelEndMessage.message = "Sadly you could not get away with money...";
				LevelEndMessage.nextLevel = "Level 1";
				LevelEndMessage.LevelSuccessfull = false;
				LevelManager.EndLevel(2);
			}
			else
			{
				LevelEndMessage.title = "You got away!";
				LevelEndMessage.message = "But the idea is that you try to steal some money...";
				LevelEndMessage.nextLevel = "Level 1";
				LevelEndMessage.LevelSuccessfull = false;
				LevelManager.EndLevel(1);
			}
		});

		LevelManager.on("StartLevel", () => {
			LevelEndMessage.lastLevel = 1;
			karen.animator.SetFloat("moveX", -1);
			employeeDownstairs.PingPong();
			guardSecurityRoom.PingPong();
			karen.fleeIfPossible = true;
		});
		
	}
}
