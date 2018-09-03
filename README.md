# SmallProjectAPI
CodementorX Small Project - Back End

Important notes:

1) lthough it is not the original way it was requested in the requirement, The JWT authentication used for the API is the Bearer schema, complying with the recommendations in the JWT documentation (See https://jwt.io/introduction/ for further details). The access token header has to be in the form

Authorization: Bearer [Access token]

Example:
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiamFjay1ibGFja0Bjb2RlbWVudG9yLmlvIiwiZXhwIjoxNTM0MjU2NDI2LCJpc3MiOiJodHRwOi8vNTIuMjAyLjg0LjEzNDo1MDAwLyIsImF1ZCI6Imh0dHA6Ly81Mi4yMDIuODQuMTM0OjUwMDAvIn0.Jkk0SMU3QU2CIvaoIdPyvvonSmX-6f4nY6TEdME2XNU

2) Unit testing is included as a separate project in the solution (code coverage: 74%). I usually take as a standard that the code coverage must be equal to or greater than 70%, although the features of each project may require a greater or lesser number.

3) Based on pure login, I assumed that each user can only manage their ideas.

4) The average score is calculated in each API call because no particular performance needs or potential bottlenecks were specified in the requirements. To improve performance for scalation purposes, this calculation could be later cached in the database as an additional column.

Online version: http://52.202.84.134:5000

Example: http://52.202.84.134:5000/ping

Feel free to contact me in case of any doubt or request to juanpabloventoso@gmail.com.

Thanks in advance!
Juan Pablo
