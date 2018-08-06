/*
Deep Lingo compiler -  This class performs the syntactic analysis, (a.k.a. parsing).
2018, ITESM CEM.
* IRVIN EMMANUEL TRUJILLO DÍAZ A01370082
* LUIS FERNANDO ESPINOSA ELIZALDE A01375758

  This file is based on the  Buttercup compiler - class that performs
  the syntactic analysis, (a.k.a. parsing). Copyright (C) 2013 Ariel Ortiz, ITESM CEM
  
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

/*
 * *******************RECURSIVE DESCENT PARSER************************+
 *
 
 
 The names placed here are only for REFERENCE LL(1) , 
 the real names are established in the TokenCategory Class
 
    id-->identifier from the token category
    
    0.BNF (BACKUS-NAUR FORM)
    Syntax defined here: http://webcem01.cem.itesm.mx:8005/s201811/tc3048/DeepLingo/DeepLingo_language_spec.html
    
    First step:
            Convert into LL(1)
    
    1.BNF!!! definitions grouped 
 
 Prog ->Def_list
    Def_list -> Def_list Def | epsilon   //***Left recursion***
    Def -> Var_def | Fun_def 
    Var_def -> "var" Var_list ";" 
    Id_list -> "Id" Id_list_count   
    Id_list_count -> "," "Id" Id_list_count | epsilon    //***Right recursion***
    Fun_def  -> "Id" "(" Param-list ")" "{" Var-def-list Stmt-lst "}"
    Param-list -> Id-list | epsilon   //**EPSILON***
    Var-def-list -> Var_def_list Var-def | epsilon  //***Left recursion***
    Stmt -> Stmt_assign | Stmt_incr | Stmt_decr | Stmt_fun_call | Stmt_if | Stmt_loop | Stmt_break | Stmt_return | Stmt_empty 
    Stmt_assign -> "Id" "=" expr ";"
    Stmt_incr -> "Id" "++" ";"
    Stmt_decr -> "Id" "--" ";"
    Stmt_fun_call -> Fun_call";"
    Fun_call -> "Id" "(" Expr_list ")"
    Expr_list -> Expr Expr_list_count | epsilon //***EPSILON***
    Expr_list_cont -> "," Expr Expr_list_count | epsilon  //**Right recursion***
    Stmt_if -> "if" "(" Expr ")" "{" Stmt_list "}" Else_if_list Else
    Else_if_list -> Else_if_list "Elseif" "(" Expr ")" "{" Stmt_list "}" | epsilon  //***Left recursion***
    Else -> "else" "{" Stmt_list "}" | epsilon  //****EPSILON****
    Stmt_loop -> "loop" "{" Stmt_list "}"
    Stmt_break -> "break" ";"
    Stmt_return -> "return" Expr ";"
    Stmt_empty -> ";"
    Expr -> Expr_or
    Expr_or -> Expr_or "||" Expr_and | Expr_and  //***Left recursion***
    Expr_and -> Expr_and "&&" Expr_comp | Expr_comp  //***Left recursion***
    Expr_comp -> Expr_comp Op_comp Expr_rel | Expr_rel  //***Left recursion***
    Op_comp -> "==" | "!="
    Expr_rel -> Expr_rel Op_rel Expr_add | Expr_add  //***Left recursion***
    Op_rel -> "<" | "<=" | ">" | ">="
    Expr_add -> Expr_add Op_add Expr_mul | Expr_mul //**left recursion**
    Op_add -> "+" | "-"
    Expr_mul -> Expr_mul  Op_mul Expr_unary | Expr_unary   //***Left recursion***
    Op_mul -> "*" | "/" | "%"
    Expr_unary -> Op_unary Expr_unary| Expr_primary //***Right recursion***
    Op_unary -> "+" | "-" | "!" 
    Expr_primary -> "Id" | Fun-call | Array | Lit | "(" expr ")"
    Array -> "["Expr_list"]"
    Lit -> "Lit_int" | "Lit_char" | "Lit_str"
    
    

    2.EBNF (EXTENDED BACKUs-NUAR FORM) - refactoring - (quitando el left recursion o righ recursion dependiendo el caso)

    Prog -> Def_List           
    Def_List -> Def*           
    Def -> Var_Def | Fun_Def
    Var_Def -> "var" Var_List ";" 
    Var_List -> Id_List
    Id_List -> "Id" Id_List_Cont
    Id_List_Cont -> ("," "Id")*
    Fun_Def -> "Id" "(" Param_List ")" "{" Var_Def_List Stmt_List "}"
    Param_List -> Id_List?
    Var_Def_List -> Var_Def*
    Stmt_List -> Stmt*
    Stmt -> Stmt_assign | Stmt_incr | Stmt_decr | Stmt_fun_call | Stmt_if | Stmt_loop | Stmt_break | Stmt_return | Stmt_empty 
    Stmt_Assign -> "Id" "=" Expr ";"
    Stmt_Incr -> "Id" "++" ";"
    Stmt_Decr -> "Id" "--" ";"
    Stmt_Fun_Call -> Fun_Call ";"
    Fun_Call -> "Id" "(" Expr_List ")"
    Expr_List -> (Expr Expr_List_Cont)?
    Expr_List_Cont -> ("," Expr)*
    Stmt_If -> "if" "(" Expr ")" "{" Stmt_List "}" Else_If_List Else
    Else_If_List -> ("elseif "(" Expr ")" "{" Stmt_List "}")*
    Else -> ("else" "{" Stmt_List "}")?
    Stmt_Loop -> "loop" "{" Stmt_List "}"
    Stmt_Break -> "break" ";"
    Stmt_Return -> "return" Expr ";"
    Stmt_Empty -> ";"
    Expr -> Expr_Or
    Expr_Or -> Expr_And ("||" Expr_And)*
    Expr_And -> Expr_Comp ("&&" Expr_Comp)*
    Expr_Comp -> Expr_Rel (Op_Comp Expr_Rel)*
    Op_Comp -> "==" | "!="
    Expr_Rel -> Expr_Add (Op_Rel Expr_Add)*
    Op_Rel -> "<" | "<=" | ">" | ">="
    Expr_Add -> Expr_Mul (Op_Add Expr_Mul)*
    Op_Add -> "+" | "-"
    Expr_Mul -> Expr_Unary (Op_Mul Expr_Unary)*
    Op_Mul -> "*" | "/" | "%"
    Expr_Unary -> OP_Unary* Expr_Primary
    Op_Unary -> "+" | "-" | "!"
    Expr_Primary -> "Id" | Fun_Call | Array | Lit | "(" Expr ")"
    Array -> "[" Expr_List "]"
    Lit -> "Lit_Int" | "Lit_Char" | "Lit_Str"
    
    3.Siempre agregar esto (ya es un LL(1)):
    
    Prog -> Def_List EOF //Program starts with a def-list and ends with an EOF. 
    Def_List -> Def*      
    Def -> Var_Def | Fun_Def
    Var_Def -> "var" Var_List ";" 
    Var_List -> Id_List
    Id_List -> "Id" Id_List_Cont
    Id_List_Cont -> ("," "Id")*
    Fun_Def -> "Id" "(" Param_List ")" "{" Var_Def_List Stmt_List "}"
    Param_List -> Id_List?
    Var_Def_List -> Var_Def*
    Stmt_List -> Stmt*
    Stmt -> Stmt_assign | Stmt_incr | Stmt_decr | Stmt_fun_call | Stmt_if | Stmt_loop | Stmt_break | Stmt_return | Stmt_empty 
    Stmt_Assign -> "Id" "=" Expr ";"
    Stmt_Incr -> "Id" "++" ";"
    Stmt_Decr -> "Id" "--" ";"
    Stmt_Fun_Call -> Fun_Call ";"
    Fun_Call -> "Id" "(" Expr_List ")"
    Expr_List -> (Expr Expr_List_Cont)?
    Expr_List_Cont -> ("," Expr)*
    Stmt_If -> "if" "(" Expr ")" "{" Stmt_List "}" Else_If_List Else
    Else_If_List -> ("elseif" "(" Expr ")" "{" Stmt_List "}")*
    Else -> ("else" "{" Stmt_List "}")?
    Stmt_Loop -> "loop" "{" Stmt_List "}"
    Stmt_Break -> "break" ";"
    Stmt_Return -> "return" Expr ";"
    Stmt_Empty -> ";"
    Expr -> Expr_Or
    Expr_Or -> Expr_And ("||" Expr_And)*
    Expr_And -> Expr_Comp ("&&" Expr_Comp)*
    Expr_Comp -> Expr_Rel (Op_Comp Expr_Rel)*
    Op_Comp -> "==" | "!="
    Expr_Rel -> Expr_Add (Op_Rel Expr_Add)*
    Op_Rel -> "<" | "<=" | ">" | ">="
    Expr_Add -> Expr_Mul (Op_Add Expr_Mul)*
    Op_Add -> "+" | "-"
    Expr_Mul -> Expr_Unary (Op_Mul Expr_Unary)*
    Op_Mul -> "*" | "/" | "%"
    Expr_Unary -> Op_Unary* Expr_Primary
    Op_Unary -> "+" | "-" | "!"
    Expr_Primary -> "Id" | Fun_Call | Array | Lit | "(" Expr ")"
    Array -> "[" Expr_List "]"
    Lit -> "Lit_Int" | "Lit_Char" | "Lit_Str"

    4. LL1 DONE! :D
*/

