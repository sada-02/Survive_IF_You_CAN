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
Contains all the prefabs of the gameobjects that will also give the idea of how linking of different objects is done .

### scriptable_items
Contains all the scriptable objects used .

### sprites
Contains all the sprites used in the game .

### scripts
Contains all the scripts . Under this folder ,

MainMenu.cs -> contains all the functions necessary for changing the scenes or UI scenes , quitting game . It would be attached to MainMenu object under MenuUI , PauseMenu object under PauseUI , GameOverUI .

MenuAudio.cs -> script for playing games on the main menu script , attach the children object and the music in the refrences section . It would be attached to audioManagerMenu .

scoreManager.cs -> for managing the score on the screen and on the game over screen .

audioManager.cs -> for managing audio in the game scene , transitioning music between main game , pause screen or game over screen , attach the game over and pause menu UI Game Object , children object and music . It would be attached to audioManager .

#### enemy folder
All the folders named goblin , minatour , mooshroom , one_eye , skeleton have the same script just the stats of the different enemy is different . These scripts contains various functions for controlling the functions of the enemy like roaming randomly , following and attacking player , tracking the last seen position of the player , droping loot , a simple logic to prevent them from stucking to the same position . The scripts are attached to the respective objects .

EnemyBase.cs -> common scripts for all the enemies .

EnemySpawner.cs -> script for spawning enemies randomly on the specified area by choosing a suitable spot on that area repeatedly after every randomly interval of time . Attach to the spawner Game Objects .

loot.cs -> a script for defining loot items also assigning probability of each item dropping .

lootbag.cs -> script for managing the loot that the enemy would drop after it gets defeated . Attach it to the enemies Game Objects like goblin , minatour , etc .

#### main_player folder
player_movement.cs -> script handling how the player would move and also how to position aim .

health_sys.cs -> for handling the health system of the player . Attached to healthbar object under Bars GameObject .

stamina_sys.cs -> for handling the stamina system of the player . Attached to staminabar object under Bars GameObject .

hunger_sys.cs -> for handling the hunger of the player . Attached to hungerbar object under Bars GameObject .

camera_follow.cs -> for moving camera along with the player and its interactions with other game objects in the game . Link it to the camera and pass in the player refrence .

arrow.cs -> for handling the arrow object on the screen .

StatsImprovement.cs -> for handling the upgrade stat screen like check whether elements required are present or not and showing the list of items required for upgrading . Attach it to statsimprovement Game Object .

StatsImprovementUI.cs -> for handling the upgrade screen UI and integrating the interaction and display simultaneously . Attach to the statsimprovementUI Game Object .

player.cs -> main script for player handling and integrating all the necessary features for interaction with the other game objects with the players like when to walk , run , attack , use items from inventory , take damage , pause/resume the game , updating hunger and stamina . Attach to the player Game Object .

#### inventory folder
inventory_item.cs -> attributes to the inventory item . 

inventory_player.cs -> handling a single slot of the inventory and have all the functions required for modifying it like adding items if possible or removing the items or splitting or clearing it .

inventory_holder.cs -> handles inventory of the player and provides refrences to it . Attach it to the player .

inventory_sys.cs -> main inventory script handling the entire inventory system , handling multiple slots at once , checking whether we can add item or not , if possible where to add , removing items , checking for presence of item within the inventory .

Inventory_UI.cs -> handles the interaction of mouse slot with the inventory slot like on clicking an item on inventory would pick it up , once picked item would follow it or clear it once clicked outside the inventory UI . It would be attached to inventory Game Object and the mouseobject under it .

inventorySlots_UI.cs -> managing the UI of the items in inventory slots before and after their interactions .

item_pickup.cs -> script managing the functionality of picking an item present on the scene .

inventory_display.cs -> backbone for managing the interaction and display of inventory like picking up the item , place an item , split , swap or stack item .

staticInventoryDisplay.cs -> script to manage and display a static inventory linking a UI slots in the inventory system, ensuring the inventory is correctly displayed and updated in real-time as items are added, removed, or modified . Attach it to the hotbar under Inventory GameObject .
