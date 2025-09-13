using System.Collections.Generic; // Needed for List<int>

public static class Arrays
{
    /// <summary>
    /// This function will produce an array of size 'length' starting with 'number' followed by multiples of 'number'.  For 
    /// example, MultiplesOf(7, 5) will result in: {7, 14, 21, 28, 35}.  Assume that length is a positive
    /// integer greater than 0.
    /// </summary>
    /// <returns>array of doubles that are the multiples of the supplied number</returns>
    public static double[] MultiplesOf(double number, int length)
    {
        // TODO Problem 1 Start
        // Remember: Using comments in your program, write down your process for solving this problem
        // step by step before you write the code. The plan should be clear enough that it could
        // be implemented by another person.
        // PLAN (Detailed Steps):
        // 1. We are given a starting number (double) and a length (int) > 0.
        // 2. We must return an array whose elements are the successive multiples of the starting number.
        //    Example: number = 7, length = 5 => [7, 14, 21, 28, 35]
        // 3. Create a double array of size 'length'.
        // 4. Loop i from 0 to length-1.
        //       For each i, compute the (i+1)-th multiple: number * (i + 1)
        //       Store it in result[i].  (i=0 gives number*1, i=1 gives number*2, ...)
        // 5. After filling the array, return it.
        // 6. Complexity: O(length) time, O(length) space (output array). No extra space besides output.
        // 7. Edge cases considered:
        //    - Negative starting number: still fine, pattern continues.
        //    - Large length: linear cost unavoidable by definition.
        //    - Floating point precision: acceptable; no special rounding requested.

        // IMPLEMENTATION:
        double[] result = new double[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = number * (i + 1);
        }
        return result;
    }

    /// <summary>
    /// Rotate the 'data' to the right by the 'amount'.  For example, if the data is 
    /// List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9} and an amount is 3 then the list after the function runs should be 
    /// List<int>{7, 8, 9, 1, 2, 3, 4, 5, 6}.  The value of amount will be in the range of 1 to data.Count, inclusive.
    ///
    /// Because a list is dynamic, this function will modify the existing data list rather than returning a new list.
    /// </summary>
    public static void RotateListRight(List<int> data, int amount)
    {
        // TODO Problem 2 Start
        // Remember: Using comments in your program, write down your process for solving this problem
        // step by step before you write the code. The plan should be clear enough that it could
        // be implemented by another person.
        // PLAN (Detailed Steps):
        // Goal: Shift (rotate) the list so the last 'amount' elements move to the front, preserving order.
        // Example: data = [1,2,3,4,5,6,7,8,9], amount = 3 -> [7,8,9,1,2,3,4,5,6]
        // 1. Let n = data.Count.
        // 2. Normalize amount with modulo: amount = amount % n (covers case amount == n producing no change).
        // 3. If amount == 0 (after normalization), return immediately (nothing to rotate).
        // 4. Identify the slice (tail) that will move to front: last 'amount' elements starting at index n-amount.
        // 5. Copy that slice using GetRange(n - amount, amount).
        // 6. Remove those last 'amount' elements from the original list using RemoveRange.
        // 7. Insert the copied slice at the beginning using InsertRange(0, tail).
        // 8. Done; list modified in place.
        // Complexity: O(n) due to copying and shifting; acceptable for this exercise.
        // Edge Cases: amount == n -> no-op after modulo; amount == 1 -> simple single-element rotate.
        // Assumptions: amount in [1, n] per problem statement (still guarded by modulo logic).

        int n = data.Count;
        if (n == 0) return;          // Safety guard (not expected per spec but harmless)
        amount %= n;                 // Normalize
        if (amount == 0) return;     // No change needed

        List<int> tail = data.GetRange(n - amount, amount); // Capture last 'amount' elements
        data.RemoveRange(n - amount, amount);                // Remove them from the end
        data.InsertRange(0, tail);                           // Insert them at the front
    }
}
