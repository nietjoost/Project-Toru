using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : LevelScript
{	
	
	public Character architect;
	public Muscle muscle;

	public Karen karen;
	public Guard guardDownStairs;
	public Guard guardUpstairs;

	public Employee employeeDownstairsLeft;
	public Employee employeeDownstairsRight;
	public Employee employeeUpstairs;

	public FatGuy fatGuy;

	public Door DownStairsDoor;	
	public Van van;
	public Desk deskUpstairs;

	public LevelIntroduction introduction;

	protected override void Awake() {
		
		base.Awake();

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "ArchitectInRoom";
			
			condition.fullfillHandler = (LevelCondition c) => {
				if (LevelManager.Condition("MuscleInRoom").fullfilled) return;

				karen.Say("You again!");
			};
			
			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "MuscleInRoom";
			
			condition.fullfillHandler = (LevelCondition c) => {
				if (LevelManager.Condition("ArchitectInRoom").fullfilled) return;

				karen.Say("Hi Handsome");
				LevelManager.Delay(2, () => {
					if (LevelManager.Condition("PlayerFoundKey").fullfilled) return;

					karen.Say("How can I help you?");
				});
			};
			
			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "PlayerFoundKey";
			
			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "KarenFleed";
			
			condition.fullfillHandler = (LevelCondition c) => {
				karen.pathfinder.setPosTarget(9, 1);
				LevelManager.Delay(0.5f, () => {
					
					karen.currentRoom.door.Open();

					Door door = karen.currentRoom.door;

					if (LevelManager.Condition("PlayerFoundKey").fullfilled) {
						LevelManager.Delay(1, () => {
							door.Close();
						});
					}
				
					karen.pathfinder.setPosTarget(17.3f, 1);
					LevelManager.Delay(1, () => {
						DownStairsDoor.Open();

						LevelManager.Delay(1, () => {
							karen.pathfinder.setPosTarget(21, 5);

							LevelManager.Delay(1, () => {
								DownStairsDoor.Close();

								LevelManager.Delay(3, () => {
									karen.transform.position = new Vector3(30, 5.2f);
									karen.stats.maxHealth = 100;
									karen.animator.SetBool("Surrendering", false);
									karen.animator.SetFloat("moveX", 1);
								});
							});
						});
					});
				});
			};
			
			LevelManager.AddCondition(condition);
		}
		
		// LevelManager.on("CharacterIsInRoom", (string roomname) => {
		// 	if (roomname == "L0 Room L") {
		// 		if (architect.currentRoom != null) {
		// 			LevelManager.Condition("ArchitectInRoom").Fullfill();
		// 		}
		// 		else
		// 		{
		// 			LevelManager.Condition("MuscleInRoom").Fullfill();
		// 		}
		// 	}
		// });

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "EmployeesTalk";
			
			condition.fullfillHandler = (LevelCondition c) => {
				if (LevelManager.Condition("EmployeesSurrender").fullfilled) return;

				DialogueText text = new DialogueText();
				text.name = "Employee:";
				text.sentences.Add("Are you our new colleague?");
				text.sentences.Add("You must be from the other office");
				text.sentences.Add("Here, have the key to your office");
				
				text.callback = () => {
					employeeDownstairsRight.Say("Hello!");
				};
				
				dialogueManager.QueueDialogue(text);

				employeeDownstairsLeft.dropBag();
			};
			
			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "EmployeesSurrender";
			
			condition.fullfillHandler = (LevelCondition c) => {
				employeeDownstairsLeft.Surrender();
				employeeDownstairsRight.Surrender();
			};
			
			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "KarenTalksToTheManager";
			
			condition.fullfillHandler = (LevelCondition c) => {

				karen.animator.SetFloat("moveX", 1);

				DialogueText text = new DialogueText();
				text.name = "Karen:";
				text.sentences.Add("You are my manager!!!!!!");
				text.sentences.Add("Why are you so stupid??");
				text.sentences.Add("It was a guy with a gun! The same one as last year!");
				text.sentences.Add("It was...");

				dialogueManager.QueueDialogue(text);
				
				text.callback = () => {
					karen.animator.SetFloat("moveX", -1);
					karen.Say("YOU");
					karen.BeKaren(architect);
					LevelManager.Delay(0.5f, () => {
						employeeUpstairs.Say("WHOA! Easy!");
					});
				};
			};
			
			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "KarenTalksToTheManagerMuscle";
			
			condition.fullfillHandler = (LevelCondition c) => {

				karen.animator.SetFloat("moveX", 1);
			};
			
			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "FatGuyRunsAway";
			
			condition.fullfillHandler = (LevelCondition c) => {
				fatGuy.Say("Whaaaa i can't deal with people!");
				fatGuy.RunAway();
				
			};
			
			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "GuardUpstairsArrest";
			
			LevelManager.AddCondition(condition);
		}

		LevelManager.on("CharacterIsInRoom", (GameObject gameObject) => {

			Character character = gameObject.GetComponent<Character>();

			if (character.currentRoom.name == "L0 Room L") {
				if (gameObject == muscle.gameObject)
				{
					LevelManager.Condition("MuscleInRoom").Fullfill();
				}
				else
				{
					LevelManager.Condition("ArchitectInRoom").Fullfill();
				}
			}
			else if (character.currentRoom.name == "L0 Room M") {
				if (character.weapon.weaponOut) {
					LevelManager.Condition("EmployeesSurrender").Fullfill();
				} else {
					LevelManager.Delay(0.5f, () => {
						LevelManager.Condition("EmployeesTalk").Fullfill();
					});
				}
			}
			else if (character.currentRoom.name == "L1 Room L") {
				guardUpstairs.Say("What are you doing here?");
				if (character.weapon.weaponOut) {
					guardUpstairs.ShootAt(character);
				} else {
					if (!LevelManager.Condition("GuardUpstairsArrest").fullfilled) {
						LevelManager.Condition("GuardUpstairsArrest").Fullfill();

						guardUpstairs.Arrest(character);
					} else {
						guardUpstairs.ShootAt(character);
					}
					
				}
				PoliceForce.getInstance().Alert(character.currentRoom);
			}
			else if (character.currentRoom.name == "L1 Room R") {
				
				if (gameObject == muscle.gameObject)
				{	
					LevelManager.Delay(0.5f, () => {
						LevelManager.Condition("KarenTalksToTheManagerMuscle").Fullfill();
					});
				}
				else
				{
					LevelManager.Delay(0.5f, () => {
						LevelManager.Condition("KarenTalksToTheManager").Fullfill();
					});	
				}

				
				
			}
			else if (character.currentRoom.name == "L2 Room R") {
				LevelManager.Delay(0.5f, () => {
					LevelManager.Condition("FatGuyRunsAway").Fullfill();
				});

				if (character.name == "Architect") {
					LevelManager.Condition("ArchitectInGameRoom").Fullfill();
				}
			}
		});

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "ArchitectInGameRoom";
			
			condition.fullfillHandler = (LevelCondition c) => {


			};
			
			LevelManager.AddCondition(condition);
		}

		LevelManager.on("IsHoldingGun", (GameObject gameObject) => {
			
			Character character = gameObject.GetComponent<Character>();
			if (character == null) {
				return;
			}

			if (character.currentRoom == null) {
				return;
			}
			
			Guard guard = character.currentRoom.GetComponent<Room>()?.GetGuardFromRoom();
			if (guard == null) {
				return;
			};

			
			// At this stage, character is holding gun while guard is in room
			LevelManager.Delay(0.5f, () => {

				if (guard.surrender) {
					return;
				}

				guard.ShootAt(character);
				guard.Say("HOLD YOUR GUN DOWN!");
				LevelManager.Condition("KarenFleed").Fullfill();
			});
		});

		LevelManager.on("PlayerHasUsedGun", (GameObject room) => {
			if (LevelManager.RandomChange(10))
			{
				SpawnPoliceCar();
				PoliceForce.getInstance().Alert(room.GetComponent<Room>());
				//LevelManager.Condition("SomeoneHeardShooting").Fullfill();
			}
		});

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
			condition.name = "MuscleFoundKey";
			
			condition.fullfillHandler = (LevelCondition c) => {
				DialogueText text = new DialogueText();
				text.name = "Found a key";
				text.sentences.Add("Muscle found key, muscle better give key to boss");
				
				dialogueManager.QueueDialogue(text);
			};
			
			LevelManager.AddCondition(condition);
		}

		LevelManager.on("NPCKilled", (gObject) => {
			PoliceForce.getInstance().AlertKill();
			PoliceForce.getInstance().Alert(gObject.GetComponent<Room>());
		});

		LevelManager.on("GuardsAlerted", (GameObject room) => {
			LevelManager.Condition("GuardsAlerted").Fullfill();
			PoliceForce.getInstance().Alert(room.GetComponent<Room>());
		});

		LevelManager.on("CharacterInRoomWithGuard", (GameObject room) => {
			LevelManager.Condition("CharacterInRoomWithGuard").Fullfill();
			PoliceForce.getInstance().Alert(room.GetComponent<Room>());
		});

		LevelManager.on("PlayerFoundKey", (GameObject gameObject) => {

			if (gameObject.name == "Muscle") {
				LevelManager.Condition("MuscleFoundKey").Fullfill();
				
				HashSet<Item> copy = new HashSet<Item>(muscle.inventory.getItemsList());

				foreach(var item in copy) {
					if (!item.name.Contains("Key")) continue;

					architect.inventory.addItem(item);
					muscle.inventory.removeItem(item);
				}
			}

			if (LevelManager.Condition("PlayerFoundKey").fullfilled) return;

			LevelManager.Condition("PlayerFoundKey").Fullfill();
			Character character = gameObject.GetComponent<Character>();

			if (character.currentRoom.name == "L0 Room L") {
				Guard guard = character.currentRoom.GetGuardFromRoom();
				if (guard == null) {
					return;
				};

				guard.Arrest(character);

				if (!LevelManager.Condition("KarenFleed").fullfilled) {
					LevelManager.Condition("KarenFleed").Fullfill();
					karen.Say("GUARD! HELP!");
				}
			}

			else if (character.currentRoom.name == "L1 Room R") {
				employeeUpstairs.bag.Clear();
				// deskUpstairs.GetComponent<Event>().
				//@todo
			}
		});

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "GuardDownstairsSurrender";
			
			condition.fullfillHandler = (LevelCondition c) => {
				guardDownStairs.Say("You will regret this!");
				guardDownStairs.targetCharacter = Character.selectedCharacter;
			};
			
			LevelManager.AddCondition(condition);
		}

		LevelManager.on("Surrendered", (GameObject gameObject) => {

			if (gameObject.name == "Architect") {
				if (LevelManager.Condition("MuscleKilled").fullfilled || muscle.surrendering || LevelManager.Condition("MuscleInVan").fullfilled) {
					LevelEndMessage.title = "You got arrested";
					LevelEndMessage.message = "Nobody was there to help";
					LevelEndMessage.nextLevel = "Level 2";
					LevelEndMessage.LevelSuccessfull = false;
					LevelManager.EndLevel(3);
					return;
				}
				return;
			}

			NPC npc = gameObject.GetComponent<NPC>();
			if (npc == null) return;

			if (npc.currentRoom.name == "L0 Room L") {
				
				LevelManager.Condition("GuardDownstairsSurrender").Fullfill();
				LevelManager.Condition("KarenFleed").Fullfill();

				
			}

			else {
				foreach(var _npc in npc.currentRoom.npcsInRoom) {
					if (_npc == gameObject) continue;

					_npc.GetComponent<NPC>().Surrender();
				}
			}
		});

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "CameraDetectedPlayer";
			
			LevelManager.AddCondition(condition);
		}

		LevelManager.on("CameraDetectedPlayer", (GameObject room) => {
			if (LevelManager.Condition("CamerasDisabled").fullfilled) return;
			
			LevelManager.Condition("CameraDetectedPlayer").Fullfill();
			PoliceForce.getInstance().Alert(room.GetComponent<Room>());
		});

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "ArchitectKilled";
			
			condition.fullfillHandler = (LevelCondition c) => {
				LevelEndMessage.title = "You got killed";
				LevelEndMessage.message = "Luck doesn't seem to be on your side.";
				LevelEndMessage.nextLevel = "Level 2";
				LevelEndMessage.LevelSuccessfull = false;
				LevelManager.EndLevel(3);
			};
			
			LevelManager.AddCondition(condition);
		}

		LevelManager.on("CopsTriggered", () => {
			LevelManager.Condition("CopsTriggered").Fullfill();
		});

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "MuscleKilled";

			condition.fullfillHandler = (LevelCondition c) => {
				if (architect.surrendering) {
					LevelEndMessage.title = "You got arrested";
					LevelEndMessage.message = "Nobody was there to help";
					LevelEndMessage.nextLevel = "Level 2";
					LevelEndMessage.LevelSuccessfull = false;
					LevelManager.EndLevel(3);
				}
			};
			
			LevelManager.AddCondition(condition);
		}

		LevelManager.on("Killed", (GameObject gameObject) => {
			if (gameObject.name == "Muscle") {
				LevelManager.Condition("MuscleKilled").Fullfill();
			} else if (gameObject.name == "Architect") {
				LevelManager.Condition("ArchitectKilled").Fullfill();
			}
		});

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "PlayerFoundPacmanMachine";
			
			condition.fullfillHandler = (LevelCondition c) => {
				DialogueText text = new DialogueText();
				text.name = "The Pacman Machine";
				text.sentences.Add("Finally! After all those years");
				text.sentences.Add("Now quickly get out!");

				architect.speed = 5;
				
				dialogueManager.QueueDialogue(text);
			};
			
			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "MuscleFoundPacmanMachine";
			
			condition.fullfillHandler = (LevelCondition c) => {
				DialogueText text = new DialogueText();
				text.name = "The Pacman Machine!!";
				text.sentences.Add("Muscle better let boss take shiny machine");
				
				dialogueManager.QueueDialogue(text);
			};
			
			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "ArchitectInVan";
			
			condition.fullfillHandler = (LevelCondition c) => {
				if (LevelManager.Condition("PlayerFoundPacmanMachine").fullfilled) {
					if (LevelManager.Condition("MuscleInVan").fullfilled) {
						LevelManager.Condition("LevelEndSuccesfull").Fullfill();
						
						return;
					}

					if (muscle.surrendering) {
						LevelManager.Condition("LevelEndSuccesfull").Fullfill();

						return;
					}

					if (LevelManager.Condition("MuscleKilled").fullfilled) {
						LevelManager.Condition("LevelEndSuccesfull").Fullfill();

						return;
					}
				} else {
					LevelEndMessage.title = "You got away";
					LevelEndMessage.message = "But you can't leave the Pacman machine behind!";
					LevelEndMessage.nextLevel = "Level 2";
					LevelEndMessage.LevelSuccessfull = false;
					LevelManager.EndLevel(3);
					return;
				}
			};
			
			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "MuscleInVan";
			
			condition.fullfillHandler = (LevelCondition c) => {
				if (LevelManager.Condition("ArchitectInVan").fullfilled) {
					LevelManager.Condition("LevelEndSuccesfull").Fullfill();
					return;
				};

				if (architect.surrendering) {
					LevelEndMessage.title = "You deserted your boss!";
					LevelEndMessage.message = "You can't leave him behind!";
					LevelEndMessage.nextLevel = "Level 2";
					LevelEndMessage.LevelSuccessfull = false;
					LevelManager.EndLevel(3);
					return;
				}
			};
			
			LevelManager.AddCondition(condition);
		}

		{
			LevelCondition condition = new LevelCondition();
			condition.name = "LevelEndSuccesfull";
			
			condition.fullfillHandler = (LevelCondition c) => {
				van.Drive();

				LevelManager.Instance().webRequest.stopTime();

				LevelManager.Delay(2, () => {
					introduction.transistionToScene = true;
					introduction.gameObject.SetActive(true);
					introduction.text.Clear();
					introduction.text.Add("You did it!");
					introduction.text.Add("You have the holy grail!");
					introduction.text.Add("Great job!");
					introduction.currentLevel = -1;
					introduction.Init();

					LevelEndMessage.title = "You've won!";
					LevelEndMessage.message = "Great job!";
					LevelEndMessage.nextLevel = "SubmitScore";
					LevelEndMessage.LevelSuccessfull = true;

					Character.selectedCharacter = null;

					

					introduction.backgroundBlack.gameObject.SetActive(true);
					{
						Color color = introduction.backgroundBlack.color;
						color.a = 0;
						introduction.backgroundBlack.color = color;
					}
					introduction.state = LevelIntroduction.State.BlackFadeIn;

				});
				
			};
			
			LevelManager.AddCondition(condition);
		}	
		
		{
			LevelCondition condition = new LevelCondition();
			condition.name = "CamerasDisabled";
			
			LevelManager.AddCondition(condition);
		}

	
		LevelManager.on("CamerasDisabled", () => {
			LevelManager.Condition("CamerasDisabled").Fullfill();
		});

		LevelManager.on("CharacterEntersVan", (GameObject gameobject) => {

			if (gameobject.name == "Muscle") {
				LevelManager.Condition("MuscleInVan").Fullfill();
			} else if (gameobject.name == "Architect") {
				LevelManager.Condition("ArchitectInVan").Fullfill();
			}
			
		});


		LevelManager.on("PlayerFoundPacmanMachine", () => {
			LevelManager.Condition("PlayerFoundPacmanMachine").Fullfill();
		});

		LevelManager.on("MuscleFoundPacmanMachine", () => {
			LevelManager.Condition("MuscleFoundPacmanMachine").Fullfill();
		});

		LevelManager.on("StartLevel", () => {
			
			LevelEndMessage.lastLevel = 2;

			LevelManager.Delay(1, () => {
				DialogueText text = new DialogueText();
				text.name = "You have a companion!";
				text.sentences.Add("He will help you to keep the hostages under control");
				text.sentences.Add("Control him the same way you control the Architect");
					
				text.callback = () => {
					LevelManager.Delay(0.5f, () => {
						architect.Say("Where is that pacman machine...");
					});
				};

				dialogueManager.QueueDialogue(text);
			});

			employeeDownstairsLeft.animator.SetFloat("moveX", -1);
			employeeDownstairsRight.animator.SetFloat("moveX", -1);
			karen.animator.SetFloat("moveX", -1);
			employeeUpstairs.animator.SetFloat("moveX", -1);

		});
	}

	
}
