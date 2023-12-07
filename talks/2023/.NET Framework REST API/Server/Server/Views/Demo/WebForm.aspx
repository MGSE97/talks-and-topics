<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="WebForm.aspx.cs" Inherits="Server.Views.Demo.WebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width" />
    <title><%: Page.Title %> - Weather Demo</title>
    <%: Styles.Render("~/Content/css") %>
    <%: Scripts.Render("~/bundles/modernizr") %>
    <%: Styles.Render("~/bundles/fontawesome") %>
    <%: Scripts.Render("~/bundles/fontawesome") %>
</head>
<body>
    <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-dark bg-dark">
        <div class="container">
            <a href="/" class="navbar-brand">Weather Demo</a>
            <button type="button" class="navbar-toggler" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" title="Toggle navigation" aria-controls="navbarSupportedContent"
                    aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse d-sm-inline-flex justify-content-start">
                <ul class="navbar-nav">
                    <li><a href="/help/index"class="nav-link">API</a></li>
                    <li><a href="/swagger" class="nav-link" target="_blank">Swagger</a></li>
                </ul>
            </div>
        </div>
    </nav>
    <div class="container body-content">
        <form id="form" runat="server">

            <main>
                <section class="row" aria-labelledby="title">
                    <div class="d-inline-flex justify-content-between align-items-baseline">
                        <h1 id="title">Web Forms</h1>
                        <h6><a class="text-decoration-none w-auto" href="https://learn.microsoft.com/cs-cz/aspnet/web-forms/overview/getting-started/getting-started-with-aspnet-45-web-forms/" target="_blank"><i class="fa fa-solid fa-external-link-alt"></i> Docs</a></h6>
                    </div>
                    <p class="lead">
                        This is a Weather API demo frontend for Web Forms. 
                        You can look through different implementations in main menu. 
                        Server API stays the same for all versions.
                    </p>
                </section>
                <hr/>
                <section class="row">
                    <div class="col-6">
                        <div class="row mb-3">
                            <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text rounded-0 rounded-start">Location:</span>
                                </div>
                                <asp:TextBox runat="server" ID="Search" CssClass="form-control mw-100" AutoCompleteType="None" AutoPostBack="True" OnTextChanged="Search_TextChanged" list="search-options" placeholder="Česká Republika"></asp:TextBox>
                                <datalist id="search-options"></datalist>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <table class="table table-striped table-bordered table-hover table-responsive text-center">
                                    <thead>
                                        <tr>
                                            <th><i class="fa-solid fa-calendar-day"></i> Date</th>
                                            <th><i class="fa-solid fa-cloud-sun"></i> Weather</th>
                                            <th><i class="fa-solid fa-temperature-half"></i> Temperature</th>
                                            <th><i class="fa-solid fa-droplet"></i> Rain</th>
                                        </tr>
                                    </thead>
                                    <tbody id="data">
                                            <% if (Weather.WeatherData is null || Weather.WeatherData.Count == 0) {%>
                                            <tr>
                                                <td colspan="4">
                                                    <div class="alert alert-warning border-warning rounded text-center" role="alert">
                                                        <b>No data</b>
                                                    </div>
                                                </td>
                                            </tr>
                                            <% } else {
                                                foreach(var weather in Weather.WeatherData) {
                                                %>        
                                                    <tr>
                                                        <td><%= weather.Date.ToLocalTime().ToString("dddd d.M.") %></td>
                                                        <td><%= weather.Type.ToHtml() %> <%= weather.Type %></td>
                                                        <td><%= weather.Temperature.ToString("f0") %> °C</td>
                                                        <td><%= weather.Rain.ToString("f2") %> mm/h</td>
                                                    </tr>
                                                <% 
                                                }
                                            } %>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="row mb-3">
                            <h4>Api Statistics</h4>
                        </div>
                        <div class="row">
                            <div class="col">
                                <table class="table table-striped table-bordered table-hover table-responsive text-center">
                                    <thead>
                                        <tr>
                                            <th>Endpoint</th>
                                            <th>Data</th>
                                        </tr>
                                    </thead>
                                    <tbody id="api-data">
                                        <% foreach (var (url, data) in (ViewState["requests"] as List<(string, string)>).Take(7)) { %>
                                            <tr>
                                                <td><%= url %></td>
                                                <td><%= data %></td>
                                            </tr>
                                        <% } %>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </section>
            </main>

        </form>
        <hr />
        <footer>
            <p>&copy; <%: DateTime.Now.Year %> - Weather Demo</p>
        </footer>
    </div>

    <%: Scripts.Render("~/bundles/jquery") %>
    <%: Scripts.Render("~/bundles/bootstrap") %>
    
    <script>
        let input = document.getElementById("Search")
        let autocomplete = document.getElementById("search-options")
        // Handle autocomplete
        input.onkeyup = async (e) => {
            if (!e.keyCode) return
            var location = input.value
            var locations = await fetch(`/api/weather/locations/search`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(location)
            })
            var response = await locations.json()
            autocomplete.innerHTML = response.map(location => `<option value="${location}"></option>`).join('')
        }
    </script>
</body>
</html>
