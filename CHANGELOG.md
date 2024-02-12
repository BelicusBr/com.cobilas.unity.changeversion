# Changelog
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project follows [Semantic Versioning](https://semver.org/spec/v2.0.0.html).
## [2.1.3] (12/02/2024)
- ### Fixed
- - Fixed an issue in the `UpdateClosedOption` class that caused it to work randomly.
- - #### Details
- - - The `UpdateClosedOption` class used the `EditorApplication.quitting` event, which was called when the Unity editor closed. However, this event was called before all other classes were finished, which caused the `UpdateClosedOption` class to malfunction.
- - #### Solution
- - - The `UpdateClosedOption` class now uses the `EditorApplication.wantsToQuit` event, which is called before the Unity editor quits. This allows the `UpdateClosedOption` class to finish correctly before the editor is closed.

## [2.1.2] (11/02/2024)
- ### Fixed
- - Fixed an issue in the `UpdateBuildOption` class that prevented it from being called correctly in the project post-build.
- - #### Details
- - - The `UpdateBuildOption` class inherited the `IPostprocessBuildWithReport` interface, which allowed it to be called after the project was built.
However, the Unity editor was calling and instantiating the `UpdateBuildOption` class instead of using the already instantiated object.
This caused the class to malfunction.
- - #### Solution
- - - The `IPostprocessBuildWithReport` interface has been removed from the `UpdateBuildOption` class.
Now, the `UpdateBuildOption` class uses the `OnPostprocessBuild` event of the `CVBuildReport` class, which is correctly called after the project is built.

## [2.1.1] (05/02/2024)
- ### Changed
- - Updated dependency `com.cobilas.unity.utility` to version `2.10.3`.
- - This update includes bug fixes and new features that do not directly impact this package.
- - The sub-dependency `com.cobilas.unity.core.net4x@1.4.1` was made explicit in the package dependencies.
## [2.0.0] - 08/09/2023
### Changed
- Dependencies have been changed.
## [2.0.0-rc1] - 01/09/2023
### Obsolete
- The file <kbd>VersionConfig.json</kbd> is not used by <kbd>ChangeVersion</kbd>, but the file will not be deleted.
### Added
- New function `Tools/ChangeVersion/Open ChangeVersion folder` has been added.
- Custom options have been added as
```c#
sealed class PreProductionCharacterOption;
sealed class UpdateBuildOption;
sealed class UpdateClosedOption;
sealed class UpdateRevisionOption;
```
### Removed
- The classes
```c#
class BaseBuildPhaseTemplate;
class BaseVersionTemplate;
static class ParseOldChangeVersion
```
have been removed.
## [1.3.0] - 29/08/2023
## Changed
- Package dependencies have been changed.
## [1.2.0-ch1] - 28/08/2023
### Changed
- The package author was changed from `Cobilas CTB` to `BélicusBr`.
## [1.1.1] - 21/02/2023
###Fixed
Now every item in `ChangeVersion` can be renamed.
## [1.0.3] - 30/01/2023
### Changed
- The methods `bool:ChangeVersionWin.ToolbarButton(string, float)` and `bool:ChangeVersionWin.ToolbarButton(string)` were commented out as they are not used.

## [1.0.2] 23/11/2021
### Fix (Lost information)
When opening the 'ChangeVersion' window, the files 'ChangeVersion/VersionConfig.txt and 'ChangeVersion/Config.txt'
were deleted but the information was not absorbed by 'ChangeVersion' which resulted in an empty value
standard.
### Fix (Duplicate library)
The duplicate library `using System.Collections.Generic;` has been removed from
'Editor\Change version\Class's\VersionTemplate.cs(Ln:3)'
## [1.0.0] 10/11/2022
### Repository com.cobilas.unity.utility started
- Released to GitHub