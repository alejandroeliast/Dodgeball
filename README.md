# Dodgeball

### Description
A project aimed to create an arcade PvP game similar to the likes of Duck Game. Players will be able to move around an arena and grab dodgeballs that will be spread around the map. Using these, players will be able to shot aiming to hit the opposing players to score points.

### Player
The Player functionality is divided into five scripts. <br>

The first, Player, works as the parent for the other player scripts due to having a reference to all child scripts. All other scripts will communicate with this script to get data from any other the sibling scripts. The remaining four scripts deal with the input, collisions, movement and actions of the player exclusive. This was done to keep the scripts organized and easy to read. <br>

The player is able to: 
- Walk and run
- Jump
- Crouch and dash while crouching
- Wall slide and wall jump
- Grab and throw a dodgeball

### Dodgeballs
The main weapon of a player. These can be grabbed from the ground, after they have stopped ricocheting, or from a spawner. When thrown, balls will bounce three times before slowing down or letting players pick them up. 