using System;
using System.Collections.Generic;

namespace Deeplingo {
    
    class Parser {   
      
      bool definition = false; //Tells if a variable is going to be declared or called
        //***START-attributes*********************
        //**SETS***
        
        //Definition: Variable or function names
        static readonly ISet<TokenCategory> firstOfDefinition =
            new HashSet<TokenCategory>() {
                TokenCategory.VAR,
                TokenCategory.IDENTIFIER
            };

        //STATEMENTS, FUNCTIONS, ETC...
        static readonly ISet<TokenCategory> firstOfStatement =
            new HashSet<TokenCategory>() {
                TokenCategory.IDENTIFIER,
                TokenCategory.IF,
                TokenCategory.LOOP,
                TokenCategory.BREAK,
                TokenCategory.RETURN,
                TokenCategory.SEMICOLON
            };
            
        static readonly ISet<TokenCategory> secondOfStatement =
            new HashSet<TokenCategory>() {
                TokenCategory.ASSIGN,
                TokenCategory.INCREMENT,
                TokenCategory.DECREMENT,
                TokenCategory.PARENTHESIS_OPEN
            };
        
        //operators
        static readonly ISet<TokenCategory> firstOfOperatorComp =
            new HashSet<TokenCategory>() {
                TokenCategory.EQUAL,
                TokenCategory.NOT_EQUAL
            };

