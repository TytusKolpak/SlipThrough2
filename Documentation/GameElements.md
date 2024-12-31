# Game elements

## Entity movement

1. Player can move using WSAD keys.
    1. Once per 15 frames it is checked if they hold the key down and if they do then the character changes position.
    2. His movement is confined by the walls at the edges of the map.
2. Make player movement start when user clicks a movement key and count frames from there.
3. Cover diagonal movement
4. Cover smooth transition between tiles
    1. Keep the decision making every X iterations, but do move in every iteration (this will make transitions smooth but at the same allow for stopping on tiles as is now, not between them)
    2. For enemies
5. Cover walking animation

## Map

1. The main map is built as a 3x6 grid where each cell is a "room" (for now that's the name).
2. Rooms:
    1. Each room is a 6x6 grid where each cell is a tile.
    2. The room's tile pattern is a 2D table of int's where each value is an index in table of passed tiles.
    3. There is one type of room for every unique shape (type being as example: with opening to the left and right).
3. Build better main map xd.
    1. I think it's fine to give up the 3x6 grid for sake of something more organic/natural.
        1. Few rooms maybe.
        2. Each with doors.
        3. Rooms:
            1. Reusable.
            2. Spawned at random location within X cells from each other minimum.
            3. Positions chosen at random.
        4. Few types of rooms.
            1. Each room has one immobile enemy sprite at its roof.
            2. Each type of room spawns a few enemies of a given type (each with their set difficulty).
    2. Not to lose the old one make an option in option view to switch between the old map and the new one
4. Start with new map, then reset it if user wants to change it
5. Close doors of an encounter after it's defeated.

## Interacting with the map

1. Make walls block movement - don't leave the screen.
2. Make Map.cs be responsible for wall detection in entity movement.
3. Be able to enter a new location by the doors in some of the rooms.
    1. New location will show new screen with new map.
4. New location to change HUDHandler (health bar, mana and text).
    1. Design health bar.
    2. Mana bar.
5. Make minor maps "spawn" player at the opposite from where they entered the door - as if the minor map was in fact placed in the world but outside of the window.
6. Sound Effects
    1. Turning on/off
7. Maybe some height differences - hide behind a building, cover its front side

## Starting Screen

1. Game name.
2. Begin.
3. Quit.
4. Scrolling background
    1. Text is white to contrast the dark background

## Options

1. Reset.
2. Quit.
3. Sound effects off/on.
4. Type of map old/new (default is new)

## Levels

1.  Rats, Spiders and Bats
2.  Cyclops and Crabs
3.  Dark Wizard and Ghosts

## Combat

1. Program Health behavior.
    1. In form of reconstructible bars (health and mana also).
    2. Both displayed independently.
2. Enemy behavior there.
    1. Movement.
    2. Death.
3. Player, enemy stats.
    1. Assign to Entity.
    2. Use in combat.
4. Have enemies have complex stats.
    1. Based on the room (difficulty level)
    2. Meaning more health than 1.
5. After the fight is over the doors in an encounter open with an animation.
6. Have player be able to go back through the door after killing all enemies in a room.
7. Have player carry a weapon.
    1. Leave it at the ground.
    2. Enable player to pick it up and carry
    3. Reset Weapon on reset
    4. Have the sword hit box display to a side or up, down
    5. Change hit box icon when an enemy would be hit
    6. Use it without stepping onto an enemy
8. Have enemies
    1. Have their health displayed beneath the sprite.
    2. Be able to attack.
9. Control the direction of attack with I,L,K,J keys instead of player direction.
10. Display encounter difficulty instead of door number in the message.
11. Make the enemy turn red and stop for a while after being hit.
12. On death enemy turns gray and turns over.
13. Show You Win! screen or just message after all encounters are complete
    1. Show You Lose if player dies
14. Make player red after being attacked too

## Done in 3.X.X

1. Background image
    1. Change the image to something thematic.
    2. Make movement horizontal.
    3. Put in another moving image with transparent background which moves faster in the same direction as the firs one to create a parallax effect.

### Window resolution

1. Remove old main map data
2. Dynamically specify which room to use in an encounter without hardcoding it since we know the pattern
3. It would be cool if it was 16:9 to enable fullscreen without stretching at different rates
    1. Right now it's:
        1. 6("rooms")x6(cells)x32px = 1152 width,
        2. 3("rooms")x6(cells)x32px = 576 height,
        3. So proportion is 2:1
    2. 16 x room width x 32 = 506 x room width -> If we want 3 cell room then width of window is 1536 already
    3. height would be 9 x 3 x 32 = 865
    4. We can give it a try and see how it goes
        1. Looks fine

## To be done in 3.X.X

### Weaponry

1. Introduce a new building
2. Have inside be built as a completely new, full size map representing inside of a building
3. Inside a new weapon can be picked up (and then used)
4. Potions can be bought? (would require gold acquiring in some way)
    1. And used to regain health, mana or some other stats
5. Shield can be bought (needs to be used with B - blocks an enemy attack)
6. Have weapons have a hitting animation - like a little rotation + position change

### Map

1. Moving map.
    1. Be able to get to the right side of current map and load next half of the map to the right.
        1. Thus making it look like there is still some land.
    2. Make the move smooth, not a jump.
    3. Generate the new map portion as 100% of view (in full window size).

### Enemy

1. Give enemy unique movement pattern.
    1. Some move towards the player when further than others,
    2. Some run away from player when in a given range,
    3. Some try try to keep distance of X and attack from range.
2. Control the hitbox pattern per-weapon, put this data in Data.json.

### Levels

1. Put in a number of enemies of the same type and a boss with larger texture and bigger stats
    1. Boss be random one of the inside enemies, appearing after all others are defeated
2. Maybe add option press C to Continue after a win to reset everything but make enemies stronger to introduce level progression

## Topic to keep in mind

1. Height differences can be implemented (by changing transparency of tiles above player) to hide behind a building or so
2. Maybe try making the movement be no longer by-cell only, so that the player can stop wherever they want
3. If the amount of health will be more than around 20 it would be better to
    1. Keep health as 10 potions each representing 10% of hp, not 1 point each
    2. Display numbers flying off of a hit target after an attack
