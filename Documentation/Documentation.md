# Order of game element creation

1. Character and enemies basic movement
    1. Basic sprites
    2. Basic movement
2. Map
3. Interacting with the map
4. Starting screen
5. Options (reset, quit)
6. Levels
7. Combat
8. Refactoring
9. Better sprites + Animation

## 1. Entity movement

Done:

1. Player can move using WSAD keys.
    1. Once per 15 frames it is checked if they hold the key down and if they do then the character changes position.
    2. His movement is confined by the walls at the edges of the map.
2. Make player movement start when user clicks a movement key and count frames from there.
3. Cover diagonal movement
4. Cover smooth transition between tiles
    1. Keep the decision making every X iterations, but do move in every iteration (this will make transitions smooth but at the same allow for stopping on tiles as is now, not between them)
    2. For enemies
5. Cover walking animation

For version 3.X.X

1. Maybe try making the movement be no longer by-cell only, so that the player can stop wherever they want

## 2. Map

Done:

1. The main map is built as a 3x6 grid where each cell is a "room" (for now that's the name).
2. Rooms:
    1. Each room is a 6x6 grid where each cell is a tile.
    2. The room's tile pattern is a 2D table of ints where each value is an index in table of passed tiles.
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
            2. Each type of room spawns a few enemies of a given type (each with their set difficulity).
    2. Not to lose the old one make an option in option view to switch between the old map and the new one
4. Start with new map, then reset it if user wants to change it

For version 3.X.X

1. Moving map - be able to get to the right side of current map and load next half of the map to the right thus making it look like there is still some land. Make the move smooth, not a jump.
2. Smith/shop building with its contents after entering - whole new map layout :)

## 3. Interacting with the map

Done:

1. Make walls block movement - don't leave the screen.
2. Make Map.cs be responsible for wall detection in entity movement.
3. Be able to enter a new location by the doors in some of the rooms.
    1. New location will show new screen with new map.
4. New location to change HUDHandler (health bar, mana and text).
    1. Design health bar.
    2. Mana bar.
5. Make minor maps "spawn" player at the oposite from where they entered the door - as if the minor map was in fact placed in the world but outside of the window.
6. Sound Effects
   1. Turning on/off
7. Maybe some height differences - hide behind a building, cover its front side 

## 4. Starting Screen

Done:

1. Game name.
2. Begin.
3. Quit.

For version 2.X.X:

1. Some cool effects:
    1. Maybe a simple image.
    2. Maybe a gif or any action.
    3. "Paralax effect"?.

## 5. Options

Done:

1. Reset.
2. Quit.

For version 2.X.X:

1. Music off/on.
2. Sound effects off/on.

## 6. Levels

Done:

1.  Rats, Spiders and Bats
2.  Cyclop and Crabs
3.  Dark Wizard and Ghosts

## 7. Commbat

Done:

1. Program Health behavior.
    1. In form of reconstructable bars (health and mana also).
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

For version 2.X.X:

1. Have player carry a weapon.
    1. Use it without stepping onto an enemy
2. Have enemies
    1. Be able to attack.
    2. Have their health displayed.
        1. Beneath the sprite?
3. Give enemy unique movement pattern?
   1. Enable emeies to damage the player as well.
   2. Random number of enemies from a given scope eg. from 5 to 10.

## 8. Refactoring (more like filling in the gaps of logic xd)

Done (or fixed):

1. HUDHandler can be a Handler rather than an entity.
2. After the fight is over the doors in an encounter open with an animation.
3. Have player be able to go back through the door after killing all enemies in a room.
4. Close doors of an encounter after it's defeated.
5. After a successfull encounter:
    1. The second doors get marked as first encounter bc the closed door's don't count as doors.
    2. Hud is not reset, stat bars get stacked.
6. After the second encounter:
    1. The doors don't open,
    2. HUD doesn't display door number.
7. Reseting: add stage resetting, hud, enemy.
8. Move Constants.cs to Data.json
    1. Map schema change implementation back in codes
    2. Map tiles can be named in a shortened pattern
        1. T for terrain, W for wall, D for door, C for chest
        2. Each can have 9 main "orientions" top, top right, right, right down and so on clockwise - this can be put into number 1-8 and middle as 0
        3. For each consecutive Letter if there is another variant it can be added as v1, v2,v 3 and so on (like v1 for dirt/ground, v2 for sand and so on)
            1. Doors or chests can have s for state rather than o for orientation
    3. This would end up letting each tile be identified by a name like:
        1. To1v1 - type:terrain, orientation:top, variant:dirt
        2. Wo6v2 - wall, left down, sand
9. Put Data.json to Contents folder
10. Change tile numbers to names (for easier handling).
11. HUD string for door name not appearing for second encounter
12. Key Handler

To be done:

1. Introduce Testing?
    1. This is getting more useful the furter we are
2. Show You Win! screen or just message after all encounters are complete
    1. Maybe add option press C to Continue (with the same enemies but stronger)
