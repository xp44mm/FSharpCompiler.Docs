namespace E69

open FSharp.Literals.StringUtils
open System

type ExpToken =
    | EOL
    | LPAREN
    | RPAREN
    | STAR
    | PLUS
    | DIGIT of int

    member this.getTag() =
        match this with
        | EOL -> "\n"
        | LPAREN -> "("
        | RPAREN -> ")"
        | STAR -> "*"
        | PLUS -> "+"
        | DIGIT _ -> "DIGIT"

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

                | PrefixChar '+' rest ->
                    yield PLUS
                    yield! loop rest

                | Prefix @"\d+" (lexeme,rest) ->
                    yield  DIGIT(Int32.Parse lexeme)
                    yield! loop rest

                | never -> failwith never
            }
        loop text

