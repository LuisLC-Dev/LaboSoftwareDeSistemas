grammar sicxeGram;

options {
    language=CSharp2; // Especifica el lenguaje objetivo de la gramática
}

// Producciones principales de la gramática
program: start propositions end; // Un programa se compone de un inicio, proposiciones y un final
start: label START num lineBreak; // La estructura del inicio
end: END label? lineBreak?; // La estructura del final
propositions: (proposition lineBreak)*; // Las proposiciones pueden ser múltiples
proposition: instruction | directive; // Una proposición puede ser una instrucción o una directiva
lineBreak: FINL; // Representa un salto de línea

// Manejo de directivas
directive: label? directiveType; // Una directiva puede tener una etiqueta opcional y un tipo de directiva
directiveType: byte | word | resb | resw | base; // Los diferentes tipos de directivas
byte: BYTE const; // Directiva byte con una constante
word: WORD num; // Directiva word con un número
resb: RESB num; // Directiva resb con un número
resw: RESW num; // Directiva resw con un número
base: BASE label; // Directiva base con una etiqueta

// Manejo de instrucciones
instruction: label? format; // Una instrucción puede tener una etiqueta opcional y un formato
format: f1 | f2 | f3 | f4; // Los diferentes formatos de instrucciones
f1: INSTR1; // Instrucciones de formato 1
f2: INSTR2_r1r2(reg COMMA reg) | INSTR2_r1r2(reg (',X'|', X')) |INSTR2_r1(reg) | INSTR2_r1n(reg COMMA num) | INSTR2_n(num); // Instrucciones de formato 2
f3: simple3 | indirect3 | immediate3; // Instrucciones de formato 3
f4: simple4 | indirect4 | immediate4; // Instrucciones de formato 4

// Modos de direccionamiento para instrucciones
simple3: INSTR3 (TEXT|num) (',X'|', X')? | 'RSUB'; // Instrucciones simples de formato 3
indirect3: INSTR3 '@'num | INSTR3 '@'TEXT; // Instrucciones indirectas de formato 3
immediate3: INSTR3 '#'num | INSTR3 '#'TEXT; // Instrucciones inmediatas de formato 3
simple4: INSTR4 TEXT (',X'|', X')? | '+RSUB'; // Instrucciones simples de formato 4
indirect4: INSTR4 '@'num | INSTR4 '@'TEXT; // Instrucciones indirectas de formato 4
immediate4:INSTR4 '#'num |INSTR4 '#'TEXT; // Instrucciones inmediatas de formato 4

// Tipos de entrada
num: NUMDEC | NUMHEX; // Números pueden ser decimales o hexadecimales
label: TEXT; // Etiquetas son texto
const: CONSTHEX | CONSTCAD; // Constantes pueden ser hexadecimales o cadenas
reg: REG; // Registros

// Reglas del Lexer
BASE: 'BASE';
RESW: 'RESW';
RESB: 'RESB';
WORD: 'WORD';
BYTE: 'BYTE';
START: 'START';
END: 'END';
COMMA: ','|', ';
INSTR1: 'FIX'|'FLOAT'|'HIO'|'NORM'|'SIO'|'TIO';
INSTR2_r1r2: 'ADDR'|'COMPR'|'DIVR'|'MULR'|'RMO'|'SUBR';
INSTR2_r1: 'CLEAR'|'TIXR';
INSTR2_r1n: 'SHIFTL'|'SHIFTR';
INSTR2_n: 'SVC';
INSTR3: 'ADD'|'ADDF'|'AND'|'COMP'|'COMPF'|'DIV'|'DIVF'|'J'|'JEQ'|'JGT'|'JLT'
        |'JSUB'|'LDA'|'LDB'|'LDCH'|'LDF'|'LDL'|'LDS'|'LDT'|'LDX'|'LPS'
        |'MUL'|'MULF'|'OR'|'RD'|'SSK'|'STA'|'STB'|'STCH'|'STF'|'STI'
        |'STL'|'STS'|'STSW'|'STT'|'STX'|'SUB'|'SUBF'|'TD'|'TIX'|'WD';
INSTR4: '+'('ADD'|'ADDF'|'AND'|'COMP'|'COMPF'|'DIV'|'DIVF'|'J'|'JEQ'|'JGT'|'JLT'
        |'JSUB'|'LDA'|'LDB'|'LDCH'|'LDF'|'LDL'|'LDS'|'LDT'|'LDX'|'LPS'
        |'MUL'|'MULF'|'OR'|'RD'|'SSK'|'STA'|'STB'|'STCH'|'STF'|'STI'
        |'STL'|'STS'|'STSW'|'STT'|'STX'|'SUB'|'SUBF'|'TD'|'TIX'|'WD');
FINL: ('\r\n')+ | ('\n')+;
REG: 'A'|'X'|'L'|'B'|'S'|'T'|'F'|'CP'|'PC'|'SW';
NUMDEC: ('0'..'9')+;
TEXT: ('a'..'z'|'A'..'Z')('0'..'9'|'a'..'z'|'A'..'Z')*;
NUMHEX_sh: (NUMDEC|'A'..'F')+;
NUMHEX: ('0'..'9'|'A'..'F')+('H' | 'h');
CONSTHEX: 'X\''NUMHEX_sh'\'';
CONSTCAD: 'C\''TEXT'\'';
