## Geospatial

### Useful links:

- https://redis.io/docs/latest/commands/?group=geo
- https://en.wikipedia.org/wiki/Haversine_formula
- https://en.wikipedia.org/wiki/Geohash
- Geospatial standards:
  - https://epsg.io/900913
  - https://spatialreference.org/ref/epsg/popular-visualisation-crs-mercator/
- https://redis.io/docs/latest/commands/geoadd/
- https://redis.io/docs/latest/commands/geohash/
- https://redis.io/docs/latest/commands/geopos/

### Notes

- Redis geospatial indexes store named points (unique name) and thier associate latitude and longitude coordinates
- Redis uses the technique of geo hashing to store geospatial points
- Behind a geospatial index (key), we can store multiple lat, long, unique name (sometimes we can store the id as a name, and this id can correspont to a key of a hash)
- Retrieval by: distance, redius, polygon, etc.
- Geospatial databases are nothing new, but Redis brings this problem domain is the ability to sore, query and update huge amounts of geosptial objects with very low latency
- Strategy for managing geospatial objects: for each long and lat pair, a GeoHash is computed. A GeoHash is a 52 bit integer value in Redis, which encodes positions in a short string of letters and digits
- Longitude can range from -180 through to +180 (so no limitation for longitude values!)
- Latitude is restricted to -85.05112878 through +85.05112878 and change
- GEOADD will fail is we use coordinates out of the limits above
- Sorted sets are the data type used to store geospatial objects
- With ZUNIONSTORE and ZINTERSTORE command will sum up the scores of the elements, and for geospatial data, this would cause the point to move! Retaining the Geospatial point is simple, you will just need to use the MIN or MAX aggregate operator
- There is no specific command for removal. Use ZREM! DEL or UNLINK would remove the entire key.
- Inspect geospatial objects stored in a key with ZRANGE - list a range of members
- Redis computes a Geohash from the Latitude and Longitude. This is a 52 bit number, which can be safely stored in a score of a Sorted Set (which is a double).

### Some commands

- `GEOADD key latitude longitude member [latitude longitude member...]` - add location to a geospatial index
- `GEORADIUS key currentlat currentlong 4 km [WITHDIST] [WITHCOORD] [COUNT int]` - where 4 is an int number; WITHDIST adds the computed distances from our specified center for each geospatial item; by default returns coordinates in an unsorted order - use ASC to get nearest to farthest
- `GEOHASH key member [member...]` - returns the geohash for one or more members of the set (11 characters representation of the hashed value)
- `GEOPOS key member [member...]` - returns the longitude and latitude of one or more members

### Adding and retrieving Geospatial Points

- Latitude and longitude are interleaved in order to form a unique value
- Each GeoHash is stored in the named key. The data type of that key is a sorted set. The GeoHash is sotred as the score, and the name of the point is used as the value of the member. The 52 bit integer of the hash is stored as the score!
- We can use the ZRANGE command to retrieve members and scores. The score can be seen for each member. This is the computed GeoHash for the point.
- If we rum GEOADD for the same named point with a different long and lat, it will perform un update

### Searching for Geospatial Objects

- `GEODIST key member1 member2 [unit]` - calculate the distance between two members held at the same key; unit can be meters, kilometers, feet and miles
- `GEORADIUS ...` - find geospatial members within specified radius; held in the same key; provide long and lat as a center point, from which the search is performed
- `GEORADIUSBYMEMBER ...` - the difference between this command and GEORADIUS is the point we are searching from (the member will be the center point);
- GEORADIUS and GEORADIUSBYMEMBER cna return coordinates, distance or hash; both can store the result into another key

### Use cases

- Finding points nearby - location-based social applications
- Targeted advertising - Customers in location; Offers by location