        static readonly ISet<TokenCategory> firstOfOperatorRel =
            new HashSet<TokenCategory>() {
                TokenCategory.GREATER,
                TokenCategory.LESS,
                TokenCategory.GREATER_EQUAL,
                TokenCategory.LESS_EQUAL
            };
            
        static readonly ISet<TokenCategory> firstOfOperatorAdd =
            new HashSet<TokenCategory>() {
                TokenCategory.PLUS,
                TokenCategory.NEG
            };
        
        static readonly ISet<TokenCategory> firstOfOperatorMul =
            new HashSet<TokenCategory>() {
                TokenCategory.MUL,
                TokenCategory.DIV,
                TokenCategory.REMAINDER
            };
        

        static readonly ISet<TokenCategory> firstOfExpr =
            new HashSet<TokenCategory>() {
                TokenCategory.PLUS,
                TokenCategory.NEG,
                TokenCategory.NOT,
                TokenCategory.IDENTIFIER,
                TokenCategory.SQUARE_BRACKET_OPEN,
                TokenCategory.INT_LITERAL,
                TokenCategory.CHAR_LITERAL,
                TokenCategory.STRING_LITERAL,
                TokenCategory.PARENTHESIS_OPEN
            };
            
        static readonly ISet<TokenCategory> firstOfUnary =
            new HashSet<TokenCategory>() {
                TokenCategory.PLUS,
                TokenCategory.NEG,
                TokenCategory.NOT
            };
            
        static readonly ISet<TokenCategory> firstOfPrimary =
            new HashSet<TokenCategory>() {
                TokenCategory.IDENTIFIER,
                TokenCategory.SQUARE_BRACKET_OPEN,
                TokenCategory.PARENTHESIS_OPEN
            };
            
        static readonly ISet<TokenCategory> literals =
            new HashSet<TokenCategory>() {
                TokenCategory.INT_LITERAL,
                TokenCategory.CHAR_LITERAL,
                TokenCategory.STRING_LITERAL,
            };
        
            
                
        IEnumerator<Token> tokenStream;
        
        //********END ATTRIBUTES************
        
        //Constructor
        public Parser(IEnumerator<Token> tokenStream) {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }

        //Instance Methods
        public TokenCategory CurrentToken {
            get { return tokenStream.Current.Category; }
        }

        public Token Expect(TokenCategory category) {
            if (CurrentToken == category) {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                return current;
            } else {
                throw new SyntaxError(category, tokenStream.Current);                
            }
        }
        //****Methods that represents the tokens of the LL(1) Grammar that we have defined!!!!!!*****
        //LL(1): Prog -> Def_List EOF 
        //AST Transformation: DONE
        public Node Program() {            
            var nodeProgram = new Program(){
              DefList()
            };
            Expect(TokenCategory.EOF);
            return nodeProgram;
        }
        
        //LL(1):Def_List -> Def*
        //AST Transformation: DONE
        public Node DefList(){
          var defListNode = new DefList();
          while(firstOfDefinition.Contains(CurrentToken)){
              defListNode.Add(Def());
          }
          return defListNode;
        }
        
        //LL(1):Def -> Var_Def | Fun_Def
        //AST Transformation: DONE
        public Node Def(){
          switch (CurrentToken) {
            case TokenCategory.VAR:
                return VarDef();

            case TokenCategory.IDENTIFIER:
                return FunDef();

            default:
                throw new SyntaxError(firstOfDefinition, 
                                      tokenStream.Current);
            }
        }
        
