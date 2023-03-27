# Exchange Order Book Time Series
This program produces a time series over an exchange order book, showing minute by minute information for three different metrics aggregated by Security ID. The three metrics are:

- Best bid: The highest price that buyers are willing to pay for a security.
- Best ask: The lowest price that sellers are willing to accept for a security.
- Bid-ask spread: The difference between the best bid and best ask prices.

## How it Works
The program reads in a log file with a series of FIX messages from the exchange's order book. Each FIX message contains information about an order to buy or sell a security, including the security ID, the order type (Bid or Ask), the price, and the quantity.

The program aggregates this information by instrument ID and calculates the best bid, best ask, and bid-ask spread for each instrument ID for each minute of trading. It then outputs this information as a time series in CSV format, with one row per minute and columns for the instrument ID, best bid, best ask, and bid-ask spread.

## Getting Started
Given a valid log file with the FIX messages, you can run the program using the following command:

<code>$ dotnet /path-to-binaries/Application.dll </code>

The program will prompt the user to enter a valid FIX log file.

Then the CSV file with the time series will be generated in the current folder, with the name `time-series.csv`

## Limitations
This program assumes that the input FIX messages are in a specific format and that the Security ID is contained in a specific field. If your FIX messages are in a different format, you may need to modify the program to parse them correctly.
