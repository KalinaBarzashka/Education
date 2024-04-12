## Big O Notation and Redis Commands

- Each command has its time complexity defined in the documentation.
- When a command is executed it's guaranteed to be atomic.
- To achieve atomicity, Redis single threads all command execution.
- All incoming commands are queued up, and await processing one by one.
- Big O is a mathematical notation, describing the limiting behavior of a function.

### Factors to consider

- Calculation of Time Complexity of the command
- Cardinality of the data
- Multiplying factor
- Clock times are not O times

### Examples

- `O(1)` commands: APPEND, EXISTS, GET, SET, HGET, LPUSH, RPOP, DEL (when removing a String data type), etc. This is constant time.
- `O(n)` commands: DEL (when multiple keys are removed). This is linear time.
- `O(m)` commands: DEL (when the key removed contains a List, Set, Sorted Set or Hash), where `m` is the number of elements in the list, set or sorted set or fields in the hash being removed.
- `O(n*m)` commands: SINTER (n is the cardinality of the smallest set and M is the number of sets)
- `O(s+n)` commands: LRANGE (s is how many values from the left the LRANGE is starting from, n is the number of elements requested)
