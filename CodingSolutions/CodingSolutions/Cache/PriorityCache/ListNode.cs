﻿namespace CodingSolutions.Cache.PriorityCache
{
    public class ListNode<T>
    {
        public T Data { get; set; }
        public ListNode<T> Next { get; set; }
        public ListNode<T> Prev { get; set; }

        public ListNode(T data)
        {
            Data = data;
        }

        public ListNode(ListNode<T> prev, T data, ListNode<T> next)
        {
            Data = data;
            Next = next;
            Prev = prev;
        }
    }
}