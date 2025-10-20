# Rimworld World edit for 1.6 

Original: https://gitlab.com/koviazin.pro/worldedit-2.0

The mod was modified to work with the 1.6 update.
It now requires "Harmony"  https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077

Some features might not work with Odyssey, like modifying space. 

WorldEditGeologicalLandforms and WorldEditRimCity are not ported to 1.6  

## BUild from source 
- Install net framework 4.8 
- modify the "WorldEdit 2.0.csproj", change the path to rimworld's dll
- replace the DLL output Mods/WorldEdit 2.0/1.6/Assemblies/


## Know issues: 

- Creating a road sometimes trigger an error about caravans, the issue was already present in the 1.5 
- The redraw all layer throws a NPE, but both other button "draw terrain" and "draw mountain" work in the tile editor  


 