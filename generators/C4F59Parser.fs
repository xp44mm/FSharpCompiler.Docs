module C4F59.C4F59Parser

open FSharpCompiler.Parsing

let parser =
    SyntacticParser(
        C4F59ParseTable.rules,
        C4F59ParseTable.kernelSymbols,
        C4F59ParseTable.parsingTable
        )

let parseTokens tokens =
    parser.parse(tokens,fun (tok:C4F59Token) -> tok.getTag())

