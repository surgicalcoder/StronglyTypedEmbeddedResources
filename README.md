# StronglyTypedEmbeddedResources
.net standard 2.0 generator for strongly named Embedded Resource names.

## This project is now archived, my recommendation is to use [ThisAssembly](https://github.com/devlooped/ThisAssembly).

## How to use

Add from Nuget - GoLive.Generator.StronglyTypedEmbeddedResources - then enjoy the EmbeddedResource.g.cs that gets generated in the root of the project.

### Why don't you use the AddSource method, why do you output a file?

Well, because AddSources is a bit flaky in some IDEs at the moment, it doesn't always get updated, so the old fashioned method of creating a file is the best.
