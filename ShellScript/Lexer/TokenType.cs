namespace ShellScript.Lexer
{
    enum TokenType
    {
        word,

        literal_int, // -1, 5 
        literal_float, // 0.55, 
        literal_string, // "some string"
        literal_bool, // true or false

        op_equals, // =
        op_comma, //, 
        op_dot, //. 
        op_semicolon, // ;
        op_colon, // ;
        op_plus, // +
        op_minus, // -
        op_asterisk, // *
        op_slash, // /
        op_percent, // %
        op_parentheses_open, // (
        op_parentheses_close, // )
        op_breackets_open, // [
        op_breackets_close, // ]
        op_angle_breackets_open, // <
        op_angle_breackets_close, // <
        op_brace_open, // {
        op_brace_close, // }
        op_exclamation_mark, // !
        op_ampersand, // &
        op_question_mark, // ?
        op_vertical_bar, // |
        op_at_sing, // @
        op_hash, // #
        op_dollar, // $
        op_underscore, // _
        op_caret, // ^
        op_tilda, // ~
        op_double_equals,
        op_double_plus,
        op_double_minus,
        op_greater_equals,
        op_less_equals,


        eof // end of file
    }
}