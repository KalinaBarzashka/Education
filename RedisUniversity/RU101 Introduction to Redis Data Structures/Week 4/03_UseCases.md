## Use case: Seat Reservations

### Requirements:

- A seat map for each event needs to be maintained so that a customer can see what is available
- A customer must be shown blocks of seats that match their needs, for example when they ask for five seats show the availability of five consecutive seats in the required tier of tickets
- Seats can be reserved and booked once and only once
- The solution needs to allow for concurrent seat bookings in the same row of blocks
- Disparate requests without the customers having to be re-forced or rebook because somebody reserved different seats in the same block

## Use case: Notifications & Fan Out

### Requirements:

- First part of requirements: use the fan-out characteristics of publish and subscribe, in order to feed several services that will produce analytics of the sales made by event, by time of day, etc.
- Second part of requirements: how a pattern subscription can be used to pick lottery winners for the opening ceremony, and show orders in real time.
