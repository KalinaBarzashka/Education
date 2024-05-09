## Lua Scripting - sever side scripting

### Overview

- Lua (from portuguese - moon) is embeddable scripting language
- Created at Pontifical Catholic University
- Open source, MIT license
- Has small footprint
- Portable, cross platform
- Lua script allows you to wrap a set of commands with all the constructs provided by a programming language, including loops, conditionals...
- reduce network traffic
- Lua is similar to a stored procedure
- The script runs where the data is located, which has some performance benefits
- Lua script is also run by Redis as a single atomic unit of work
- Redis is a single threaded system
- Redis treats `EVAL` as a single atomic command
- Redis maintains a bi-directional conversion table of data types between Lua and Redis and vise versa
- Run server side, so any intermediate results do not have to be transferred between the server and the client
- Blocking operation

### Running Lua script

- `EVAL script numkeys key [key...] arg [arg...]` - where the script is passed as a string
- There are 2 ways to execute a Redis command from the Lua environment - call and pcall - they differ in how they deal with errors returned by Redis server
  - `redis.call` will simply propagate the error back, causing EVAL to fail; causes script to terminate when Redis returns an error
  - `redis.pcall` will return a structure representing the error response, which can then be dealt with programatically
- Within a Lua script, two predefined arrays are created - `KEYS` is an array for every key that is passed into the `EVAL` command; `ARGV` is an array holding every argument that's passed into the `EVAL` command
- The key names can be hardcoded, without passing them in the array, which is less flexible
- Lua arrays are NOT zero-based, they are 1-based

### Managing Scripts

- `EVAl` is treated as a single atomic command - any commands submitted after `EVAL` will be queued up and will wait for `EVAL` to complete
- A script is passed to Redis over the network; its then parsed; then its executed; the return values are returned to the client
- Redis maintain a cache of compiled scripts, because parsing a script has some overhead - `SCRIPT LOAD` command
- `SCRIPT LOAD script` - takes a given script, parses it and then returns the script's hash digest (we can invoke the script by passing the hash digest to the `EVALSHA` command)
- `EVALSHA sha1 numkeys key [key...] arg [arg...]`
- `SCRIPT EXISTS sha1 [sha1...]`
- `SCRIPT FLUSH`
- `SCRIPT KILL` - termiante the currently executing Lua script
- `SCRIPT DEBUG YES|SYNC|NO` - never use in Production
- The `SCRIPT LOAD` command returns a SHA-1 digest for the passed in script

### When a script is not atomic

- Default 5 econd execution time
- Long running scripts are not terminated (when the execution treshold has beed exceeded, Redis will not terminate the script)
- The server logs messages indicating a long running script
- Redis will start accepting new commands from other clients - accepts admin commands, all other commands rejected with BUSY response
- If no data has been written, we can use SCRIPT KILL, if data has been written, only option is to execute the SHUTDOWN NOSAVE command
- The goal is to ensure that scripts never exceed the execution threshold - scope script to minimum - performace depend on the cardinality of the data set (for commands != O(1))
- Script is transactional boundary

### Rules

- №1 - Keys should be passed in, not hard-coded
- №2 - Arrays are <b>one-based</b>
- №3 - Lua has a single numeric type, so <b>floats are truncated</b>. Is we need to preserve the full value, retrieve, store and return the value as a string.

### Use cases

- Limited counters (for rate limiting) - count by period; adjust counter; allow request if threshold not exceeded

### Examples

- eval "return {KEYS[1], {ARGV[1], ARGV[2]}}" 1 hash-key field1 field2
