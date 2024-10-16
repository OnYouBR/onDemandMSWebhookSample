# Azure Function v4 Webhook for OnYou Events

This repository contains a mocked implementation of the webhook as specified on the '**Webhook for Interaction Events**' page of the OnYou API [**Technical Manual**](https://insights.onyou.com.br/api-manual).

The webhook was written in **C# and .NET 8** to be deployed as an **Azure Function v4**.

This is an illustrative example of **how to handle real-time events from OnYou’s API**, as they occur during the workflow to invite, engage, and convert regular consumers (of OnYou’s clients) into one-time mystery shoppers.

## Prerequisites

- Azure account
- .NET 8 SDK
- Azure Functions Core Tools (v4)

## Solution Structure

The solution, named `OnYouCampaign.sln`, is located in the `source` folder and is includes the `Webhook.csproj`.

## Setup and Build
To setup and build the sample component:

1. Clone the repository:

   ```shell
   git clone https://github.com/OnYouBR/onDemandMSWebhookSample.git
   ```

2. Navigate to the solution directory:

   ```shell
   cd source
   ```

3. Build the solution:

   ```shell
   dotnet build OnYouCampaign.sln
   ```

## Usage

To start the function app locally:

1. Navigate to build directory:

   ```shell
   cd bin\Debug\net8.0
   ```

2. Start the function:

   ```shell
   func start Webhook
   ```

## Contributing

Direct contributions via GitHub are not allowed for this project.

However, we welcome your input and suggestions via email. Please send your contributions to <labs@onyou.com.br>.

## License

This project is licensed under the MIT License.

## Contact

For any questions or feedback regarding this project, please visit [labs.onyou.com.br](https://labs.onyou.com.br) or send an email to <labs@onyou.com.br>.
