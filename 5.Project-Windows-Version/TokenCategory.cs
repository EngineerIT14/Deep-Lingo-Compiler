/*
Deep Lingo compiler - Token categories for the scanner.
2018, ITESM CEM.
* IRVIN EMMANUEL TRUJILLO DÍAZ A01370082
* LUIS FERNANDO ESPINOSA ELIZALDE A01375758

  This file is based on the Buttercup compiler - Token categories for the scanner.
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

namespace Deeplingo {

    enum TokenCategory {
        AND,
        ASSIGN,
        BREAK,
        CHAR_LITERAL,
        COMMA,
        DECREMENT,
        DIV,
        DOUBLE_QUOT,
        ELSE,
        ELSE_IF,
        EOF,
        EQUAL,
        GREATER,
        GREATER_EQUAL,
        IDENTIFIER,
        IF,
        ILLEGAL_CHAR,
        INCREMENT,
        INT_LITERAL,
        KEY_OPEN,
        KEY_CLOSED,
        LESS,
        LESS_EQUAL,
        LOOP,
        MUL,
        NEG,
        NOT,
        NOT_EQUAL,
        OR,
        PARENTHESIS_OPEN,
        PARENTHESIS_CLOSE,
        PLUS,
        RETURN,
        REMAINDER,
        SQUARE_BRACKET_OPEN,
        SQUARE_BRACKET_CLOSE,
        SEMICOLON,
        SINGLE_QUOT,
        STRING_LITERAL,
        VAR
    }
}

