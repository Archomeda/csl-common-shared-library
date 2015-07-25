# Cities Skylines: Common Shared Library
[![AppVeyor](https://img.shields.io/appveyor/ci/Archomeda/csl-common-shared-library/master.svg?label=AppVeyor)](https://ci.appveyor.com/project/Archomeda/csl-common-shared-library/branch/master)
[![Travis](https://img.shields.io/travis/Archomeda/csl-common-shared-library/master.svg?label=Travis)](https://travis-ci.org/Archomeda/csl-common-shared-library)
![MyGet](https://img.shields.io/myget/cities-skylines/v/CslCommonShared.svg?label=MyGet)
![Cities Skylines](https://img.shields.io/badge/Cities_Skylines-v1.1.1c-blue.svg)

This small library is used for my own mods to prevent duplicated code across
multiple mods. You can use it for your own mods as well, but keep in mind that
this library is not complete and may have breaking changes over time. It only
contains various classes that I only needed myself. You can, however, submit
pull requests in order to include more functionality if you want.

## Usage
**Please do not upload this as a separate mod on the Steam Workshop! I won't
authorize any request!** Instead, this library is meant to be **included
within your mod**. Over time, it's possible that this library will have breaking
changes, and I really don't feel like maintaining backwards compatibility for
everything that I change, only because that the Steam Workshop doesn't provide
proper versioning and dependency management.

In order to use this library in your mod, add the NuGet feed
`https://www.myget.org/F/cities-skylines/api/v2` to your project. The best way
is to add a `.nuget\NuGet.Config` file (check one of
[my mods](https://github.com/Archomeda/csl-ambient-sounds-tuner) for an
example). You need the package `CslCommonShared`. When a newer version is
available, make sure that you update the library and adept your code. If you
want to stick with a specific version, surround the version with `[` and `]`.
For more advanced configuration, please check the NuGet documentation. Do note
that older versions are not supported by me.

## Compilation Notes
Note that setting up your development environment is a bit different from the
Cities Skylines wiki. As you might have noticed, there aren't any hardcoded
references to the assemblies of Cities Skylines. Instead, these dependencies are
currently maintained by me on a NuGet server. This means that upon building, the
dependencies should be resolved automatically. If, for some reason, this doesn't
work, please check if the feed `https://www.myget.org/F/cities-skylines/api/v2`
has been added and that NuGet automatically restores packages upon building.

When a newer version of Cities Skylines is released, it's possible that the
NuGet feed or `packages.config` gets outdated. If there's no apparent work in
progress to update either of those, don't hesitate to create an issue to make me
aware of it.
