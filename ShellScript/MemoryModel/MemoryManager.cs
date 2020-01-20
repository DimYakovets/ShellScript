using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShellScript.Exceptions;

namespace ShellScript.MemoryModel
{
    class MemoryManager
    {
        public Dictionary<string, ObjectModel> Objects { get; set; }
        public MemoryManager()
        {
            Objects = new Dictionary<string, ObjectModel>();
        }
        public void Create(string name, string type)
        {
            Objects.Add(name, ObjectModel.CreateDefault(type));
        }
        public void SetValue(ObjectModel obj, params string[] path)
        {
            Objects[path[0]].SetValue(path.Skip(1).ToArray(), obj);
        }
        public void DeleteObject(string name)
        {
        }
        public ObjectModel GetValue(string[] path)
        {
            if (path.Length == 1)
            {
                if (Objects.ContainsKey(path[0]))
                {
                    return Objects[path[0]];
                }
                else
                {
                    throw new VariableDoesNotExistsException(path[0]);
                }
            }
            else
            {
                if (Objects.ContainsKey(path[0]))
                {
                    return Objects[path[0]].GetValue(path.Skip(1).ToArray());
                }
                else
                {
                    throw new VariableDoesNotExistsException(path[0]);
                }
            }
        }
        public void Clear()
        {
            Objects.Clear();
        }
        public override string ToString()
        {
            string str = "";
            foreach (var item in Objects)
            {
                str += $"{item.Key} = {item.Value.ToString()}\n";
            }
            return str;
        }
    }
}
