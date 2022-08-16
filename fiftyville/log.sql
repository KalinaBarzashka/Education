-- SELECT id and description of crime scene reports - found out how many intervies were taken; 10:15
SELECT id, description FROM crime_scene_reports WHERE year = 2021 AND month = 07 AND day = 28 AND street = 'Humphrey Street';
-- Find more information from the interviews; within 10 minutes of the teft, the thief escape with a car; get footage; ATM on Leggett Street; flight on 29/07/2021
SELECT id, name, transcript FROM interviews WHERE year = 2021 AND month = 07 AND day = 28;
-- Check ATM transactions
SELECT id, account_number, transaction_type, amount FROM atm_transactions WHERE year = 2021 AND month = 07 AND day = 28 AND atm_location = 'Leggett Street' AND transaction_type = 'withdraw';
-- CHECK bank accounts of the previously found ATM transactions and account numbers and get peoples name and license_plates
SELECT bank_accounts.person_id, people.name, people.license_plate FROM bank_accounts JOIN people ON bank_accounts.person_id = people.id WHERE account_number IN (SELECT account_number FROM atm_transactions WHERE year = 2021 AND month = 07 AND day = 28 AND atm_location = 'Leggett Street' AND transaction_type = 'withdraw');
-- CHECK bakery security logs for license_plate and time between 10:15 and 10:25
SELECT activity, license_plate, hour, minute FROM bakery_security_logs WHERE year = 2021 AND month = 07 AND day = 28 AND hour = 10 AND minute BETWEEN 15 AND 25;
--CHECK phone calls
SELECT caller, receiver, duration FROM phone_calls WHERE  year = 2021 AND month = 07 AND day = 28 AND duration < 60;