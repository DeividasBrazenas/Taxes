# ASP.NET Core WebApi for Tax calculation.

Written in Visual Studio 2019

#### Database restore
> Tools > NuGet Package Manager > Package Manager Console > update-database

#### Start Web Service
> dotnet run Taxes.Service.csproj

### Endpoints

#### Municipalities
> GET: /odata/municipalities

> GET: /odata/municipalities/id

> POST: /odata/municipalities     
Payload: {"Name":"Vilnius"}

> PUT: /odata/municipalities/id    
Payload: {"Name":"Vilnius", "Id":1}

> DELETE: /odata/municipalities/id    


#### Taxes
> Available frequencies: Yearly, Monthly, Weekly, Daily. If frequency is daily, start and end dates should be the same

> GET: /odata/taxes

> GET: /odata/taxes/id

> POST: /odata/taxes
Payload: {"MunicipalityId":21, "Frequency":"Yearly", "StartDate":"2016-01-01", "EndDate":"2016-12-31", "Value":0.1}

> PUT: /odata/taxes/id
Payload: {"MunicipalityId":21, "Frequency":"Yearly", "StartDate":"2016-01-01", "EndDate":"2016-12-31", "Value":0.1, "Id":1}

> DELETE: /odata/taxes/id

### MunicipalityWithTax
> GET: /odata/municipalitywithtax

Parameters: name, date (for example: /odata/municipalitywithtax?name=Vilnius&date=2019-06-04)
