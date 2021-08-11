# Tech4Gaming Deals

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![JavaScript](https://img.shields.io/badge/javascript-%23323330.svg?style=for-the-badge&logo=javascript&logoColor=%23F7DF1E)
![NodeJS](https://img.shields.io/badge/node.js-%2343853D.svg?style=for-the-badge&logo=node.js&logoColor=white)
![Express.js](https://img.shields.io/badge/express.js-%23404d59.svg?style=for-the-badge&logo=express&logoColor=%2361DAFB)
![Heroku](https://img.shields.io/badge/heroku-%23430098.svg?style=for-the-badge&logo=heroku&logoColor=white)
![MongoDB](https://img.shields.io/badge/MongoDB-%234ea94b.svg?style=for-the-badge&logo=mongodb&logoColor=white)

By Marco Stari

## Technologies Used

- C#
- Xamarin
- Javascript
- Node js
- Express js
- Heroku: cloud platform to run the backend code
- MongoDB: Main database
- Cloudinary: Database to store the images

## Description

`Tech4Gaming Deals` is a iOS & Android app that was requested by [Tech4Gaming](https://www.youtube.com/c/Tech4Gaming/featured). The goal was to create an app where his followers could access a list of amazon products selected by him.

The app showed in the video is the **admin version** where, unlike in the regular app, you can add, edit and delete products (obviously in the app intended for the final user this is not possible)

To **keep prices low** (and also to experiment with some services, libraries and frameworks I never used) I decided to create the main app in c# with Xamarin to make it cross platform, to use node js with express js for the back-end and to rely on services like: Heroku (to host the back-end code), mongoDB as the main database to keep product data and Cloudinary since it allowed me to store images at a lower cost

## Features

- Upload, edit and delete offers
  - The products are selected manually from amazon and are shared through an affiliate link
  - The admin can also select a date and time after which to delete the offer automatically
- Add offers to a "shopping cart" list
- Search a product by name
- Filter products by categories
