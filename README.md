# eCommerce
A simple web-based application for internet shop.
## Deployment URL
The app is deployed on https://ecommerce-olsj.onrender.com/
## Choice of language and framework
I decided to expirement with ASP.NET framework for web applications. Additionally, I used official MongoDB Driver for .NET, which maps plain objects with Mongo BSON objects. The app uses cookie-based authentication with two roles. The app does not allow accessing the admin or user functionality without proper authorization.
## User guide
I already created three users and three admins, however, one can register through regular means by clicking the Register button in the upper-right side.
To login with preexisting user, please use the following usernames: user1, user2, user3, admin1, admin2, admin3. The password for each of them is equal to their username.
Logging in as a user or admin, it is possible to see your profile page by clicking on the username in the upper-right side.
The user or admin can leave a rating and review by clicking the details button on the main Browse page in the bottom.
When logging in as an admin, it is possible to see additional user interface elements. For example, it is possible to edit or delete items on the main Browse page. Additionally, admins can access user management page, which allows to remove or add users, and item management page, which allows to create items and remove or create categories.
## MongoDB usage
Throughout the application I try to use the flexibility of MongoDB by not storing null attributes in the database and storing ratings and reviews as lists inside the collections. Therefore, it is possible to use only two collections: Items and Users. However, I added the Categories collection to make them somewhat fixed.
