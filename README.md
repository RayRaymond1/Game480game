# The Way of Words - Development Documentation

## How to play the current version (Draft 3)

1. Open the game file in Unity
2. Open the scenes folder
3. Click on Dojo Scene 1 for the cutscene or Dojo Scene 2 for the gameplay
4. Click the play button

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

To be added

### 4-12-24

To be added

### All game assets used as of now have been imported from other sources. None of them were made by us.
