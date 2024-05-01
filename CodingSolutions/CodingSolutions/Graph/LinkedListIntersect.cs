public class LinkedListsIntersect
{
    public static bool DoesCycleExists(SinglyLinkedList node)
    {
        var slow = node;
        var fast = node;

        while (fast != null && fast.Next != null)
        {
            slow = slow.Next;
            fast = fast.Next.Next;

            if (fast == slow)
            {
                return true;
            }
        }

        return false;
    }

    public static TraversalResult CheckCycleOrIntersection(SinglyLinkedList node, HashSet<char> visitedNodes)
    {
        var isCycleDetected = DoesCycleExists(node);
        if (isCycleDetected)
        {
            return TraversalResult.HasCycle;
        }

        while (node != null)
        {
            if (visitedNodes.Contains(node.Value))
            {
                return TraversalResult.HasIntersection;
            }
            visitedNodes.Add(node.Value);
            node = node.Next;
        }

        return TraversalResult.HasNoIntersection;
    }

    public static TraversalResult DoLinkedListsIntersect(HashSet<char> nodeValuesToTest, Graph graph)
    {
        var visited = new HashSet<char>();
        foreach (var nodeValue in nodeValuesToTest)
        {
            var node = graph.GetNode(nodeValue);

            var result = CheckCycleOrIntersection(node, visited);
            if (result == TraversalResult.HasIntersection || result == TraversalResult.HasCycle)
            {
                return result;
            }
        }
        return TraversalResult.HasNoIntersection;
    }

    public IEnumerable<string> Main(string[] userInputs)
    {
        var graph = new Graph();
        var nodeValuesToTest = new HashSet<char>();
        var output = new List<string>();
        string edgeIdentifier = "->";
        char intersectionTestIdentifier = ',';

        foreach (var userInput in userInputs)
        {
            var isEdge = userInput.Contains(edgeIdentifier);

            if (isEdge)
            {
                var nodeValues = userInput.Split(edgeIdentifier);
                var node1Value = nodeValues[0].First();
                var node2Value = nodeValues[1].First();
                graph.AddEdge(node1Value, node2Value);
            }
            else
            {
                // Nodes to test
                var intersectionOutput = string.Empty;
                nodeValuesToTest.Clear();
                nodeValuesToTest = userInput.Split(intersectionTestIdentifier).Select(c => c.Trim()[0]).ToHashSet();
                var result = DoLinkedListsIntersect(nodeValuesToTest, graph);
                if (result == TraversalResult.HasCycle)
                {
                    intersectionOutput = "Error Thrown!";
                }
                else if (result == TraversalResult.HasNoIntersection)
                {
                    intersectionOutput = "False";
                }
                else
                {
                    intersectionOutput = "True";
                }
                output.Add(intersectionOutput);
            }
        }

        return output;
    }
}

#region Boiler Plate

public enum TraversalResult
{
    HasIntersection,
    HasNoIntersection,
    HasCycle
}

public class SinglyLinkedList
{
    public char Value;
    public SinglyLinkedList Next;

    public SinglyLinkedList(char value)
    {
        Value = value;
        Next = null;
    }
}

public class Graph
{
    public Dictionary<char, SinglyLinkedList> NodeValueNodeMap = new();

    public void AddEdge(char node1Value, char node2Value)
    {
        var node1 = GetNode(node1Value);
        var node2 = GetNode(node2Value);
        node1.Next = node2;
    }

    public SinglyLinkedList GetNode(char nodeValue)
    {
        if (!NodeValueNodeMap.ContainsKey(nodeValue))
        {
            NodeValueNodeMap[nodeValue] = new SinglyLinkedList(nodeValue);
        }
        return NodeValueNodeMap[nodeValue];
    }
}
#endregion