## Faceted Search

### Attribute Search

- Object inspection
- Faceted search (also called inverted index)
- Hashed index
- https://en.wikipedia.org/wiki/Faceted_search
- https://en.wikipedia.org/wiki/Inverted_index

### Redis, unlike a traditional database, does not have secondary indices, so we cannot create un index on field/s in order to query by them

### Method 1: Object Inspection

- In this method we retreive all objects and check each one for a match
- As the data set grows, the cost of execution increases, e.g. for a 1M elements, 1M GET commands must be executed
- This works reasonably well on small data volumes, but proportionally increases as the cardinality grows
- Limiting factor: The number of keys that have to be matched and inspected

### Method 2: Set Intersection

- Big O for SINTER command is `O(n*m)` (n is the cardinality of the smallest set and M is the number of sets)
- Start with the set with least number of elements
- Here we retreive matched objects only
- Having the server perform less work will improve CPU, network and memory usage
- It also helps the concurrency of the system
- The cost here is governed by the cardinality of the smaller set and the cardinality is governed by the data distribution of the values for the attributes
- There are two limiting factors for faceted searches using Sets. Firstly is the data distribution of matching attribute values. The second is the number of attributes being matches.
- Limiting factor: The data distribution of the attribute values being matched AND The number of attributes that have to be matched

### Method 3: Hashed Keys

- Hashing search values (this method is like combined index in RDBs)
- Combine all the searchable attributes
- Compound indices require that you need the leading part of the index (e.g. disabled_access must be always present, but we do not need to have medal event and venues in order to use the index)
- We can determine the order of the compound index keys but you need a clear idea of access patterns
- Solution: create a consistent hash of the attributes we're searching for (the hash value we generate must be reproducible based on the same input values)
- Alghorithms like SHA-256, SHA-512, MD5 and RIPEMD160...
- The hash value is used as a key and each event sku is added to a set with a matching hash
- Each combination of attribute values results in a Set! The Set only contains matches for all criteria
- Here we call SSCAN to retrieve the desired matches
- The cost is simply how many matches exist for a combination of attributes (we dont care how many attributes we are matching)
- As the number of attributes multiplies, then the cost does not change
- This technique optimizes the retrieval access path, but this comes at additional cost when the data is modified
- We need to be familiar with the rate of change of the data
- https://en.wikipedia.org/wiki/Consistent_hashing
- Limiting factor: The number of matches for any given attribute combination
