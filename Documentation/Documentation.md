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

For version 2.X.X

1. Cover diagonal movement
2. Cover smooth transition between tiles
3. Cover walking animation (eg. left right bobbing)

## 2. Map

Done:

1. The main map is built as a 3x6 grid where each cell is a "room" (for now that's the name).
2. Rooms:
    1. Each room is a 6x6 grid where each cell is a tile.
    2. The room's tile pattern is a 2D table of ints where each value is an index in table of passed tiles.
    3. There is one type of room for every unique shape (type being as example: with opening to the left and right).

For version 2.X.X

1. Build better main map xd.
    1. I think it's fine to give up the 3x6 grid for sake of something more organic/natural.

## 3. Interacting with the map

Done:

1. Make walls block movement - don't leave the screen.
2. Make Map.cs be responsible for wall detection in entity movement.
3. Be able to enter a new location by the doors in some of the rooms.
    1. New location will show new screen with new map.
4. New location to change HUD (health bar, mana and text).
    1. Design health bar.
    2. Mana bar.
5. Make minor maps "spawn" player at the oposite from where they entered the door - as if the minor map was in fact placed in the world but outside of the window.

For version 2.X.X

1. Maybe some height differences - low ground, high ground, starircases
2. Actual rooms with semi transparent roofs

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

1. Reset
2. Quit

## 6. Levels

1.  Wolf
2.  Werevolf
3.  Cerber

## 7. Commbat

1. Program Health behavior.
2. Enemy behavior there.
3. Player, enemy stats.
4. Tie together combat.

## 8. Refactoring

1. HUD can be a Handler rather than an entity.
