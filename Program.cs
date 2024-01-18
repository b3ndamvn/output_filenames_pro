using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


class Node
{
    public string Name { get; private set; }
    public long Size { get; private set; }
    private Dictionary<string, Node> children;


    public Node(string name, long size)
    {
        Name = name;
        Size = size;
        children = new Dictionary<string, Node>();
    }

    public void AddPath(string path, long size)
    {
        string[] parts = path.Split('\\');
        Node current = this;
        for (int i = 0; i < parts.Length; i++)
        {
            string part = parts[i];
            if (!current.children.ContainsKey(part))
            {
                current.children[part] = new Node(part, size);
            }
            current = current.children[part];
        }
    }

    public void PrintTree(StreamWriter outputFile, int indent)
    {
        List<Node> folders = new List<Node>();
        List<Node> files = new List<Node>();
        if (Name != "")
        {
            outputFile.WriteLine(new string(' ', indent) + Name);
        }
        foreach (Node child in children.Values)
        {
            if (child.Name.ToLower() == child.Name & child.Size >= 0)
            {
                files.Add(child);
            }
            else
            {
                folders.Add(child);
            }
        }
        folders = folders.OrderBy(node => node.Name).ToList();
        files = files.OrderByDescending(node => node.Size).ThenBy(node => node.Name).ToList();
        foreach (Node child in folders)
        {
            child.PrintTree(outputFile, indent + 4);
        }
        foreach (Node child in files)
        {
            child.PrintTree(outputFile, indent + 4);
        }
    }
}

class Program
{
    static void Main()
    {
        string inputFilePath = "input.txt";
        string outputFilePath = "output.txt";

        string[] lines = File.ReadAllLines(inputFilePath);
        Node root = new Node("", -1); // создаем корневой узел, который не будет выведен в файл
        foreach (string line in lines)
        {
            string[] parts = line.Split(' ');
            string path = parts[0];
            long size = -1; // т.к. Node подразумевает наличие хоть какого-то значения size, укажем -1, т.к. файл с таким размером быть не может
            if (parts.Length > 1)
            {
                size = long.Parse(parts[1]);
            }
            root.AddPath(path, size);
        }

        using (StreamWriter outputFile = new StreamWriter(outputFilePath))
        {
            root.PrintTree(outputFile, -4); // значение indent = -4, т.к. иначе файл начинается с отступом из-за того, что пропускаем root
        }

        Console.WriteLine("Готово");
    }
}
