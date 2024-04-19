using System;
using System.Text;

public class Test
{
    public static void Main()
    {
        int[] a = { 5, 3, 4, 1, -1 };
        foreach (int curr in a)
        {
            Console.Write(curr + ", ");
        }
        Console.WriteLine();

        ITree maxTree = new SegmentTree(a);
        Console.WriteLine(maxTree.Query(2, 4));
        Console.WriteLine(maxTree);

        ITree minTree = new SegmentTree(a, new MinSegmentTreeOperator());
        Console.WriteLine(minTree.Query(2, 4));
        Console.WriteLine(minTree);

        ITree sumTree = new SegmentTree(a, new SumSegmentTreeOperator());
        Console.WriteLine(sumTree.Query(2, 4));
        Console.WriteLine(sumTree);
    }
}

public interface IOperator
{
    int Operate(int value1, int value2);
    int BaseValue();
}

public class MaxSegmentTreeOperator : IOperator
{
    public int Operate(int value1, int value2)
    {
        return Math.Max(value1, value2);
    }
    public int BaseValue()
    {
        return int.MinValue;
    }
}

public class MinSegmentTreeOperator : IOperator
{
    public int Operate(int value1, int value2)
    {
        return Math.Min(value1, value2);
    }
    public int BaseValue()
    {
        return int.MaxValue;
    }
}

public class SumSegmentTreeOperator : IOperator
{
    public int Operate(int value1, int value2)
    {
        return value1 + value2;
    }
    public int BaseValue()
    {
        return 0;
    }
}

public interface ITree
{
    void Add(int index, int value);
    int Query(int queryLeftIndex, int queryRightIndex);
}

public class SegmentTree : ITree
{
    private int[] tree;
    private int size;
    private IOperator @operator = new MaxSegmentTreeOperator();

    private void AddUtil(int index, int value, int left, int right, int position = 0)
    {
        if (left == right)
        {
            tree[position] = value;
            return;
        }
        int mid = (left + right) / 2;
        if (index <= mid)
        {
            AddUtil(index, value, left, mid, position * 2 + 1);
        }
        else
        {
            AddUtil(index, value, mid + 1, right, position * 2 + 2);
        }
        tree[position] = @operator.Operate(tree[position * 2 + 1], tree[position * 2 + 2]);
    }

    private int QueryUtil(int left, int right, int queryLeftIndex, int queryRightIndex, int position = 0)
    {
        if (right < queryLeftIndex || left > queryRightIndex)
        {
            return @operator.BaseValue();
        }
        if (queryLeftIndex <= left && right <= queryRightIndex)
        {
            return tree[position];
        }

        int mid = (left + right) / 2;
        return @operator.Operate(QueryUtil(left, mid, queryLeftIndex, queryRightIndex, position * 2 + 1), QueryUtil(mid + 1, right, queryLeftIndex, queryRightIndex, position * 2 + 2));
    }

    public SegmentTree(int n)
    {
        int size = n;
        tree = new int[4 * size];
    }

    public SegmentTree(int[] array)
    {
        size = array.Length;
        tree = new int[4 * size];
        for (int i = 0; i < size; i++)
        {
            Add(i, array[i]);
        }
    }

    public SegmentTree(int n, IOperator o)
    {
        @operator = o;
        int size = n;
        tree = new int[4 * size];
    }

    public SegmentTree(int[] array, IOperator o)
    {
        @operator = o;
        size = array.Length;
        tree = new int[4 * size];
        for (int i = 0; i < size; i++)
        {
            Add(i, array[i]);
        }
    }

    public void Add(int index, int value)
    {
        AddUtil(index, value, 0, size - 1);
    }

    public int Query(int queryLeftIndex, int queryRightIndex)
    {
        return QueryUtil(0, size - 1, queryLeftIndex, queryRightIndex, 0);
    }

    public override string ToString()
    {
        StringBuilder result = new StringBuilder("[");
        for (int i = 0; i < 4 * size; i++)
        {
            result.Append(tree[i] + ", ");
        }
        result.Remove(result.Length-2,2);
        result.Append(']');
        return result.ToString();
    }
}
