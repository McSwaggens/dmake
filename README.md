# DMake
An alternative multi-platform C/C++ build tool

![Screenshot](https://i.imgur.com/uXJAnY4.png)

# Description
DMake is a streamlined C/C++ build tool replacing CMake and GNU Make that targets Unix and Windows(MinGW).

# Folder structure
In order to use DMake you need a source folder with your C/C++ code inside.

**Source Folder Names**
- `src/` *default*
- `source/`
- `sources/`
- `code/`
- `scripts/`

**Binary Directory Names**
- `bin/` *Default*
- `output/`
- `out/`

# Features
- Config
- Multi-threaded compilation
- Easy to use
- Built in runner and installer
- No config required

# Support
DMake is a multiplatform build tool.

Tested platforms:

 | Platform      | Supported         |
 | ------------- | ----------------: |
 | Linux         |   Not Tested      |
 | Windows       |   Working         |
 | OSX           |   Not Tested      |
 
 #### Windows
 
 DMake on Windows specifically targets [MinGW](http://www.mingw.org/) `g++` and `gcc`.
 It **does not** support Visual C++, DMake is intended for a Unix like development environment.
 
 #### Unix (Linux & OSX)
 
 DMake should work without any prerequisites on Unix systems, assuming you already have mono installed.
 
 When using the `install` flag in DMake, it will move the executable to `/usr/bin/`.
 
 # Why make DMake?
 - Make is confusing and painfull to use and maintain.
 - CMake is better but the syntax is still too complicated and annoying to maintain.
 - CMake also generates a lot of annoying junkfiles.
 - CMake is also limited to just being a build tool, DMake can install and run your program.
 - Make and CMake *require* you to create a script to compile even the smallest of programs.
 
 DMake fixes all of these problems and does more. It doesn't *require* you to write a script/config file to compile your program. DMake does not generate any junk files, keeping your project squeaky clean.
 DMake is the easiest tool to compile your C/C++ code.
 
 #License
 [MIT](https://choosealicense.com/licenses/mit/)
 
 
