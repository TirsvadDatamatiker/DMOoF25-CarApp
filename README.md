[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]

# CarApp
Car registrations app

## Table of Contents

- [About The Project](#about-the-project)
- [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Build and Run](#build-and-run)
- [Features](#features)
- [Todo](#todo)
- [Known issues](#known-issues)
- [Folder Structure](#folder-structure) 

## About The Project

This project is a car registration app that allows users to register their cars and view the cars that are already registered. The app is built using .NET for Console. The app is built as a part of the course DMOoF25 at UCL.

## Getting Started

To get a local copy up and running follow these simple steps.

### Prerequisites

This is an example of how to list things you need to use the software and how to install them.

- .NET 9.0
    ```
    https://dotnet.microsoft.com/download/dotnet/9.0
    ```

- Visual Studio 2022
    ```
    https://visualstudio.microsoft.com/
    ```

- Install Sqlite and Dapper

    Go to the project folder and run the following commands

    ```
    dotnet nuget add source https://api.nuget.org/v3/index.json
    dotnet add package Dapper
    dotnet add package Microsoft.Data.Sqlite
    ```

### Build and Run

1. Clone the repo
    ```sh
    git clone git@github.com:DMOoF25-Team-11/CarApp.git
    ```

2. Open the project in Visual Studio 2022

3. Build the project

4. Run the project

5. Optionally, you can import sample data by pressing 'F7' in menu and then 'F1' in database menu

You may need to remove / change key bindings ´F11´ in the VS buildin terminal to use the application properly.

## Features

- [x] Add car
- [x] Load / Save car to database
- [x] Calculate trip fuel price
- [x] Print rapport of car
- [x] Export / Import json data

## Todo

- [ ] Clear Database

## Known issues

- [ ] When returned from database menu, then main menu title missing first letter. Temporary fix wrtting a char to console before clear.
- [ ] When import and export json give user info about the process

## Folder Structure

```sh
./logo/             #Contains the logo of the project.
./documentation/    #Contains the documentation for the project.
./images/           #Contains images used in the documentation.
./CarApp/           #Contains the source code for the project.
```

[contributors-shield]: https://img.shields.io/github/contributors/TirsvadDatamatiker/DMOoF25-CarApp?style=for-the-badge
[contributors-url]: https://github.com/TirsvadDatamatiker/DMOoF25-CarApp/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/TirsvadDatamatiker/DMOoF25-CarApp?style=for-the-badge
[forks-url]: https://github.com/TirsvadDatamatiker/DMOoF25-CarApp/network/members
[stars-shield]: https://img.shields.io/github/stars/TirsvadDatamatiker/DMOoF25-CarApp?style=for-the-badge
[stars-url]: https://github.com/TirsvadDatamatiker/DMOoF25-CarApp/stargazers
[issues-shield]: https://img.shields.io/github/issues/TirsvadDatamatiker/DMOoF25-CarApp?style=for-the-badge
[issues-url]: https://github.com/TirsvadDatamatiker/DMOoF25-CarApp/issues
[license-shield]: https://img.shields.io/github/license/TirsvadDatamatiker/DMOoF25-CarApp?style=for-the-badge
[license-url]: https://github.com/TirsvadDatamatiker/DMOoF25-CarApp/blob/master/LICENSE
