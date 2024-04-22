## Bitmaps Explained

### Useful links:

- https://redis.io/docs/latest/develop/data-types/bitmaps/
- https://en.wikipedia.org/wiki/Bit_field
- https://redis.io/docs/latest/commands/bitcount/
- https://redis.io/docs/latest/commands/bitop/
- https://redis.io/docs/latest/commands/bitpos/
- https://redis.io/docs/latest/commands/bitfield/

### Notes

- There are 2 families of commands when dealing with bit data - bitfields and bit arrays
- Bitmaps are not an actual data type, but a set of bit-oriented operations defined on the String type which is treated like a bit vector
- Since strings are binary safe blobs and their maximum length is 512 MB, they are suitable to set up to 2^32 different bits
- Redis supports bitfields and bit arrays
- Redis strings allow us to store binary data
- There are set of commands that allows to treat this binary string as bitmaps - set or get any bit in the string with constant time complexity
- Redis supports variable-length bit fields using all of the standard bitwise operations - AND, OR, Exclusive OR (XOR), NOT...

### Use cases

- They allow for compact representation of date -> a histogram of counters or values or other fixed sized numeric data is ideally suited
- A use case for bit arrays are permission bits and masks, e.g. Linux style bit permissions

### Commands

- `SETBIT key offset value` - set a bit value at a given offset
- `GETBIT key offset` - get a bit value at a given offset
- `BITCOUNT key [start end [BYTE|BIT]]` - return the number of 1-bits; by default the offset is in bytes, rather than in bits; can specify negative positions
- `BITOP operation destkey key [key...]` - perform byte wise operations such as AND, OR, XOR, NOT...
- `BITPOS key bit [start] [end]` - finds the index of the first set or unset bit from a given index in the string; determine the position of a bit in a String

- `BITFIELD key [GET encoding offset|[OVERFLOW WRAP|SAT|FAIL] SET encoding offset value|INCRBY encoding offset increment [GET encoding offset|[OVERFLOW WRAP|SAT|FAIL]` where encoding (type) specify whether the value is signed, denoted by an `i`, or unsigned, denoted by un `u` and then the size (number of bits), resulting to for example `u8` - unsigned and 8 bits in length

- `TYPE key`
- `OBJECT ENCODING key`

### Bitfields

- Provides a way to get, set and increment a value in a bit field
- Allows for manipulation of one or more variable length integers within the string data type
- Example encodings - u4, u8, u10
- Index `0`is the highest order bit of the first byte on the left-hand side of the string (zero-based index)
- Bitfields also allow indexing by position
- Limits: up to 64 signed bits (i64) or 63 bits for unsigned (u63)
- Schemaless
- The passed `value` to the command is not the same as binary result (e.g. 2 for u8 is actually 00000010)
- `bitfield bf1 set u8 #1 5` - specify offset by position (using hash, #, e.g. pound sign); bitfield bf1 get u8 #1 get u8 8
- The Hash or pound sign is used to indicate the position by offset, based on the type provided. This simplifies calculating offsets. A multiple of the provided type to calculate the bit offset

### Bit arrays

- Allow manipulation of individual bits within a string datatype
- Bits arre addressed with a zero-based offset
- Bits are always limited by the maximum size of a string but it's not recommended to store and manipulate such large bit fields
- Using BITFILD is preffered, you canuse u1 type in the size to set an individual bit
