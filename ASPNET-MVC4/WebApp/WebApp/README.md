ASPNET-MVC4
===

##### Installation

1. Get nuget deps from command line
	<pre>
	$ .nuget/nuget.exe restore
	</pre>
2. Run migrations (from package manager console in VS)
	<pre>
	update-database -verbose -ConnectionStringName AuthContext
	</pre>

Token based authentication with OWIN, Web API 2, MVC4

There's a user setup with credentials
<pre>
username: iduser
password: password
</pre>


##### Using the API

First Register a user (this is how the db gets created, will fix soon)

<pre>
POST /api/account/token
Content-Type: application/json

{
  "userName": "Taiseer",
  "password": "SuperPass",
  "confirmPassword": "SuperPass"
}
</pre>

to get token

<pre>
POST /token
Content-Type: application/x-www-form-urlencoded

grant_type=password&username=Taiseer&password=SuperPass
</pre>

will return a bearer token

<pre>
{
"access_token": "EPpWBBlo2vutllIQDpw4YcgK0K928Ei5ZCqz1kFZbAx2r1elO01iTgn5zLHYf53OaR9rozECDpWi3j3tblk8IqRpFY0es8U55pyd5Qj7Bp6HT74ramkqEC9pl2tG0uB6KQVU3N0XFlh59MMFSISIJOGDeYp5yjHaKs8Uh9SyDzQow3yXNHoUh9QJazgAK6pHuiV0nZcSZZIrJt4KoAPudWVX_WkkzpPJ1F48Orrfe7A",
"token_type": "bearer",
"expires_in": 86399
}
</pre>

##### Useful Reading

* <a href="http://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity/" target="_blank">based on this example</a>
