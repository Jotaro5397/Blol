using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : MonoBehaviour
{
    public GameObject objectToSplitPrefab; // Assign your GameObject prefab here through the Inspector
    private int splitCount = 0; // Counter for the number of times the object has split
    private const int maxSplits = 2; // Maximum number of allowed splits

    // Define sizes for each split level
    private Vector3 originalSize;
    private Vector3 firstSplitSize;
    private Vector3 secondSplitSize;

    private static List<Split> splitInstances = new List<Split>();

    private void Awake()
    {
        splitInstances.Add(this);
        // Assuming originalSize is set in the Inspector or elsewhere before Awake is called
        originalSize = transform.localScale; // Save the original size

        // Define specific sizes for each split
        firstSplitSize = originalSize * 0.75f; // Example size after first split
        secondSplitSize = originalSize * 0.5f; // Example size after second split
    }

    private void OnDestroy()
    {
        splitInstances.Remove(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && splitCount < maxSplits)
        {
            SplitAndDuplicate();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ConjoinWithNearest();
        }
    }

    void SplitAndDuplicate()
    {
        splitCount++; // Increment the split counter

        // Adjust size based on the number of splits
        if (splitCount == 1)
        {
            transform.localScale = firstSplitSize;
        }
        else if (splitCount == 2)
        {
            transform.localScale = secondSplitSize;
        }

        GameObject duplicate = Instantiate(objectToSplitPrefab, transform.position, transform.rotation);
        duplicate.transform.position += new Vector3(0.5f, 0, 0); // Adjust so duplicates don't overlap exactly
        duplicate.transform.localScale = transform.localScale; // Match the scale based on split count

        // Ensure the duplicate has the same split count and sizes
        var splitScript = duplicate.GetComponent<Split>();
        if (splitScript != null)
        {
            splitScript.splitCount = this.splitCount;
            splitScript.originalSize = this.originalSize;
            splitScript.firstSplitSize = this.firstSplitSize;
            splitScript.secondSplitSize = this.secondSplitSize;
        }
    }

    void ConjoinWithNearest()
    {
        float closestDistance = float.MaxValue;
        Split closestSplit = null;

        foreach (var otherSplit in splitInstances)
        {
            if (otherSplit != this)
            {
                float distance = Vector3.Distance(transform.position, otherSplit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSplit = otherSplit;
                }
            }
        }

        float conjoinDistanceThreshold = 1.0f; // Adjust this threshold as needed

        if (closestSplit != null && closestDistance <= conjoinDistanceThreshold)
        {
            // Determine the new size after conjoining
            if (splitCount == 2)
            {
                transform.localScale = firstSplitSize; // Reverse to first split size
                splitCount--;
            }
            else if (splitCount == 1)
            {
                transform.localScale = originalSize; // Reverse to original size
                splitCount--;
            }

            Destroy(closestSplit.gameObject); // Destroy the nearest object
        }
    }
}
