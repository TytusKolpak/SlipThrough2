# Weird code explanation

## Player

### Draw

What is heightInStep?!

```cs
int heightInStep = timeForShift / 2 - Math.Abs(idleIterations - timeForShift / 2);
```

Idea is to mimic walking - human rises and lowers their center of mass with every step, so it kinda looks like they are moving up and down every step. So if we icrease and lower position of the player sprite a little between every tile it will look a little like they are actualy walking.

Why these calculations?

Because:

```
         A:  0, 1, 2, 3, 4, 5, 6, 7 (idleIterations)
         B:  4                      (timeForShift/2)
       A-B: -4,-3,-2,-1, 0, 1, 2, 3
 -abs(A-B): -4,-3,-2,-1, 0,-1,-2,-3
B-abs(A-B):  0, 1, 2, 3, 4, 3, 2, 1
```
