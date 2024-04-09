## Redis Lists

### Lists are

- Ordered sequence/collection of Strings
- The order is maintained during insertion and removal of elements
- Duplicates are allowed
- Elements can be added and removed at **Left** or **Right** (head and tail are not used as terminology)
- Not a common use case, but elements can be inserted relative to another
- Can be used to implement stack or queue
- A single list can hold over `4 000 000 000` entries
- No concept of nesting a list in a list, because the **underpinning of all structures in Redis is the String data type**
- Internally Redis implements lists using a doubly linked list
- Indices are zero based

### Some commands

```
RPUSH key value [value...]
LPUSH key value [value...]
LPOP key
RPOP key
LRANGE playlist start end
LLEN key
LINDEX key index
LINSERT key BEFORE|AFTER pivot value
LSET key index value
LREM key count value
```

### Lists performance

- LPOP, RPUSH, LLEN are all constant time complexity operations `O(1)`
- LRANGE is `O(s+n)` where `s` is the distance of the start offset from the head, and n is the number of elements in the specified range

### Use cases

- Activity stream
- Inter process communication (queue that supports a producer-consumer pattern - produce and consume elements in the correct order)
