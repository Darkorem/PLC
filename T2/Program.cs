﻿using System;
using System.Collections.Generic;

public class ParseNode
{
    public ParseNode()
    {
        Children= new();
    }

    public string Name { get; set; }

    public int Token { get; set; }
    public string Lexeme {get; set; }

    public List<ParseNode> Children {get; set; }
}


public class Program
{
    static Dictionary<int,string> TokenDictionary;
    public const int LETTER  = 2;
    public const int  DIGIT = 1;
    public const int LOGIC  = 3;
    public const int  FLOAT = 4;
    public const int  VARIABLE = 5;
    public const int PERIOD = 6;
    public const int  UNKNOWN  = 99;

    /* Token codes */
    public const int INT_LIT  = 10;
    public const int IDENT = 11;
    public const int ASSIGN_OP = 20;
    public const int ADD_OP = 21;
    public const int SUB_OP = 22;
    public const int MULT_OP = 23;
    public const int DIV_OP = 24;
    public const int LEFT_PAREN = 25;
    public const int RIGHT_PAREN = 26;
    public const int MODULO = 27;

    public const int equals = 29;
    public const int less_than = 30;
    public const int greater_than = 31;
    public const int less_than_or_equal = 32;
    public const int greater_than_or_equal = 33;
    public const int ampers_and = 34;
    public const int pipe = 35;
    public const int variable_identifier = 36;
    public const int integer_literal = 37;
    public const int float_literal = 38;
    public const int left_curly_brace = 39;
    public const int right_curly_brace = 40;
    public const int EOF = 0;


    public const int STMT = 41;
    public const int STMT_LIST = 42;
    public const int WHILE_LOOP = 43;
    public const int IF_STMT = 44;
    public const int BLOCK = 45;
    public const int DECLARE = 46;
    public const int ASSIGN = 47;
    public const int EXPR = 48;
    public const int TERM = 49;
    public const int FACT = 50;
    public const int BOOL_EXPR = 51;
    public const int BTERM = 52;
    public const int BAND = 53;
    public const int BOR = 54;

    public const int exclamation_mark = 55;
    public const int not_equal_operator = 56;


    static Front front;
    static int nextToken;
    static string lexeme;

    static ParseNode ParseTreeRoot;

    public static void Main(string[] args)
    {
        TokenDictionary = new();
        TokenDictionary.Add( 10,"INT_LIT");//  = 10;
		TokenDictionary.Add( 11,"IDENT");//   = 11;
		TokenDictionary.Add( 20,"ASSIGN_OP");//   = 20;
		TokenDictionary.Add( 21,"ADD_OP");//   = 21;
		TokenDictionary.Add( 22,"SUB_OP");//   = 22;
		TokenDictionary.Add( 23,"MULT_OP");//   = 23;
		TokenDictionary.Add( 24,"DIV_OP");//   = 24;
		TokenDictionary.Add( 25,"LEFT_PAREN");//   = 25;
		TokenDictionary.Add( 26,"RIGHT_PAREN");//   = 26;
		TokenDictionary.Add( 27,"MODULO");
		TokenDictionary.Add( 6,"PERIOD");
		TokenDictionary.Add( 29,"equals");//  = 29;
		TokenDictionary.Add( 30,"less_than");//  = 30;
		TokenDictionary.Add( 31,"greater_than");//  = 31;
		TokenDictionary.Add( 32,"less_than_or_equal");//  = 32;
		TokenDictionary.Add( 33,"greater_than_or_equal");//  = 33;
		TokenDictionary.Add( 34,"ampers_and");//  = 34;
		TokenDictionary.Add( 35,"pipe");//  = 35;
		TokenDictionary.Add( 36,"variable_identifier");//  = 36;
		TokenDictionary.Add( 37,"integer_literal");//  = 37;
		TokenDictionary.Add( 38,"float_literal");//  = 38;
        TokenDictionary.Add( 39, "left_curly_brace");
        TokenDictionary.Add( 40, "right_curly_brace");
		TokenDictionary.Add( 0 ,"EOF");//   = 0;

        TokenDictionary.Add( 41, "STMT");//  = 41;
        TokenDictionary.Add( 42, "STMT_LIST");//  = 42;
        TokenDictionary.Add( 43, "WHILE_LOOP");//  = 43;
        TokenDictionary.Add( 44, "IF_STMT");//  = 44;
        TokenDictionary.Add( 45, "BLOCK");//  = 45;
        TokenDictionary.Add( 46, "DECLARE");//  = 46;
        TokenDictionary.Add( 47, "ASSIGN");//  = 47;
        TokenDictionary.Add( 48, "EXPR");//  = 48;
        TokenDictionary.Add( 49, "TERM");//  = 49;
        TokenDictionary.Add( 50, "FACT");//  = 50;
        TokenDictionary.Add( 51, "BOOL_EXPR");//  = 51;
        TokenDictionary.Add( 52, "BTERM");//  = 52;
        TokenDictionary.Add( 53, "BAND");//  = 53;
        TokenDictionary.Add( 54, "BOR");//  = 54;
        TokenDictionary.Add( 55, "exclamation_mark");//  = 55
        TokenDictionary.Add( 56, "not_equal_operator");//  = 56
	
        /* Test Front.cs */
        front = new Front(TokenDictionary);
        Console.WriteLine("Hello, World!");
        //front.execute();

        (nextToken, lexeme) = front.lex();

        ParseTreeRoot = new ParseNode() { Name = "root"};
        expr(ParseTreeRoot);
        ParseTreeRoot = ParseTreeRoot.Children[0];

        System.Console.WriteLine("parse tree:");
        PrintParseTree(ParseTreeRoot);

        /*
        var program = new  T2.construct_execute.expr(ParseTreeRoot);
        System.Console.WriteLine($"program result: {program.execute()}");
        */
        
    }

