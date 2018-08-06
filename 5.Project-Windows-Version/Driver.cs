/*
Deep Lingo compiler - Program driver
2018, ITESM CEM.
* IRVIN EMMANUEL TRUJILLO DÍAZ A01370082
* LUIS FERNANDO ESPINOSA ELIZALDE A01375758

  This file is based on the Buttercup compiler - Program driver by Ariel Ortiz,
  Copyright (C) 2013.

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
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Deeplingo {


    public class Driver {

        const string VERSION = "0.5";

        //-----------------------------------------------------------
        static readonly string[] ReleaseIncludes = {
            "Lexical analysis",
            "Syntactic analysis",
            "AST construction",
            "Semantic analysis",
            "CIL Code generation"
        };

        //-----------------------------------------------------------
        void PrintAppHeader() {
            Console.WriteLine("Deep Lingo compiler , version " + VERSION);
            Console.WriteLine("Copyright \u00A9 2017 by I.Trujillo and L.Espinosa, ITESM CEM."                
            );
            Console.WriteLine("This compiler is based on the Buttercup compiler by A.Ortiz, Copyright (C) 2013.");
            Console.WriteLine("This program is free software; you may "
                + "redistribute it under the terms of");
            Console.WriteLine("the GNU General Public License version 3 or "
                + "later.");
            Console.WriteLine("This program has absolutely no warranty.");
        }

        //-----------------------------------------------------------
        void PrintReleaseIncludes() {
            Console.WriteLine("Included in this release:");            
            foreach (var phase in ReleaseIncludes) {
                Console.WriteLine("   * " + phase);
            }
        }

       //-----------------------------------------------------------
        void Run(string[] args) {

            PrintAppHeader();
            Console.WriteLine();
            PrintReleaseIncludes();
            Console.WriteLine();

            if (args.Length != 2) {
                Console.Error.WriteLine(
                    "Please specify the name of the input file.");
                Environment.Exit(1);
            }

            try {            
                var inputPath = args[0]; 
                var outputPath = args[1];
                var input = File.ReadAllText(inputPath);
                var parser = new Parser(new Scanner(input).Start().GetEnumerator());
                var program = parser.Program();//it returns a node of nodes :v (AN AST).
               //Console.WriteLine(program.ToStringTree());
                Console.WriteLine("Syntax OK.");
            
                var semanticGlobal = new SemanticAnalyzerGlobal();
                semanticGlobal.Visit((dynamic) program);  //It casts with dynamic type so a node can be converted depending the type and token category of the node of the ast :D
                var semanticsLocal = new SemanticAnalyzerLocal(semanticGlobal.TableGlobalFunctions, semanticGlobal.TableSymbolsGlobalVars);
                semanticsLocal.Visit((dynamic) program);
                
                Console.WriteLine("Semantics OK.");
                Console.WriteLine();
                
            
                // ContainerOfArrays valorObtenido = ContainerOfArrays.GetAndDeleteElementFromSet(semanticGlobal.TableGlobalFunctions.getTable(), "assert");
                // Console.WriteLine("Este es el elemento que fue recuperado y borrado del set: [" + String.Join(",", valorObtenido.CustomArray) + "]:v");
                // GlobalLocalTableStructure tabla = (GlobalLocalTableStructure) valorObtenido.CustomArray[3];
                // Console.WriteLine(tabla.Contains("perros"));
                // tabla.Add(new ContainerOfArrays(new List<object>() { "funcionX", ":v" }));
                // Console.WriteLine("Este es el elemento que fue recuperado y borrado del set: [" + String.Join(",", valorObtenido.CustomArray) + "]:v");*/
                // Console.WriteLine("Global Function Table Table\n");
                // Console.WriteLine("====================\n");
                // Console.WriteLine(semanticGlobal.TableGlobalFunctions.ToString());
                // Console.WriteLine("==================== Note: Ignore one \"End of Referenced Table of current function :v\" that is above this line :v\n");
                // Console.WriteLine("Global Symbol Var Table\n");
                // Console.WriteLine(semanticGlobal.TableSymbolsGlobalVars.ToString());
                // Console.WriteLine("Global Function Table");
                // Console.WriteLine("=====================");
                // foreach (var entry in semanticGlobal.TableGlobalFunctions) {
                //     Console.WriteLine(entry);                        
                // }
                
                Console.WriteLine("============");
             
                CILGenerator codeGenerator = new CILGenerator(semanticGlobal.TableGlobalFunctions,semanticGlobal.TableSymbolsGlobalVars);
                File.WriteAllText(outputPath,codeGenerator.Visit((dynamic) program));
                Console.WriteLine("Generated CIL code to '" + outputPath + "'.");
                Console.WriteLine();
                
            
                
            } catch (Exception e) {

                if (e is FileNotFoundException 
                    || e is SyntaxError 
                    || e is SemanticError) {
                    Console.Error.WriteLine(e.Message);
                    Environment.Exit(1);
                }

                throw;
            }
        }

        //-----------------------------------------------------------
        public static void Main(string[] args) {
            new Driver().Run(args);
        }
    }
}

