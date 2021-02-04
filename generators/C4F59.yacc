lines : lines expr "\n"
      | lines "\n"
      | /* empty */
      ;
expr : expr "+" expr
     | expr "-" expr
     | expr "*" expr
     | expr "/" expr
     | "(" expr ")"
     | "-" expr %prec UMINUS
     | NUMBER
     ;

%%

%left "+" "-"
%left "*" "/"
%right UMINUS