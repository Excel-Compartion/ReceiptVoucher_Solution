using Azure.Core;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.VisualBasic;
using MudBlazor.Charts;
using MudBlazor;
using ReceiptVoucher.Core.Enums;
using ReceiptVoucher.Core.Helper;
using System.Buffers.Text;
using System.Dynamic;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net;
using System.Numerics;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Reflection;
using System;
using System.Security.Claims;
using static MudBlazor.CategoryTypes;
using static MudBlazor.Colors;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Text.Json;

namespace ReceiptVoucher.Client.Services
{

    /// <summary>
    /// 
    /// The code you shared is a custom implementation of the AuthenticationStateProvider class, which is used to provide information about
    /// the user's authentication state in Blazor applications¹. The custom provider uses a local storage service and an HTTP client to get and set 
    /// the authentication token and the user's identity and claims.Here is a line-by-line explanation of the code:
    /// - `public class CustomAuthStateProvider : AuthenticationStateProvider`: This line defines a new class named
    /// CustomAuthStateProvider that inherits from the AuthenticationStateProvider class.
    /// - `private readonly ILocalStorageService _localStorageService;`: This line declares a private and read-only
    /// field of type ILocalStorageService, which is an interface for accessing the browser's local storage². The field is 
    /// named _localStorageService and it will be used to store and retrieve the authentication token.
    /// - `private readonly HttpClient _http;`: This line declares a private and read-only field of type HttpClient, 
    /// which is a class for sending HTTP requests and receiving HTTP responses³. The field is named _http and it will
    /// be used to communicate with the web API that provides the authentication token and the user's claims.
    /// - `public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient http)`: This line defines 
    /// a public constructor for the CustomAuthStateProvider class that accepts two parameters: a local storage service 
    /// and an HTTP client.The constructor assigns the parameters to the fields declared earlier.
    /// - `public override async Task<AuthenticationState> GetAuthenticationStateAsync()`: This line defines a 
    /// public and asynchronous method that overrides the GetAuthenticationStateAsync method from the base class.
    /// 
    /// The method returns a Task object that represents an asynchronous operation that produces an AuthenticationState object,
    /// which contains the user's identity and claims⁴.
    /// - `string authToken = await _localStorageService.GetItemAsStringAsync("authToken");`: This line declares a string variable
    /// named authToken and assigns it the value of the item stored in the local storage with the key "authToken". The GetItemAsStringAsync method is an asynchronous method that returns a Task object that represents an asynchronous operation that produces a string value². The await keyword is used to wait for the task to complete and get the result.
    /// - `var identity = new ClaimsIdentity();`: This line declares a variable named identity and assigns it a new instance of the ClaimsIdentity class, which represents a user's identity based on a set of claims⁵. The variable is initialized with an empty identity, meaning no claims are specified.
    /// - `_http.DefaultRequestHeaders.Authorization = null;`: This line sets the Authorization property of the DefaultRequestHeaders property of the _http field to null. The DefaultRequestHeaders property is a collection of HTTP headers that are sent with every request made by the _http field³. The Authorization property is a header that specifies the credentials to authenticate the user with the web API.Setting it to null means no credentials are provided.
    /// - `if (!string.IsNullOrEmpty(authToken))`: This line checks if the authToken variable is not null or empty, meaning there is a valid authentication token stored in the local storage.
    /// - `try`: This line starts a try block, which is used to execute some code that may throw an exception and handle it gracefully.
    /// - `identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");`: This line assigns a new instance of the ClaimsIdentity class to the identity variable, passing two parameters: a collection of claims and a string. The collection of claims is obtained by calling the ParseClaimsFromJwt method with the authToken variable as the argument.The ParseClaimsFromJwt method is a private method defined later in the code that extracts the claims from a JSON Web Token(JWT). The string is "jwt", which is the authentication type of the identity, meaning the identity is based on a JWT.
    /// - `_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", ""));`: This line sets the Authorization property of the DefaultRequestHeaders property of the _http field to a new instance of the AuthenticationHeaderValue class, passing two parameters: a string and another string. The first string is "Bearer", which is the scheme of the authorization header, meaning the credentials are a bearer token.The second string is the authToken variable with the double quotes removed, which is the value of the bearer token. This line adds the authentication token to the HTTP header that will be sent with every request made by the _http field.
    /// - `catch`: This line starts a catch block, which is used to handle any exception that may occur in the try block.
    /// - `await _localStorageService.RemoveItemAsync("authToken");`: This line calls the RemoveItemAsync method of the _localStorageService field with the argument "authToken". The RemoveItemAsync method is an asynchronous method that returns a Task object that represents an asynchronous operation that removes an item from the local storage with the given key². The await keyword is used to wait for the task to complete.This line removes the authentication token from the local storage if it is invalid or expired.
    /// - `identity = new ClaimsIdentity();`: This line assigns a new instance of the ClaimsIdentity class to the identity variable, with no parameters.This line resets the identity to an empty identity, meaning no claims are specified.
    /// - `var user = new ClaimsPrincipal(identity);`: This line declares a variable named user and assigns it a new instance of the ClaimsPrincipal class, passing the identity variable as the parameter.The ClaimsPrincipal class represents a user's identity and roles based on one or more claims identities. The user variable represents the current user of the application.
    /// - `var state = new AuthenticationState(user);`: This line declares a variable named state and assigns it a new instance of the AuthenticationState class, passing the user variable as the parameter.The AuthenticationState class represents the user's authentication state at a given point in time⁴. The state variable represents the current authentication state of the application.
    /// - `NotifyAuthenticationStateChanged(Task.FromResult(state));`: This line calls the NotifyAuthenticationStateChanged method of the base class, passing a Task object that represents an asynchronous operation that produces the state variable as the result.The NotifyAuthenticationStateChanged method is a protected method that notifies the AuthenticationStateProvider subscribers that the authentication state has changed⁴. The Task.FromResult method is a static method that creates a Task object that has completed successfully with a given result.This line triggers the authentication state change event with the current authentication state.
    /// - `return state;`: This line returns the state variable as the result of the GetAuthenticationStateAsync method.
    /// - `private byte[] ParseBase64WithoutPadding(string base64)`: This line defines a private method named ParseBase64WithoutPadding that accepts a string parameter named base64 and returns an array of bytes.The method is used to parse a base64-encoded string without padding characters and convert it to a byte array.
    /// - `switch (base64.Length % 4)`: This line starts a switch statement, which is used to execute different code blocks based on the value of an expression.The expression is the remainder of dividing the length of the base64 parameter by 4, which is used to determine how many padding characters are missing from the base64-encoded string.
    /// - `case 2: base64 += "=="; break;`: This line defines a case block, which is executed when the expression matches the value of the case label.The case label is 2, meaning the base64-encoded string is missing two padding characters.The code block appends two equal signs to the base64 parameter, which are the padding characters for base64 encoding. The break statement ends the execution of the switch statement.
    /// - `case 3: base64 += "="; break;`: This line defines another case block, which is executed when the expression matches the value of the case label.The case label is 3, meaning the base64-encoded string is missing one padding character.The code block appends one equal sign to the base64 parameter, which is the padding character for base64 encoding. The break statement ends the execution of the switch statement.
    /// - `return Convert.FromBase64String(base64);`: This line returns the result of calling the FromBase64String method of the Convert class, passing the base64 parameter as the argument.The FromBase64String method is a static method that converts a base64-encoded string to a byte array.This line returns the byte array representation of the base64-encoded string.
    /// - `private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)`: This line defines a private method named ParseClaimsFromJwt that accepts a string parameter named jwt and returns an enumerable collection of Claim objects.The method is used to parse the claims from a JSON Web Token (JWT), which is a compact and secure way of transmitting information between parties as a JSON object.
    /// - `var payload = jwt.Split('.')[1];`: This line declares a variable named payload and assigns it the value of the second element of the array returned by calling the Split method of the jwt parameter, passing a dot as the argument.The Split method is an instance method that splits a string into substrings based on a separator character. The jwt parameter is a string that consists of three parts separated by dots: a header, a payload, and a signature.The payload variable contains the payload part of the JWT, which is a base64-encoded JSON object that contains the claims.
    /// - `var jsonBytes = ParseBase64WithoutPadding(payload);`: This line declares a variable named jsonBytes and assigns it the result of calling the ParseBase64WithoutPadding method with the payload variable as the argument.The ParseBase64WithoutPadding method is a private method defined earlier in the code that
    /// </summary>

    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;

        public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient httpClient)
        {
            this._localStorageService = localStorageService;
            _httpClient = httpClient;
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string authToken = await _localStorageService.GetItemAsStringAsync("authToken");

            var identity = new ClaimsIdentity();
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(authToken))
            {
                try
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", ""));
                }
                catch
                {
                    await _localStorageService.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();
                }
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            try
            {
                var payload = jwt.Split('.')[1];
                var jsonBytes = ParseBase64WithoutPadding(payload);
                var keyValuePairs = JsonSerializer
                    .Deserialize<Dictionary<string, object>>(jsonBytes);

                var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));

                return claims;
            }
            catch (Exception ex )
            {
                // Handle the exception: Log the error, invalidate the token, etc.
                //Console.WriteLine($"Error parsing Base-64: {ex.Message}");
                throw new Exception(ex.Message); // Return null (needs handling in calling code)

                //return Enumerable.Empty<Claim>(); // Or throw a custom exception
            }

        }
    }
}
