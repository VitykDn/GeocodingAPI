
Geocoding project
GeocodingWebAPI is an Asp.Net Core Web Api that allows users to easily geocode addresses into coordinates and vice versa using Nominatim geolocation services. The project supports mandatory fields such as latitude and longitude. The resulting address model includes details such as address, city, state, postal code, and country, which are cached in the database along with the original request.
Technical tasks completed so far include the implementation of the address and coordinate functions, and the recording of results in the database.

Technologies used:
- .NET 6.0
- MySQL 
- ASP.NET CORE 6
- Nominatim
- xUnit

Completed technical tasks:
	The function of obtaining an address allows you to translate coordinates into a special address model, which includes: address, city, state, postal code and country. When received, the results are recorded in the database together with the requests. Entering both latitude and longitude is mandatory for the service to work.
	The function for obtaining coordinates offers greater flexibility to the user by allowing them to input only the address fields that they know, without the need to fill in all fields. If certain fields are not filled in, the function will return the first set of coordinates that match the provided information. However, it's important to note that when the request is cached, it's stored in its original form, including any null or missing fields. This may result in some inaccuracies or inconsistencies in the cached data, but it allows for a more flexible and user-friendly experience.

Encountered Problems
- While creating unit tests, I encountered certain problems, as a result, I did not have time to complete this task, as a sample there are non-working examples of tests.
