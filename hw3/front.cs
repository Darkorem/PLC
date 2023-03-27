using System;
/* front.c - a lexical analyzer system for simple
 arithmetic expressions */

public class Front
{
	public Front(Dictionary<int,string> _TokenDictionary)
	{
		TokenDictionary = _TokenDictionary;
		
		var file_path = "front.in";

		if(!System.IO.File.Exists(file_path))
		{
			System.Console.WriteLine("ERROR - cannot open front.in ");
			//return 0;
		}
		
		//if ((in_fp = new System.IO.StreamReader(("front.in")) == NULL)
		//	System.Console.WriteLine("ERROR - cannot open front.in \n");
		
		in_fp = new System.IO.StreamReader(file_path);
		getChar();
	}

/* Global declarations */
/* Variables */
int charClass;
char[] lexeme = new char[100];
char nextChar;
int lexLen;
int token;
int nextToken;
System.IO.TextReader in_fp;


Dictionary<int,string> TokenDictionary = new();
/* Character classes */



/******************************************************/
/* main driver */
public int execute() 
{
	/* Open the input data file and process its contents */

	var file_path = "front.in";

	if(!System.IO.File.Exists(file_path))
	{
		System.Console.WriteLine("ERROR - cannot open front.in ");
		return 0;
	}
	
	 //if ((in_fp = new System.IO.StreamReader(("front.in")) == NULL)
	 //	System.Console.WriteLine("ERROR - cannot open front.in \n");
	 
	in_fp = new System.IO.StreamReader(file_path);
	getChar();
	 do 
	 {
	 	lex();
	 } 
	 while (nextToken != Program.EOF);

	
	 return 0;
}
/******************************************************/
/* lookup - a function to look up operators and
 parentheses and return the token */
int lookup(char ch) {
	 switch (ch) 
	 {
		 case '(':
			 addChar();
			 nextToken = Program.LEFT_PAREN;
			 break;
		 case ')':
			 addChar();
			 nextToken = Program.RIGHT_PAREN;
			 break;
		 case '+':
			 addChar();
			 nextToken = Program.ADD_OP;
			 break;
		 case '-':
			 addChar();
			 nextToken = Program.SUB_OP;
			 break;
		 case '*':
			 addChar();
			 nextToken = Program.MULT_OP;
			 break;
		 case '/':
			 addChar();
			 nextToken = Program.DIV_OP;
			 break;
		case '%':
			 addChar();
			 nextToken = Program.MODULO;
			 break;
		case '<':
			 addChar();
			 nextToken = Program.less_than;
			 break;
		case '>':
			 addChar();
			 nextToken = Program.greater_than;
			 break;
		case '=':
			 addChar();
			 nextToken = Program.equals;
			 break;
		case '&':
			 addChar();
			 nextToken = Program.ampers_and;
			 break;
		case '|':
			 addChar();
			 nextToken = Program.pipe;
			 break;
		case '.':
			 addChar();
			 nextToken = Program.PERIOD;
			 break;			 
		 default:
			 addChar();
			 nextToken = Program.EOF;
			 break;
	 }
	 return nextToken;
}

/******************************************************/
/* addChar - a function to add nextChar to lexeme */
void addChar() 
{
	if (lexLen <= 98) 
	{
		lexeme[lexLen++] = nextChar;
		lexeme[lexLen] = '\0';
	} else
	System.Console.WriteLine("Error - lexeme is too long \n");
}


/******************************************************/
/* getChar - a function to get the next character of
 input and determine its character class */
void getChar() 
{
	nextChar = (char) in_fp.Read();
	 if (nextChar != Program.EOF) 
	 {
		 if (Char.IsLetter(nextChar))//(isalpha(nextChar))
		 	charClass = Program.LETTER;
		 else if (Char.IsDigit(nextChar))
		 	charClass = Program.DIGIT;
		 else
		 	charClass = Program.UNKNOWN;
	 } 
	 else
	 	charClass = Program.EOF;
}


/******************************************************/
/* getNonBlank - a function to call getChar until it
 returns a non-whitespace character */
void getNonBlank() 
{
	while (Char.IsWhiteSpace(nextChar))
	getChar();
}

/******************************************************/
/* lex - a simple lexical analyzer for arithmetic
 expressions */
public (int Token, string Lexeme) lex() 
{
	 lexLen = 0;
	 getNonBlank();
	 switch (charClass) 
	 {
		/* Variable Identifiers */
		 case Program.LETTER:
			 addChar();
			 getChar();
			 while (charClass == Program.LETTER || charClass == Program.DIGIT) 
			 {
				 addChar();
				 getChar();
			 }
			 nextToken = Program.IDENT;
			 break;
		/* Integer literals */
		 case Program.DIGIT:
			 addChar();
			 getChar();
			 while (charClass == Program.DIGIT) {
				 addChar();
				 getChar();
			 }
			 nextToken = Program.INT_LIT;
		 	break;
		/* Parentheses and operators */
		 case Program.UNKNOWN:
			 lookup(nextChar);
			 getChar();
			 break;
			/* EOF */
		case Program.EOF:
			 nextToken = Program.EOF;
			 lexeme[0] = 'E';
			 lexeme[1] = 'O';
			 lexeme[2] = 'F';
			 lexeme[3] = '\0';
		 	break;
	 } /* End of switch */
	 //System.Console.WriteLine($"Next token is: {TokenDictionary[nextToken]} {nextToken}, Next lexeme is {Convert(lexeme)}");

	 return (nextToken, Convert(lexeme));
} 

	string Convert(char[] value)
	{
		var result = new System.Text.StringBuilder();

		foreach(var c in value)
		{
			if(c != '\0')
			{
				result.Append(c);
			}
			else
			{
				break;
			}
		}

		return result.ToString();
	}
}