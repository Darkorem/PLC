using System;

namespace T2.construct_execute;

/*
<STMT> --> <IF_STMT> | <BLOCK> | <ASSIGN> | <DECLARE> | <WHILE_LOOP>
<STMT_LIST> --> { <STMT> `;` }
<WHILE_LOOP> --> `while` `(` <BOOL_EXPR> `)` <BLOCK> 
<IF_STMT> --> `if` `(` <BOOL_EXPR> `)` <BLOCK>  [ `else`  <BLOCK> ] 
<BLOCK> --> `{` <STMT_LIST> `}`
<DECLARE> --> `DataType` ID {`,` ID }
<ASSIGN> --> ID `=` <EXPR>
<EXPR> --> <TERM> {(`+`|`-`) <TERM>}
<TERM> --> <FACT> {(`*`|`/`|`%`) <FACT>}
<FACT> --> ID | INT_LIT | FLOAT_LIT | `(` <EXPR> `)`
<BOOL_EXPR> --> <BTERM> {(`>`|`<`|`>=`|`<=`) <BTERM>}
<BTERM> --> <BAND> {(`==`|`!=`) <BAND>}
<BAND> --> <BOR> {`&&` <BOR>}
<BOR> --> <EXPR> {`&&` <EXPR>}
*/



//<EXPR> --> <TERM> {(`+`|`-`) <TERM>}
public class expr
{
    term _t1;

    List<string> op;

    List<term> _t2;
    
    public expr(ParseNode value)
    {
        op = new();
        _t2 = new();
        
        _t1 = new term(value.Children[0]);

        if(value.Children.Count > 1)
        {
            for(var i = 1; i < value.Children.Count; i+=2)
            {
                var new_operator_string = value.Children[i].Lexeme;
                var new_term = new term(value.Children[i+1]);
                
                op.Add(new_operator_string);
                _t2.Add(new_term);
            }
        }        
    }

    public double execute()
    {
        
        var result =  _t1.execute();

        for(var i = 1; i < op.Count; i+=2)
        {
            var current_op = op[i];
            var current_term = _t2[i];
            if(current_op == "+")
            {
                result = result + current_term.execute();
            }
            else if(current_op == "-")
            {
                result = result - current_term.execute();
            }
            else
            {
                System.Console.WriteLine("This should NOT happen");
            }


        }

        return result;
    }
}

//<term> -> <factor> {(* | /|%) <factor>}
public class term
{
    factor _f1;


    List<string> op;

    List<factor> _f2;
    public term(ParseNode value)
    {
        op = new();
        _f2 = new();
        
        _f1 = new factor(value.Children[0]);

        if(value.Children.Count > 1)
        {
            for(var i = 1; i < value.Children.Count; i+=2)
            {
                var new_operator_string = value.Children[i].Lexeme;
                var new_factor = new factor(value.Children[i+1]);
                
                op.Add(new_operator_string);
                _f2.Add(new_factor);
            }
        }        
    }

    public double execute()
    {
        
        var result =  _f1.execute();

        for(var i = 1; i < op.Count; i+=2)
        {
            var current_op = op[i];
            var current_term = _f2[i];
            if(current_op == "*")
            {
                result = result * current_term.execute();
            }
            else if(current_op == "/")
            {
                result = result / current_term.execute();
            }
            else if(current_op == "%")
            {
                result = result % current_term.execute();
            }
            else
            {
                System.Console.WriteLine("This should NOT happen");
            }


        }

        return result;
    }
}

/*
<FACT> --> ID | INT_LIT | FLOAT_LIT | `(` <EXPR> `)`
*/
public class factor
{
    string id;
    double? number;
    expr _expr;
    public factor(ParseNode value)
    {
         if
         (
            value.Children.Count == 1 &&
            value.Children[0].Name == "IDENT")
        {
            id = value.Children[0].Lexeme;
        }
        else if(double.TryParse(value.Lexeme, out var new_number))
        {
            number = new_number;
        }
        else if(value.Children.Count == 3 &&
                value.Children[0].Name == "LEFT_PAREN" &&
                value.Children[0].Name == "RIGT_PAREN" 
        )
        {
            _expr = new expr(value.Children[1]);
        }
        else
        {
            
            System.Console.WriteLine("This should not happen.");
        }
    }

    public double execute()
    {
        if(!string.IsNullOrWhiteSpace(id))
        {
            //return value of the varialbe identified by id;
            return 45;
        }
        else if(number.HasValue)
        {
            return number.Value;
        }
        else
        {
            return _expr.execute();
        }
        
    }
}