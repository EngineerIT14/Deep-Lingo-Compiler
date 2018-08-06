/*
Deep Lingo compiler -  Symbol Global var table class.
2018, ITESM CEM.
* IRVIN EMMANUEL TRUJILLO DÍAZ A01370082
* LUIS FERNANDO ESPINOSA ELIZALDE A01375758

  This file is based on the Buttercup compiler - Symbol table class.
  Copyright (C) 2013 Ariel Ortiz, ITESM CEM
  
              
              
             **Symbol Global vars TABLE**    
                 ________________
                 |nameVariable  |
                 |nameVariable  |
                 |............. |
                 |nameVariable  |


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
using System.Text;
using System.Collections.Generic;

namespace Deeplingo {

    public class GlobalSymbolVarTable { //symbol table global vars table.

        HashSet<string> tableSet;
        
        public GlobalSymbolVarTable(){
            tableSet = new HashSet<string>();
        }

        //-----------------------------------------------------------
        public override string ToString() {
            var sb = new StringBuilder();
            sb.Append("=======================\n");
            foreach (string entry in tableSet) {
                sb.Append(String.Format("{0}\n", 
                                        entry));
            }
            sb.Append("=======================\n");
            return sb.ToString();
        }
        
        //-----------------------------------------------------------
        public bool Contains(string name) {
            return tableSet.Contains(name);
        }
        
        public bool Add(string name){
            return tableSet.Add(name);
        }
        
         public IEnumerator<string> GetEnumerator() {
            return tableSet.GetEnumerator();
        }
        
    }
}
