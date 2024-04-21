## Object Storage with Hashes

### Useful links:

- https://redis.io/docs/latest/commands/?group=hash

### Hashes characteristics

- Hashes are used to store and manipulate pairs of field names and values
- Hash is like mini key value store within a key
- The underpinning of all data structures in Redis is the string data type, so there is no nesting concept
- We can store a serialized object in Redis as a string in either binary or text and then deserialize the data structure in our code when you read it from Redis (there is cost overhead for changing the data and then sending it back to Redis), but more efficient way is to use Hash data structure
- Hashes are schema-less
- There is no order of fields
- `HSCAN` is prefered than `HMGET` when requesting a large number of fields because its cursor based
- Individual fields cannot be expired

### COMMANDS

- `HSET key field value [field value...]`
- `HMGET key field [field...]`
- `HGETALL key` O(n); blocking operation; transfer the entire hash
- `HSCAN key cursor [MATCH pattern] [COUNT count]`
- `HGET key field` O(1)
- `HEXISTS key field` returns 1 if the field exists and 0 if not
- `HSETNX key field value` will set the value only if the field does not already exist
- `HINCRBY key field increment`
- `HINCRBYFLOAT key field increment`
- `HDEL key field [field...]` when the last value is removed it will cause the key to be removed

### Storing complex objects

- ReJSON and Graph are potential solutions
- Use hash for each object (Multiple hashes)
- Multiple hashes + Sets (set is used to store the relationship ends to provides the ability to find related items more simply)
- Use flattering relationship in a single hash (flatten the hierarchy) - common strategy, use field naming convention to describe the hierarchy

#### Flattering relationship in a single hash

- Advantages: atomic updates, atomic deletes, no need for transactions, encapsulation
- Downsides: relationship maintenance, which end to store (if the data model allows the relationship ends to move from one object to another), large objects after flattering

#### Multiple hashes / Multiple hashes + Set

- For each relationship we break the sub object/sub documents into separate hashes
- Relationship ends: Add set for each relationship end and key name to Set at each end / OR encode it in the code (without usind Set)
- Advantages: extensible structures, independently stored, a TTL can be applied separately (expiration)
- Downsides: Many keys required to represent the object, relationship maintenance, cluster (all the associated keys that make up the structure would need to be stored in the same shard in order to allow transactions to apply changes to multiple hashes)
