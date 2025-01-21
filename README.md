# Survive_IF_You_CAN

It is a small 2D top down RPG based survival game , in which the player has to survive as long as possible from the monsters continuously spawning in the scene .

## Assets 
### Audio 
Contains all the music and sfx audios used in the game .

### Scenes
Contains all the scenes used in the game like main menu and the main game .

### Animations 
Contains all the animations that have been applied on the gameobjects in this project .

### Prefabs 
Contains all the prefabs of the gameobjects .

### scriptable_items
Contains all the scriptable objects used .

### sprites
Contains all the sprites used in the game .

### scripts
Contains all the scripts . Under this folder ,

MainMenu.cs -> contains all the functions necessary for changing the scenes or UI scenes , quitting game .

MenuAudio.cs -> script for playing games on the main menu script .

scoreManager.cs -> for managing the score on the screen and on the game over screen .

AudioManager.cs -> for managing audio in the game scene , between main game , pause screen or game over screen .

#### enemy folder
All the folders named goblin , minatour , mooshroom , one_eye , skeleton have the same script just the stats of the different enemy is different . These scripts contains various functions for controlling the functions of the enemy like roaming randomly , following and attacking player , droping loot , etc . It also contains the items that the enemy can spawn , you can assign the probability of the drop .

EnemyBase.cs -> common scripts for all the enemies .

EnemySpawner.cs -> script for spawning enemies randomly on the specified area by choosing a suitable spot on that area repeatedly after every randomly interval of time .

loot.cs -> a script for defining loot items .

lootbag.cs -> script for managing the loot that the enemy would drop after it gets defeated .

#### main_player folder
player_movement.cs -> script handling how the player would move and also how to position aim .

health_sys.cs -> for handling the health system of the player .

stamina_sys.cs -> for handling the stamina system of the player .

hunger_sys.cs -> for handling the hunger of the player .

camera_follow.cs -> for moving camera along with the player .

arrow.cs -> for handling the arrow object on the screen .

StatsImprovement.cs -> for handling the upgrade stat screen like check whether elements required are present or not and showing the list of items required for upgrading .

StatsImprovementUI.cs -> for handling the upgrade screen UI .

player.cs -> main script for player handling and integrating all the necessary features for interaction with the other game objects with the players like when to walk , run , attack , use items from inventory , take damage , pause/resume the game , etc .

#### inventory folder
inventory_item.cs -> attributes to the inventory item . 

inventory_player.cs -> handling a single slot of the inventory and have all the functions required for modifying it .

inventory_holder.cs -> handles inventory of the player . 
