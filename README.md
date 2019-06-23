# CRMTestAPI

REST API to manage customer data for a small shop.

## How to run the API

#### Using [Microsoft Visual Studio](https://visualstudio.microsoft.com/es/)
1. Configure the [MySQL](https://www.mysql.com/) connection string in *CRMTestApi/appsettings.json*
2. Run CRMTestAPI

Now, we can access the API by navigating to *https://localhost:5001/*

If we are in development mode (we can configure the environment in *CRMTestAPI/Properties/launchsettings.json*) we will be able to access the API documentation by navigating to *https://localhost:5001/[swagger](https://swagger.io/tools/swagger-ui/)*

#### Using [Docker](https://www.docker.com/)

Navigate to the projects' root path and:

1. Generate an SSL certificate for the docker container:
    ```
    dotnet dev-certs https -v -ep ./Temp/cert-aspnetcore.pfx -p password
    ```

2. Build and run the Docker container:
    ```
    docker-compose build 
    docker-compose up
    ```
    Also, we can execute ```docker-compose up --build``` to do both things at the same time.

Now, we can access our API by navigating to *https://localhost:8081/*

We can run integration tests (*currently in development*) by executing:
```
docker-compose -f docker-compose.integration.yml up --build
```
## API Documentation

Content-Type header must be set to *application/json*

Authorization header must be set to *Bearer <token>* to access protected resources.

#### Authorization

* ```[POST] /login``` Get authentication token sending *email* and *password*

#### Users (requires authentication token with *admin* role)
* ```[GET] /users``` Get all users
* ```[GET] /users/{id}``` Get specific user
* ```[POST] /users``` Create a user sending *email*, *password*, *name* and *surname*
* ```[PUT] /users/{id}``` Edit a user sending *name* and *surname*
* ```[DELETE] /users/{id}``` Delete a user
	
#### Roles (requires authentication token with *admin* role)
  * ```[POST] /roles``` Create a role sending *type* and *userId*
  * ```[DELETE] /roles/{id}``` Delete a role

#### Customers (requires authentication token)
* ```[GET] /customers``` Get all customers
* ```[GET] /customers/{id}``` Get specific customer
* ```[POST] /customers``` Create a customer sending *name* and *surname*
* ```[PUT] /customers/{id}``` Edit a customer sending *name* and *surname*
* ```[DELETE] /customers/{id}``` Delete a customer
* ```[POST] /customers/{id}/photos``` Upload a photo sending *file* (Content-Type header must be set to *multipart/form-data*)
* ```[GET] /customers/{id}/photos``` Download a user photo