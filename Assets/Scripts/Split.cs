using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : MonoBehaviour
{
    public GameObject objectToSplitPrefab; // Assign your GameObject prefab here through the Inspector

    void Update()
    {
        // When the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.X))
        {
            SplitAndDuplicate();
        }
    }

    void SplitAndDuplicate()
    {
        // Reduce current object's size by half
        transform.localScale *= 0.5f;

        // Instantiate a duplicate object
        GameObject duplicate = Instantiate(objectToSplitPrefab, transform.position, transform.rotation);

        // Move the duplicate to the right (or any direction you prefer) and reduce its size to match the original
        duplicate.transform.position += new Vector3(transform.localScale.x, 0, 0); // Adjust this vector to control where the duplicate appears
        duplicate.transform.localScale = transform.localScale; // Ensure the duplicate has the same scale as the now smaller original

        // Optionally, if you want to recursively allow the duplicates to split as well,
        // ensure they have this script attached and a reference to the prefab.
        // This line assumes the prefab has the Splitter component with the correct prefab assigned.
        // duplicate.GetComponent<Splitter>().objectToSplitPrefab = this.objectToSplitPrefab;
    }
}
