# GenerateCRUDWithTests
Framework able to generate c# MVC project with specflow tests for database and controllers

Before start: open terminal, go to project directoy and execute dotnet run
Go to page localhost:5080 

You will see 3 fields:

In first enter path where you want to create a project:
Something like this: C:/Users/mac/Desktop

In second enter name of project

In third enter entities and fields for them that you want to create in your project:
Something like with syntax like in example:

Entity:  Customer(  Username:   string, Surname:string,   Age:integer);@

Entity:User(Username:string,  Surname:string, Age:integer,Password:integer);@

Entity:Me(Name:string,Ekma:string,Dde:integer,Hehe:integer);@
Entity:me(Dbe:char,abc:double,dbe:double,Hehe:integer);

types that are allowed for fields for now:string,integer,bool,char,double;

