## Use case: Inventory control

### Useful links:

- https://university.redis.com/asset-v1:redislabs+RU101+2018_01+type@asset+block@Handout__Use_case_2_-_Inventory_Control.jpg
- https://university.redis.com/asset-v1:redislabs+RU101+2018_01+type@asset+block@Handout__Use_case_2_-_Reservations.jpg

### Requirements:

- A ticket can be purchased once and only once
- During the purchase flow, inventory needs to be reserved so that others don't buy the same ticket
- If the purchase does not complete, any reserved inventory needs to be returned to the available pool
- Purchased tickets can be viewed by customers
- Customers can make multiple purchases for the same event
- We'll be using hashes, sets and transactions to solve the problem

!!! Hash scan has constant time complexity. However, if we are processing a large volume of ticket requests, there could be a large number of pending requests that would need to be examined during each invocation of the "ExpireReservation()" function. If we need to deal with a large volume, then scanning a hash may not be the best solution. There are many alternatives, but sorted set could be utilized. We can use the timestamp as the score, with the OrderId as the value. Then ZRANGE can be used to get the element with the lowest score, and thus the oldest entry. Use WITHSCORES so we can use the score to compare it to see if the threshold has been exceeded. ZRANGE is O(Log(N)+M). !!!
