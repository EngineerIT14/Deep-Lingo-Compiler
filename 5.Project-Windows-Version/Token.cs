/*
Deep Lingo compiler - Token class for the scanner.
2018, ITESM CEM.
* IRVIN EMMANUEL TRUJILLO DÍAZ A01370082
* LUIS FERNANDO ESPINOSA ELIZALDE A01375758

  This file is based on the  Buttercup compiler - Token class for the scanner.
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

    class Token {

        readonly string lexeme;

        readonly TokenCategory category;

        readonly int row;

        readonly int column;

        public string Lexeme { 
            get { return lexeme; }
        }

        public TokenCategory Category {
            get { return category; }          
        }

        public int Row {
            get { return row; }
        }

        public int Column {
            get { return column; }
        }

        public Token(string lexeme, 
                     TokenCategory category, 
                     int row, 
                     int column) {
            this.lexeme = lexeme;
            this.category = category;
            this.row = row;
            this.column = column;
        }
      
        public override string ToString() {
            return string.Format("{{{0}, \"{1}\", @({2}, {3})}}",
                                 category, lexeme, row, column);
        }
    }
}

