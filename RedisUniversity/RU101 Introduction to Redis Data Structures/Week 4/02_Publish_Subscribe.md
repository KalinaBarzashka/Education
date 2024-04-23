## Publish / Subscribe

### Useful links

- https://redis.io/docs/latest/develop/interact/pubsub/
- https://redis.io/docs/latest/commands/?group=pubsub
- https://redis.io/docs/latest/commands/psubscribe/
- https://redis.io/docs/latest/commands/punsubscribe/

### Notes

- The functionality in Redis allows for simple message buses to be created
- Redis acts as a broker for multiple clients providing a simple way to post and consume messages and events
- For single Redis node, the ordering of messages in a channel is guaranteed
- Publish and subcsribe does not guaranteed delivery. If we sub after a message was published, we wont receive it
- Fire and forget mechanism
- Message value can be any arbitrary binary string
- 1 MB payload, 100 clients will cause 100 MB of data to be transferred over the network
- As the number of patterns (when using PSUBSCRIBE) increases the more work and therefore the more latency will be introduced
- Performance - key consideration - designing payload size, number of connected subscribers, number of patterns and the potential network resources that will be required to move the data

### Commands for Simple Syndication

- `PUBLISH channel message`
- `SUBSCRIBE channel [channel...]` - blocking call, but unlike other blocking commands, Redis will accept other commands, like (P|S)UNSUBSCRIBE, PING, QUIT, RESET, (P|S)SUBSCRIBE
- `UNSUBSCRIBE [channel [channel...]]`

### Commands for Patterned Syndication - use judiciously

- `PSUBSCRIBE pattern [pattern...]`
- `PUNSUBSCRIBE [pattern [pattern...]]`

### Commands for Admin; introspect the pub/sub system

- `PUBSUB subcommand [argument [argument...]]` - allows for introspection of the publish and subscribe mechanism
- Subcommands:
  - `CHANNELS [pattern]` - return list of active channels
  - `NUMSUB [channel-1 channel-N]` - returns number of subscribers excluding patterned subscribers
  - `NUMPAT` - returns the number of patterned subscribers and the number of patterns

### Supported glob-style patterns for PSUBSCRIBE

- h`?`llo subscribes to hello, hallo and hxllo
- h`*`llo subscribes to hllo and heeeello
- h`[ae]`llo subscribes to hello and hallo, but not hillo
- h`[^e]`llo subscribes to hallo, hbllo, ... but not hello

### Use cases

- Fan Out - distribute a message or event to many consumers, add and remove consumers without refactoring code
- Interprocess (inter-service) communication
- Best suiter for feeds and streams, like a chat log
