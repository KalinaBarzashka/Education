## Redis Cardinality & Capped Collections

Check the cardinality of a collection:

**LLEN key** - for lists
**SCARD key** - for sets
**ZCARD key** - from sorted sets

### Capped collections

- Retain subset of members
- Also called limited (or capped) collection
- Use cases: leadership board, recent posts in an Activity Stream

### How to

#### Achieve capped collection with List data structure

- LTRIM key start stop - trimming can be specified from the left with a positive index, or from the right with a negative index; this command specify the elements we want to **retain**
- A pattern for capped lists is to push an element with LPUSH or RPUSH and then trim to the given size

#### Achieve capped collection with Sorted Set data structure

- ZREMRANGEBYRANK key start stop - uses zero-based index; this command specify the elements we want to **remove**
- Pattern to cap the cardinality: ZADD - new element added, then ZREMRANGEBYRANK - cap the set

### Use cases

- Sorted Sets: Leaderboard
- Lists: Activity stream

### Set Operations with Sets and Sorted Sets

- Set operations for sorted sets can operate on both sorted sets and sets

```
ZINTERSTORE destination numkeys key [key...] WEIGHTS weight [weight...] AGGREGATE SUM|MIN|MAX
```

_Weights is used to provide a multiplying factor for each input set that is then used by the aggregate part of a command. Aggregate is where you specify how the new score is computed._

```
ZUNIONSTORE destination numkeys key [key...] WEIGHTS weight [weights...] AGGREGATE SUM|MIN|MAX
```

_Weights parameter ._
