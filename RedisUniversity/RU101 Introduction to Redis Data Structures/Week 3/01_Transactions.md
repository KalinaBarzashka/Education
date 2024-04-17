## Transactions

### Useful links:

- https://redis.io/docs/latest/commands/?group=transactions
- https://redis.io/docs/latest/develop/connect/clients/
- https://en.wikipedia.org/wiki/Database_transaction
- https://en.wikipedia.org/wiki/ACID
- https://redis.io/docs/latest/commands/?group=transactions
- https://redis.io/docs/latest/develop/connect/clients/
- https://en.wikipedia.org/wiki/Optimistic_concurrency_control

### Notes

- Commands are processed in the order that the server received them
- Each data manipulation command is atomic (either completes or not)
- Redis single threads command execution to achieve atomicity
- Transactions are used to execute multiple commands as a single unit, where the group of commands must be atomic as a whole
- All the commands that are encapsulated in a transaction are serialized and executed sequentially. This guarantees that all the commands are executed in a single isolated operation
- Redis does not support nested transactions
- There is no rollback in Redis
- To avoid performance penalty of a rollback mechanism, Redis forgoes this on the basis that programming errors can't ever be eliminated and should be dealt with in pre-production cycles. This allows Redis to operate with maximum throughput and minimal latency

### Commands

- `MULTI` indicates the start of a transaction. Subsequent commands are queued up in the transaction pending execution
- `EXEC` is used to excute the queued commands
- `DISCARD` throws away queued commands
- `WATCH key [key...]` start watching a key/keys
- `UNWATCH` stop watching keys

### Possible errors:

- Programming errors: Syntax (Redis will not exec the commands); Operation on incorrect data type (only the error command will not be executed, but the subsequent commands will)
- System errors: Out of memory (partial writes and inconsistencies are resolved before the Redis process can restart)

### Optimistic Concurrency Control (optimistic locking)

- Mechanism that allows us to specify an interest in an object and get a notification if that object has changed
- Abort transaction if observed key has changed
- Keyspace notifications
- `WATCH key [key...]` - declare interest in one or more keys; Must be called before the transaction is started, so decide upfront the keys that needs to be observed
- Multile `WATCH key [key...]` command can be executed before the `MULTI`. The effects are cumulative. Subsequent `WATCH key [key...]` commands does not override previous keys being watched
- When `EXEC` is called the transaction will fail if any watched key have been modified
- `UNWATCH` is used to remove all watched keys
- `WATCH key [key...]` are local to the current client and connection
- After successful `EXEC`, all the watched keys are automatically `UNWATCH`-ed
