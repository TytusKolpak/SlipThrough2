### Building overview - initial idea

1. As in [Slip Through](https://github.com/TytusKolpak/SlipThroughGame) I think this one can be build in 5x6 grid, divded into 3 "Acts" of 2 rows.
2. Every fight will not be automatic, but rather than that will work simillar to the [Shooter](https://github.com/TytusKolpak/Shooter) game in Golang.

## Misc

Git hook to keep track of versioning:

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
