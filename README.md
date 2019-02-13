# Parallel Tasks

This code shows how to execute asynchronous operations in parallel in order to reduce overall execution time.

The application runs two operations in serial, tracking the execution time and then runs the same operations in parallel.
The total execution time in parallel is more or less equal to the time taken by the most expensive operation.

## Benchmark

![alt text](https://raw.githubusercontent.com/mizrael/parallel-tasks/master/capture.png "Serial and Parallel execution")