    static void PrintCurrentToken()
    {
        System.Console.WriteLine($"token: {TokenDictionary[nextToken]} lexeme: {lexeme}");
    }

    // <BOR> --> <EXPR> {`&&` <EXPR>}
 static void bor(ParseNode Parent) 
    {

        var node = new ParseNode() { Name = "bor"};

        Parent.Children.Add(node);
        
        expr(node);

        if
        ( 
            nextToken == pipe
        ) 
        {
            or_operator(node);
            expr(node);
        }
       
    }

    // <BAND> --> <BOR> {`&&` <BOR>}
 static void band(ParseNode Parent) 
    {

        var node = new ParseNode() { Name = "band"};

        Parent.Children.Add(node);
        
        bor(node);

        if
        ( 
            nextToken == ampers_and
        ) 
        {
            and_operator(node);
            bor(node);
        }
       
    }
// <BTERM> --> <BAND> {(`==`|`!=`) <BAND>}
 static void bterm(ParseNode Parent) 
    {

        var node = new ParseNode() { Name = "bterm"};

        Parent.Children.Add(node);
        
        band(node);

        if
        ( 
            nextToken == equals
        ) 
        {
            equals_operator(node);
            band(node);
        }
       
    }

//<BOOL_EXPR> --> <BTERM> {(`>`|`<`|`>=`|`<=`) <BTERM>}
    static void bool_expr(ParseNode Parent) 
    {

        var node = new ParseNode() { Name = "bool_expr"};

        Parent.Children.Add(node);
        
        bterm(node);

        if
        ( 
            nextToken == less_than ||
            nextToken == greater_than
        ) 
        {
            comparison_operator(node);
            bterm(node);
        }
       
    }

      /* expr
    Parses strings in the language generated by the rule:
    <EXPR> --> <TERM> {(`+`|`-`) <TERM>}
    */
    static void expr(ParseNode Parent) 
    {

        var node = new ParseNode() { Name = "expr"};

        Parent.Children.Add(node);
        
        //System.Console.WriteLine("Enter <expr>");
        /* Parse the first term */
        term(node);
        /* As long as the next token is + or -, get
        the next token and parse the next term */
        while 
        (
            nextToken == ADD_OP || 
            nextToken == SUB_OP
        ) 
        {
            node.Children.Add(new ParseNode() {  Name = lexeme});
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
            term(node);
        }
    }

    /* term
    Parses strings in the language generated by the rule:
    <TERM> --> <FACT> {(`*`|`/`|`%`) <FACT>}
    */
    static void term(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "term"};

        Parent.Children.Add(node);
        factor(node);
        
