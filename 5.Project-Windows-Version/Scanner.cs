/*
Deep Lingo compiler - This class performs the lexical analysis, (a.k.a scanning).
2018, ITESM CEM.
* IRVIN EMMANUEL TRUJILLO DÍAZ A01370082
* LUIS FERNANDO ESPINOSA ELIZALDE A01375758

  This file is based on the   Buttercup compiler - class that performs the lexical analysis, 
  (a.k.a. scanning). Copyright (C) 2013 Ariel Ortiz, ITESM CEM
  
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
using System.Text;
using System.Text.RegularExpressions;

namespace Deeplingo {

    class Scanner {

        readonly string input;
        //**regular expressions**.
        //with the configurations that were made,
                                                  //it is possible to give a name 
                                                  //to the regular expression like this:?<nombre>.
                                                  //next will be the regular expression. So the regex will be a definition of that name
                                                  //spaces are ignored (you can use the /s).
                                                  //the '|' means an or expression.
        static readonly Regex regex = new Regex(  //Regex rule: Larger Regex need to be evaluated first.
            @"                             
                (?<EqualTo>         [=][=]                                   )
              | (?<NotEqual>        [!][=]                                   )
              | (?<Or>              [|][|]                                   )
              | (?<And>             [&][&]                                   )
              | (?<GreaterEqual>    [>][=]                                   )
              | (?<LessEqual>       [<][=]                                   )
              | (?<Increment>       [+][+]                                   )
              | (?<Decrement>       [-][-]                                   )
              | (?<Assign>          [=]                                      )
              | (?<Comment>         [/][*](.|\n)*?[*][/])|([/][/].*          )
              | (?<Identifier>      [a-zA-Z][a-zA-Z_0-9]*                    )
              | (?<IntLiteral>      -?\d+                                    ) 
              | (?<Not>             [!]                                      )
              | (?<Less>            [<]                                      )
              | (?<Greater>         [>]                                      )
              | (?<Mul>             [*]                                      )
              | (?<Div>             [/]                                      )
              | (?<Neg>             [-]                                      )
              | (?<Plus>            [+]                                      )
              | (?<Remainder>       [%]                                      )
              | (?<CharacterLiteral> ('([^\\'\n]   |
                                       \\[nrt\\'""] |
                                       \\u[\da-fA-F]{6})')                      )
              | (?<StringLiteral>    (""([^\\""\n] |
                                      \\[nrt\\'""] |
                                      \\u[\da-fA-F]{6})*"")                     ) # The {6} is a quantifier. the \\ is needed in order to prevent an exception during the parsing process, We are using a verbatim string so we need to represent the single quote with a double quote 
              | (?<Newline>         \n                                       )
              | (?<CarriageReturn>  \r                                       )
              | (?<Tab>             \t                                       )
              | (?<Backslash>       \\                                       )
              | (?<SingleQuote>     \'                                       )
              | (?<DoubleQuote>     \""                                      )
              | (?<UnicodeCharacter> \\u[\da-fA-F]{6}                           )    
              | (?<ParLeft>         [(]                                      )
              | (?<ParRight>        [)]                                      )
              | (?<KeyLeft>         [{]                                      )
              | (?<KeyRight>        [}]                                      )
              | (?<SquareBLeft>     [[]                                      )
              | (?<SquareBRight>    []]                                      )
              | (?<Semicolon>       [;]                                      )
              | (?<Comma>           [,]                                      )
              | (?<WhiteSpace>      \s                                       )     # Must go anywhere after Newline. SPACES, TABS
              | (?<Other>           .                                        )     # Must be last: match any other character.
            ", 
            //Second arguments: flags of the format of the regular expressions
            RegexOptions.IgnorePatternWhitespace 
                | RegexOptions.Compiled //run faster.
                | RegexOptions.Multiline //Allows multiline in the string-.
            );
        
        //**KEYWORDS**
        //idictionary works as a  'map function' of other programming languages.
        static readonly IDictionary<string, TokenCategory> keywords =
            new Dictionary<string, TokenCategory>() {
                {"break", TokenCategory.BREAK},
                {"else", TokenCategory.ELSE},
                {"elseif", TokenCategory.ELSE_IF},
                {"if", TokenCategory.IF},
                {"loop", TokenCategory.LOOP},
                {"return", TokenCategory.RETURN},
                {"var", TokenCategory.VAR}
            };
      //red elements in the document (nonkewords)
        static readonly IDictionary<string, TokenCategory> nonKeywords =
            new Dictionary<string, TokenCategory>() {
                {"And", TokenCategory.AND},
                {"Assign", TokenCategory.ASSIGN},
                {"CharacterLiteral", TokenCategory.CHAR_LITERAL},
                {"Comma", TokenCategory.COMMA},
                {"Div", TokenCategory.DIV},
                {"Decrement", TokenCategory.DECREMENT},
                {"DoubleQuotes", TokenCategory.DOUBLE_QUOT},
                {"EqualTo", TokenCategory.EQUAL},
                {"Greater", TokenCategory.GREATER},
                {"GreaterEqual", TokenCategory.GREATER_EQUAL},
                {"Increment", TokenCategory.INCREMENT},
                {"IntLiteral", TokenCategory.INT_LITERAL},
                {"KeyLeft", TokenCategory.KEY_OPEN},
                {"KeyRight", TokenCategory.KEY_CLOSED},
                {"Less", TokenCategory.LESS},
                {"LessEqual", TokenCategory.LESS_EQUAL},
                {"Mul", TokenCategory.MUL},
                {"Neg", TokenCategory.NEG},
                {"Not", TokenCategory.NOT},
                {"NotEqual", TokenCategory.NOT_EQUAL},
                {"Or", TokenCategory.OR},
                {"ParLeft", TokenCategory.PARENTHESIS_OPEN},
                {"ParRight", TokenCategory.PARENTHESIS_CLOSE},
                {"Plus", TokenCategory.PLUS},
                {"Remainder", TokenCategory.REMAINDER},
                {"Semicolon", TokenCategory.SEMICOLON},
                {"SingleQuote", TokenCategory.SINGLE_QUOT},
                {"StringLiteral", TokenCategory.STRING_LITERAL},
                {"SquareBLeft", TokenCategory.SQUARE_BRACKET_OPEN},
                {"SquareBRight", TokenCategory.SQUARE_BRACKET_CLOSE}
            };

        public Scanner(string input) {
            this.input = input;
        }
        
        //Compares each element inside the input with the specified keywords 
        //and non keywords tokens using Regex format. Then it prints each 
        //element according with its token category with its row & column.
        public IEnumerable<Token> Start() {

            var row = 1;
            var columnStart = 0;
                                                        //anonymous function
            Func<Match, TokenCategory, Token> newTok = (m, tc) =>
                new Token(m.Value, tc, row, m.Index - columnStart + 1);
        /**Evaluation of elements identified**/
            foreach (Match m in regex.Matches(input)) { //m is an element that accomplish one of the regex statements.
               //Groups is like a dictionary that it will have the regex definitions.
                //If it finds a new line. Increase the row number and the columnstart will be
                //at the index position+lenght of the \n so we can have the count of the exact position of the column.
                if (m.Groups["Newline"].Success) {
                    // Found a new line.
                    row++;
                    columnStart = m.Index + m.Length;
                }
                else if (m.Groups["WhiteSpace"].Success) {
                     // Skip white space
                }
               else if(m.Groups["Comment"].Success){
                    row += Regex.Matches(m.Groups["Comment"].Value, "\n").Count;  //m is a complete comment, if  \n appears in the comment, count it and append it to the row.
            
                } else if (m.Groups["Identifier"].Success) {

                    if (keywords.ContainsKey(m.Value)) {

                        // Matched string is a Deeplingo keyword.
                        yield return newTok(m, keywords[m.Value]);                                               

                    } else { 

                        // Otherwise it's just a plain identifier.
                        yield return newTok(m, TokenCategory.IDENTIFIER);
                    }

                } else if (m.Groups["Other"].Success) {

                    // Found an illegal character.
                    yield return newTok(m, TokenCategory.ILLEGAL_CHAR);

                } else {

                    // Match must be one of the non keywords.
                    foreach (var name in nonKeywords.Keys) {
                        if (m.Groups[name].Success) {
                            yield return newTok(m, nonKeywords[name]);
                            break;
                        }
                    }
                }
            }

            yield return new Token(null, 
                                   TokenCategory.EOF, 
                                   row, 
                                   input.Length - columnStart + 1);
        }
    }
}