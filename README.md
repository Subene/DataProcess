# MailDataProcess

Description
The Mail Data Processing API is designed to parse structured expense claim data embedded in plain text input and extract relevant information such as cost center, total amount, payment method, vendor, description, and date. The API ensures data integrity by checking for balanced tags, validating required fields, and calculating tax information.

Features

Text Parsing: Parses plain text input with marked-up sections to extract expense details.
Tag Validation: Checks for balanced tags and ensures required tags (e.g., <total>) are present.
Default Handling: Defaults the cost centre to "UNKNOWN" if not specified.
Validation: Rejects input with missing <total> tags or unbalanced markup.
Calculation: Calculates sales tax (assumed at 10%) and derives the total amount excluding tax.
Error Handling: Handles cases with missing or invalid data gracefully and returns informative error responses.

Project Structure

Controllers: ExpenseController handles HTTP POST requests and initiates the parsing process.
Helpers: ExpenseParser contains the logic for extracting and validating data from the input text.
Models: ExpenseData represents the parsed output structure with fields for all extracted data.

Notes

This API accepts text/plain content type and is tested using tools like Postman and Swagger.
The API returns 415 Unsupported Media Type if the input format is incorrect or not properly set as text/plain.
Ensure that the input text includes proper markup tags to avoid rejection due to validation errors.

Usage

Send a POST request to the /api/expense/get endpoint with the plain text body.
The API will parse the input, validate tags, and return a structured JSON response with the extracted data or an error if parsing fails.
