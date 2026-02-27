# JASON Compiler

A simple compiler frontend built in C# (.NET WinForms) for a custom language called **JASON**. It performs lexical analysis (scanning) and syntactic analysis (parsing), then displays the results in a GUI.

---

## Project Structure

```
JASON_Compiler/
├── Scanner.cs           ← Lexer / Tokenizer
├── Parser.cs            ← Parser + Parse Tree builder
├── JASON_Compiler.cs    ← Main compiler controller (glues scanner + parser)
├── Errors.cs            ← Global error collector
├── Form1.cs/.Designer   ← Main GUI window
├── Form2.cs/.Designer   ← Secondary GUI window (unused/in progress)
├── Program.cs           ← App entry point
└── JasonTestCase.txt    ← Sample JASON source code for testing
```

---

## File-by-File Breakdown

### `Scanner.cs` — The Lexer
Responsible for breaking raw source code text into a list of **tokens**.

- Defines the `Token` class (holds a lexeme string + its `Token_Class` enum value)
- Defines the `Token_Class` enum — all valid token types: keywords (`begin`, `end`, `if`, `while`, etc.), operators (`+`, `-`, `=`, `<`, etc.), and literals
- The `Scanner` class has a `StartScanning(string SourceCode)` method that walks through the source character by character and classifies each token
- Handles: reserved words, identifiers, integer/real constants, operators, and `{ }` comments (skips them)
- Unrecognized tokens get added to the global error list
- After scanning, results are stored in `JASON_Compiler.TokenStream`

---

### `Parser.cs` — The Parser
Responsible for checking the **grammatical structure** of the token stream and building a **parse tree**.

- Defines the `Node` class — a simple tree node with a name and list of children
- The `Parser` class has a `StartParsing(List<Token>)` method that runs a **recursive descent parser**
- Currently has stub methods:
    - `Program()` — top-level rule; calls Header, DeclSec, Block, then expects a `.`
    - `Header()` — placeholder (not yet implemented)
    - `DeclSec()` — placeholder for variable declarations
    - `Block()` — placeholder for statements
- `match(Token_Class)` — consumes the next token and checks it matches the expected type; adds a parsing error if not
- `PrintParseTree()` / `PrintTree()` — converts the `Node` tree into a WinForms `TreeView`-compatible `TreeNode` tree for display

---

### `JASON_Compiler.cs` — The Compiler Controller
The central coordinator that wires everything together.

- Holds static references to the `Scanner`, `Parser`, token stream, and parse tree root
- `Start_Compiling(string SourceCode)` runs the full pipeline: Scanner → Parser
- After compilation, `treeroot` holds the resulting parse tree

---

### `Errors.cs` — The Error Handler
A dead-simple global error collector.

- Just a static `List<string>` called `Error_List`
- Both the scanner and parser append error messages to it
- The GUI reads from it after compilation to display errors

---

### `Form1.cs` — The Main GUI
The primary user-facing window. Contains:

- **textBox1** — where you type/paste your JASON source code
- **dataGridView1** — displays the token list (lexeme + token class) after scanning
- **treeView1** — displays the parse tree after parsing
- **textBox2** — displays any errors collected during compilation
- **button1 ("Compile !")** — triggers `JASON_Compiler.Start_Compiling()`, then populates all the above
- **button2 ("Clear All")** — resets everything for a fresh run

---

### `Form2.cs` — Secondary GUI (Work in Progress)
A more polished layout that was being designed but is not wired up yet. It includes panels for JASON code input, token classes, a symbol table, and error list — but the compile button has no event handler connected.

---

### `Program.cs` — Entry Point
Standard WinForms boilerplate. Launches `Form1`.

---

## The JASON Language (from the test case)

Based on `JasonTestCase.txt`, JASON looks like this:

```
program xyz ;
declare
    integer x, y;
    real z;
begin
    set x = 10;
    set y = 5;
    write y;
    if x < y then
        set x = 10;
    endif;
    write x;
end.
```

Key language features:
- Programs start with `program <name> ;` and end with `end.`
- Variable declarations go in a `declare` block (`integer` or `real`)
- Statements go between `begin` and `end`
- Assignment uses `set <var> = <expr> ;`
- Conditionals: `if ... then ... endif`
- Loops: `while ... do ... endwhile` and `until ... do ... enduntil`
- I/O: `read` and `write`
- Comments are wrapped in `{ curly braces }`
- Case-insensitive (source is lowercased before scanning)

---

## Compilation Pipeline

```
Source Code (string)
       ↓
   Scanner.StartScanning()
       ↓
   TokenStream (List<Token>)
       ↓
   Parser.StartParsing()
       ↓
   Parse Tree (Node) + Errors (Errors.Error_List)
       ↓
   Form1 displays tokens, tree, and errors
```
