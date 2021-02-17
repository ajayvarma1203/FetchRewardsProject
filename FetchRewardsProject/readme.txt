-------------------------------------------

FetchRewards Take Home Assessment -- Back End Engineer

Requirements:
1. C# ASP.NET v3.1

Tools to be used to run and test:
1. Visual Studio
2. Postman

Assumptions:
No two transactions are done at the exact same time. They have atleast seconds of gap in between.

Rest API Descriptions:
For the project on a whole, I have used Model Controller Architecture and implemented API for the respective REST API calls under Controllers Package.
For the data, I have just used File system to store the data instead of a database.
There are two files which have been used.
1. Transactions.txt which will be used (created if it doesn't already exist) whenever the addTransaction API is called.
   This file is automatically flushed in case of a bad request.

2. Balances.txt which will be used to calculate and store all the final balances. This will be automatically flushed whenever any new addTransaction request is given.

I used three controllers each for an API
1. AddTransactionController

    This is to add transactions individually in any order into the file.

    URL: https://localhost:5001/api/addTransaction
    Headers: content-type - application/json

    It is a POST API which accepts a json input of transaction which has keys and values for payer, points and timestamp

    Example Payload:

    i)   { "payer": "DANNON", "points": 1000, "timestamp": "2020-11-02T14:00:00Z" }
    ii)  { "payer": "UNILEVER", "points": 200, "timestamp": "2020-10-31T11:00:00Z" }
    iii) { "payer": "DANNON", "points": -200, "timestamp": "2020-10-31T15:00:00Z" }
    iv)  { "payer": "MILLER COORS", "points": 10000, "timestamp": "2020-11-01T14:00:00Z" }
    v)   { "payer": "DANNON", "points": 300, "timestamp": "2020-10-31T10:00:00Z" }

2. SpendController

    This is to take input of the points to be given to payers.
    It gives a JSON response of payers with the points spent.

    URL: https://localhost:5001/api/addTransaction
    Headers: content-type - application/json

    It is a POST API which accepts a json input of points to be distributed/used.

    Example Payload:
    { "points": 5000 }

    Example Response:
    [{ "payer": "DANNON", "points": -100 },
     { "payer": "UNILEVER", "points": -200 },
     { "payer": "MILLER COORS", "points": -4,700 }]

3. BalanceController

    This gives the list of payers with their final balances after all the transactions.

    URL: https://localhost:5001/api/balance

    It is a GET API which gives a JSON Object response of all the users with their balances.

    Example Response:
    {"Dannon": 1000, "Unilever": 0, "Miller Coors": 5300}

Two Model classes have been used, PayerAccount.cs for the details of Payer to be encapsulated and similarly Transaction.cs to encapsulate Transaction data.

Application Default Port : 5001

Please use postman to test the APIs.
Import the project in Visual Studio and run the solution.
Once the server is up and running, make the calls as in the test case accordingly in Postman with the above provided URLs and payload styles.

-------------------------------------------