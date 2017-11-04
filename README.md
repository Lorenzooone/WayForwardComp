# WayForwardComp
Here I try to understand how the decompression used by WayForward in Shantae (GBC) works.

It's a LZ77 type of compression, but it has a particular sized buffer and it doesn't have the signature like the standard GBA one does.

In the end I RE the compression and now the release can compress/decompress the game's font!
(It saves 1 whole byte too!)
In the future I might make some changes, though.
(Making it work for other graphics too could be an idea)
