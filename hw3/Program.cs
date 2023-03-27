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

    //const int assign = 28;
    public const int equals = 29;
    public const int less_than = 30;
    //public const int less_than_operator = 40;
    public const int greater_than = 31;
    public const int less_than_or_equal = 32;
    public const int greater_than_or_equal = 33;
    public const int ampers_and = 34;
    public const int pipe = 35;
    public const int variable_identifier = 36;
    public const int integer_literal = 37;
    public const int float_literal = 38;
    //const int PERIOD = 39;
    public const int EOF = 0;

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
        //TokenDictionary.Add( 40, "less_than_operator");
		TokenDictionary.Add( 0 ,"EOF");//   = 0;
	
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


        var program = new  hw3.construct_execute.expr(ParseTreeRoot);
        System.Console.WriteLine($"program result: {program.execute()}");
        
    }

    static void PrintCurrentToken()
    {
        System.Console.WriteLine($"token: {TokenDictionary[nextToken]} lexeme: {lexeme}");
    }

 // ||
 static void expr(ParseNode Parent) 
    {

        var node = new ParseNode() { Name = "expr"};

        Parent.Children.Add(node);
        
        a(node);

        if
        ( 
            nextToken == pipe
        ) 
        {
            or_operator(node);
            a(node);
        }
       
    }

    // &&
 static void a(ParseNode Parent) 
    {

        var node = new ParseNode() { Name = "a"};

        Parent.Children.Add(node);
        
        e(node);

        if
        ( 
            nextToken == ampers_and
        ) 
        {
            and_operator(node);
            e(node);
        }
       
    }
// ==
 static void e(ParseNode Parent) 
    {

        var node = new ParseNode() { Name = "e"};

        Parent.Children.Add(node);
        
        c(node);

        if
        ( 
            nextToken == equals
        ) 
        {
            equals_operator(node);
            c(node);
        }
       
    }

//<expr> -> <l> { <comparison_operator> <l>}
    static void c(ParseNode Parent) 
    {

        var node = new ParseNode() { Name = "c"};

        Parent.Children.Add(node);
        
        l(node);

        if
        ( 
            nextToken == less_than ||
            nextToken == greater_than
        ) 
        {
            comparison_operator(node);
            l(node);
        }
       
    }

      /* expr
    Parses strings in the language generated by the rule:
    <l> -> <term> {(+ | -) <term>}
    */
    static void l(ParseNode Parent) 
    {

        var node = new ParseNode() { Name = "l"};

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
       // System.Console.WriteLine("Exit <l>");
    }

    /* term
    Parses strings in the language generated by the rule:
    <term> -> <factor> {(* | /) <factor>}
    */
    static void term(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "term"};

        Parent.Children.Add(node);
        //System.Console.WriteLine("Enter <term>");
        /* Parse the first factor */
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
        System.Console.WriteLine("Exit <term>");
    }

    /* factor
    Parses strings in the language generated by the rule:
    <factor> -> id | int_constant | ( <expr> )
    */
    static void factor(ParseNode Parent) 
    {
        var node = new ParseNode() { Name = "factor"};

        Parent.Children.Add(node);
        //System.Console.WriteLine("Enter <factor>");
        /* Determine which RHS */
        if 
        (
            nextToken == IDENT 
        )
        {
            /* Get the next token */
            node.Children.Add(new ParseNode() {  Name = "IDENT", Token= IDENT, Lexeme = lexeme});
            PrintCurrentToken();
            (nextToken, lexeme) = front.lex();
            /* If the RHS is ( <expr> ), call lex to pass over the
            left parenthesis, call expr, and check for the right
        
            parenthesis */
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
        else if
        (
            nextToken == INT_LIT ||
            nextToken == PERIOD

        )// <number>
        {
            /*
                        <number>:= <INT_LIT> | <float>
            <INT_LIT>
            <float> := <period> <INT_LIT>| <INT_LIT> <period> {<INT_LIT>}

            */
            if(nextToken == PERIOD)
            {
                    //Parent.Children.Add(node);

                    (nextToken, lexeme) = front.lex();
                    if(nextToken == INT_LIT)
                    {
                        node.Lexeme = "." + lexeme;
                        (nextToken, lexeme) = front.lex();
                    }
                    else
                        error();
            }
            else if(nextToken == INT_LIT)
            {
                var current_lexeme = lexeme;
                (nextToken, lexeme) = front.lex();
                if(nextToken == PERIOD)
                {
                    (nextToken, lexeme) = front.lex();
                    node.Lexeme = current_lexeme + "." + lexeme;
                    (nextToken, lexeme) = front.lex();
                   
                }
                else
                {
                    node.Lexeme = current_lexeme;
                }
            }
            
        }
        else
                error();
        
        System.Console.WriteLine("Exit <factor>");
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