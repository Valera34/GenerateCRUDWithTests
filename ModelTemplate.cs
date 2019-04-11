using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
namespace FrameworkForGenerationCRUD
{
    public class ModelTemplate
    {
        public void GenerateClass(string path,string className, Field[] fields,string projectName)
        {
         //   string startupPath = System.IO.Directory.GetCurrentDirectory() + @"\Models\" + className + ".cs";
            string startupPath = path + @"\Models\" + className + ".cs";
            //  string generateClass ="";
            string import = "using System.Collections.Generic;";
            using (StreamWriter sw = new StreamWriter(startupPath, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(import);
                sw.WriteLine("namespace "+ projectName +".Models {");
                sw.WriteLine("public class " + className + " {");
                sw.WriteLine(" public int " + className + "Id { get; set; } ");
                foreach (Field field in fields)
                {
                    sw.WriteLine("public " + ToLower(field.Type) + " " + ToUpperFirst(field.Name) + " { get; set; }");
                }
                sw.WriteLine("}");
                sw.WriteLine("}");
            }
        }
        public static string ToLower(object obj)
        {
            return obj.ToString().ToLower();
        }
        public static string ToUpperFirst(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return string.Empty;
            }
            char[] word = fieldName.ToCharArray();
            word[0] = char.ToUpper(word[0]);
            return new string(word);
        }
    }
    public class Field
    {
        public string Name { get; set; }
        public Types Type { get; set; }
        public Field(string name, Types type)
        {
            this.Name = name;
            this.Type = type;
        }

        public Field()
        {
        }
    }
    public enum Types
    {
        Int,
        String,
        Double,
        Bool,
        Char
    }

}






