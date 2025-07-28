public class Heap
{
    private Node[] m_heap = new Node[51];
    private int m_count = 0;

    public int Count => m_count;
    public bool IsEmpty() => m_count == 0;

    public bool Contains(Node node)
    {
        for (int i = 1; i <= m_count; i++)
        {
            if (m_heap[i] == node)
            {
                return true;
            }
        }
        return false;
    }

    public void Push(Node node)
    {
        if (m_count + 1 >= m_heap.Length)
        {
            var new_heap = new Node[m_heap.Length * 2];

            for (int i = 1; i <= m_count; i++)
            {
                new_heap[i] = m_heap[i];
            }

            m_heap = new_heap;
        }

        m_count++;
        var index = m_count;

        while (index > 1 && node.F < m_heap[index / 2].F)
        {
            m_heap[index] = m_heap[index / 2];
            index /= 2;
        }

        m_heap[index] = node;
    }

    public Node Pop()
    {
        if (m_count == 0)
        {
            return null;
        }

        var return_node = m_heap[1];
        var last_node = m_heap[m_count];
        m_count--;

        var parent = 1;
        while (true)
        {
            var left = parent * 2;
            var right = left + 1;

            if (left > m_count)
            {
                break;
            }

            var smaller = (right <= m_count && m_heap[right].F < m_heap[left].F) ? right : left;

            if (last_node.F <= m_heap[smaller].F)
            {
                break;
            }

            m_heap[parent] = m_heap[smaller];
            parent = smaller;
        }

        m_heap[parent] = last_node;
        

        return return_node;
    }
}