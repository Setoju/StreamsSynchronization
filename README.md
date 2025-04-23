# Parallel Minimum Finder

This C# code demonstrates finding the minimum element in a large array using both a parallel (multi-threaded) and a single-threaded approach. It then compares the execution time of both methods.

## Overview

The program does the following:

1.  **Generates a large array:** An array of 1 billion random integers is created.
2.  **Finds the minimum in parallel:**
    * The array is divided into chunks based on the number of available processor cores.
    * Each chunk is processed by a separate thread to find the minimum element within that range.
    * The overall minimum is determined by comparing the minimums found by each thread.
3.  **Finds the minimum single-threaded:** The code iterates through the entire array in a single thread to find the minimum element.
4.  **Compares execution times:** The time taken for both the parallel and single-threaded minimum finding operations is measured and printed to the console.

## Output

The program will output the minimum element found using both methods and their respective execution times in milliseconds. The parallel execution time is expected to be lower than the single-threaded execution time, especially on multi-core processors.
