# Deep Lingo compiler - Project make file:
# IRVIN EMMANUEL TRUJILLO DÍAZ A01370082 AND LUIS FERNANDO ESPINOSA ELIZALDE A01375758. 2018, ITESM,CEM.
# This file is based on the Buttercup compiler - Project make file made by Ariel Ortiz, ITESM CEM
#
# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
#
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with this program.  If not, see <http://www.gnu.org/licenses/>.



deeplingo.exe:  CILGenerator.cs  Driver.cs Scanner.cs Token.cs TokenCategory.cs \
	Parser.cs SyntaxError.cs Node.cs SpecificNodes.cs SemanticAnalyzerGlobal.cs \
	SemanticAnalyzerLocal.cs SemanticError.cs GlobalLocalTableStructure.cs \
	GlobalSymbolVarTable.cs ContainerOfArrays.cs \

# csc (Windows or if you have csc in linux you can use it):
			csc -out:deeplingo.exe CILGenerator.cs Driver.cs Scanner.cs Token.cs TokenCategory.cs \
	Parser.cs SyntaxError.cs Node.cs SpecificNodes.cs SemanticAnalyzerGlobal.cs \
	SemanticError.cs GlobalLocalTableStructure.cs GlobalSymbolVarTable.cs \
	ContainerOfArrays.cs SemanticAnalyzerLocal.cs
#  mcs (Using MonoCompiler):
#			mcs -out:deeplingo.exe  CILGenerator.cs Driver.cs Scanner.cs Token.cs TokenCategory.cs \
	Parser.cs SyntaxError.cs Node.cs SpecificNodes.cs SemanticAnalyzerGlobal.cs \
	SemanticError.cs GlobalLocalTableStructure.cs GlobalSymbolVarTable.cs \
	ContainerOfArrays.cs SemanticAnalyzerLocal.cs

deeplingolib.dll: deeplingolib.cs
	mcs /t:library deeplingolib.cs
#	csc /t:library deeplingolib.cs

clean:
# windows:
#	del deeplingo.exe
# linux:
	 rm deepLingo.exe deeplingolib.dll
