# Hacbuild v1.1 - JulesOnTheRoad's mod
Hacbuild is a utility to rebuild *Switch GameCards* (XCI) made by LucaFraga
Original version can be found in https://github.com/LucaFraga/hacbuild
This is the revised version made by julesontheroad github.com/julesontheroad/hacbuild

## Changes
- Erased need for XCI Header Key
- Readapted hfs0manager to build hfs0 partitions with hashregions bigger than 0x200
- Added new functions

### Usage
- xci auto builder: Builds a XCI from a directory containing the folders 'normal', 'secure' and 'update' and a 'game_info.ini' file.
  `hacbuild.exe xci_auto <input_folder> <output_file>` 
- xci auto builder with file deletion while building: Same but with input folders and temporal file deletion while building.
  `hacbuild.exe xci_auto_del <input_folder> <output_file>`
- Manual builder for xci from root.hfs0:  Builds a XCI from a directory containing 'root.hfs' and 'game_info.ini'
  `hacbuild.exe xci <input_folder> <output_file>`
- root.hfs0 auto builder:  Builds a root.hfs0 from a directory containing the folders 'normal', 'secure' and 'update'.
  `hacbuild.exe rhfs0_auto <input_folder>`
- root.hfs0 auto builder with file deletion while building: Same but with input folders and temporal file deletion while building.
  `hacbuild.exe rhfs0_auto_del <input_folder>`
- Manual builder for hfs0 (normal/secure/update): Builds a hfs0 partition
  `hacbuild.exe hfs0 <input_folder> <output_file>`
- Manual builder for root.hfs0: Builds a hfs0 partition, currently it needs a multiplier, needs work to read the multiplier from input hfs0 files.
  `hacbuild.exe root_hfs0 <input_folder> <output_file> <multiplier>`
- Get 'game_info' file named as file input name: Get game_info file named as the input xci in the folder you set.
  `hacbuild.exe read xci <input_file> <output_folder>`
- Get 'game_info' file named as gameinfo.ini: Get game_info file named as gameinfo.ini in the folder you set.
   `hacbuild.exe read xci <input_file> <output_folder>`

### Functionalities
- Builds\rebuilds xci files and hfs0 partitions and root.hfs0 files. 
- Rebuilds hfs0 partitions with less padding than the official ones.
- Reads xci files to obtain game_info files.
- Can build empty hfs0 partitions.

### Limitations
- The switch don't like secure partition with less than 4 files inside.
- You can put several content inside the secure partition but you can't put more than 8 different games inside. It may be due to horizon qlauncher, it may be able to read them without error with a custom launcher.
- Only tickets for updates get read by horizon inside the secure partition.
- Still need to make code to generate automatically the multiplier when building a root hfs0 from other hfs0 partitions

## Credits & useful links
- [Original hacbuild](https://github.com/LucaFraga/hacbuild) by LucaFraga
hacbuild was inspired by:
- [hactool](https://github.com/SciresM/hactool) by SciresM
- [NXTools](https://github.com/jakcron/NXTools) by jackron
We'd also like to thank [SwitchBrew](http://switchbrew.org/index.php?title=Main_Page) for their extensive research and documentation of the Nintendo Switch.






