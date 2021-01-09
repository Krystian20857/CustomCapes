[![AppVeyor Page](https://img.shields.io/appveyor/build/Krystian20857/customcapes?style=for-the-badge&logo=appveyor)](https://ci.appveyor.com/project/Krystian20857/customcapes)

# CustomCapes
Simple software for custom minecraft capes. It allows to create persistent capes profiles for every player.

# Downloads
Click appveyor badge then go to artifacts and download _CustomCapes.exe_.

## Features
  * Works locally - require no external server
  * Easy to use - simple user interface
  * Persistent profiles - user profiles are save to disk
  * Compatible with existing cape servers - if server cannot find cape for player cape is take directly from cape server
  
## How it works?
  Application create local http server which hosts all nessecery files then overrides windows hosts file.
  All files are stored under: %AppData%/CustomCapes
  
## Disadvantages
  * Http server uses 80 port(default http port) it may cause issue with other http servers.
  
## Build
 ``` c#
 dotnet tool restore
 dotnet cake build/build.cake
 ```
