using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


namespace JSON
{
    public class DictionaryJson
    {
        public class JsonDictionary
        {
            public Dictionary<object, object> dict;

            public JsonDictionary(object AnonymusObject = null)
            {
                dict = new Dictionary<object, object>();
                if (AnonymusObject == null) { return; }
                var properties = AnonymusObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var item in properties)
                {
                    if (item.GetValue(AnonymusObject).GetType().Name.StartsWith("<>f__AnonymousType")) dict.Add(item.Name, new JsonDictionary(item.GetValue(AnonymusObject)));
                    else dict.Add(item.Name, item.GetValue(AnonymusObject));
                }
            }

            public JsonDictionary(Dictionary<object, object> dict)
            {
                this.dict = dict;
            }


            public object this[object index]
            {
                get
                {
                    return dict[index];
                }
            }

        }

        private static void Check(object value, ref short tubs, StringBuilder json)
        {
            if (value is JsonDictionary) Dict2((JsonDictionary)value, ref tubs, json);
            else if (value is object[]) List2((object[])value, json);
            else if (value is string) json.Append($"{'"'}{(string)value}{'"'}");
            else json.Append(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
        }

        private static void List2(object[] obj, StringBuilder json)
        {
            json.Append("[");

            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i] is string) json.Append($"{'"'}{Convert.ToString(obj[i], System.Globalization.CultureInfo.InvariantCulture)}{'"'}");
                else json.Append(Convert.ToString(obj[i], System.Globalization.CultureInfo.InvariantCulture));
                if (i != obj.Length - 1) json.Append(", ");
            }

            json.Append("]");
        }

        private static void Dict2(JsonDictionary dict, ref short tubs, StringBuilder json)
        {
            short CurrentTubs = tubs++;

            json.Append("{\n");
            short i = 0;

            foreach (KeyValuePair<object, object> point in dict.dict)
            {
                if (point.Key is string) json.Append($"{new string(' ', 3 * (CurrentTubs + 1))} {'"'}{Convert.ToString(point.Key, System.Globalization.CultureInfo.InvariantCulture)}{'"'}: ");
                else json.Append($"{new string(' ', 3 * (CurrentTubs + 1))} {Convert.ToString(point.Key, System.Globalization.CultureInfo.InvariantCulture)}: ");
                Check(point.Value, ref tubs, json);
                if (i++ != dict.dict.Count - 1) json.Append(",\n");
            }

            json.Append("\n" + new string(' ', 4 * CurrentTubs) + "}");
        }

        public static string ToJson(JsonDictionary dict)
        {
            StringBuilder json = new StringBuilder();
            short tabs = 0;
            Dict2(dict, ref tabs, json);
            return json.ToString();
        }

        public enum JsonExceptionType
        {
            SyntaxError = 0
        }

        public class JsonException : Exception
        {
            public new string Message;
            public JsonExceptionType Type;

            public JsonException(string Message, JsonExceptionType Type)
            {
                this.Message = Message;
                this.Type = Type;
            }
        }

        private enum TokenType
        {
            LeftCurly, RightCurly, LeftSquare, RightSquare, Colon, Comma, Value
        }

        private class Token
        {
            public TokenType Type;
            public object Value;

            public Token(TokenType Type, object Value = null)
            {
                this.Type = Type;
                this.Value = Value;
            }
        }


        private static void Check(TokenType first, List<Token> tokens)
        {
            if (first == tokens[0].Type) { tokens.RemoveAt(0); }
            else throw new JsonException($"{first.ToString()} must be, but {tokens[0].Type.ToString()} was givven.", JsonExceptionType.SyntaxError);
        }

        private static object GetValue(List<Token> tokens)
        {
            switch (tokens[0].Type)
            {
                case TokenType.Value:
                    object val = tokens[0].Value;
                    tokens.RemoveAt(0);
                    return val;
                case TokenType.RightCurly:
                    return Dict(tokens);
                case TokenType.RightSquare:
                    return List(tokens);
                default:
                    throw new JsonException($"Value and RightCurly must be", JsonExceptionType.SyntaxError);
            }
        }

        private static object GetKey(List<Token> tokens)
        {
            if (tokens[0].Type == TokenType.Value)
            {
                object val = tokens[0].Value;
                tokens.RemoveAt(0);
                return val;
            }
            else throw new JsonException("Value must be", JsonExceptionType.SyntaxError);
        }

        private static object[] List(List<Token> tokens)
        {
            short i = 0;
            short count = 0;

            Check(TokenType.RightSquare, tokens);

            while (tokens[i].Type != TokenType.LeftSquare) { if (tokens[i++].Type == TokenType.Value) count++; }

            object[] lst = new object[count];

            for (int i2 = 0; i2 < lst.Length - 1; i2++)
            {
                lst[i2] = GetValue(tokens);
                Check(TokenType.Comma, tokens);
            }

            lst[lst.Length - 1] = GetValue(tokens);
            tokens.RemoveAt(0);

            return lst;
        }

        private static JsonDictionary Dict(List<Token> tokens)
        {
            JsonDictionary dict = new JsonDictionary();

            Check(TokenType.RightCurly, tokens);

            while (true)
            {
                object Key = GetKey(tokens);
                Check(TokenType.Colon, tokens);
                dict.dict.Add(Key, GetValue(tokens));
                if (tokens[0].Type != TokenType.LeftCurly)
                {
                    Check(TokenType.Comma, tokens);
                }
                else
                {
                    tokens.RemoveAt(0);
                    break;
                }
            }

            return dict;
        }

        public static JsonDictionary Parse(string Json)
        {
            List<Token> Tokens = new List<Token>();
            string CurrentValue;


            for (int i = 0; i < Json.Length; i++)
            {

                switch (Json[i])
                {
                    case '{':
                        Tokens.Add(new Token(TokenType.RightCurly));
                        continue;
                    case '}':
                        Tokens.Add(new Token(TokenType.LeftCurly));
                        continue;
                    case '[':
                        Tokens.Add(new Token(TokenType.RightSquare));
                        continue;
                    case ']':
                        Tokens.Add(new Token(TokenType.LeftSquare));
                        continue;
                    case ':':
                        Tokens.Add(new Token(TokenType.Colon));
                        continue;
                    case ',':
                        Tokens.Add(new Token(TokenType.Comma));
                        continue;
                    case '"':
                        CurrentValue = "";
                        i++;
                        while (Json[i] != '"')
                        {
                            CurrentValue += Json[i];
                            i++;
                        }

                        Tokens.Add(new Token(TokenType.Value, CurrentValue));
                        continue;
                    default:
                        if (!char.IsDigit(Json[i]))
                        {
                            if (Json[i] == ' ' || Json[i] == '\n' || Json[i] == '\t') break;
                            else throw new JsonException($"{Json[i]} is uncorrent symbol", JsonExceptionType.SyntaxError);
                        }

                        CurrentValue = "";

                        while (char.IsDigit(Json[i]) || Json[i] == '.')
                        {
                            CurrentValue += Json[i++];
                        }
                        i--;

                        if (int.TryParse(CurrentValue, out int i2)) Tokens.Add(new Token(TokenType.Value, i2));
                        else Tokens.Add(new Token(TokenType.Value, float.Parse(CurrentValue, System.Globalization.CultureInfo.InvariantCulture)));

                        break;

                }
            }
            return Dict(Tokens);
        }
    }
}