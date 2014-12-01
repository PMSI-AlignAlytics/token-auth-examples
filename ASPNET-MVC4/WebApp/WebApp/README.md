ASPNET-MVC4
===


Token based authentication with OWIN, Web API 2, MVC4

There's a user setup with credentials
<pre>
username: iduser
password: password
</pre>


##### Installation

To test the SSO feature make sure AlignAuth is running first

Before you start make sure you have localdb installed with an instance called Projects.  Please read more <a href="http://www.mssqltips.com/sqlservertip/2694/getting-started-with-sql-server-2012-express-localdb/" target="_blank">here</a>

Open VS Command line tool to &lt;GIT_PATH&gt;\token-auth-examples\ASPNET-MVC4\WebApp

<pre>
$ .nuget/nuget.exe restore
$ cd WebApp
$ msbuild WebApp.csproj "/target:Build" "/p:Configuration=Debug;Platform=AnyCPU"
$ bin\Program.exe apps\omega.json
</pre>

##### Useful Reading

* <a href="http://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity/" target="_blank">based on this example</a>
