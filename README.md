# WayForwardComp
Here I try to understand how the decompression used by WayForward in Shantae (GBC) works.

It's a LZ77 type of compression, but it has a particular sized buffer and it doesn't have the signature like the standard GBA one does.

In the end I RE the compression and now the release can compress/decompress the game's font!
(It saves 1 whole byte too!)
In the future I might make some changes, though.
(Making it work for other graphics too could be an idea)

* Notes about non-decompressable graphics
Block at 0A1B to see arrangements/stuff
3631C4 Erase this file - Arrangement
36327E Really? - Arrangement
383F88 Continue - Arrangement
23BFB6 GEMS - Arrangement
297F90 OUT - frame 1 GFX
29FF80 OUT - frame 2 GFX
2A25D0 OUT - frame 3 GFX
2A2620 OUT - frame 4 GFX
377476 Press Start - GFX