        //LL(1):Var_Def -> "var" Var_List ";" 
        //AST Transformation: DONE
        public Node VarDef(){
          Expect(TokenCategory.VAR);
          definition = true; //variables are going to be declared
          var varListNode = VarList();
          Expect(TokenCategory.SEMICOLON);
          definition = false;//variables have already been declared
          return varListNode;
         
        }
        
        //LL(1):Var_List -> Id_List
        //AST Transformation: DONE
        public Node VarList(){
             return IdList();
        }
        
        //LL(1):Id_List -> "Id" Id_List_Cont
              //AST Transformation: DONE
        public Node IdList(){
          Node idNode;
          var idToken = Expect(TokenCategory.IDENTIFIER);
          if(definition){ //this is condition is useful for the semantic analysis stage
            idNode = new VarDef(){ 
              AnchorToken = idToken
            };
          }
          else{
            idNode = new Identifier(){ 
              AnchorToken = idToken
            };
          }
          if(this.CurrentToken == TokenCategory.COMMA){//there is another element..
            return IdListCont(idNode);
          }
          else{
              return idNode;
          }
        }

        //LL(1):Id_List_Cont -> ("," "Id")*
        //AST Transformation: DONE
         public Node IdListCont(Node node){
          var nodeIdListCont = new IdList();
          Node idNode;
          nodeIdListCont.Add(node);
            //LL1****
          while(this.CurrentToken == TokenCategory.COMMA){
             Expect(TokenCategory.COMMA); //expect == advance :v
             if(definition){//this is condition is useful for the semantic analysis stage
              idNode = new VarDef(){ 
                AnchorToken=Expect(TokenCategory.IDENTIFIER)
              };
            }
            else{
              idNode = new Identifier(){ 
                AnchorToken=Expect(TokenCategory.IDENTIFIER)
              };
            }
            nodeIdListCont.Add(idNode);
          } //END LL1.
          return nodeIdListCont;
        }
        
      
         //LL(1):Fun_Def -> "Id" "(" Param_List ")" "{" Var_Def_List Stmt_List "}"
         //AST Transformation: DONE
         public Node FunDef(){
          var idToken = Expect(TokenCategory.IDENTIFIER); //represents the name of the function.
          Expect(TokenCategory.PARENTHESIS_OPEN);
          var paramListNode = ParamList();
          Expect(TokenCategory.PARENTHESIS_CLOSE);
          Expect(TokenCategory.KEY_OPEN);
          var varDefListNode = VarDefList();
          var stmtListNode = StmtList();
          Expect(TokenCategory.KEY_CLOSED);
          var funDefNode = new FunDef(){ 
              paramListNode,
              varDefListNode,
              stmtListNode
          };
          funDefNode.AnchorToken = idToken;
          return funDefNode;
        }
    
        //LL(1):Param_List -> Id_List?
        //AST Transformation: DONE
        public Node ParamList(){
          var paramListNode = new ParamList();
          if(this.CurrentToken == TokenCategory.IDENTIFIER){
            paramListNode.Add(IdList());
          };
          return paramListNode;
        }
        
        //LL(1):Var_Def_List -> Var_Def*
        //AST Transformation: DONE
        public Node VarDefList(){
          var nodeVarDefList = new VarDefList();
          while(this.CurrentToken == TokenCategory.VAR){
              nodeVarDefList.Add(VarDef());
          }
          return nodeVarDefList;
        }
        
        //LL(1):Stmt_List -> Stmt*
        //AST Transformation: DONE
        public Node StmtList(){
          var nodeStmList = new StmtList();
          while(firstOfStatement.Contains(CurrentToken)){ //contains does an expect.
                nodeStmList.Add(Stmt());
          }
          return nodeStmList;
        }
        

