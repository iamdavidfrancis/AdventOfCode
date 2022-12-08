using System.Text.RegularExpressions;

namespace AdventOfCode._2022
{
    internal class Day07 : IAsyncAdventOfCodeProblem
    {
        // Note: For this, I pre-processed the input slightly.
        public async Task RunProblemAsync()
        {
            using (TextReader reader = File.OpenText("./2022/Day07.txt"))
            {
                string? line;
                DirectoryNode root = new DirectoryNode {
                    Name = "/"
                };

                List<DirectoryNode> directories = new();
                directories.Add(root);

                Node current = root;

                bool inList = false;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var (operation, args) = GetOperation(line);
                    var currentDir = (current as DirectoryNode)!;

                    switch (operation) {
                        case FileOperation.ChangeDirectory:
                            inList = false;

                            if (args == "/") {
                                current = root;
                                continue;
                            } else if (args == "..") {
                                current = current.Parent!;
                                continue;
                            }

                            if (currentDir.children.Any(c => c.Name == args)) {
                                current = currentDir.children.Single(c => c.Name == args);
                            } else {
                                var newDir = new DirectoryNode {
                                    Name = args,
                                    Parent = currentDir
                                };

                                directories.Add(newDir);

                                current = newDir;
                            }
                            break;
                        case FileOperation.List:
                            inList = true;
                            break;
                        default:
                            if (!inList) {
                                Console.WriteLine("Something went wrong.");
                            }

                            var splitLine = line.Split(" ");

                            var dirOrSize = splitLine[0];
                            var newName = splitLine[1];

                            if (dirOrSize == "dir") {
                                var newDir = new DirectoryNode {
                                    Name = newName,
                                    Parent = currentDir
                                };

                                directories.Add(newDir);
                                currentDir.children.Add(newDir);
                            } else {
                                var size = Int32.Parse(dirOrSize);
                                var newFile = new FileNode {
                                    Name = newName,
                                    Size = size,
                                    Parent = currentDir
                                };
                                currentDir.children.Add(newFile);
                            }

                            break;
                    }
                }

                var sum = directories.Where(d => d.TotalSize <= 100000).Sum(d => d.TotalSize);
                Console.WriteLine($"Part 1: {sum}");

                var usedSpace = root.TotalSize;
                var unusedSpace = 70_000_000 - usedSpace;
                var spaceNeeded = 30_000_000 - unusedSpace;

                var dirToDelete = directories.OrderBy(d => d.TotalSize).First(d => d.TotalSize >= spaceNeeded);
                Console.WriteLine($"Part 2: {dirToDelete.TotalSize}");
            }
        }

        private (FileOperation, string? args) GetOperation(string line) {
            if (!line.StartsWith("$")) {
                return (FileOperation.NotOperation, null);
            }

            line = line.Replace("$ ", string.Empty);

            if (line.StartsWith("cd")) {
                return (FileOperation.ChangeDirectory, line.Replace("cd ", string.Empty));
            }

            return (FileOperation.List, null);
        }

        public enum FileOperation {
            ChangeDirectory,
            List,
            NotOperation
        }
        
        public class Node {
            public string? Name;
            public Node? Parent;
        }

        public class DirectoryNode : Node {
            public List<Node> children = new();

            public int TotalSize => 
                children.Where(c => c is DirectoryNode).Sum(c => (c as DirectoryNode)!.TotalSize) +
                children.Where(c => c is FileNode).Sum(c => (c as FileNode)!.Size );

        }

        public class FileNode : Node {
            public int Size;
        }
    }
}

