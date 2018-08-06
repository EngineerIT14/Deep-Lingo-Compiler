/*
Deep Lingo compiler - Specific nodesubclasses for the AST (Abstract Syntax Tree
2018, ITESM CEM.
* IRVIN EMMANUEL TRUJILLO DÍAZ A01370082
* LUIS FERNANDO ESPINOSA ELIZALDE A01375758

  This file is based on the Buttercup compiler - Specific node 
  subclasses for the AST (Abstract Syntax Tree). Copyright (C) 2013 
  Ariel Ortiz, ITESM CEM.


  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/


namespace Deeplingo {
    
    //LL1
    class Program: Node {} //
    class DefList: Node {} //
    class IdList: Node{} //
    class FunDef: Node{} //
    class ParamList: Node{} //
    class VarDefList: Node{} //
    class StmtList: Node{} //
    class FunCall: Node{} //
    class ExprList: Node{} //
    class ElseIfList: Node{} //
    class Else: Node{} //
    class StmtEmpty: Node{} //
    class OpComp: Node{} //
    class OpRel: Node{} //
    class OpAdd: Node{} //
    class OpMul: Node{} //
    class OpUnary: Node{} //
    ///////////////////////////////////
    //lists of operators

    //class OpMulList: Node{}

    
    
    /////////////////////////////
    //TOKENS
    class Array: Node{} //
    class IntLiteral: Node{} //
    class StringLiteral: Node{} //
    class CharLiteral: Node{} //
    class Assign: Node{} //
    class And: Node{} //
    class Break: Node{} //
    class Decrement: Node{} //
    class ElseIf: Node{} //
    class Identifier: Node{} //
    class If: Node{} //
    class Increment: Node{} //
    class Loop: Node{} //
    class Or: Node{} //
    class Return: Node{} //
    class VarDef: Node{} //
}