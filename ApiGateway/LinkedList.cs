using System;

namespace ApiGateway
{
    public class Node
    {
        public DateTime TimeStamp;
        public Node Next;

        public Node(DateTime data)
        {
            TimeStamp = data;
            Next = null;
        }
    }

    public class LinkedList
    {
        private Node head;

        public LinkedList()
        {
            head = null;
        }

        // Add a new node at the end of the list
        public void AddLast(DateTime data)
        {
            Node newNode = new Node(data);
            if (head == null)
            {
                head = newNode;
            }
            else
            {
                Node current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
        }

        // Count the number of nodes in the list
        public int Count()
        {
            int count = 0;
            Node current = head; 
            while (current != null) 
            { 
                count++; current = current.Next; 
            }
            return count;
        }

        // Get the head node of the list
        public Node GetHead() 
        { 
            return head; 
        }

        // Remove the head node
        public void RemoveHead() 
        {
            if (head != null) 
            {
                head = head.Next;
            };
        }
    }
}
