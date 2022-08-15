SELECT title FROM movies
JOIN 
WHERE id = (SELECT movie_id FROM
                (SELECT movie_id FROM stars JOIN people ON stars.person_id = people.id WHERE name = "Johnny Depp")
                WHERE movie_id = (SELECT movie_id FROM stars JOIN people ON stars.person_id = people.id WHERE name = "Helena Bonham Carter"));