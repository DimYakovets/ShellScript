using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ShellScript.MemoryModel
{
    class ObjectModel
    {
        public const string INT = "int";
        public const string FLOAT = "float";
        public const string STRING = "string";
        public const string BOOL = "bool";
        public const string OBJECT = "object";

        private Dictionary<string, ObjectModel> Properties;

        public string Value { get; private set; }
        public string Type { get; private set; }

        public ObjectModel(string type, string value, Dictionary<string, ObjectModel> properties = null)
        {
            Type = type;
            Value = value;
            Properties = properties ?? new Dictionary<string, ObjectModel>();
        }
        public static ObjectModel CreateDefault(string typename)
        {
            if (typename == INT)
            {
                return new ObjectModel(INT, "0", null);
            }
            if (typename == FLOAT)
            {
                return new ObjectModel(FLOAT, "0", null);
            }
            if (typename == BOOL)
            {
                return new ObjectModel(BOOL, "false", null);
            }
            if (typename == STRING)
            {
                return new ObjectModel(STRING, "", null);
            }
            throw new Exception();
        }
        public ObjectModel GetValue(params string[] path)
        {
            if (path == null || path.Length == 0)
            {
                return this;
            }
            else
            {
                return Properties[path[0]].GetValue(path.Skip(1).ToArray());
            }

        }
        public void SetValue(string[] path, ObjectModel obj)
        {
            if (path.Length == 0)
            {
                if (obj.Type == Type)
                {
                    Value = obj.Value;
                    Properties = obj.Properties;
                }
                else if (Type == FLOAT && obj.Type == INT)
                {
                    Value = obj.Value;
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                Properties[path[0]].SetValue(path.Skip(1).ToArray(), obj);
            }
        }
        public override string ToString()
        {
            return $"<{Type}> : \"{Value}\"";
        }

        public static ObjectModel Sum(ObjectModel o1, ObjectModel o2)
        {
            if (o1.Type == STRING && o2.Type == STRING)
            {
                return new ObjectModel(STRING, o1.Value + o2.Value);
            }
            else if (decimal.TryParse(o1.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal a) && decimal.TryParse(o2.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal b))
            {
                var type = o1.Type == FLOAT || o2.Type == FLOAT ? FLOAT : INT;
                return new ObjectModel(type, (a + b).ToString().ToLower());
            }
            throw new Exception();
        }
        public static ObjectModel Substract(ObjectModel o1, ObjectModel o2)
        {
            if (o1.Type == STRING && o2.Type == STRING)
            {
                return new ObjectModel(STRING, o1.Value.Replace(o2.Value, ""));
            }
            else if (decimal.TryParse(o1.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal a) && decimal.TryParse(o2.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal b))
            {
                var type = o1.Type == FLOAT || o2.Type == FLOAT ? FLOAT : INT;
                return new ObjectModel(type, (a - b).ToString().ToLower());
            }
            throw new Exception();
        }
        public static ObjectModel Multiplication(ObjectModel o1, ObjectModel o2)
        {
            if (decimal.TryParse(o1.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal a) && decimal.TryParse(o2.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal b))
            {
                var type = o1.Type == FLOAT || o2.Type == FLOAT ? FLOAT : INT;
                return new ObjectModel(type, (a * b).ToString().ToLower());
            }
            throw new Exception();
        }
        public static ObjectModel Division(ObjectModel o1, ObjectModel o2)
        {
            if (decimal.TryParse(o1.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal a) && decimal.TryParse(o2.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal b))
            {
                var type = o1.Type == FLOAT || o2.Type == FLOAT ? FLOAT : INT;
                return new ObjectModel(type, type == INT ? decimal.Floor(a / b).ToString().ToLower() : (a / b).ToString().ToLower());
            }
            throw new Exception();
        }
        public static ObjectModel Power(ObjectModel o1, ObjectModel o2)
        {
            if (decimal.TryParse(o1.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal a) && decimal.TryParse(o2.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal b))
            {
                var type = o1.Type == FLOAT || o2.Type == FLOAT ? FLOAT : INT;
                return new ObjectModel(type, Math.Pow((double)a, (double)b).ToString().ToLower());
            }
            throw new Exception();
        }

        public static ObjectModel LessThan(ObjectModel o1, ObjectModel o2)
        {
            if (decimal.TryParse(o1.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal a) && decimal.TryParse(o2.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal b))
            {
                return new ObjectModel(BOOL, (a < b).ToString().ToLower());
            }
            throw new Exception();
        }
        public static ObjectModel GreaterThan(ObjectModel o1, ObjectModel o2)
        {
            if (decimal.TryParse(o1.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal a) && decimal.TryParse(o2.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal b))
            {
                return new ObjectModel(BOOL, (a > b).ToString().ToLower());
            }
            throw new Exception();
        }
        public static ObjectModel LessThanEquals(ObjectModel o1, ObjectModel o2)
        {
            if (decimal.TryParse(o1.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal a) && decimal.TryParse(o2.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal b))
            {
                return new ObjectModel(BOOL, (a <= b).ToString().ToLower());
            }
            throw new Exception();
        }
        public static ObjectModel GreaterThanEquals(ObjectModel o1, ObjectModel o2)
        {
            if (decimal.TryParse(o1.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal a) && decimal.TryParse(o2.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal b))
            {
                return new ObjectModel(BOOL, (a >= b).ToString().ToLower());
            }
            throw new Exception();
        }
        public static ObjectModel Equals(ObjectModel o1, ObjectModel o2)
        {
            var prop = false;
            if (o1.Properties.Count == o2.Properties.Count && o1.Properties.Count == 0)
            {
                prop = true;
            }
            else if (o1.Properties.Count == o2.Properties.Count)
            {
                foreach (var item in o1.Properties)
                {
                    if (o2.Properties.ContainsKey(item.Key))
                    {
                        prop = Equals(o1.Properties[item.Key], o2.Properties[item.Key]).Value.ToLower() == "true";
                    }
                    else
                    {
                        prop = false;
                        break;
                    }
                }
            }
            var exp = o1.Type == o2.Type && o1.Value == o2.Value && prop;
            return new ObjectModel(BOOL, exp.ToString().ToLower());
        }
        public static ObjectModel NotEquals(ObjectModel o1, ObjectModel o2)
        {
            var exp = o1.Type != o2.Type || o1.Value != o2.Value || o1.Properties != o2.Properties;
            return new ObjectModel(BOOL, exp.ToString().ToLower());
        }
    }
}