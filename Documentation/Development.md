# Development

## Documentation 

(as for final version 2.X.X)

Approach to documentation has been changed a couple of times. Since this is a solo project I assume the necessary documentation are the inline comments in code. No special tool is necessary for pure code explanation.

Project structure overview:

![Structure](Structure.drawio.svg)

Purposes / rough outline of the project elements (files/classes):

**Game1:** This is the entry point of the whole project. Inside all the assets like graphic and sound files are loaded, along with custom data from `Data.json`. This custom data is structured in a logical way in the `DataStructure.cs` file. 

**Data Structure:** Describes how the raw data in `Data.json` should be treated and what are relations between its parts.

**Game Manager:** The most top level element of the whole game to actually have some custom logic. It creates a gap between what is in `Game1.cs` because it has to be and all the custom logic specific for this game. It runs the managers for audio, the view (anything displayed in the window) and the keyboard.

**Audio Manager:** Manages all the sounds that are played in the game, provides methods that can be used from elsewhere to signify that an even has in fact happened. It feels better to move the player if there is a sound of them moving.

**View Manager:** Manages which view is displayed in the window. It is an abstraction for the next 3 views.

**Start View:** Specifies what is visible on the StartView, which is the first view that the player sees. It displays the name of the game and what the user can do.

**Options View:** Specifies what is visible on the OptionsView, which is displayed anytime the players switches to it from the main view. It displays a number of settings and allows for their changing.

**Main Game View:** This is the main component of the game. It manages what happens when the user actually plays the game. It includes the Map, Entities, Hud and Weapons.

**Moving Background:** Is just there to handle what happens with the background in the start view (and options view), which is displaying a moving and looping image. 

**Map Manager:** The largest element of the game right now, it specifies how the s that the player is on look and work.

**Map Handler:** Is a separate element from the map manager to create a visible separation between how the maps look and work generally and how they are drawn, also it keeps track of the very current one.

**HUD Manager:** Deals with the Heads Up Display providing additional helpful info mainly during the encounters, where life of the player and the enemies needs to be displayed.

**Weapon Manager:** Deals with the weapons that an entity can carry. How they behave and what do they do.

**Enemy Manager:** Manges all the enemies that are in the main view. Spawns them and keeps track of them.

**Enemy:** One of the Entity types along with the Player. It's here to direct how an enemy works and specifies all their elements.

**Player:** This one specifies what the player does regarding movement their parameters and so on.

**Combat Handler:** Ties together the interaction between the player and the enemies, covers these parts of action which are not specifically one or the others. 

## Publishing

The content of the game in its playable form is in `Builds` folder. To build I use this command:

```shell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o "Builds"
```

## Naming convention for the tiles

General rule:

1. Map tiles can be named in a shortened pattern
    1. T for terrain, W for wall, D for door, C for chest
    2. Each can have 9 main "orientations" top, top right, right, right down and so on clockwise - this can be put into number 1-8 and middle as 0
    3. For each consecutive Letter if there is another variant it can be added as v1, v2,v 3 and so on (like v1 for dirt/ground, v2 for sand and so on)
        1. Doors or chests can have s for state rather than o for orientation
2. This would end up letting each tile be identified by a name like:
    1. To1v1 - type:terrain, orientation:top, variant:dirt
    2. Wo6v2 - wall, left down, sand

### Terrain

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

## Versioning

The versioning pattern I assume to treat in this way:

1. Version is X.Y.Z - Major.Minor.Unit.
2. Unit version (Z) will just be for commit and ease of tracking.
    1. I expect larger commits at least per 1 point in below To be done sections.
3. Minor version (Y) will be for the game element which is being developed. So that it is also clear which commit focuses on which part.
    1. Once the Y value is increased the elements before are left as they are.
    2. Any further changes to them I assume to perform after changing Major version.
4. Major version (X) will be for large game changes.
    1. This one will be the only one the end user would care about since only at its end will the game be complete and in the form suitable for intended gameplay.
    2. Once I have all elements of the game implemented to any satisfying extent I will be able to come back and improve any parts which I consider as with room for improvement, change or refactor.

Git hook (local) to keep track of versioning:

```shell
#!/bin/sh

COMMIT_MSG_FILE=$1
COMMIT_SOURCE=$2
SHA1=$3

### Search for the value of Version
# XML file path
xml_file="SlipThrough2.csproj"
node_name="Version"  # Node you want to search for

# Check if the file exists
if [ ! -f "$xml_file" ]; then
    echo "Error: File '$xml_file' does not exist."
    exit 1
fi

# Extract the version number using grep
version=$(grep -oP "<$node_name>\K[0-9]+\.[0-9]+\.[0-9]+(?=</$node_name>)" "$xml_file")

# Check if version was found
if [ -z "$version" ]; then
    echo "Error: Version node <$node_name> not found in the file."
    exit 1
fi

# Separate X, Y, and Z parts of the version
IFS='.' read -r major minor patch <<< "$version"

# Increment the patch version
((patch++))

# Construct the new version
new_version="$major.$minor.$patch"

# Replace the old version with the new version in the file
sed -i "s/<$node_name>$version<\/$node_name>/<$node_name>$new_version<\/$node_name>/" "$xml_file"

# Notify
echo "Version updated from $version to $new_version in $xml_file"

# Add a new line to the commit and a custom message
echo "" >> $COMMIT_MSG_FILE
echo "Version: $new_version" >> $COMMIT_MSG_FILE
```
