namespace C4F59

type C4F59Expr =
    | Add      of C4F59Expr * C4F59Expr
    | Sub      of C4F59Expr * C4F59Expr
    | Mul      of C4F59Expr * C4F59Expr
    | Div      of C4F59Expr * C4F59Expr
    | Negative of C4F59Expr
    | Number   of int