        /* As long as the next token is * or /, get the
        next token and parse the next factor */
        while 
        (
            nextToken == MULT_OP || 
            nextToken == DIV_OP  ||
            nextToken == MODULO
        ) 
        {
            node.Children.Add(new ParseNode() {  Name = lexeme});
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
            factor(node);
        }
        //System.Console.WriteLine("Exit <term>");
    }

    /* factor
    Parses strings in the language generated by the rule:
    <FACT> --> ID | INT_LIT | FLOAT_LIT | `(` <EXPR> `)`
    */
    static void factor(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "factor"};

        Parent.Children.Add(node);
        //id(node);
        //int_lit(node);
        //float_lit(node);
        //expr(node);
        if 
        (
            nextToken == IDENT 
        )
        {
            node.Children.Add(new ParseNode() {  Name = "IDENT", Token= IDENT, Lexeme = lexeme});
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
            id(node);
        }

        else if (nextToken == LEFT_PAREN) 
        {
                node.Children.Add(new ParseNode() {  Name = "LEFT_PAREN", Token = LEFT_PAREN, Lexeme = lexeme});
                PrintCurrentToken();
                (nextToken, lexeme) = front.lex();
                expr(node);
                if (nextToken == RIGHT_PAREN)
                {
                    node.Children.Add(new ParseNode() {  Name = "RIGHT_PAREN", Token = RIGHT_PAREN, Lexeme = lexeme});
                    PrintCurrentToken();
                    (nextToken, lexeme) = front.lex();
                }
                else
                    error();
        }
        else if(nextToken == float_literal)
        {
            node.Children.Add(new ParseNode() {  Name = lexeme});
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
            float_lit(node);
        }
        else if (nextToken == INT_LIT)
        {
            node.Children.Add(new ParseNode() {  Name = lexeme});
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
            int_lit(node);
        } 
        else
         {error();
                System.Console.WriteLine("This should NOT happen");
            }       
        //System.Console.WriteLine("Exit <factor>");
    }

static void id(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "IDENT"};

        Parent.Children.Add(node);

        if(nextToken == IDENT)
        {
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
        }
    }

static void int_lit(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "INT_LIT"};

        Parent.Children.Add(node);

        if(nextToken == INT_LIT)
        {
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
        }
    }

static void float_lit(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "float_literal"};

        Parent.Children.Add(node);

        if(nextToken == float_literal)
        {
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
        }
    }

//<STMT> --> <IF_STMT> | <BLOCK> | <ASSIGN> | <DECLARE> | <WHILE_LOOP>
static void stmt(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "STMT"};

        Parent.Children.Add(node);

        PrintCurrentToken();
        (nextToken, lexeme) = front.lex();
    }

//<STMT_LIST> --> { <STMT> `;` }
static void stmt_list(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "STMT_LIST"};

        Parent.Children.Add(node);

        PrintCurrentToken();
        (nextToken, lexeme) = front.lex();
    }

//<WHILE_LOOP> --> `while` `(` <BOOL_EXPR> `)` <BLOCK> 
static void while_loop(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "WHILE_LOOP"};

        Parent.Children.Add(node);

        PrintCurrentToken();
        (nextToken, lexeme) = front.lex();
    }

//<IF_STMT> --> `if` `(` <BOOL_EXPR> `)` <BLOCK>  [ `else`  <BLOCK> ] 
static void if_stmt(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "IF_STMT"};

        Parent.Children.Add(node);

        PrintCurrentToken();
        (nextToken, lexeme) = front.lex();
    }

//<BLOCK> --> `{` <STMT_LIST> `}`
static void block(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "BLOCK"};

        Parent.Children.Add(node);

        PrintCurrentToken();
        (nextToken, lexeme) = front.lex();
    }

//<DECLARE> --> `DataType` ID {`,` ID }
static void declare(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "DECLARE"};

        Parent.Children.Add(node);

        PrintCurrentToken();
        (nextToken, lexeme) = front.lex();
    }

//<ASSIGN> --> ID `=` <EXPR>
static void assign(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "ASSIGN"};

        Parent.Children.Add(node);

        Parent.Children.Add(node);

        Parent.Children.Add(node);

        if(nextToken == IDENT)
        {
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
            if(nextToken == equals)
            {
                PrintCurrentToken();
                (nextToken, lexeme) = front.lex();
                equal_operator(node);
                if(nextToken == EXPR)
                {
                    node.Children.Add
                    (
                        new() { Name = "IDENT", Lexeme = "IDENT"}
                    );
                    (nextToken, lexeme) = front.lex();
                    expr(node);
                }
            }
            else
            {
                node.Children.Add
                (
                    new() { Name = "IDENT", Token = IDENT, Lexeme = "IDENT"}
                );
                id(node);
            }
        } 
        else
        {
            expr(node);
        }
    }

