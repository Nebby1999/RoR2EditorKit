### 0.1.1

- RoR2EditorKitSettings:
    * Removed the "EditorWindowsEnabled" setting.
    * Added an EnabledInspectors setting.
        * Lets the user choose what inspectors to enable/disable.
    * Added a MainManifest setting.
        * Lets RoR2EditorKit know the main manifest it'll work off, used in the SerializableContentPackWindow.

- Inspectors:
    * Added InspectorSetting property
        * Automatically Gets the inspector's settings, or creates one if none are found.
    * Inspectors can now be toggled on or off at the top of the inspector window.
    
- Editor Windows: 
    * Cleaned up and documented the Extended Editor Window class.
    * Updated the SerializableContentPack editor window:
        * Restored function for Drag and Dropping multiple files
        * Added a button to each array to auto-populate the arrays using the main manifest of the project.

### 0.1.0

- Reorganized CreateAsset Menu
- Added EntityStateConfiguration creator, select state type and hit create. Optional checkbox for setting the asset name to the state's name.
- Added SurvivorDef creator, currently halfway implemented.
- Added BuffDef creator, can automatically create a networked sound event for the start sfx.
- Removed EntityStateConfiguration editor window.
- Implemented a new EntityStateConfiguration inspector
- Internal Changes

### 0.0.1

- Initial Release