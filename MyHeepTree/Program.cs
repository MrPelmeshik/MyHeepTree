using System;
using System.Globalization;
using System.Linq;
using MyHeapTree;
using MyGenerator;

public class Program
{
    private const int N = 26;
    private const int MIN = 100;
    private const int MAX = 999;

    static void Main()
    {
        /*9. Сгенерировать 26 трехзначных неповторяющихся чисел
                (элементов).
            Вывести их на экран.
            Показать процесс построения Max-Heap-Tree.
            Вывести на экран полученное дерево.
            Дополнительное задание: в отчете перечислить последовательность
            вершин построенного Max-Heap-Tree, соответствующую прямому порядку
            прохождения (NLR).*/

        
        Console.Out.WriteLine("\nSTART");
        
        var randomUniqueSet = GeneratorValues.GetUniqueRandomValues(N, MIN, MAX);
        OutputArr(randomUniqueSet, true, true);

        #region MaxHeapTree

        //MaxHeapTree maxHeapTree = new MaxHeapTree(randomUniqueSet)
        var arr = MaxHeapTree.SortArray(randomUniqueSet);
        OutputArr(arr, true);
        
        #endregion
        
        #region BTree

        /*
        BTree bTree = new BTree();
        for (int i = 0; i < N; i++)
        {
            bTree.Add(randomUniqueSet[i]);
        }

        /*Console.Out.WriteLine("");
        bTree.IntegrationTest_3();#1#
        
        Console.Out.WriteLine("");
        bTree.OutputBTreeInColumn();
        
        Console.Out.WriteLine("");
        bTree.OutputBTree();
        
        /*Console.Out.WriteLine("");
        bTree.OutputInfo();#1#*/

        #endregion

        Console.Out.WriteLine("\nSTOP");
    }
    
    private static void OutputArr(int[] arr, bool hrz = false, bool needIndex = false)
    {
        Console.Out.WriteLine("\nData:");
        for (var i = 0; i < arr.Length; i++)
        {
            if (hrz)
                Console.Out.Write(needIndex ? $"{i + 1}-{arr[i]} " : $"{arr[i]} ");
            else
                Console.Out.WriteLine(needIndex ? $"\t{i + 1} - \t{arr[i]}" : $"\t{arr[i]}");
        }

        Console.Out.WriteLine("");
    }
}
