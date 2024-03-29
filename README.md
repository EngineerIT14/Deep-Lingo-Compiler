Deep Lingo compiler, version 0.5
===============================
* IRVIN TRUJILLO
* LUIS ESPINOSA 

This program is free software. You may redistribute it under the terms of
the GNU General Public License version 3 or later. See license.txt for
details. This compiler is based on the Buttercup compiler, version 0.5
made by Ariel Ortiz.

Included in this release:

   * Lexical analysis.
   * Syntactic analysis.
   * AST construction.
   * Semantic analysis.
   * CIL Code generation.

**Deep Lingo Programming Language Description**     [Read document.](http://34.212.143.74/s201811/tc3048/DeepLingo/DeepLingo_language_spec.html)

***To build, use the terminal and go to the directory where the MakeFile is and type the following***

    *Linux:    make
    *Windows:  NMAKE

[If you are using windows and NMAKE is unrecognized, then go to this link in order to install it.](https://msdn.microsoft.com/en-us/library/dd9y37ha.aspx)

***install wine if you are using Linux***

    apt-get install wine


***Generate il file that will contain the CIL(Common Intermediate Language) code that represents the deeplingo code.***

    * Using Linux: wine ./deeplingo.exe ./TestPrograms/<file_name> ./il/<file_name>.il
    * Using Windows: deeplingo.exe ./TestPrograms/<file_name> ./il/<file_name>.il

***Move to the il directory and then generate the executable with the following command***

    ilasm <file_name>.il

***Run the executable***

    * Using Linux: wine <file_name>.exe
    * Using Windows: <file_name>.exe

<file_name> is the name of a Deeplingo source file. You can try with
these files:

    * arrays.deep
    * binary.deep
    * factorial.deep
    * literals.deep
    * next_day.deep
    * palindrome.deep
    * ultimate.deep
