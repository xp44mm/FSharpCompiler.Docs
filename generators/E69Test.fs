namespace E69

open Xunit
open Xunit.Abstractions
open System.IO
open FSharp.Literals
open FSharpCompiler.Yacc
open FSharp.xUnit
open FSharpCompiler.Parsing

type YaccFileParserTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    //所有冲突都解决了，可以生成解析表
    let path = Path.Combine(__SOURCE_DIRECTORY__, @"E69.yacc")
    let text = File.ReadAllText(path)
    let yaccFile = YaccFile.parse text
    let yacc = ParseTable.create(yaccFile.mainRules, yaccFile.precedences)

    [<Fact>]
    member this.``file content``() =
        show yaccFile

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
        Should.equal yacc.rules E69ParseTable.rules
        Should.equal yacc.kernelSymbols E69ParseTable.kernelSymbols
        Should.equal yacc.parsingTable E69ParseTable.parsingTable

    [<Fact>]
    member this.``terminals``() =
        let grammar = Grammar.from yaccFile.mainRules
        let terminals = grammar.symbols - grammar.nonterminals
        let result = Render.stringify terminals
        output.WriteLine(result)

    [<Fact>]
    member this.``tokenize``() =
        let inp = "1+2*(4+3)" + System.Environment.NewLine
        let tokens = E69.ExpToken.from inp
        let result = Render.stringify (List.ofSeq tokens)
        output.WriteLine(result)

    [<Fact>]
    member this.``parse tokens``() =
        let tokens = [DIGIT 1;PLUS;DIGIT 2;STAR;LPAREN;DIGIT 4;PLUS;DIGIT 3;RPAREN;EOL]
        let tree = E69.Parser.parseTokens tokens
        let result = Render.stringify tree
        let y = Interior("line",[
            Interior("expr",[
                Interior("expr",[
                    Interior("term",[
                        Interior("factor",[
                            Terminal(DIGIT 1)])])]);
                Terminal PLUS;
                Interior("term",[
                    Interior("term",[
                        Interior("factor",[
                            Terminal(DIGIT 2)])]);
                    Terminal STAR;
                    Interior("factor",[
                        Terminal LPAREN;
                        Interior("expr",[
                            Interior("expr",[
                                Interior("term",[Interior("factor",[Terminal(DIGIT 4)])])]);
                            Terminal PLUS;
                            Interior("term",[
                                Interior("factor",[Terminal(DIGIT 3)])])]);
                        Terminal RPAREN])])]);
            Terminal EOL])
        output.WriteLine(result)
