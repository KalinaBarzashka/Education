## Use case: Inventory control

### Useful links:

- https://university.redis.com/asset-v1:redislabs+RU101+2018_01+type@asset+block@Handout__Use_case_2_-_Inventory_Control.jpg

### Requirements:

- A ticket can be purchased once and only once
- During the purchase flow, inventory needs to be reserved so that others don't buy the same ticket
- If the purchase does not complete, any reserved inventory needs to be returned to the available pool
- Purchased tickets can be viewed by customers
- Customers can make multiple purchases for the same event
- We'll be using hashes, sets and transactions to solve the problem

### Inventory Control

- a
