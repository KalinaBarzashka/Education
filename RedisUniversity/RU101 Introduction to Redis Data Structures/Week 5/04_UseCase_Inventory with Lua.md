## Use Case: Inventory with Lua

### Overview: Inventory Management with Lua

- Modeling Events, Holds and Purchases
- Managing Lua Scripts with Python
- Modeling a Purchase Flow (implement a state machine in Lua)
- Reserving Tickets
- Purchase Flow
- There are 2 components: Purchases and Events
- Purchase - contains all the information relevant to a purchase (sku, qty, ts, customer_id, state)
- Event - sku, name, available:General, price:General
- We'll model purchases and events as hashes
- Purchase hold - time-limited intent to purchase a number of general admission tickets - we'll use hashes again (hold:customer_id:event_id -> qty and state fields); we'll set expiration for these keys
- We need to associate all of the holds with a particular event -> create an event specific set to cointain a reference to each outstanding hold (holds:event_id) - set

### Managing Lua Scripts with Python

- Global variables are not allowed in Redis-based Lua scripts, use local variables
- Nil in Lua is equivalent to Python's None
- We need to register our script before using it, returns a callable script object
- Behind the scenes, the Redis Python Cliet loads the script using `SCRIPT LOAD` command and then cache the SHA signature of the script
- register_script -> keeps track of the script's SHA, allowing the client to use EVALSHA for script invocations behind the scenes
- Lua uses the double-equal operator (==) for comparisons and the keyword "and" for the boolean AND operation
