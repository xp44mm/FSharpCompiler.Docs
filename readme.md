This project is a help document for a series of NuGet packages, including:

```
FSharpCompiler.Analyzing
FSharpCompiler.Lex
FSharpCompiler.Parsing
FSharpCompiler.Yacc
```

The preface


编译原理龙书是程序员的必读书籍，其中介绍了两个编译器前端生成工具，lex，yacc。用于C语言，现在，世界上也有其他语言的版本。比如F#版本的编译器自动生成工具。但是，他们都是简单迁移lex和yacc到新平台。并没有利用新语言的特性，没有改进易用性。

分为词法生成器，语法生成器两部分，两部分是可以独立使用的，毫无关联。

没有语义动作，和语言分离。

编译器生成工具生成的是查询表数据，而非执行程序，可以很简单迁移到其他平台。

你不需要深入学习F#语言就可以生成一个全功能的LALR语法的编译器。

语法符号看作字符串，不必带附属数据的复杂类型。

