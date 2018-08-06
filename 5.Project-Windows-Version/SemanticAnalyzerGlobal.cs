/*
Deep Lingo compiler - Semantic analyzer Global.
2018, ITESM CEM.
* IRVIN EMMANUEL TRUJILLO DÍAZ A01370082
* LUIS FERNANDO ESPINOSA ELIZALDE A01375758


Description of this class: Basically, this semantic analyzer 
looks for global variables and functions in a AST and it stores it them in a set.

              
             **Symbol Global vars TABLE**    
                 ________________
                 |nameVariable  |
                 |nameVariable  |
                 |............. |
                 |nameVariable  |
                 
                             **Symbol Global Function TABLE**    
_____________________________________________________________________________________________________________________________
| nameFunction  |predifined or user defined| Arity(number of parameters) |  instance of the class GlobalLocalTableStructure |<--This will be a list that is an atrribute of the "ContainerOfArrays" instance.
| nameFunction  |predifined or user defined| Arity(number of parameters) |  instance of the class GlobalLocalTableStructure |<--This will be a list that is an atrribute of the "ContainerOfArrays" instance.
| ..........   |...........................| ...........................
| nameFunction  |predifined or user defined| Arity(number of parameters) |  instance of the class GlobalLocalTableStructure |<--This will be a list that is an atrribute of the "ContainerOfArrays" instance.





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


    class SemanticAnalyzerGlobal {
        //attributes
        ContainerOfArrays container;
        bool flagMain = false; //the program contains a main?
        bool flagIsMain = false; //the current function is main?
     
        
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
        //constructor
        public SemanticAnalyzerGlobal() {
            TableGlobalFunctions = new GlobalLocalTableStructure();
            TableSymbolsGlobalVars = new GlobalSymbolVarTable();
            TableGlobalFunctions.Add(new ContainerOfArrays(new List<object>() { "printi", "P", "1", "-" }));
            TableGlobalFunctions.Add(new ContainerOfArrays(new List<object>() { "printc", "P", "1", "-" }));
            TableGlobalFunctions.Add(new ContainerOfArrays(new List<object>() { "prints", "P", "1", "-" }));
            TableGlobalFunctions.Add(new ContainerOfArrays(new List<object>() { "println", "P", "0", "-" }));
            TableGlobalFunctions.Add(new ContainerOfArrays(new List<object>() { "readi", "P", "0", "-" }));
            TableGlobalFunctions.Add(new ContainerOfArrays(new List<object>() { "reads", "P", "0", "-" }));
            TableGlobalFunctions.Add(new ContainerOfArrays(new List<object>() { "printi", "P", "0", "-" }));
            TableGlobalFunctions.Add(new ContainerOfArrays(new List<object>() { "new", "P", "1", "-" }));
            TableGlobalFunctions.Add(new ContainerOfArrays(new List<object>() { "size", "P", "1", "-" }));
            TableGlobalFunctions.Add(new ContainerOfArrays(new List<object>() { "add", "P", "2", "-" }));
            TableGlobalFunctions.Add(new ContainerOfArrays(new List<object>() { "get", "P", "2", "-" }));
            TableGlobalFunctions.Add(new ContainerOfArrays(new List<object>() { "set", "P", "3", "-" }));
        }

        //-----------------------------------------------------------
        public void Visit(Program node) {
            Visit((dynamic) node[0]); //visit first children :v
            if(!flagMain){
                throw new SemanticError(
                    "Main function is missing");
            }
        }

        //-----------------------------------------------------------
        public void Visit(DefList node) {
            VisitChildren(node);
        }

        //-----------------------------------------------------------
        public void Visit(IdList node) {
            VisitChildren(node);
        }
        
        //-----------------------------------------------------------
        public void Visit(VarDef node) {
            string variableName = node.AnchorToken.Lexeme;
            if (this.TableSymbolsGlobalVars.Contains(variableName)) {
                throw new SemanticError(
                    "Duplicated variable: " + variableName,
                    node.AnchorToken);
            }
            else {
                TableSymbolsGlobalVars.Add(variableName);
            }
        }

        //-----------------------------------------------------------
        public void Visit(FunDef node) {
              
            string functionName = node.AnchorToken.Lexeme;
            if(functionName == "main"){
                flagMain = true;
                flagIsMain = true;
            }
            if (TableGlobalFunctions.Contains(functionName)) {
                throw new SemanticError(
                    "Duplicated function: " + functionName,
                    node.AnchorToken);

            } else {
                container = new ContainerOfArrays(new List<object>() { functionName, "U" });
                Visit((dynamic) node[0]);
            }
        }

        //-----------------------------------------------------------
        public void Visit(ParamList node) {
              
            int sons=0, parameters=0;
            foreach (var n in node) {
                sons += 1;
            }
            if(sons>0){
                if(flagIsMain){
                    throw new SemanticError(
                    "Main can't have parameters");
                }
                foreach (var n in node[0]) {
                    parameters += 1;
                }
                if(parameters>0){
                    container.CustomArray.Add(parameters.ToString());
                }
                else{
                    container.CustomArray.Add("1");
                }
            }
            else{
                container.CustomArray.Add("0");
            }
            container.CustomArray.Add(new GlobalLocalTableStructure());
            TableGlobalFunctions.Add(container);
            flagIsMain = false;
        }

        //-----------------------------------------------------------
        void VisitChildren(Node node) { //visit all children :v
            foreach (var n in node) {
                Visit((dynamic) n);
            }
        }
    }
}
