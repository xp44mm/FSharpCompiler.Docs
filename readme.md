This project is a help document for a series of NuGet packages, including:

```
FSharpCompiler.Analyzing
FSharpCompiler.Lex

FSharpCompiler.Parsing
FSharpCompiler.Yacc
```

The preface

编译原理龙书是程序员的必读书籍，其中介绍了两个编译器前端生成工具，lex，yacc。用于C语言，现在，世界上也有其他语言的版本。比如F#版本的编译器自动生成工具。但是，他们都是简单迁移lex和yacc到新平台。并没有利用新语言的特性，没有改进易用性。

原来的程序是多年前研发，受限于当时的软件，硬件条件，做了许多额外的复杂工作。本系列包可以利用高级语言和现代高速频率，大容量内存的计算机。

一次循环只做一件事。

本程序性能大约是C版本的2~4倍时间左右，大欧时间是同一数量级的。

分为词法生成器，语法生成器两部分，两部分是可以独立使用的，毫无关联。

所有的类型都是抽象语法树类型的可区分联合实例。

没有语义动作，和语言分离。

编译器生成工具生成的是查询表数据，而非执行程序，可以很简单迁移到其他平台。

你不需要深入学习F#语言就可以生成一个全功能的LALR语法的编译器。

语法符号看作字符串，不必带附属数据的复杂类型。

帮助文件请到文件夹FSharpCompiler.Docs中查找。

