# Workshop

Example of contract test with two microservices on c#

- Requirements
- Start our application
- Consumer Section
- Provider Section
- Environment expected

## Requirements

- Dotnet Core 3.5
- Pact Net
- PactFlow

## Start our application

First we will install the nugget required for contract, in our case we will use `Pact Foundation`

```bash
  # We require run the command in each project that will be use pact foundation
  dotnet add package PactNet.Windows
```

To run the application, 

```bash
  # We require run the command in each project that will be use pact foundation
  dotnet watch run
```

## Consumer Section

We require create a mock service that response when the consumer call the provider service.

After create the mock provider service, we can create our test of contract and it will generate a json with the information about the contract between the services



## Provider Section

For the provider section, the first step is create a middleware, because contract will send request to our application to verify that everything is correct.

After we create the middleware, we use a provider state with the functionality of `before Test` used in other languages


## Environment expected

The objective of contract is receive an early feedback, but is know that the manual proccess is very slowly and if we want automate the proccess of contract test, we require use other services, how `Pact Flow` or `Pact broker`, with those service we can storage all contract created with the consumer, and verify the provider have de behavior expected