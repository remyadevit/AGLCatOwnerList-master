# Cat-Owner List

A sample solution for a programming challenge from AGL.

### Note:

- This is VS2019 solution that runs on .NET Core 2.0.
- It consists of a Console App, a .NET Standard Library and Test Projects.
- It uses XUnit and Moq fosr unit testing.
- All unit tests are under the "Tests" folder, when viewing the solution on VS2019.


### Background

The coding challenge is to build an app that consumes data from a JSON web service and outputs a list of all the cats in alphabetical order under a heading of the gender of their owner. For example:

Male

  * Angel
  * Molly
  * Tigger

Female

  * Gizmo
  * Jasper
  


  ### To Test:

- Download this solution to your system and open the solution file (AGLCatOwnerSolution.sln) with VS2017 or above.
- Once loaded on VS2019, press Ctrl-F5. This should launch a console app showing the results. 
- If the app is unable to connect to the People Web Service, it will show a friendly text message on the console.