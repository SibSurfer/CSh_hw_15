class Program
{
    static List<string> Permutations(string s)
    {
        var result = new List<string>();
        PermutationsHelper(s.ToCharArray(), 0, result);
        return result.OrderBy(x => x).ToList();
    }

    static void PermutationsHelper(char[] chars, int index, List<string> result)
    {
        if (index == chars.Length - 1)
        {
            result.Add(new string(chars));
            return;
        }

        for (int i = index; i < chars.Length; i++)
        {
            Swap(ref chars[index], ref chars[i]);
            PermutationsHelper(chars, index + 1, result);
            Swap(ref chars[index], ref chars[i]);
        }
    }

    static void Swap(ref char a, ref char b)
    {
        char temp = a;
        a = b;
        b = temp;
    }

    static void Main(string[] args)
    {
        Console.WriteLine(string.Join(" ", Permutations("AB"))); 
        Console.WriteLine(string.Join(" ", Permutations("CD"))); 
        Console.WriteLine(string.Join(" ", Permutations("EF")));
        Console.WriteLine(string.Join(" ", Permutations("NOT"))); 
        Console.WriteLine(string.Join(" ", Permutations("RAM"))); 
        Console.WriteLine(string.Join(" ", Permutations("YAW"))); 
    }
}

