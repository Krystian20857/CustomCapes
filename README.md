# CustomCapes
Simple software for custom minecraft capes. It allows to create persistent capes profiles for every player.

## Features
  * Works locally - require no external server
  * Easy to use - simple user interface
  * Persistent profiles - user profiles are save to disk
  
## How it works?
  Application create local http server which hosts all nessecery files then overrides windows hosts file.
  All files are stored under: %AppData%/CustomCapes
  
## Disadvantages
  * Http server uses 80 port(default http port) it may cause issue with other http servers.
  
