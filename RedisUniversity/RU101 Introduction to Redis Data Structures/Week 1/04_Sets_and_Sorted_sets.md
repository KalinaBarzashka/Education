## Redis Sets and Sorted Sets

_They allow for unique collections of elements to be manipulated._

### Sets are

- Unordered collection of Strings that contains no duplicates
- Efficient membership checks
- Support standard mathematical set operations, like intersection, difference and union
- The structure is comprised of Strings only, cannot be nested with other lists, sets and so on

### Some commands for sets

```
SADD key member [member...]
SMEMBERS key
SSCAN key cursor [MATCH pattern] [COUNT count]
SISMEMBER key member
SREM key member [member...]
SPOP key [count]
```

_SPOP removes a random element from the set._

```
SCARD key
```

_Get cardinality or number of members in a set._

### Use cases for sets

- Tag Clound - maintain a separate list for each object, that we want to tag
- Unique visitors for a web page for a given period of time

### Sorted sets are

- Ordered collection of unique members
- Good choice for priority queues, low-latency leaderboards and secondary indexing in general
- Provides responsive in-memory access
- Each member has a score, used to keep the order of the members by score
- The score is a floating point number
- If two elements have the same score, the tie is broken by the lexical order of the elements
- Supports union and intersection commands, but you should store the results in another sorted set
- ZDIFF is supported in Redis 6.2
- No nesting supported
- Supports manipulation by value, position, score or lexigraphically

### Some commands for sets

```
ZADD key [NX|XX] [CH] [INCR] score member [score member...]
ZINCRBY key increment member
ZRANK key member
ZREVRANK key member
ZSCORE key member
ZCOUNT key min max
ZREM key member [member...]
```

_NX only adds member if the value does not already exist. XX only updates existing element. CH provides the number of elements changes by the command._
_ZRAND and ZREVRANK return the index position of the element._

```
ZRANGE key start stop [WITHSCORES]
ZREVRANGE key start stop [WITHSCORES]
ZRANGEBYSCORE KEY start stop
```

_Iterates over a sorted set in order of score. ZRANGE traverses a range of members from the lowest score to the highest score. Start and stop indices are zero-based._
_Example zrangebyscore hw1-8 (3 +inf_

### Use cases for sorted sets

- Leaderboards
- Proprity queues

### Operations for sets and sorted sets - check Venn diagrams

- A union B returns the combination of all unique values in both sets
- A intersection B returns the values that are present in both sets
- A difference B returns the remaining values after the values present in B have been removed from A

```
SINTER set1 set2 [set...]
SINTERSTORE
SUNION set1 set2 [set...]
SUNIONSTORE
SDIFF set1 set2 [set...]
SDIFFSTORE
```

### NOTE: Redis does not store empty lists, sets, sorted sets or hashes. Therefore, the key is removed if this condition is met.
