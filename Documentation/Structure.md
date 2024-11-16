# Structure

## Structure of a single class

Most classes are built in this pattern:

0. Fields
1. Constructor
2. Update
3. Draw
4. Helper functions

## Stucture of connection between classes

(Program -> Game1 -> GameManager)

1. GameManager
    1. MapManager
        1. MapHandler
    2. Player
        1. HUD
    3. Enemy
