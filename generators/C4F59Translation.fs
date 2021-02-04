module C4F59.C4F59Translation

open FSharpCompiler.Parsing

/// 
let rec translateExpr = function
    | Interior("expr",[e1;Terminal PLUS;e2;]) ->
        C4F59Expr.Add(translateExpr e1, translateExpr e2)
    | Interior("expr",[e1;Terminal MINUS;e2;]) ->
        C4F59Expr.Sub(translateExpr e1, translateExpr e2)
    | Interior("expr",[e1;Terminal STAR;e2;]) ->
        C4F59Expr.Mul(translateExpr e1, translateExpr e2)
    | Interior("expr",[e1;Terminal DIV;e2;]) ->
        C4F59Expr.Div(translateExpr e1, translateExpr e2)
    | Interior("expr",[Terminal LPAREN;e;Terminal RPAREN;]) ->
        translateExpr e
    | Interior("expr",[Terminal MINUS;e;]) ->
        C4F59Expr.Negative(translateExpr e)
    | Interior("expr",[Terminal (NUMBER n);]) ->
        C4F59Expr.Number n
    | never -> failwithf "%A" never.firstLevel

/// 
let rec translateLines tree = 
    [
        match tree with
        | Interior("lines",[lines;expr;Terminal EOL]) ->
            yield! translateLines lines
            yield translateExpr expr
        | Interior("lines",[lines;Terminal EOL]) ->
            yield! (translateLines lines)
        | Interior("lines",[]) ->
            ()
        | _ -> failwithf "%A" tree.firstLevel
    ]

