### Building overview - initial idea

1. As in [Slip Through](https://github.com/TytusKolpak/SlipThroughGame) I think this one can be build in 5x6 grid, divded into 3 "Acts" of 2 rows.
2. Every fight will not be automatic, but rather than that will work simillar to the [Shooter](https://github.com/TytusKolpak/Shooter) game in Golang.

## Order of creation
1. [x] Character and enemies basic movement 
   1. [x] Basic sprites
   2. [x] Basic movement
2. [x] Map
3. [ ] Interacting with the map
4. [ ] Starting screen
5. [ ] Options (reset, quit)
6. [ ] Levels
   1. [ ] Wolf
   2. [ ] Werevolf
   3. [ ] Cerber
7. [ ] Platforming on fight
   1. [ ] Entering new screen
   2. [ ] Player movement there
   3. [ ] Enemy behavior there
   4. [ ] Player, enemy stats
   5. [ ] Combat
8. [ ] Refactoring
9. [ ] Better sprites + Animation

### 3. Interacting with the map

1. Make walls block movement - don't leave the screen
1. Be able to enter a new location by the doors in some of the rooms
	1. New location will show new screen with new map. Some platforming maybe?
	1. New location will change HUD (health bar, mana or smthng)
