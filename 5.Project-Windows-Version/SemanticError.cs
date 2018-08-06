/*
Deep Lingo compiler -  Semantic error exception class
2018, ITESM CEM.
* IRVIN EMMANUEL TRUJILLO DÍAZ A01370082
* LUIS FERNANDO ESPINOSA ELIZALDE A01375758

  This file is based on the Buttercup compiler - Semantic error exception class.
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

namespace Deeplingo {

    class SemanticError: Exception {

        public SemanticError(string message, Token token):
            base(String.Format(
                "Semantic Error: {0} \n" +
                "at row {1}, column {2}.",
                message,
                token.Row,
                token.Column)) {
        }
        
        //Error that only receives a string
        public SemanticError(string message):
            base(String.Format(
                "Semantic Error: {0}",
                message)) {
        }
    }
}
