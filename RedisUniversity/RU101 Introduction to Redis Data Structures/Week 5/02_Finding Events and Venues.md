## Finding Events and Venues

### Overview

- Finding venues from a given point (e.g. finding venue within 5 kilometers of "Tokyo Station")
- Finding events from a given subway station (e.g. finding a "Football" event within a given distance) - we are using reverse index technique!
- Finding venues on a given subway line (e.g. the distance from other venues from the Keiyo line) - we are using reverse index technique!; a key for each subway line can be maintained with a geospatial entry for each venue on that subway line

### Finding Venues from Another Point or Member

- Create a geospatial object for each venue
- create_venue function accepts the venue structure and creates the geospatial object. It then uses the long. and lat. to add to the key holding the geospatial positions for all venues
- The key of the sorted set is made up of "geo" (domain prefix), and "venue" (to discriminate the type of geospatial object the sorted set contains)
