##Batch running the test, construct phone numbers as array object
$phoneNumbers = @(
    "0812345678"
)

$phoneNumbers | ForEach-Object {
    & gulp test --phone "$_"
}