        //LL(1):Stmt -> Stmt_assign | Stmt_incr | Stmt_decr | Stmt_fun_call | Stmt_if | Stmt_loop | Stmt_break | Stmt_return | Stmt_empty 
        //AST Transformation: DONE
        public Node Stmt(){
          switch (this.CurrentToken) {

            case TokenCategory.IF:
                return StmtIf();
                

            case TokenCategory.LOOP:
                return StmtLoop();
                
                
            case TokenCategory.BREAK:
                return StmtBreak();
                
                
            case TokenCategory.RETURN:
                return StmtReturn();
                
                
            case TokenCategory.SEMICOLON:
                return StmtEmpty();
                
                
            case TokenCategory.IDENTIFIER:
                var identifierToken = Expect(TokenCategory.IDENTIFIER);
                switch (CurrentToken){
                  
                  case TokenCategory.ASSIGN:
                    var assigmentNode = StmtAssign(); //recibes un nODO assigment 
                    assigmentNode.AnchorToken = identifierToken; //Colocas el identificador como token del nodo assigm,ent recuperado, ver ejemplo del profesor
                    return assigmentNode; //Regresas nodo assigment
                    
                  
                  case TokenCategory.INCREMENT:
                    var nodoIncrement = StmtIncr(); //recibes un nODO Increment
                    var nodoIdentifier1 = new Identifier(){ //Se crea un nodo Identifier
                        AnchorToken = identifierToken
                    };
                    nodoIncrement.Add(nodoIdentifier1); //asignas el nodo Identifier como hijo del nodo Increment
                    return nodoIncrement; //Regresas nodo Increment
             
                  
                  case TokenCategory.DECREMENT: //Hace lo mismo que increment
                    var nodoDecrement = StmtDecr();
                    var nodoIdentifier2 = new Identifier(){ //Se crea un nodo Identifier
                        AnchorToken = identifierToken
                    };
                    nodoDecrement.Add(nodoIdentifier2);
                    return nodoDecrement;
                    
                  
                  case TokenCategory.PARENTHESIS_OPEN:
                    var nodoFuncion = new FunCall(){ //Crea un nodo funcion
                        AnchorToken = identifierToken //asignas identificador como token
                    };
                    nodoFuncion.Add(StmtFunCall()); //se agrega como hijo lo que sea que regrese la funcion
                    return nodoFuncion; //regresa nodo Funcion
                  
                  default:
                    throw new SyntaxError(secondOfStatement, 
                                          tokenStream.Current);
                }
            default:
                throw new SyntaxError(firstOfStatement, 
                                      tokenStream.Current);
            }
        }
        
        //LL(1):Stmt_Assign -> "Id" "=" Expr ";"
        //El anchortoken de assigment no es "=", sino el identificador, ver ejemplo del profesor linea 150
        //AST Transformation: DONE
        public Node StmtAssign(){
          Expect(TokenCategory.ASSIGN);
          var exprNode = Expr();
          var assignNode = new Assign(){ exprNode};
          Expect(TokenCategory.SEMICOLON);
          return assignNode;
        }
        
        
        //LL(1) Stmt_Incr -> "Id" "++" ";"
        //AST Transformation: DONE
        public Node StmtIncr(){
          var incrementNode = new Increment(){
              AnchorToken=Expect(TokenCategory.INCREMENT)
          };
          Expect(TokenCategory.SEMICOLON);
          return incrementNode;
        }
        
        //LL(1) Stmt_Decr -> "Id" "--" ";"
        //AST Transformation: DONE
        public Node StmtDecr(){
          var decrementNode = new Decrement(){
              AnchorToken=Expect(TokenCategory.DECREMENT)
          };
          Expect(TokenCategory.SEMICOLON);
          return decrementNode;
        }
        
        //LL(1) Stmt_Fun_Call -> Fun_Call ";"
        //AST Transformation: DONE
        public Node StmtFunCall (){
          var funCallNode = FunCall();
          Expect(TokenCategory.SEMICOLON);
          return funCallNode;
        }
        
        //LL(1) Fun_Call -> "Id" "(" Expr_List ")"
        //AST Transformation: DONE
        public Node FunCall(){
          Expect(TokenCategory.PARENTHESIS_OPEN);
          var exprListNode = ExprList();
          Expect(TokenCategory.PARENTHESIS_CLOSE);
          return exprListNode;
        }
        
       
        //LL(1) Expr_List -> (Expr Expr_List_Cont)?
        //AST Transformation: DONE          
        public Node ExprList(){
            if(firstOfExpr.Contains(this.CurrentToken)){
                var exprNode = Expr();
                if(this.CurrentToken == TokenCategory.COMMA){//Si el nodo tiene hermanitos, crea una lista
                    return ExprListCont(exprNode);
                }
                else{ //Si no, envialo como hijo unico
                    return exprNode;
                } 
            }
            return new ExprList(); //envia un nodo Lista vacio
        }
        
        //LL(1) Expr_List_Cont -> ("," Expr)*
        //AST Transformation: DONE          
        public Node ExprListCont(Node node){ //Recibe al hermanito mayor
            var exprListContNode = new ExprList(); //Crea el nodo Lista
            exprListContNode.Add(node); //agrega al nodo lista el hermanito mayor
            while(this.CurrentToken == TokenCategory.COMMA){
                Expect(TokenCategory.COMMA);
                exprListContNode.Add(Expr());
            }
            return exprListContNode;
        }
        
        
        //LL(1) Stmt_If -> "if" "(" Expr ")" "{" Stmt_List "}" Else_If_List Else
        //AST Transformation: DONE  
        public Node StmtIf(){
              var ifToken = Expect(TokenCategory.IF);
              Expect(TokenCategory.PARENTHESIS_OPEN);
              var exprNode = Expr();
              Expect(TokenCategory.PARENTHESIS_CLOSE);
              Expect(TokenCategory.KEY_OPEN);
              var stmtListNode = StmtList();
              Expect(TokenCategory.KEY_CLOSED);
              var elseIfListNode = ElseIfList();
              var elseNode = Else();
              
              var ifNode = new If(){ 
                exprNode,
                stmtListNode,
                elseIfListNode,
                elseNode
              };
              ifNode.AnchorToken = ifToken;
              return ifNode;
        }
        
