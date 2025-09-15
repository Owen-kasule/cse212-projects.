using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Enqueue three items with distinct priorities and dequeue once then again.
    // Expected Result: First dequeue returns highest priority value; second returns next highest; internal list shrinks.
    // Defect(s) Found (before code fix): Did not remove dequeued item; loop skipped last item (missed candidate when highest at end).
    public void TestPriorityQueue_1()
    {
        var pq = new PriorityQueue();
        pq.Enqueue("low", 1);
        pq.Enqueue("mid", 5);
        pq.Enqueue("high", 9);
        var first = pq.Dequeue();
        Assert.AreEqual("high", first);
        var second = pq.Dequeue();
        Assert.AreEqual("mid", second);
    }

    [TestMethod]
    // Scenario: Two highest priorities tied; confirm earliest inserted among ties dequeued first.
    // Expected Result: Value "b" (earliest with highest priority 10) then value "c" (same priority later) on second dequeue.
    // Defect(s) Found (before code fix): Tie handling wrong due to >= causing later item chosen; removal missing.
    public void TestPriorityQueue_2()
    {
        var pq = new PriorityQueue();
        pq.Enqueue("a", 3);
        pq.Enqueue("b", 10); // earliest high
        pq.Enqueue("c", 10); // later high
        pq.Enqueue("d", 5);
        Assert.AreEqual("b", pq.Dequeue());
        Assert.AreEqual("c", pq.Dequeue());
    }

    [TestMethod]
    // Scenario: Dequeue on empty queue.
    // Expected Result: InvalidOperationException with message "The queue is empty.".
    // Defect(s) Found: None (path already correct).
    public void TestPriorityQueue_Empty()
    {
        var pq = new PriorityQueue();
        try
        {
            pq.Dequeue();
            Assert.Fail("Expected exception not thrown");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual("The queue is empty.", ex.Message);
        }
    }

    [TestMethod]
    // Scenario: Multiple dequeues with mixed priorities including ties to verify stable removal across sequence.
    // Expected Result: Order: p3(30), p2(20 earliest tie), p4(20 later tie), p1(5)
    // Defect(s) Found (before fix): Without removal, same element returned repeatedly; last element missed in scan.
    public void TestPriorityQueue_ChainedDequeues()
    {
        var pq = new PriorityQueue();
        pq.Enqueue("p1", 5);
        pq.Enqueue("p2", 20);
        pq.Enqueue("p3", 30);
        pq.Enqueue("p4", 20);

        Assert.AreEqual("p3", pq.Dequeue());
        Assert.AreEqual("p2", pq.Dequeue());
        Assert.AreEqual("p4", pq.Dequeue());
        Assert.AreEqual("p1", pq.Dequeue());
    }
    // Add more test cases as needed below.
}