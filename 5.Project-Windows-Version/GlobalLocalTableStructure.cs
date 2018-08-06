/*
  Deep Lingo compiler - Semantic analyzer Global.
2018, ITESM CEM.
* IRVIN EMMANUEL TRUJILLO DÍAZ A01370082
* LUIS FERNANDO ESPINOSA ELIZALDE A01375758


Description of this class: This class represents the structure of a table
, in this case, the global function table and the local var table



                              **Symbol Global Function TABLE**           
________________________________________________________________________
| nameFunction  |predifined or user defined| Arity(number of parameters) |  instance of the class GlobalLocalTableStructure |<--This will be a list that is an atrribute of the "ContainerOfArrays" instance.
| nameFunction  |predifined or user defined| Arity(number of parameters) |  instance of the class GlobalLocalTableStructure |<--This will be a list that is an atrribute of the "ContainerOfArrays" instance.
| ..........   |...........................| ...........................
| nameFunction  |predifined or user defined| Arity(number of parameters) |  instance of the class GlobalLocalTableStructure |<--This will be a list that is an atrribute of the "ContainerOfArrays" instance.



                              **SYMBOL LOCAL TABLE**           
________________________________________________________________________
|name          |parameter or local variable| POSITION IN THE PARAM LIST   <--This will be a list that is an atrribute of the "ContainerOfArrays" instance. 
|name          |parameter or local variable| POSITION IN THE PARAM LIST   <--This will be a list that is an atrribute of the "ContainerOfArrays" instance. 
| ..........   |...........................| ...........................
|name          |parameter or local variable| POSITION IN THE PARAM LIST   <--This will be a list that is an atrribute of the "ContainerOfArrays" instance. 



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

    public class GlobalLocalTableStructure { //this is for the Symbol Global Function TABLE and SYMBOL LOCAL TABLE. 

        HashSet<ContainerOfArrays> tableSet = new HashSet<ContainerOfArrays>();
        
        //-----------------------------------------------------------
        public override string ToString() {
            var sb = new StringBuilder();
            foreach (ContainerOfArrays entry in tableSet) {
                sb.Append("|" + String.Join(", ",entry.CustomArray) + "|\n");
            }
            sb.Append("End of Referenced Table of current function :V");
            return sb.ToString();
        }

        //-----------------------------------------------------------
        public bool Contains(string name) {
            return tableSet.Contains(new ContainerOfArrays(new List<object>() { name }));
        }
        
        public bool Add(ContainerOfArrays container){
              return tableSet.Add(container);
        }
        
        public HashSet<ContainerOfArrays> getTable(){
            return tableSet;
        }
        
        
    }
}