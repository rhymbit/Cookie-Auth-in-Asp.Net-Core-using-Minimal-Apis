# 🍪Cookie Authentication in Asp.Net Core using Minimal APIs🍪

This is a sample project to demonstrate how to use cookie authentication in Asp.Net Core using Minimal APIs.  
No session tokens are stored on the server. Instead of session tokens the server uses Json Web Tokens.
When a request is made to the server's authenticated endpoints, the server :-

- Intercepts the request before it reaches the JWT authentication middleware.
- Takes out the JWT from the request's cookie.
- Assign the JWT to the request's Authorization header.
- Passes the request to the JWT authentication middleware.

The server supports Access and Refresh tokens.

## How to run the project 🏃‍♂️

To run the Asp.Net server, `cd` into the `Api` folder and run the following command:-

```bash
dotnet run
```

To verify that the server is functioning correctly, a React App is also provided. To run the React App, `cd` into the `frontend` folder and run the following command:-

```bash
yarn # to install the dependencies (and please use yarn instead of npm)
yarn dev # to run the React App
```

Then visit `http://localhost:5173` in your browser.

## Important Note 📝

The cookies `domain` property is set to `.localhost` in the `Api` project. So the react app should be able to save the cookies. If you are using a different domain, please change the `domain` property in the `Api` project.
