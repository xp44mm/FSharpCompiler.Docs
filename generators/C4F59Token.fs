namespace C4F59

open FSharp.Literals.StringUtils
open System

type C4F59Token =
    | EOL
    | LPAREN
    | RPAREN
    | STAR
    | DIV
    | PLUS
    | MINUS
    | NUMBER of int

    member this.getTag() =
        match this with
        | EOL -> "\n"
        | LPAREN -> "("
        | RPAREN -> ")"
        | STAR -> "*"
        | DIV -> "/"
        | PLUS -> "+"
        | MINUS -> "-"
        | NUMBER _ -> "NUMBER"

    static member from (text:string) =
        let rec loop (inp:string) =
            seq {
                match inp with
                | "" -> ()

                | Prefix @"[\s-[\n]]+" (_,rest) // 空白
                    -> yield! loop rest

                | Prefix @"\n" (_,rest) -> //换行
                    yield EOL
                    yield! loop rest

                | PrefixChar '(' rest ->
                    yield LPAREN
                    yield! loop rest

                | PrefixChar ')' rest ->
                    yield RPAREN
                    yield! loop rest

                | PrefixChar '*' rest ->
                    yield STAR
                    yield! loop rest

                | PrefixChar '/' rest ->
                    yield DIV
                    yield! loop rest

                | PrefixChar '+' rest ->
                    yield PLUS
                    yield! loop rest

                | PrefixChar '-' rest ->
                    yield MINUS
                    yield! loop rest

                | Prefix @"\d+" (n,rest) ->
                    yield  NUMBER(Int32.Parse n)
                    yield! loop rest

                | never -> failwith never
            }
        loop text