        //LL(1) Else_If_List -> ("elseif" "(" Expr ")" "{" Stmt_List "}")*
        //AST Transformation: DONE  
        public Node ElseIfList(){
            var elseIfListNode = new ElseIfList();
            while(this.CurrentToken == TokenCategory.ELSE_IF){
                var elseIfNode = new ElseIf(){
                  AnchorToken=Expect(TokenCategory.ELSE_IF)
                };
                Expect(TokenCategory.PARENTHESIS_OPEN);
                elseIfNode.Add(Expr());
                Expect(TokenCategory.PARENTHESIS_CLOSE);
                Expect(TokenCategory.KEY_OPEN);
                elseIfNode.Add(StmtList());
                Expect(TokenCategory.KEY_CLOSED);
                elseIfListNode.Add(elseIfNode);
            }
            return elseIfListNode;
        }
        
        //LL(1) Else -> ("else" "{" Stmt_List "}")?
              //AST Transformation: DONE  
        public Node Else(){
          var elseNode = new Else();
          if(this.CurrentToken == TokenCategory.ELSE){
            var elseToken = Expect(TokenCategory.ELSE);
            Expect(TokenCategory.KEY_OPEN);
            elseNode.Add(StmtList());
            Expect(TokenCategory.KEY_CLOSED);
            elseNode.AnchorToken = elseToken;
          }
          return elseNode;
        }
        
        //LL(1) Stmt_Loop -> "loop" "{" Stmt_List "}"
              //AST Transformation: DONE  
        public Node StmtLoop(){
          var loopNode = new Loop(){
            AnchorToken=Expect(TokenCategory.LOOP)
          };
          Expect(TokenCategory.KEY_OPEN);
          loopNode.Add(StmtList());
          Expect(TokenCategory.KEY_CLOSED);
          return loopNode;
        }
        
        //LL(1) Stmt_Break -> "break" ";"
               //AST Transformation: DONE  
        public Node StmtBreak(){
          var breakNode = new Break(){
            AnchorToken =Expect(TokenCategory.BREAK)
          };
          Expect(TokenCategory.SEMICOLON);
          return breakNode;
        }
        
        //LL(1) Stmt_Return -> "return" Expr ";"
            //AST Transformation: DONE  
        public Node StmtReturn(){
          var returnNode = new Return(){
            AnchorToken = Expect(TokenCategory.RETURN)
          };
          returnNode.Add(Expr());
          Expect(TokenCategory.SEMICOLON);
          return returnNode;
        }

        
        //***********LOS DE ARRIBA YA LOS HICE (REVISALOS LUIS xfa) DE LA LINEA 652 PA ABAJO  O SEA HACIA EL 0 :V
                
        //LL(1) Stmt_Empty -> ";"
        public Node StmtEmpty(){
          return new StmtEmpty(){
            AnchorToken=Expect(TokenCategory.SEMICOLON)
          };
        }
        
        
        
        //LL(1) Expr -> Expr_Or
        //AST Transformation: DONE
        public Node Expr(){
          var exprORNode = ExprOr(); 
          return exprORNode;
        }
        
        //LL(1) Expr_Or -> Expr_And ("||" Expr_And)*
        //AST Transformation: DONE
        public Node ExprOr(){
          var exprAnd1Node = ExprAnd();
          while(this.CurrentToken == TokenCategory.OR){
            var exprAnd2Node = new Or(){
                AnchorToken = Expect(TokenCategory.OR)
            };
            exprAnd2Node.Add(exprAnd1Node);
            exprAnd2Node.Add(ExprAnd());
            exprAnd1Node = exprAnd2Node; 
          }
          return exprAnd1Node;
        }
        
        //LL(1) Expr_And -> Expr_Comp ("&&" Expr_Comp)*
        //AST Transformation: DONE
        public Node ExprAnd(){
          var exprComp1Node = ExprComp();
          while(CurrentToken == TokenCategory.AND){
            var exprComp2Node = new And(){
                AnchorToken = Expect(TokenCategory.AND)
            };
            exprComp2Node.Add(exprComp1Node);
            exprComp2Node.Add(ExprComp());
            exprComp1Node = exprComp2Node; 
          }
          return exprComp1Node;
        }
        
        //LL(1) Expr_Comp -> Expr_Rel (Op_Comp Expr_Rel)*
        //AST Transformation: DONE
        public Node ExprComp(){
          var exprRel1Node = ExprRel();
          while(firstOfOperatorComp.Contains(CurrentToken)){
            var exprRe2Node = new OpComp(){
                AnchorToken = OpComp()
            };
            exprRe2Node.Add(exprRel1Node);
            exprRe2Node.Add(ExprRel());
            exprRel1Node = exprRe2Node; 
          }
          return exprRel1Node;
        }
        
