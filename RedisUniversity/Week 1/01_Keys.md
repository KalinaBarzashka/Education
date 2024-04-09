## Redis Keys

Redis is **key-based** data structure (key-value store)
**0** - Does not exists | or **(nil)**
**1** - OK
**2** - Does not exists anymore

Redis supports **polymorphism** (represent different types of data over time for the same key).

### A keys is

- Unique
- Binary safe (any binary sequence can be used as a key)
- Key name can be up to 512MB in size (may increase in future versions)
- Colon is used as a separator for domain within the key name
- Case sensitive (the server uses binary comparison)
- Each value of a key has a data type (for example string is a data type of a structure stored at the key) but also an encoding (a string data type value can contain integer value, therefor integer encoding)

_Example_ for Redis user structure key name: user:id:followers

```
SET key value [EX seconds] [PX milliseconds] [NX|XX]
```

_NX checks for non-existence; XX - checks for existence_

```
GET key
```

```
EXISTS key [key...]
```

```
TYPE key
```

```
OBJECT ENCODING key
```

### Logical database

Within a logical database, a single flat key space exists. There are no automatic separation of key names into named groups such as buckets or collections.

A logical database is identified by a zero-based index. The default is **database 0**.

Within a logical databse, the key names are unique, but the same key name can appear in multiple logical databases.

Logical databases have number of restrictions:

- Redis cluster only supports **database 0**
- Support for logical databases is not ubiquitous across tools and frameworks. Many of them assume **database 0** is used.

### Get list of all keys in a database

| KEYS command                                           | SCAN command                                                                          |
| ------------------------------------------------------ | ------------------------------------------------------------------------------------- |
| Blocks entries until complete (iterates over all keys) | Iterates using a cursor (also blocks but only for handful of keys)                    |
| Never use in production!                               | Safe for production                                                                   |
| Useful for local debugging                             | Returns a slot reference, which can be used in subsequent calls to continue iteration |
|                                                        | May return 0 or more keys per call                                                    |

#### KEYS

```
KEYS pattern
```

#### SCAN

```
SCAN cursor [MATCH pattern] [COUNT count] [TYPE type]
```

_Match and Count parameters are required_

### Remove keys in a database

```
DEL key [key...]
```

_removes the key and the memory associated with the key; performed as blocking operation_

```
UNLINK key [key...]
```

_the key is unlinked and will no longer exist; the memory associated with the key value is reclaimed by an asynchronous process; non-blocking command_

### Expiration of keys (TTL)

- Expiration time can be set in milliseconds, seconds or UNIX timestamp (unix epoch time)
- TTL can be set on creation of the key or afterwards
- Expiration can be removed

#### Commands to set TTL

```
EXPIRE key seconds
EXPIREAT key timestamp
PEXPIRE key milliseconds
PEXPIREAT key milliseconds-timestamp
```

#### Commands to examine TTL

```
TTL key
PTTL key
```

_TTL for seconds, PTTL for milliseconds_

#### Command to remove TTL

```
PERSIST key
```

### Additional resources

Wikipedia article on Glob style wildcards:
https://en.wikipedia.org/wiki/Glob_(programming)
