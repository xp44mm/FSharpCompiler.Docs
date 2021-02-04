namespace C4F59

open Xunit
open Xunit.Abstractions
open System.IO
open FSharp.Literals
open FSharpCompiler.Yacc
open FSharp.xUnit
open FSharpCompiler.Parsing

type C4F59Test(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    //所有冲突都解决了，可以生成解析表
    let path = Path.Combine(__SOURCE_DIRECTORY__, @"C4F59.yacc")
    let text = File.ReadAllText(path)
    let yaccFile = YaccFile.parse text

    let yacc = ParseTable.create(yaccFile.mainRules, yaccFile.precedences)
    [<Fact>]
    member this.``yacc file content``() =
        //show yaccFile
        let y = {
            mainRules=[
                ["lines";"lines";"expr";"\n"];
                ["lines";"lines";"\n"];
                ["lines"];
                ["expr";"expr";"+";"expr"];
                ["expr";"expr";"-";"expr"];
                ["expr";"expr";"*";"expr"];
                ["expr";"expr";"/";"expr"];
                ["expr";"(";"expr";")"];
                ["expr";"-";"expr"];
                ["expr";"NUMBER"]
                ];
            precedences=[
                LeftAssoc,[TerminalKey "+";TerminalKey "-"];
                LeftAssoc,[TerminalKey "*";TerminalKey "/"];
                RightAssoc,[ProductionKey ["expr";"-";"expr"]]
                ]
            }
        Should.equal y yaccFile

    [<Fact>]
    member this.``generate parse table``() =
        let result =
            [
                "let rules = " + Render.stringify yacc.rules
                "let kernelSymbols = " + Render.stringify yacc.kernelSymbols
                "let parsingTable = " + Render.stringify yacc.parsingTable
            ] |> String.concat System.Environment.NewLine
        output.WriteLine(result)

    [<Fact>]
    member this.``validate parse table``() =
        Should.equal yacc.rules         C4F59ParseTable.rules
        Should.equal yacc.kernelSymbols C4F59ParseTable.kernelSymbols
        Should.equal yacc.parsingTable  C4F59ParseTable.parsingTable

    [<Fact>]
    member this.``terminals``() =
        let grammar = Grammar.from yaccFile.mainRules
        let terminals = grammar.symbols - grammar.nonterminals
        let result = Render.stringify terminals
        output.WriteLine(result)

    [<Fact>]
    member this.``tokenize``() =
        let inp = "-1/2+3*(4-5)" + System.Environment.NewLine
        let tokens = C4F59Token.from inp
        let result = Render.stringify (List.ofSeq tokens)
        output.WriteLine(result)

    [<Fact>]
    member this.``parse tokens``() =
        let tokens = [MINUS;NUMBER 1;DIV;NUMBER 2;PLUS;NUMBER 3;STAR;LPAREN;NUMBER 4;MINUS;NUMBER 5;RPAREN;EOL]
        let tree = C4F59Parser. parseTokens tokens
        let result = Render.stringify tree
        output.WriteLine(result)

    [<Fact>]
    member this.``translate to expr``() =
        let tokens = [MINUS;NUMBER 1;DIV;NUMBER 2;PLUS;NUMBER 3;STAR;LPAREN;NUMBER 4;MINUS;NUMBER 5;RPAREN;EOL]
        let tree = C4F59Parser. parseTokens tokens
        let exprs = C4F59Translation.translateLines tree

        let result = Render.stringify exprs
        output.WriteLine(result)
