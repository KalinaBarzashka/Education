-- SELECT id and description of crime scene reports - found out how many intervies were taken
SELECT id, description FROM crime_scene_reports WHERE year = 2021 AND month = 07 AND day = 28 AND street = 'Humphrey Street';
-- Find more information from the interviews; within 10 minutes of the teft, the thief escape with a car; get footage; ATM on Leggett Street
SELECT id, name, transcript FROM interviews WHERE year = 2021 AND month = 07 AND day = 28;