        //LL(1) Op_Comp -> "==" | "!="
              //AST Transformation: DONE
        public Token OpComp(){
          switch (CurrentToken) {
            case TokenCategory.EQUAL:
                 return Expect(TokenCategory.EQUAL);
                
            case TokenCategory.NOT_EQUAL:
                 return Expect(TokenCategory.NOT_EQUAL);
    
            default:
                throw new SyntaxError(firstOfOperatorComp, 
                                      tokenStream.Current);
            }
        }
        
        //LL(1) Expr_Rel -> Expr_Add (Op_Rel Expr_Add)*
        //AST Transformation: DONE
        public Node ExprRel(){
          var expr_Add1Node = ExprAdd();
          while(firstOfOperatorRel.Contains(CurrentToken)){
            var expr_Add2Node = new OpRel(){
                AnchorToken = OpRel()
            };
            expr_Add2Node.Add(expr_Add1Node);
            expr_Add2Node.Add(ExprAdd());
            expr_Add1Node = expr_Add2Node;
          }
          return expr_Add1Node;
        }
        
        //LL(1) Op_Rel -> "<" | "<=" | ">" | ">="
        //AST Transformation: DONE
        ////Regresa el token de una operacion de relacion
        public Token OpRel(){
          switch (CurrentToken) {

            case TokenCategory.GREATER:
                return Expect(TokenCategory.GREATER);
                
            case TokenCategory.LESS:
                return Expect(TokenCategory.LESS);

            case TokenCategory.GREATER_EQUAL:
                return Expect(TokenCategory.GREATER_EQUAL);
  
            case TokenCategory.LESS_EQUAL:
                return Expect(TokenCategory.LESS_EQUAL);

            default:
                throw new SyntaxError(firstOfOperatorRel, 
                                      tokenStream.Current);
            }
        }
        
        //LL(1) Expr_Add -> Expr_Mul (Op_Add Expr_Mul)*
         //AST Transformation: DONE
        public Node ExprAdd(){
          var exprMul1Node = ExprMul(); //Guardar valor de ExprMul en una variable
          while(firstOfOperatorAdd.Contains(CurrentToken)){
            var exprMul2Node = new OpAdd() { //Crear nodo de suma
                AnchorToken = OpAdd() //Asignar token del metodo OpAdd
            };
            exprMul2Node.Add(exprMul1Node); //Meter al nodo la variable 1 (hijo)
            exprMul2Node.Add(ExprMul()); //Meter al nodo lo que regrese el metodo
            exprMul1Node = exprMul2Node; //Asignar variable2 a variable1 (nuevo padre)
          }
          return exprMul1Node;
        }
        
        //LL(1) Op_Add -> "+" | "-"
                        //AST Transformation: DONE
        ////Regresa el token de una operacion de suma
        public Token OpAdd(){
          switch (CurrentToken) {
             case TokenCategory.PLUS: 
                    return Expect(TokenCategory.PLUS);
                    
             case TokenCategory.NEG:
                    return Expect(TokenCategory.NEG);
            default:
                throw new SyntaxError(firstOfOperatorAdd, 
                                      tokenStream.Current);
            }
        }
        
        //LL(1) Expr_Mul -> Expr_Unary (Op_Mul Expr_Unary)*
        //AST Transformation: DONE
        public Node ExprMul(){
          var exprUnary1Node = ExprUnary(); //Guardar valor de ExprUnary en una variable
          while(firstOfOperatorMul.Contains(CurrentToken)){
            var exprUnary2Node = new OpMul() { //Crear nodo de multiplicacion
                AnchorToken = OpMul() //Asignar token del metodo OpMul
            };
            exprUnary2Node.Add(exprUnary1Node); //Meter al nodo la variable 1 (hijo)
            exprUnary2Node.Add(ExprUnary()); //Meter al nodo lo que regrese el metodo
            exprUnary1Node = exprUnary2Node; //Asignar variable2 a variable1 (nuevo padre)
          }
          return exprUnary1Node;
        }
        
        //LL(1) Op_Mul -> "*" | "/" | "%"
                        //AST Transformation: DONE
        //Regresa el token de una operacion de multiplicacion
        public Token OpMul(){
          switch (CurrentToken) {

            case TokenCategory.MUL:
                return Expect(TokenCategory.MUL);
  
            case TokenCategory.DIV:
                 return Expect(TokenCategory.DIV);
                
            case TokenCategory.REMAINDER:
                 return Expect(TokenCategory.REMAINDER);
       
            default:
                throw new SyntaxError(firstOfOperatorMul, 
                                      tokenStream.Current);
            }
        }
        
