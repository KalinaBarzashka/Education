## Redis Hashes

### Hashes are

- Collection of **field-value** pairs and look a lot like JSON objects
- Mutable objects (add, change, increment, remove field-value pairs)
- Store field values as Strings (they are flat)
- Schemaless
- Equivalent to rows in RDBMS table
- Are not recursive (a hash cannot contain a hash) - no nesting hierarchy
- Structure is dynamic - can change over time

### Hash performance

- Most hash commands are `O(1)` - perform task in a constant amount of time, regardless of the size of the hash
  - HGET, HSET, HINCRBY, HDEL
- HGETALL is `O(n)`. This command is practical for relatively small hashes

### Some commands

```
HSET key field value [field value...]
HSETNX key field value [field value...]
HGETALL key
HGET key field
HSCAN key cursor [MATCH pattern] [COUNT count]
HMGET key field [field...]
HEXISTS key field
```

_To add more field-value pairs, just use HSET command again with the same key, but only the new field-value pair._

```
HKEYS key
HVALS key
```

_Get field names or field values. Used for test purposes mainly._

```
HDEL key field [field...]
```

```
HINCRBY key field increment
HINCRBYFLOAT key feild increment
```

### Use cases:

- Rate limiting, a.k.a. we need to track, during a given period of time, how many requests have been made to the same endpoint
- Session cache (there are some advantages than storing the session as a blob - avoid to rewrite the whole blob when something changes)
