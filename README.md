# Cities Skylines: Common Shared Library
[![AppVeyor status](https://ci.appveyor.com/api/projects/status/n8d0t6pp7x3b0gel/branch/master?svg=true)](https://ci.appveyor.com/project/Archomeda/csl-common-shared-library/branch/master)
[![Travis status](https://travis-ci.org/Archomeda/csl-common-shared-library.svg?branch=master)](https://travis-ci.org/Archomeda/csl-common-shared-library)

This small library is used for my own mods to prevent duplicated code across
multiple mods. You can use it for your own mods as well, but keep in mind that
this library is not complete and may have breaking changes over time. It only
contains various classes that I only needed myself. You can, however, submit
pull requests in order to include more functionality if you want.

## Usage
***Note:*** *Please do* ***not*** *upload this as a separate mod on the Steam
Workshop! I won't authorize any request!*

This library is meant to be **included within your mod**. The reason for this,
is that this library will have multiple versions at some point, and they might
be incompatible with each other. By including it and giving it an unique version
number, we prevent this incompatibility (versioning is done automatically upon
building). The game will automatically use the correct version you compiled
against (meaning, the one you ship with your mod).

You can include it within your mod in multiple ways, but the way I've done it
(for now), is by creating a git subtree for the mods that depend on this
library. You can take a look at
[Extended Toolbar](https://github.com/Archomeda/csl-scrollable-toolbar) or
[Ambient Sound Tuner](https://github.com/Archomeda/csl-ambient-sounds-tuner) as
examples.

It's possible that I'll change this method at some point, but that depends
entirely on how extensively I'm using this.

### Compilation Notes
Please note that setting up your development environment is a bit different from
the Cities Skylines wiki. Instead of hardcoding various dependencies in the
solution file, you have to specify a new NuGet package source in your Visual
Studio settings. The source URL you have to add is
`https://www.myget.org/F/cities-skylines/api/v2`. Make sure that NuGet restores
the dependencies automatically upon building.

Also, please refrain from adding those hardcoded references in the solution
file when submitting a pull request.
