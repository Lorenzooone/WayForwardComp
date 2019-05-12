# WayForwardComp
Here I try to understand how the decompression used by WayForward in Shantae (GBC) works.

It's a LZ77 type of compression, but it has a particular sized buffer and it doesn't have the signature like the standard GBA one does.

In the end I RE the compression and now the release can compress/decompress the game's font!
(It saves 1 whole byte too!)
In the future I might make some changes, though.
(Making it work for other graphics too could be an idea)

## Notes about non-decompressable graphics

Block at 0x0A1B to see arrangements/stuff

0x3631C4 Erase this file - Arrangement

0x36327E Really? - Arrangement

0x383F88 Continue - Arrangement

0x23BFB6 GEMS - Arrangement

0x297F90 OUT - frame 1 GFX

0x29FF80 OUT - frame 2 GFX

0x2A25D0 OUT - frame 3 GFX

0x2A2620 OUT - frame 4 GFX

0x377476 Press Start - GFX
