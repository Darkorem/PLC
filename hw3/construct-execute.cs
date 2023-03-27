using System;

namespace hw3.construct_execute;



//<expr> -> <a> <or_operator> <a>
public class expr
{
    a _a1;
    string or_operator;

    a _a2;
    public expr(ParseNode value)
    {
        _a1 = new a(value.Children[0]);
        if(value.Children.Count > 1)
        {
            or_operator = get_string_value(value.Children[1]);
            _a2  = new a(value.Children[2]);
        }
    }

    public double execute()
    {
        return 0d;
    }




    public static string get_string_value(ParseNode value)
    {
        string result = null;

        if(value.Children.Count == 0)
        {
            result = value.Lexeme;
        }
        else 
        {
            foreach(var child in value.Children)
            {
                result = get_string_value(child);
                if(!string.IsNullOrWhiteSpace(result))
                {
                    break;
                }
            }
        }


        return result;
        
        
    }
}

// <or_operator> := <pipe>|<pipe><pipe>

public class or_operator
{
    string op;

    
    public or_operator(ParseNode value)
    {
        System.Console.WriteLine("here");
        
    }

    public double execute()
    {
        return 0d;
    }
}

// <pipe>
public class pipe
{
    public pipe(ParseNode value)
    {
        
    }

    public double execute()
    {
        return 0d;
    }
}


//<l> -> <term> {(+ | -) <term>}
public class l
{
    term _t1;


    List<string> op;

    List<term> _t2;
    public l(ParseNode value)
    {
        if(value.Children.Count == 1)
        {
            _t1 = new term(value.Children[0]);
        }
        else
        {
            System.Console.WriteLine("here");
        }
        
    }

    public double execute()
    {
        return 0d;
    }
}

//<term> -> <factor> {(* | /|%) <factor>}
public class term
{
    factor _f;
    List<string> operator_list = new();
    List<factor> factor_list = new();
    public term(ParseNode value)
    {
        
        _f = new factor(value.Children[0]);

        for(var i = 1; i < value.Children.Count; i+=2)
        {
            operator_list.Add(value.Children[i].Name);
            factor_list.Add(new factor(value.Children[i+1]));
    
        }
    }

    public double execute()
    {
        return 0d;
    }
}

//<factor> -> id | number | ( <expr> )
public class factor
{
    string id;
    double number;
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
        else
        {
            _expr = new expr(value.Children[1]);
        }
    }

    public double execute()
    {
        return 0d;
    }
}

//<a> -> <e> <and_operator> <e>
public class a
{
    e _e1;
    string and_operator;
    e _e2;

    public a(ParseNode value)
    {
        _e1 = new e(value.Children[0]);
        if(value.Children.Count > 1)
        {
            and_operator = expr.get_string_value(value.Children[1]);
            _e2 = new e(value.Children[2]);
        }
    }

    public double execute()
    {
        return 0d;
    }
}

//<e> -> <c> <and_operator> <c>
public class e
{
    c _c1;
    string and_operator;
    c _c2;

    public e(ParseNode value)
    {
        _c1 = new c(value.Children[0]);
        if(value.Children.Count > 1)
        {
            and_operator = expr.get_string_value(value.Children[1]);
            _c2 = new c(value.Children[2]);
        }
    }

    public double execute()
    {
        return 0d;
    }
}

//<c> -> <l> <comparison_operator> <l>

public class c
{

    l _l1;
    string comparison_operator;
    l _l2;
    public c(ParseNode value)
    {
        _l1 = new l(value.Children[0]);

        if(value.Children.Count > 1)
        {
            comparison_operator = expr.get_string_value(value.Children[1]);
            _l2 = new l(value.Children[2]);
        }
    }

    public double execute()
    {
        return 0d;
    }
}