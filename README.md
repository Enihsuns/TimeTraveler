# TimeTraveler
A video game developed on Unity3D, allowing players running on an ancient Chinese street

## Introduction
Time Traveler is a 3D running game developed by Unity. Players will become time travelers of ancient China. This game allows users to interact with Kinect. Users need to run in front of Kinect to run virtual roles. The game also allows the user to control the speed of the run.

- **Kinect Control** 
Kinect control section is the most distinctive feature of the game. The characters in the game will have the same action as real-life users. If the user runs faster, the character will also speed up. The speed control portion is implemented by calculating the distance between the left knee and the right knee and the time took by the user to switch between the left and right legs. The greater the distance between the knees, the shorter the switching time and the faster the character. In addition, Kinect enables players to make different poses in the game.

- **Game Model** 
There are totally 3 kinds of roads and they will be randomly generated when the player is running. The people and obstacles on the road are also randomly generated. In this way, the game tries to create a changing world by utilizing the same resources.

- **Weather and Time** 
There are 6 skyboxes in the game, including ‘Morning’, ‘Day’, ‘Dusk’, ‘Night’, ‘Rain’ and ‘Snow’. When the skybox is ‘Rain’ or ‘Snow’, particle systems will appear to imitate rain or snow.

- **Background Music and Sound Effects** 
The sound effects of people on the road have 3D surround effect. The background music will be changed according to current skybox.

## Description of the source code
The source code is in the folder ‘Asset/Scripts’. Some parts of the code are not written by me, basically the Kinect part. I have applied a Kinect controller package from Unity store, which is not uploaded here. 
The code I have written include:
- GameManager: Define the game state. Act like a state machine. 
- MoveController: It acts as a component of the character. It controls the transform of character, the transform of camera and other things related to the collider of the character. 
- MovingNPCController: Prevent the NPCs on the road to be blocked with something. 
- MyGestureListener: Listening to the ‘MyRun’ gesture from Kinect. It should be used together with the code in ‘Asset/Scripts/KinectScripts’.
- ObstacleInitializer: A component of road. When the road is newly generated, this code will help to generate obstacles and people on it. Also, it helps the people to make sound effects by using coroutine.
- Code in ‘Asset/Scripts/GameStateScripts’: Including game state controllers. Each controller is a component of the gameobject GameManager. They are inactive at the beginning of the game, and GameManager will decide which should be active.
