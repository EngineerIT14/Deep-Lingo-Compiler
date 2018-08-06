/*
Deep Lingo compiler - Semantic analyzer.
2018, ITESM CEM.
* IRVIN EMMANUEL TRUJILLO DÍAZ A01370082
* LUIS FERNANDO ESPINOSA ELIZALDE A01375758


Description of this class:Basically, this semantic analyzer looks 
for local variables in a AST in a specfic function and it stores it them in a set.

The set looks like this:

                           **SYMBOL LOCAL TABLE**           
________________________________________________________________________
|name          |parameter or local variable| POSITION IN THE PARAM LIST   <--This will be a list that is an atrribute of the "ContainerOfArrays" instance. 
|name          |parameter or local variable| POSITION IN THE PARAM LIST   <--This will be a list that is an atrribute of the "ContainerOfArrays" instance. 
| ..........   |...........................| ...........................
|name          |parameter or local variable| POSITION IN THE PARAM LIST   <--This will be a list that is an atrribute of the "ContainerOfArrays" instance. 

Note: We created the class "ContainerOfArrays"  and
each element will be an instance of that class so it can be compared in the set, 
this was done in orde to solve the problem of storing arrays in a set.

  This file is based on the Buttercup compiler - Semantic analyzer.
  Copyright (C) 2013 Ariel Ortiz, ITESM CEM
  
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


using System;
using System.Collections.Generic;

namespace Deeplingo {

    class SemanticAnalyzerLocal {
        
        ContainerOfArrays container;
        GlobalLocalTableStructure globalTableFuncs;
        GlobalSymbolVarTable globalTableVars;
        GlobalLocalTableStructure localFunctionTable;
        bool inFunction = false;
        bool inFunctionCall = false;
        List<int> numberArguments = new List<int>();
        int numberFunCall = 0;
        int numberLoops = 0; //it is useful for the "break" statemnt
        
        //-----------------------------------------------------------
        //getter and setter
        public GlobalLocalTableStructure TableGlobalFunctions {
            get;
            private set;
        }
            
        //-----------------------------------------------------------
        //getter and setter
        public GlobalSymbolVarTable TableSymbolsGlobalVars {
            get;
            private set;
        }
        
        //-----------------------------------------------------------
        //Constructor.
        public SemanticAnalyzerLocal(GlobalLocalTableStructure TableGlobalFunctions, GlobalSymbolVarTable TableSymbolsGlobalVars) {
            globalTableFuncs = TableGlobalFunctions;
            globalTableVars = TableSymbolsGlobalVars;
        }

        /*
        Note: Each visit is for each node indicated in the SpecificNode.cs
        */
        //-----------------------------------------------------------
        public void Visit(Program node) {
            Visit((dynamic) node[0]); //visit first children :v
        }
        
        //-----------------------------------------------------------
        public void Visit(DefList node) {
            VisitChildren(node);
        }

        //-----------------------------------------------------------
        //-----------------------------------------------------------
        public void Visit(IdList node) {
            VisitChildren(node);
        }
        
        //-----------------------------------------------------------
        public void Visit(VarDef node) {
            if(inFunction){
                var variableName = node.AnchorToken.Lexeme;
                
                if (localFunctionTable.Contains(variableName)) {
                    throw new SemanticError(
                        "Duplicated variable: " + variableName,
                        node.AnchorToken);
                }
                else {
                    localFunctionTable.Add(new ContainerOfArrays(new List<object>() { variableName, "local", "-" }));
                }
            }
        }

        //-----------------------------------------------------------
        public void Visit(FunDef node) {
             inFunction = true;
             var fuctionName = node.AnchorToken.Lexeme;
             container = ContainerOfArrays.GetAndDeleteElementFromSet(globalTableFuncs.getTable(), fuctionName);
             container.CustomArray[3] = new GlobalLocalTableStructure(); //creating a new table for the current function...
             localFunctionTable = (GlobalLocalTableStructure) container.CustomArray[3]; //si ya vi, es que customArray guarda objects..
             VisitChildren(node);
             globalTableFuncs.Add(container);
             inFunction = false;
        }

        //-----------------------------------------------------------
        public void Visit(ParamList node) {
              
            int sons=0, parameters=0;
            foreach (var n in node) {
                sons += 1;
            }
            if(sons>0){
                foreach (var n in node[0]) {
                    var parameterName = n.AnchorToken.Lexeme;
                    if (localFunctionTable.Contains(parameterName)) {
                        throw new SemanticError(
                            "Duplicated variable: " + parameterName,
                            n.AnchorToken);
                    }
                    else {
                        localFunctionTable.Add(new ContainerOfArrays(new List<object>() { parameterName, "parameter", parameters.ToString() }));
                        parameters += 1;
                    }
                }
                if(parameters==0){
                    var parameterName = node[0].AnchorToken.Lexeme;
                    if (localFunctionTable.Contains(parameterName)) {
                        throw new SemanticError(
                            "Duplicated variable: " + parameterName,
                            node[0].AnchorToken);
                    }
                    else {
                        localFunctionTable.Add(new ContainerOfArrays(new List<object>() { parameterName, "parameter", "0" }));
                    }
                }
            }
        }
        
        //-----------------------------------------------------------
        public void Visit(VarDefList node) {
            VisitChildren(node);
        }
        
        //-----------------------------------------------------------
        public void Visit(StmtList node) {
            VisitChildren(node);
        }
        
        //-----------------------------------------------------------
        public void Visit(FunCall node) {
            var functionName = node.AnchorToken.Lexeme;
            ContainerOfArrays containerFun;
            numberFunCall += 1;
            if(numberFunCall > numberArguments.Count){
                numberArguments.Add(0);
            }
            numberArguments[numberFunCall-1] = 0;
            if(!globalTableFuncs.Contains(functionName)){
                if(!(((string)container.CustomArray[0]).Equals(functionName))){
                    throw new SemanticError(
                        "Undeclared function called: " + functionName,
                        node.AnchorToken);
                }
                else{
                    inFunctionCall = true;
                    numberArguments[numberFunCall-1] += 1;
                    Visit((dynamic) node[0]);
                    inFunctionCall = false;
                    if(!((string)container.CustomArray[2]).Equals(numberArguments[numberFunCall-1].ToString())){
                        throw new SemanticError(
                            "The function call receives a different number of parameters: " + functionName,
                            node.AnchorToken);
                    }
                }
            }
            else{
                containerFun = ContainerOfArrays.GetAndDeleteElementFromSet(globalTableFuncs.getTable(), functionName);
                inFunctionCall = true;
                numberArguments[numberFunCall-1] += 1;
                Visit((dynamic) node[0]);
                inFunctionCall = false;
                if(!((string)containerFun.CustomArray[2]).Equals(numberArguments[numberFunCall-1].ToString())){
                        throw new SemanticError(
                            "The function call receives a different number of parameters: " + functionName,
                            node.AnchorToken);
                }
                globalTableFuncs.Add(containerFun);
            }
            numberFunCall -= 1;
            
        }
        
        //-----------------------------------------------------------
        public void Visit(ExprList node) {
            if(inFunctionCall){
                numberArguments[numberFunCall-1] = 0;
                foreach(var n in node){
                    Visit((dynamic) n);
                    numberArguments[numberFunCall-1] += 1;
                }
            }
            else{
                VisitChildren(node);
            }
        }
        
        //-----------------------------------------------------------
        public void Visit(Or node) {
            VisitChildren(node);
        }
        
        //-----------------------------------------------------------
        public void Visit(And node) {
            VisitChildren(node);
        }
        
        //-----------------------------------------------------------
        public void Visit(OpComp node) {
            VisitChildren(node);
        }
        
        //-----------------------------------------------------------
        public void Visit(OpRel node) {
            VisitChildren(node);
        }
        
        //-----------------------------------------------------------
        public void Visit(OpAdd node) {
            VisitChildren(node);
        }
        
        //-----------------------------------------------------------
        public void Visit(OpMul node) {
            VisitChildren(node);
        }
        
        //-----------------------------------------------------------
        public void Visit(OpUnary node) {
            Visit((dynamic) node[0]);
        }
        
        //-----------------------------------------------------------
        public void Visit(Identifier node) {
            var identifierName = node.AnchorToken.Lexeme;
            if (!localFunctionTable.Contains(identifierName)) {
                   if(!globalTableVars.Contains(identifierName)) {
                       throw new SemanticError(
                        "Undeclared variable: " + identifierName,
                        node.AnchorToken);
                   }
            }
        }
        
        //-----------------------------------------------------------
        public void Visit(Array node) {
            Visit((dynamic) node[0]);
        }
     
        //-----------------------------------------------------------
        public void Visit(StringLiteral node) {}
        
        //-----------------------------------------------------------
        public void Visit(CharLiteral node) {}
        
        //-----------------------------------------------------------
        public void Visit(IntLiteral node) {
            var intLiteral = node.AnchorToken.Lexeme;
            int number;
            if(!Int32.TryParse(intLiteral, out number)){
                throw new SemanticError(
                    "Int Literal is larger than the possible 'int32' range: " + intLiteral,
                    node.AnchorToken);
            }
        }
        
        //-----------------------------------------------------------
        public void Visit(If node) {
            VisitChildren(node);
        }
        
        //-----------------------------------------------------------
        public void Visit(ElseIfList node) {
            VisitChildren(node);
        }
        
        //-----------------------------------------------------------
        public void Visit(ElseIf node) {
            VisitChildren(node);
        }
        
        //-----------------------------------------------------------
        public void Visit(Else node) {
            VisitChildren(node);
        }
        
        //-----------------------------------------------------------
        public void Visit(Loop node) {
            numberLoops += 1;
            Visit((dynamic) node[0]);
            numberLoops -= 1;
        }
        
        //-----------------------------------------------------------
        public void Visit(Break node) {
            var breakName = node.AnchorToken.Lexeme;
            if(numberLoops<1){
                throw new SemanticError(
                    "'Break' statement outside of loop" + breakName,
                    node.AnchorToken);
            }
        }
        
        //-----------------------------------------------------------
        public void Visit(Return node) {}
        
        //-----------------------------------------------------------
        public void Visit(StmtEmpty node) {}
        
        //-----------------------------------------------------------
        public void Visit(Assign node) {
            var idName = node.AnchorToken.Lexeme;
            if (!localFunctionTable.Contains(idName) && !globalTableVars.Contains(idName)) {
                throw new SemanticError(
                    "Asignando a variable no declarada: " + idName,
                    node.AnchorToken);
            }
            Visit((dynamic) node[0]);
        }
        
        //-----------------------------------------------------------
        public void Visit(Increment node) {
            Visit((dynamic) node[0]);
        }
        
        //-----------------------------------------------------------
        public void Visit(Decrement node) {
            Visit((dynamic) node[0]);
        }
        
        //-----------------------------------------------------------
        void VisitChildren(Node node) { //visit all children :v
            foreach (var n in node) {
                Visit((dynamic) n);
            }
        }
        
    }
}