        //LL(1) Expr_Unary -> OP_Unary* Expr_Primary
                //AST Transformation: DONE
        public Node ExprUnary(){
            if(firstOfUnary.Contains(CurrentToken)){ //THIS IS BECAUSE  'OP_Unary' CAN BE 0 OR MORE!
                var opUnaryNode = new OpUnary(); //Creas un nodo OpUnary
                while(firstOfUnary.Contains(CurrentToken)){
                    opUnaryNode.AnchorToken = OpUnary(); //Asignas el token que le corresponde
                }
                var exprPrimaryNode = ExprPrimary(); //Guarda lo que regresa la ExprPrimary
                opUnaryNode.Add(exprPrimaryNode); //Se le agrega al ultimo Nodo opUnary creado lo guaradado en exprPrimary
                return opUnaryNode; //Regresa el ultimo nodo
            }
            else{
                var node = ExprPrimary(); //Guarda lo que regresa la ExprPrimary
                return node;
            }
            
            
        }
        
        //LL(1) Op_Unary -> "+" | "-" | "!"
         //AST Transformation: DONE
         //Regresa el token de la OpUnary
        public Token OpUnary(){
            switch(this.CurrentToken){
                case TokenCategory.PLUS: 
                    return Expect(TokenCategory.PLUS);
                    
                case TokenCategory.NEG:
                    return Expect(TokenCategory.NEG);

                case TokenCategory.NOT:
                   return Expect(TokenCategory.NOT);

                default:
                    throw new SyntaxError(firstOfUnary, 
                                          tokenStream.Current);
            }

        }
        
        
        //LL(1) Expr_Primary -> "Id" | "Fun_Call" | Array | Lit | "(" Expr ")"
        public Node ExprPrimary(){
          if(literals.Contains(CurrentToken)){
            var litNode = Lit(); 
            return litNode; //Regresa el nodo que se genere en Lit()
          }
          else{
            switch(this.CurrentToken){
                case TokenCategory.IDENTIFIER: 
                    var identifierToken = Expect(TokenCategory.IDENTIFIER); //Guarda el valor del identificador
                    if(CurrentToken == TokenCategory.PARENTHESIS_OPEN){ //Verifica si es una función
                      var funNode = FunCall(); //Guarda lo que regrese FunCall()
                      var resultNode1 = new FunCall(){ funNode }; //Crea un nodo función
                      resultNode1.AnchorToken = identifierToken; //Agrega un toquen al nodo función
                      return resultNode1; //Regresa el nodo función
                    }
                    //Si es solo un identificador, crea un nodo identificador asignale su token
                    var resultNode2 = new Identifier(){
                        AnchorToken = identifierToken
                    };
                    return resultNode2; //Regresa el nodo identificador
                    
                case TokenCategory.SQUARE_BRACKET_OPEN:
                    var arrayNode = Array(); //Guarda lo que te regrese el metodo Array
                    return arrayNode; //Regresalo
                
                case TokenCategory.PARENTHESIS_OPEN:
                    Expect(TokenCategory.PARENTHESIS_OPEN);
                    var resultNode = Expr(); //Guarda lo que regrese el Expr()
                    Expect(TokenCategory.PARENTHESIS_CLOSE);
                    return resultNode; //Regresa lo que sea que sea que te haya regresado Expr()
                    
                default:
                    throw new SyntaxError(firstOfPrimary, 
                                          tokenStream.Current);
            }
          }
        }
        
        //LL(1) Array -> "[" Expr_List "]"
        public Node Array(){
            Expect(TokenCategory.SQUARE_BRACKET_OPEN);
            var exprListNode = ExprList();
            var arrayNode = new Array(){ exprListNode }; //Crea un nodo array y le mete lo que sea que este en "exprListNode"
            Expect(TokenCategory.SQUARE_BRACKET_CLOSE);
            return arrayNode; //Regresa el nodo array
        }
        
        //LL(1) Lit -> "Lit_Int" | "Lit_Char" | "Lit_Str"
                        //AST Transformation: DONE
        public Node Lit(){
            switch(this.CurrentToken){
                case TokenCategory.INT_LITERAL:
                    //Regresa un nodo integer y su token 
                    return new IntLiteral()
                    {
                        AnchorToken = Expect(TokenCategory.INT_LITERAL)
                    };
                      
                case TokenCategory.CHAR_LITERAL:
                    //Regresa un nodo char y su token 
                    return new CharLiteral()
                    {
                        AnchorToken = Expect(TokenCategory.CHAR_LITERAL)
                    };
                  
                case TokenCategory.STRING_LITERAL:
                    //Regresa un nodo integer y su token 
                    return new StringLiteral()
                    {
                        AnchorToken = Expect(TokenCategory.STRING_LITERAL)
                    };
                      
                default:
                    throw new SyntaxError(literals, 
                                          tokenStream.Current);
            }
        }    
                
    }
}
