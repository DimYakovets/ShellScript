using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ShellScript.Lexer
{
    class Lexer
    {
        public Token[] Tokinaze(string code)
        {
            int line = 1, column = 1, i = 0;
            var list = new List<Token>();

            char peek() => i >= code.Length ? '\0' : code[i];
            char seenext() => i + 1 >= code.Length ? '\0' : code[i + 1];
            void next() => i++;

            bool isSkipable(char ch) => ch == ' ' || ch == '\n' || ch == '\t' || ch == '\r';
            bool isChar(char ch) => ch > 64 && ch < 91 || ch > 96 && ch < 123 || ch == '_' || isNumber(ch);
            bool isNumber(char ch) => ch > 47 && ch < 58;
            bool isOperator(char ch) => (ch > 32 && ch < 48 && ch != '\'' && ch != '\"') || (ch > 57 && ch < 65) || (ch > 90 && ch < 96) || (ch > 122 && ch < 127);

            while (i <= code.Length)
            {
                var current = peek();

                if (isSkipable(current))
                {
                    next();
                    if (current == '\n')
                    {
                        line++;
                        column = 1;
                    }
                    else
                    {
                        column++;
                    }
                }
                else if (isChar(current) && !isNumber(current))
                {
                    var buffer = current.ToString();
                    next();
                    current = peek();
                    while (isChar(current))
                    {
                        buffer += current;
                        next();
                        current = peek();
                    }
                    if (buffer == "true" || buffer == "false")
                    {
                        list.Add(new Token(TokenType.literal_bool, buffer, line, column));
                    }
                    else
                    {
                        list.Add(new Token(TokenType.word, buffer, line, column));
                    }

                    column += buffer.Length;
                }
                else if ((current == '-' && isNumber(seenext())) || isNumber(current))
                {
                    var regint = new Regex("^-*[0-9]+$");
                    var regfloat = new Regex("^-*[0-9]+(.[0-9]+)*$");

                    string buffer = current.ToString();
                    next();
                    current = peek();

                    while (isNumber(current) || current == '.' || current == 'f' || current == '-')
                    {
                        buffer += current;
                        next();
                        current = peek();
                    }
                    if (regint.IsMatch(buffer))
                    {
                        list.Add(new Token(TokenType.literal_int, regint.Match(buffer).Value, line, column));
                    }
                    else if (regfloat.IsMatch(buffer))
                    {
                        list.Add(new Token(TokenType.literal_float, regfloat.Match(buffer).Value, line, column));
                    }
                    else
                    {
                        throw new Exception($"Invalid number ({line}, {column}).");
                    }
                    column += buffer.Length;
                }
                else if (isOperator(current))
                {
                    if (current == '=') list.Add(new Token(TokenType.op_equals, current.ToString(), line, column));
                    else if (current == ',') list.Add(new Token(TokenType.op_comma, current.ToString(), line, column));
                    else if (current == '.') list.Add(new Token(TokenType.op_dot, current.ToString(), line, column));
                    else if (current == ';') list.Add(new Token(TokenType.op_semicolon, current.ToString(), line, column));
                    else if (current == ':') list.Add(new Token(TokenType.op_colon, current.ToString(), line, column));
                    else if (current == '+') list.Add(new Token(TokenType.op_plus, current.ToString(), line, column));
                    else if (current == '-') list.Add(new Token(TokenType.op_minus, current.ToString(), line, column));
                    else if (current == '*') list.Add(new Token(TokenType.op_asterisk, current.ToString(), line, column));
                    else if (current == '/') list.Add(new Token(TokenType.op_slash, current.ToString(), line, column));
                    else if (current == '%') list.Add(new Token(TokenType.op_percent, current.ToString(), line, column));
                    else if (current == '(') list.Add(new Token(TokenType.op_parentheses_open, current.ToString(), line, column));
                    else if (current == ')') list.Add(new Token(TokenType.op_parentheses_close, current.ToString(), line, column));
                    else if (current == '[') list.Add(new Token(TokenType.op_breackets_open, current.ToString(), line, column));
                    else if (current == ']') list.Add(new Token(TokenType.op_breackets_close, current.ToString(), line, column));
                    else if (current == '<') list.Add(new Token(TokenType.op_angle_breackets_open, current.ToString(), line, column));
                    else if (current == '>') list.Add(new Token(TokenType.op_angle_breackets_close, current.ToString(), line, column));
                    else if (current == '{') list.Add(new Token(TokenType.op_brace_open, current.ToString(), line, column));
                    else if (current == '}') list.Add(new Token(TokenType.op_brace_close, current.ToString(), line, column));
                    else if (current == '!') list.Add(new Token(TokenType.op_exclamation_mark, current.ToString(), line, column));
                    else if (current == '&') list.Add(new Token(TokenType.op_ampersand, current.ToString(), line, column));
                    else if (current == '?') list.Add(new Token(TokenType.op_question_mark, current.ToString(), line, column));
                    else if (current == '|') list.Add(new Token(TokenType.op_vertical_bar, current.ToString(), line, column));
                    else if (current == '@') list.Add(new Token(TokenType.op_at_sing, current.ToString(), line, column));
                    else if (current == '#') list.Add(new Token(TokenType.op_hash, current.ToString(), line, column));
                    else if (current == '$') list.Add(new Token(TokenType.op_dollar, current.ToString(), line, column));
                    else if (current == '_') list.Add(new Token(TokenType.op_underscore, current.ToString(), line, column));
                    else if (current == '^') list.Add(new Token(TokenType.op_caret, current.ToString(), line, column));
                    else if (current == '~') list.Add(new Token(TokenType.op_tilda, current.ToString(), line, column));
                    next();
                    column++;
                }
                else if (current == '"')
                {
                    string buffer = string.Empty;
                    next();
                    current = peek();
                    while (current != '"')
                    {
                        if (current == '\\')
                        {
                            next();
                            current = peek();
                            if (current == 'n')
                            {
                                buffer += "\n";
                            }
                            else if (current == 't')
                            {
                                buffer += "\t";
                            }
                            else if (current == 't')
                            {
                                buffer += "\r";
                            }
                            else if (current == '0')
                            {
                                buffer += "\0";
                            }
                            else
                            {
                                buffer += current;
                            }
                            next();
                            current = peek();
                        }
                        else
                        {
                            buffer += current;
                            next();
                            current = peek();
                        }
                    }
                    list.Add(new Token(TokenType.literal_string, buffer, line, column));
                    next();
                    column += buffer.Length;
                }
                else if (current == '\0')
                {
                    list.Add(new Token(TokenType.eof, "eof", line, column));
                    break;
                }
                else
                {
                    throw new Exception($"Unknow character ({line},{column}) : '{current}'");
                }
            }
            var exp = list.ToArray();
            ConcatWords(ref exp);
            ConcatOperators(ref exp);
            return exp;
        }
        private void ConcatWords(ref Token[] tokens)
        {
            var last = TokenType.op_dot;
            int startindex = -1;
            int endindex = -1;
            var list = new List<Token>();

            for (int i = 0; i < tokens.Length; i++)
            {
                if ((tokens[i].Type == TokenType.word || tokens[i].Type == TokenType.op_dot) && last != tokens[i].Type)
                {
                    if (startindex < 0)
                    {
                        startindex = i;
                    }
                    list.Add(tokens[i]);
                    last = tokens[i].Type;
                    endindex = i;
                }
                else
                {
                    if (startindex > -1 && last != TokenType.op_dot)
                    {
                        var path = string.Join("", list.Select(p => p.Text).ToArray());
                        tokens = Replace(tokens, startindex, endindex, new Token(TokenType.word, path, tokens[startindex].Line, tokens[startindex].Column));
                        i = -1;
                    }
                    list.Clear();
                    startindex = -1;
                }
            }
        }
        private void ConcatOperators(ref Token[] tokens)
        {
            //==
            for (int i = 1; i < tokens.Length; i++)
            {
                if (tokens[i].Type == TokenType.op_equals && tokens[i - 1].Type == TokenType.op_equals)
                {
                    tokens = Replace(tokens, i - 1, i, new Token(TokenType.op_double_equals, "==", tokens[i - 1].Line, tokens[i - 1].Column));
                    i = 0;
                }
            }
            //++
            for (int i = 1; i < tokens.Length; i++)
            {
                if (tokens[i].Type == TokenType.op_plus && tokens[i - 1].Type == TokenType.op_plus)
                {
                    tokens = Replace(tokens, i - 1, i, new Token(TokenType.op_double_plus, "++", tokens[i - 1].Line, tokens[i - 1].Column));
                    i = 0;
                }
            }
            //--
            for (int i = 1; i < tokens.Length; i++)
            {
                if (tokens[i].Type == TokenType.op_minus && tokens[i - 1].Type == TokenType.op_minus)
                {
                    tokens = Replace(tokens, i - 1, i, new Token(TokenType.op_double_minus, "--", tokens[i - 1].Line, tokens[i - 1].Column));
                    i = 0;
                }
            }
            //>=
            for (int i = 1; i < tokens.Length; i++)
            {
                if (tokens[i].Type == TokenType.op_equals && tokens[i - 1].Type == TokenType.op_angle_breackets_close)
                {
                    tokens = Replace(tokens, i - 1, i, new Token(TokenType.op_greater_equals, ">=", tokens[i - 1].Line, tokens[i - 1].Column));
                    i = 0;
                }
            }
            //<=
            for (int i = 1; i < tokens.Length; i++)
            {
                if (tokens[i].Type == TokenType.op_equals && tokens[i - 1].Type == TokenType.op_angle_breackets_open)
                {
                    tokens = Replace(tokens, i - 1, i, new Token(TokenType.op_less_equals, "<=", tokens[i - 1].Line, tokens[i - 1].Column));
                    i = 0;
                }
            }
        }
        private T[] Replace<T>(T[] array, int startindex, int endindex, T item)
        {
            var list = new List<T>();
            for (int i = 0; i < array.Length; i++)
            {
                if (i == startindex)
                {
                    list.Add(item);
                }
                if (i < startindex || i > endindex)
                {
                    list.Add(array[i]);
                }
            }
            return list.ToArray();
        }
    }
}