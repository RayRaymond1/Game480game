# The Way of Words - Development Documentation

## How to play the current version (Draft 5)

1. Open the game file in Unity
2. Open the scenes folder
3. Click on one of the scenes containing levels

For Level 1: Click on Dojo_Scene(final, needs enemies)

For Level 2: City_level

For Level 3: Building_level0

4. Click the play button

How to complete the current build of Level 2:
1. In order to attack the boss you must defeat the rest of the enemies first. They each take 1 or 2 words to defeat.
2. Type the words that appear on the four drones, then type the word on the boss. Repeat this one more time to defeat the boss.

Note: The layout/gameplay of Level 2 is not final


## [Design Document](https://docs.google.com/document/d/1RnADUqa4XkYSha4MJzp1s47EtEJZHQJguFQMNVEDugk/edit?usp=sharing)

## [Development Plan](https://docs.google.com/document/d/1OyMZX1aCOAXCOVbCiCbSqsiujDuK_RaFvbFi-OqoGJM/edit?usp=sharing) (Updated 2-21-24)

## [Concept Document](https://docs.google.com/document/d/1MUrVNBQ8p_VT9coivICi4tCYFXty_MS96A12-YqLs7o/edit?usp=sharing)

## [Team Contract](https://docs.google.com/document/d/1iJdKovIKsm1fYGri0wpCYbsGHCb3OqX8M1YUjYPesc0/edit?usp=sharing) (Updated 2-21-24)

## Progress Reports

### 2-2-24

This week, we began building the game in Unity. We created a preliminary build of the game which is the bare bones of what the game will turn into. Currently, the only feature it has is the display of the current word and the ability to take in keyboard input to type the word. Behind the scenes, there are also action events that recognize when a correct or incorrect key is pressed and when a word is failed or completed. These will be used to build up the other components, such as animations, taking damage, and tracking level completion. We began working on the script for the game, which is a rough draft of the opening cutscene. We have a general idea of what will happen in the cutscene right after the first level as well. We also collected some publicly available assets to use in our game, including models for the environments, some weapons, and some visual effects. Finally, we launched the GitHub repository and added some of our files as assets.

### 2-23-24

Two new team members have joined since our last progress report, and we have made significant progress as a result. Our draft of the game’s script now covers the story up through the second level, and we have come up with new ideas for each character’s role in the story. We also now have an idea of what the setting of Level 2 is going to look like. We have implemented the basic version of an enemy including moving towards the player and resetting if the player fails to complete the word in time. While this may seem barebones, it is purposefully left that was to make creating different enemy types and testing easier. All other functions except for one are trivial to implement and only require adding one function to an in-game event. The next step is to finish implementing the targeting system so that the player can rotate between different enemies. 

Currently with Level 1, the map design and lighting are finished. There are certain surfaces that need a shader to help it to either reflect or look like water, but as of now we are currently creating cutscenes. The cutscenes at this stage are early block outs to show how the camera will be in each shot, and hopefully we can start adding voices and animations right after. For Level 2, we have drawn out some diagrams of how progression through the level might work. Finally, we are in the process of creating the game’s UI and UX, including the main menu, pause menu, game over screen, and options menu.

### 3-15-24

This week, we collected some music to potentially use in the game. We tried to find a good mix of cutscene and battle music, as well as music for both the past and future settings. We worked on the words for the word bank of level 3. We also started to try and get a layout of level 2 in unity. The introduction cutscene and the system to start it up and allow it to call events once it's finished is implemented in. Since that’s complete we can start moving into the assets for level 2 and start to replace the capsule placeholder model for enemies with actual models with animation and sfx. We also created the targeting system for the player. Now the player is able to switch between different enemies. Along with that, the player will now take damage when they fail to complete the word and the game will keep track of the player's score.

### 3-29-24

This week, we have just about completed level 1 and have begun building level 2. We worked on collecting assets for level 3 as well as assets for enemies. We also came up with a layout for level 3. In addition to this, the pause menu and dialogue for cutscenes and tutorials have been initiated. This is mostly from the UI perspective, however things are subject to change. The pause menu has three different buttons: resume, restart, and return. Both resume and restart are self-explanatory, however, return simply goes to the main menu. Dialogue boxes are another addition to the game, as they explain mechanics in the mechanics section, and often the story, users can click through them if they want to, however. The last edition is the health bar, simply put when the user gets attacked, the health bar will represent that by taking damage. After completely losing all your health, the user will be sent back to the game over screen, allowing them to restart the scene and try again if they would like.

Regarding the enemies, there has been a lot of work done. The entire code for them has been overhauled with the exception of the typing section. The movement has been converted from manual calculations to an AI, animations have been added, and code has been compartmentalized for easier future use. The enemy is now almost finished. There are still some bugs that need to be fixed, like how when there are multiple enemies the events are called for all of them instead of being called for one.

The script for Level 3’s opening cutscene has been reduced in length in order to more closely match the length of the previous cutscenes. It is still longer than we feel is necessary, so we may shorten it even more. We have also created a 50 second trailer for the game. It features gameplay and cutscene clips from the first level.

### 4-19-24

This week, we finalized the game’s script and the design document as a whole. We also chose what music would play during each level, cutscene, and menu. We added assets for level 3 such as rooms and objects in the level. We created a layout for all of the enemies for level 3 and moved the wire boss to level 3. We collected sound effects to implement into the game as it nears completion.

We created and finished the in-game enemy for typeFACE. It is going to be a larger enemy with 4 drones around it. The player defeats typeFACE by attacking each of the drones which then allows you to hit the actual boss. Repeat this process two more times and then you have defeated the boss. There were also other minor things that we worked on, such as fixing some UI bugs, commenting code, and fixing some enemy AI bugs. In addition to this the user interface has been updated with regards to music options. This music will now be represented throughout each scene and will be changed with the slider in options.

Cutscene 4 and the voices for every cutscene up until cutscene 4 were added. Some fixes were deployed to each cutscene to account for some voice lines length and some added assets as well. Progress on cutscene 5 and beyond is beginning now that we have the voice of the Rebel and the model for them.

### All game assets used as of now have been imported from other sources. None of them were made by us.
