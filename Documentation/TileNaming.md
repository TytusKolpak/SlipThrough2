# Naming convention for the tiles

General rule:

1. Map tiles can be named in a shortened pattern
    1. T for terrain, W for wall, D for door, C for chest
    2. Each can have 9 main "orientions" top, top right, right, right down and so on clockwise - this can be put into number 1-8 and middle as 0
    3. For each consecutive Letter if there is another variant it can be added as v1, v2,v 3 and so on (like v1 for dirt/ground, v2 for sand and so on)
        1. Doors or chests can have s for state rather than o for orientation
2. This would end up letting each tile be identified by a name like:
    1. To1v1 - type:terrain, orientation:top, variant:dirt
    2. Wo6v2 - wall, left down, sand

## Terrain

1. v1 is ground
2. v2 is ground with rocks
3. v3 is ground with different rocks
4. v4 is sand
5. v5 is sand with particles

Main map:

-   ORL Open Right to Left
-   OR Open to the Right
-   ORD Open Right and Down

Easy encounter:

-   TRE Top right edge
