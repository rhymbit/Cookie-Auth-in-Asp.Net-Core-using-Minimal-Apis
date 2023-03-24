# Cookie Authentication in Asp.Net Core using Minimal APIs🍪

This is a sample project to demonstrate how to use cookie authentication in Asp.Net Core using Minimal APIs.  
No session tokens are stored on the server. Instead of session tokens the server uses Json Web Tokens.  
The server supports Access and Refresh tokens.  
When a request is made to the server's authenticated endpoints, the server :-

- Intercepts the request before it reaches the JWT authentication middleware.
- Takes out the JWT from the request's cookie.
- Assign the JWT to the request's Authorization header.
- Passes the request to the JWT authentication middleware.
- This 👆🏻 process is done by this little piece of code [here](https://github.com/prateek332/Cookie-Auth-in-Asp.Net-Core-using-Minimal-Apis/blob/04401e69ccab6ff920111e95fc50756d40965d68/Api/Program.cs#L88-L101), which is then added over [here](https://github.com/prateek332/Cookie-Auth-in-Asp.Net-Core-using-Minimal-Apis/blob/04401e69ccab6ff920111e95fc50756d40965d68/Api/Program.cs#L41).

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

## Important Notes 📝

- The cookies `domain` property is set to `.localhost` in the `Api` project. So the react app should be able to save the cookies. If you are using a different domain, please change the `domain` property in the `Api` project.
- It's important to add

  ```js
  credentials: "include";
  ```

  to all the `fetch` requests in the frontend app. This is because the cookies are set to `SameSite=Strict` and `Secure=true`. So the cookies will not be sent to the server unless the `credentials` property is set to `include`. Look at [this](https://github.com/prateek332/Cookie-Auth-in-Asp.Net-Core-using-Minimal-Apis/blob/04401e69ccab6ff920111e95fc50756d40965d68/frontend/src/components/TestAuthentication.tsx#L12-L14) for reference.

- The server was written using Visual Studio. So if shit hits the fan, you can always open the `Api.sln` file in Visual Studio and run the project from there.
