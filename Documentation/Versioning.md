# Versioning

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
