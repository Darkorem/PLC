/* ifstmt
 Parses strings in the language generated by the rule:
 <ifstmt> -> if (<boolexpr>) <statement>
 [else <statement>]
 */
void ifstmt(void) {
 if (nextToken != IF_CODE)
 	error();
 else {
	lex();
 	if (nextToken != LEFT_PAREN)
 		error();
 	else {
 		lex();
 		boolexpr();
 		if (nextToken != RIGHT_PAREN)
 			error();
 		else {
 			lex();
		 	statement();
 			if (nextToken == ELSE_CODE) {
 				lex();
 				statement();
 			}
 		}
 	}
 }
}