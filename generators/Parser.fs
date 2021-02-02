module E69.Parser

open FSharpCompiler.Parsing
open E69

let parser =
    SyntacticParser(
        E69ParseTable.rules,
        E69ParseTable.kernelSymbols,
        E69ParseTable.parsingTable)

let parseTokens tokens =
    parser.parse(tokens,fun (tok:ExpToken) -> tok.getTag())