//<less_than> := <
    static void less_than_operator(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "less_than_operator"};

        Parent.Children.Add(node);

        PrintCurrentToken();
        (nextToken, lexeme) = front.lex();
    }
//<greater_than> := >
static void greater_than_operator(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "greater_than_operator"};

        Parent.Children.Add(node);

        PrintCurrentToken();
        (nextToken, lexeme) = front.lex();
    }
//<equal_symbol> := =
static void equal_operator(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "equal_operator"};

        Parent.Children.Add(node);

        PrintCurrentToken();
        (nextToken, lexeme) = front.lex();
    }
//<comparison_operator> := <less_than> | <greater_than> | <less_than><equal_symbol> | <greater_than><equal_symbol>
    static void comparison_operator(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "comparison_operator"};

        Parent.Children.Add(node);

        if(nextToken == less_than)
        {
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
            if(nextToken == equals)
            {
                node.Children.Add
                (
                    new() { Name = "less_than_or_equal", Lexeme = "<="}
                );
                (nextToken, lexeme) = front.lex();
            }
            else
            {
                node.Children.Add
                (
                    new() { Name = "less_than", Token = less_than, Lexeme = "<"}
                );
            }
        }
        else if(nextToken == greater_than)
        {
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
            if(nextToken == equals)
            {
                node.Children.Add
                (
                    new() { Name = "greater_than_or_equal", Lexeme = ">="}
                );
                (nextToken, lexeme) = front.lex();
            }
            else
            {
                node.Children.Add
                (
                    new() { Name = "greater_than", Token = greater_than, Lexeme = ">"}
                );
            }
        }
        

    }
    //<comparison_operator> := <less_than> | <greater_than> | <less_than><equal_symbol> | <greater_than><equal_symbol>
    static void equals_operator(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "equals_operator"};

        Parent.Children.Add(node);

        if(nextToken == equals)
        {
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
            if(nextToken == equals)
            {
                node.Children.Add
                (
                    new() { Name = "equal_to", Lexeme = "=="}
                );
                (nextToken, lexeme) = front.lex();
            }
            else
            {
                node.Children.Add
                (
                    new() { Name = "ASSIGN_OP", Token = equals, Lexeme = "="}
                );
            }
        } 

    }

    static void not_equal(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "not_equal_operator"};

        Parent.Children.Add(node);

        if(nextToken == exclamation_mark)
        {
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
            if(nextToken == equals)
            {
                node.Children.Add
                (
                    new() { Name = "not_equal_operator", Lexeme = "!="}
                );
                (nextToken, lexeme) = front.lex();
            }
            else
            {
                node.Children.Add
                (
                    new() { Name = "exclamation_mark", Token = not_equal_operator, Lexeme = "!"}
                );
            }
        } 

    }

    static void and_operator(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "and_operator"};

        Parent.Children.Add(node);

        if(nextToken == ampers_and)
        {
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
            if(nextToken == ampers_and)
            {
                node.Children.Add
                (
                    new() { Name = "and", Lexeme = "&&"}
                );
                (nextToken, lexeme) = front.lex();
            }
            else
            {
                node.Children.Add
                (
                    new() { Name = "ampers_and", Token = ampers_and, Lexeme = "&"}
                );
            }
        } 

    }

    static void or_operator(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "or_operator"};

        Parent.Children.Add(node);

        if(nextToken == pipe)
        {
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
            if(nextToken == pipe)
            {
                node.Children.Add
                (
                    new() { Name = "or", Lexeme = "||"}
                );
                (nextToken, lexeme) = front.lex();
            }
            else
            {
                node.Children.Add
                (
                    new() { Name = "pipe", Token = pipe, Lexeme = "|"}
                );
            }
        } 

    }

    static void error()
    {
        System.Console.WriteLine("error");
    }


    static void PrintParseTree(ParseNode node, int level = 0)
    {
        for(int i = 0; i < level; i++)
            System.Console.Write("\t");
        
        System.Console.WriteLine($"{node.Name}:{node.Token} {node.Lexeme}");
        foreach(var child in node.Children)
        {
            PrintParseTree(child, level + 1);
        }
    }
  
}