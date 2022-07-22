grammar Lang;

/*
 * Tokens
 */

fragment A          : ('A'|'a') ;
fragment V          : ('V'|'v') ;
fragment G          : ('G'|'g') ;

fragment LOWERCASE : [a-z];
fragment UPPERCASE : [A-Z];

WORD : (LOWERCASE | UPPERCASE)+;
WHITESPACE : (' '|'\t')+ -> skip;
AVG : A V G;
LBR :  '(' ;
RBR :  ')' ;
ADD :  '+' ;
SUB :  '-' ;
MUL :  '*' ;
DIV :  '/' ;
NUMBER: '-'?[0-9]+;
COMMA: ',' ;
CELL: WORD NUMBER;

/*
 * Rules
 */

start : expression;
expression : CELL                                                 # Cell
           | NUMBER                                               # Number
           | LBR inner=expression RBR                             # Parentheses
           | AVG LBR CELL COMMA CELL RBR                          # AverageOfRange
           | left=expression operator=(MUL|DIV) right=expression  # MultiplicationOrDivision
           | left=expression operator=(ADD|SUB) right=expression  # AdditionOrSubtraction
           ;
           