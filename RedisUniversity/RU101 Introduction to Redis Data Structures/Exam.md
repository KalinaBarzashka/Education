## 1.1

Given the following commands are executed:
`bitfield bfq-1 set u8 #0 3`
`bitfield bfq-2 set u8 #1 3`
`bitop or bfq-3 bfq-1 bfq-2`
What does the following command return?
`bitfield bfq-3 get u16 0`

- 771

## 1.2

When a message is published with the PUBLISH command, does Redis guarantee the order of messages in a single node?

- Yes

## 1.3

In the sample data in the Virtual Lab, there are two Geospatial keys:
`geo:events:Football`
`geo:events:Athletics`
If you need to find Geospatial points that are in both keys, what command should you use?

- ZINTERSTORE

## 1.4

Within a transaction, are changes made by a command visible to subsequent commands in the same transaction?

- Yes. In a queued set of commands, changes made in that Transaction are visible to the subsequent commands in the same Transaction. Therefore, if you perform a SET, INCR or other command that changes a value, and subsequent retrieval via a GET, HGET, LPOP etc., will see the key modified by the prior commands in the transaction.

## 1.5

In the sample data in the Virtual Lab, there are two keys:
`geo:events:Football`
`geo:events:Softball`
What is the distance in kilometers between "Sapporo Dome" in the geo:events:Football key and "Yokohama Stadium" in geo:events:Softball key?

- 855.8835

## 1.6

If an expiration of a key is set as follows
`set fe-16 hello`
`expireat fe-16 2524608000`
What does the following command return?
`ttl fe-16`

- The seconds remaining until the key expires

## 1.7

Can you execute the STRLEN command on a key that was set with BITFIELD?

- Yes. The BITFIELD command manipulates bitfields with a String datatype. The STRLEN command operates on any value held in the String datatype, regardless of the encoding of the value.

## 1.8.

In the sample data in the Virtual Lab, there are two Geospatial keys:
`geo:events:"Football"`
`geo:events:"Modern pentathlon"`
What command is used to find the Union of all Geospatial objects, maintaining the correct GeoHash for each Geospatial point?

- zunionstore foo 2 geo:events:Football geo:events:"Modern pentathlon" aggregate min
- zunionstore foo 2 geo:events:Football geo:events:"Modern pentathlon" aggregate max

## 2.1

If the following commands are executed
`del fe-21`
`lpush fe-21 foo`
`lpop fe-21`
What is the output of the following command?
`type fe-21`

- None

## 2.2

When performing an Intersection between sets, what are the limiting factors of the command?

- The number of sets operated on
- The cardinality of the smallest set

# 2.3

The ZADD commands accepts a string representation of a double precision floating point number. What other constant values does the command accept?

- +inf
- -inf

## 2.4

Once a MULTI command has been executed by a client, how many more MULTI commands can be executed by the same client before an EXEC or DISCARD is called?

- Zero

## 2.5

If the following commands are executed
`watch foo`
`watch bar`
Which keys are being observed?

- foo and bar

## 2.6

If the following commands are executed by two separate clients:
`client-1> subscribe foo foo:bar`
`client-2> psubscribe foo*`
What does the following command return if executed by a third client?
`client-3> pubsub numsub foo`

- 1

## 2.7

For a field in a hash, how would you decrement the value by one?

- HINCRBY key field -1

## 2.8

The list, my-list, is created with 1000 elements. Given the following two commands:
`lindex my-list 1`
`lindex my-list -2`
Is the time complexity for these two commands the same?

- Yes. The time complexity for LINDEX is defined as O(N), where N is the number of elements traversed. Indexes are zero based, so lindex my-list 1 needs to traverse the zero-th element to get to index position 1. So, N is 1 in this case. A negative index means traversing from the right of the list. -1 is the last (or right-most) element, therefore -2 is the penultimate element. For the command lindex my-list -2, then N is also 1.

## 3.1

A Sorted Set is created as follows:
`zadd fe-31 1 a 2 b 3 c 4 d 5 e 6 f`
What commands would return members c, d, e and f?

- zrange fe-31 2 -1
- zrange fe-31 -4 -1
- zrange fe-31 2 5
- zrangebyscore fe-31 3 6
- zrangebyscore fe-31 (2 6
- zrangebyscore fe-31 3 +inf

## 3.2

With the BITCOUNT command, what is the unit used for the start and end position?

- Byte

## 3.3

The virtual lab contains the following key:
`event:PDMM-JUOT-FPFF-BBLO`
What are the total available seats in all seat tiers for that event?

- 23291

## 3.4

If a Lua script exceeds the execution time threshold, what happens if another client submits a GET command?

- The Server responds with a BUSY response

## 3.5

In your virtual lab, the following keys contain the venues associated with each sporting event:
`geo:events:Cycling`
`geo:events:Marathon`
What is the number of unique venues from these two keys?

- Apart from visually inspecting each sorted set, you can find the correct results using ZUNIONSTORE.
  `zunionstore foo 2 geo:events:Cycling geo:events:Marathon aggregate min` - (integer) 3
  The return value is the number of elements in the Sorted Key "foo".
  The aggregate min parameters is critical, since by default ZUNIONSTORE with aggregate the scores. Since "Imperial Palace Garden", is present in both sets, without the specification of this clause, it would cause this Geospatial point to be moved.

## 3.6

Which Lua script would preserve and return the floating point value?

- eval "return {'3.1415927'}" 0
- eval "local val = '3.1415927' return {val}" 0

## 3.7

When can a WATCH command be executed by a client?

- Before a MULTI command

## 3.8

Can UTF-8 data be stored and retrieved with the String data type without any loss or corruption of the data?

- Yes. Strings in Redis are a binary safe sequence of bytes. Therefore UTF8 data can be stored and retrieved safely.
